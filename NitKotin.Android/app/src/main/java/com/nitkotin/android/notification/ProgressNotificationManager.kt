package com.nitkotin.android.notification

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import androidx.core.content.ContextCompat
import com.nitkotin.android.MainActivity
import com.nitkotin.android.R
import com.nitkotin.android.domain.LocalizationService
import com.nitkotin.android.ui.MainUiState

class ProgressNotificationManager(private val context: Context) {
    private val channelId = "nitkotin-progress"
    private val notificationId = 1001

    fun showProgress(state: MainUiState) {
        if (!canPostNotifications()) {
            return
        }

        createChannel(state)
        val launchIntent = Intent(context, MainActivity::class.java)
        val pendingIntent = PendingIntent.getActivity(
            context,
            0,
            launchIntent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE,
        )

        val notification = NotificationCompat.Builder(context, channelId)
            .setSmallIcon(R.drawable.ic_stat_nitkotin)
            .setContentTitle(LocalizationService.getString(state.language, "notification_title"))
            .setContentText("${state.savedAmountText} • ${state.elapsedText}")
            .setStyle(NotificationCompat.BigTextStyle().bigText("${state.savedAmountText}\n${state.elapsedText}"))
            .setContentIntent(pendingIntent)
            .setOngoing(true)
            .setOnlyAlertOnce(true)
            .setPriority(NotificationCompat.PRIORITY_LOW)
            .build()

        NotificationManagerCompat.from(context).notify(notificationId, notification)
    }

    private fun createChannel(state: MainUiState) {
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.O) return

        val manager = context.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        val channel = NotificationChannel(
            channelId,
            LocalizationService.getString(state.language, "notification_channel_name"),
            NotificationManager.IMPORTANCE_LOW,
        ).apply {
            description = LocalizationService.getString(state.language, "notification_channel_description")
        }
        manager.createNotificationChannel(channel)
    }

    private fun canPostNotifications(): Boolean {
        return Build.VERSION.SDK_INT < Build.VERSION_CODES.TIRAMISU ||
            ContextCompat.checkSelfPermission(context, android.Manifest.permission.POST_NOTIFICATIONS) == PackageManager.PERMISSION_GRANTED
    }
}
