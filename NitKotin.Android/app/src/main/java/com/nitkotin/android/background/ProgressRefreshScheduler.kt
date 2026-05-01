package com.nitkotin.android.background

import android.content.Context
import androidx.work.ExistingPeriodicWorkPolicy
import androidx.work.PeriodicWorkRequestBuilder
import androidx.work.WorkManager
import java.util.concurrent.TimeUnit

object ProgressRefreshScheduler {
    private const val WorkName = "nitkotin-progress-refresh"

    fun schedule(context: Context) {
        val request = PeriodicWorkRequestBuilder<ProgressRefreshWorker>(15, TimeUnit.MINUTES)
            .build()

        WorkManager.getInstance(context).enqueueUniquePeriodicWork(
            WorkName,
            ExistingPeriodicWorkPolicy.UPDATE,
            request,
        )
    }
}