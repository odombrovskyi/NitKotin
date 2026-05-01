package com.nitkotin.android.background

import android.content.Context
import androidx.work.CoroutineWorker
import androidx.work.WorkerParameters
import com.nitkotin.android.data.repository.PreferencesRepository
import com.nitkotin.android.notification.ProgressNotificationManager
import com.nitkotin.android.ui.MainUiState
import com.nitkotin.android.widget.ProgressWidgetUpdater
import kotlinx.coroutines.flow.first
import java.time.Instant
import com.nitkotin.android.domain.LocalizationService
import com.nitkotin.android.domain.SavingsCalculator

class ProgressRefreshWorker(
    appContext: Context,
    workerParams: WorkerParameters,
) : CoroutineWorker(appContext, workerParams) {
    override suspend fun doWork(): Result {
        val preferencesRepository = PreferencesRepository(applicationContext)
        val config = preferencesRepository.config.first()
        val state = MainUiState(
            language = config.languagePreference,
            hasStartedTracking = config.hasStartedTracking,
            quitDateTime = config.quitDateTime,
            packsPerDay = config.packsPerDay,
            packPriceUah = config.packPriceUah,
            savedAmountText = SavingsCalculator.formatCurrency(
                SavingsCalculator.calculateSavedAmount(config, Instant.now()),
                config.languagePreference,
            ),
            elapsedText = SavingsCalculator.formatElapsed(config, Instant.now()),
            firstRunMessage = LocalizationService.getString(config.languagePreference, "first_run_message"),
            dashboardHint = LocalizationService.getString(config.languagePreference, "dashboard_hint"),
        )

        ProgressNotificationManager(applicationContext).showProgress(state)
        ProgressWidgetUpdater.update(applicationContext, state)
        ProgressRefreshScheduler.scheduleNext(applicationContext)
        return Result.success()
    }
}