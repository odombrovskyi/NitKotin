package com.nitkotin.android.ui

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.CatalogProduct
import com.nitkotin.android.data.model.SmokingConfig
import com.nitkotin.android.data.repository.AssetCatalogRepository
import com.nitkotin.android.data.repository.AssetMotivationalPhraseRepository
import com.nitkotin.android.data.repository.PreferencesRepository
import com.nitkotin.android.background.ProgressRefreshScheduler
import com.nitkotin.android.domain.LocalizationService
import com.nitkotin.android.domain.ProductSuggestionService
import com.nitkotin.android.domain.RecoveryTimelineService
import com.nitkotin.android.domain.SavingsCalculator
import com.nitkotin.android.notification.ProgressNotificationManager
import com.nitkotin.android.widget.ProgressWidgetUpdater
import kotlinx.coroutines.delay
import kotlinx.coroutines.Job
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch
import java.time.ZoneId
import java.time.format.DateTimeFormatter
import java.time.Duration
import java.time.Instant

class MainViewModel(application: Application) : AndroidViewModel(application) {
    private companion object {
        const val UiTickMillis = 1000L
        const val PhraseRotationIntervalSeconds = 3
    }

    private val app = application
    private val preferencesRepository = PreferencesRepository(application)
    private val catalogRepository = AssetCatalogRepository(application)
    private val phraseRepository = AssetMotivationalPhraseRepository(application)
    private val recoveryTimelineService = RecoveryTimelineService()
    private val notificationManager = ProgressNotificationManager(application)

    private val _uiState = MutableStateFlow(MainUiState())
    val uiState: StateFlow<MainUiState> = _uiState.asStateFlow()

    private var latestConfig = SmokingConfig()
    private var latestPhrases = listOf<String>()
    private var latestProducts = listOf<CatalogProduct>()
    private var phraseIndex = 0
    private var lastProductRefresh = Instant.now()
    private val dateFormatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")
    private var elapsedSeconds = 0L
    private var uiTickerJob: Job? = null

    init {
        observeConfig()
    }

    fun setScreenActive(isActive: Boolean) {
        if (isActive) {
            if (uiTickerJob?.isActive == true) {
                return
            }
            uiTickerJob = viewModelScope.launch {
                publishState(
                    latestConfig,
                    lastProductRefresh,
                    rotatePhrase = false,
                    forceSuggestionRefresh = false,
                    updateExternalSurfaces = false,
                )
                while (true) {
                    delay(UiTickMillis)
                    elapsedSeconds += 1
                    publishState(
                        latestConfig,
                        lastProductRefresh,
                        rotatePhrase = elapsedSeconds % PhraseRotationIntervalSeconds == 0L,
                        forceSuggestionRefresh = false,
                        updateExternalSurfaces = false,
                    )
                }
            }
        } else {
            uiTickerJob?.cancel()
            uiTickerJob = null
        }
    }

    fun setLanguage(language: AppLanguage) {
        viewModelScope.launch {
            preferencesRepository.setLanguage(language)
        }
    }

    fun updateSettings(quitDateTime: Instant, packsPerDay: Double, packPriceUah: Double) {
        persistInputs(
            latestConfig.copy(
                quitDateTime = quitDateTime,
                packsPerDay = packsPerDay,
                packPriceUah = packPriceUah,
            )
        )
    }

    fun updatePacksPerDay(value: Double) {
        persistInputs(latestConfig.copy(packsPerDay = value))
    }

    fun updatePackPrice(value: Double) {
        persistInputs(latestConfig.copy(packPriceUah = value))
    }

    fun updateQuitDateTime(value: Instant) {
        viewModelScope.launch {
            preferencesRepository.setQuitDateTime(value)
        }
    }

    fun startTrackingNow() {
        viewModelScope.launch {
            preferencesRepository.startTrackingNow(Instant.now())
        }
    }

    fun refreshProducts() {
        viewModelScope.launch {
            lastProductRefresh = Instant.now()
            publishState(
                latestConfig,
                lastProductRefresh,
                rotatePhrase = false,
                forceSuggestionRefresh = true,
                updateExternalSurfaces = true,
            )
        }
    }

    private fun persistInputs(config: SmokingConfig) {
        viewModelScope.launch {
            preferencesRepository.updateInputs(
                quitDateTime = config.quitDateTime,
                packsPerDay = config.packsPerDay,
                packPriceUah = config.packPriceUah,
            )
        }
    }

    private fun observeConfig() {
        viewModelScope.launch {
            preferencesRepository.config.collectLatest { config ->
                latestConfig = config
                latestPhrases = phraseRepository.load(config.languagePreference).map { it.text }
                latestProducts = catalogRepository.load(config.languagePreference)
                if (phraseIndex >= latestPhrases.size) {
                    phraseIndex = 0
                }
                elapsedSeconds = 0L
                lastProductRefresh = Instant.now()
                publishState(
                    config,
                    lastProductRefresh,
                    rotatePhrase = false,
                    forceSuggestionRefresh = true,
                    updateExternalSurfaces = true,
                )
            }
        }
    }

    private suspend fun publishState(
        config: SmokingConfig,
        lastRefresh: Instant,
        rotatePhrase: Boolean,
        forceSuggestionRefresh: Boolean,
        updateExternalSurfaces: Boolean,
    ) {
        if (rotatePhrase && latestPhrases.isNotEmpty()) {
            phraseIndex = (phraseIndex + 1) % latestPhrases.size
        }

        val now = Instant.now()
        val savedAmount = SavingsCalculator.calculateSavedAmount(config, now)
        val suggestions = if (forceSuggestionRefresh || _uiState.value.productSuggestions.isEmpty()) {
            ProductSuggestionService(latestProducts).getSuggestions(savedAmount.toDouble())
        } else {
            _uiState.value.productSuggestions
        }
        val milestones = recoveryTimelineService.getVisibleMilestones(
            smokeFreeDuration = if (config.hasStartedTracking) Duration.between(config.quitDateTime, now) else Duration.ZERO,
            language = config.languagePreference,
        )
        val uiState = buildUiState(
            config = config,
            savedAmount = SavingsCalculator.formatCurrency(savedAmount, config.languagePreference),
            suggestions = suggestions,
            milestones = milestones,
            lastRefresh = lastRefresh,
            now = now,
        )
        _uiState.value = uiState
        if (updateExternalSurfaces) {
            notificationManager.showProgress(uiState)
            ProgressWidgetUpdater.update(app, uiState)
            ProgressRefreshScheduler.schedule(app)
        }
    }

    private fun buildUiState(
        config: SmokingConfig,
        savedAmount: String,
        suggestions: List<com.nitkotin.android.data.model.SuggestedProduct>,
        milestones: List<com.nitkotin.android.data.model.RecoveryMilestoneSnapshot>,
        lastRefresh: Instant,
        now: Instant,
    ): MainUiState {
        val language = config.languagePreference
        return MainUiState(
            language = language,
            hasStartedTracking = config.hasStartedTracking,
            quitDateTime = config.quitDateTime,
            packsPerDay = config.packsPerDay,
            packPriceUah = config.packPriceUah,
            savedAmountText = savedAmount,
            elapsedText = SavingsCalculator.formatElapsed(config, now),
            firstRunMessage = LocalizationService.getString(language, "first_run_message"),
            dashboardHint = LocalizationService.getString(language, "dashboard_hint"),
            currentPhrase = latestPhrases.getOrNull(phraseIndex) ?: LocalizationService.fallbackPhrases(language).firstOrNull()?.text.orEmpty(),
            productUpdatedAtText = LocalizationService.format(language, "products_updated", dateFormatter.withZone(ZoneId.systemDefault()).format(lastRefresh)),
            productSuggestions = suggestions,
            recoveryMilestones = milestones,
        )
    }
}
