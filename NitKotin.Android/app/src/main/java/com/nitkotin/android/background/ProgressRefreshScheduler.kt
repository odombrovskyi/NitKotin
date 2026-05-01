package com.nitkotin.android.background

import android.content.Context
import androidx.work.ExistingWorkPolicy
import androidx.work.OneTimeWorkRequestBuilder
import androidx.work.WorkManager
import java.util.concurrent.TimeUnit

object ProgressRefreshScheduler {
    private const val WorkName = "nitkotin-progress-refresh"
    private const val RefreshIntervalMinutes = 10L

    fun schedule(context: Context) {
        val request = OneTimeWorkRequestBuilder<ProgressRefreshWorker>()
            .setInitialDelay(RefreshIntervalMinutes, TimeUnit.MINUTES)
            .build()

        WorkManager.getInstance(context).enqueueUniqueWork(
            WorkName,
            ExistingWorkPolicy.KEEP,
            request,
        )
    }

    fun scheduleNext(context: Context) {
        val request = OneTimeWorkRequestBuilder<ProgressRefreshWorker>()
            .setInitialDelay(RefreshIntervalMinutes, TimeUnit.MINUTES)
            .build()

        WorkManager.getInstance(context).enqueueUniqueWork(
            WorkName,
            ExistingWorkPolicy.REPLACE,
            request,
        )
    }
}