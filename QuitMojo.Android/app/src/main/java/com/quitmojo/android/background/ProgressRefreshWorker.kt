package com.quitmojo.android.background

import android.content.Context
import androidx.core.app.NotificationManagerCompat
import androidx.work.CoroutineWorker
import androidx.work.WorkerParameters
import com.quitmojo.android.data.repository.PreferencesRepository
import com.quitmojo.android.ui.MainUiState
import com.quitmojo.android.widget.ProgressWidgetUpdater
import kotlinx.coroutines.flow.first
import java.time.Instant
import com.quitmojo.android.domain.LocalizationService
import com.quitmojo.android.domain.SavingsCalculator

class ProgressRefreshWorker(
    appContext: Context,
    workerParams: WorkerParameters,
) : CoroutineWorker(appContext, workerParams) {
    private companion object {
        const val LegacyProgressNotificationId = 1001
    }

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

        NotificationManagerCompat.from(applicationContext).cancel(LegacyProgressNotificationId)
        ProgressWidgetUpdater.update(applicationContext, state)
        ProgressRefreshScheduler.scheduleNext(applicationContext)
        return Result.success()
    }
}