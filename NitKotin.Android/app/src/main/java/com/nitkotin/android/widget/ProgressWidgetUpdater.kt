package com.nitkotin.android.widget

import android.app.PendingIntent
import android.appwidget.AppWidgetManager
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.widget.RemoteViews
import com.nitkotin.android.MainActivity
import com.nitkotin.android.R
import com.nitkotin.android.data.model.SmokingConfig
import com.nitkotin.android.domain.SavingsCalculator
import com.nitkotin.android.ui.MainUiState
import java.time.Instant

object ProgressWidgetUpdater {
    fun update(context: Context, state: MainUiState) {
        val appWidgetManager = AppWidgetManager.getInstance(context)
        val componentName = ComponentName(context, ProgressAppWidgetProvider::class.java)
        val widgetIds = appWidgetManager.getAppWidgetIds(componentName)
        if (widgetIds.isEmpty()) {
            return
        }

        val launchIntent = Intent(context, MainActivity::class.java)
        val pendingIntent = PendingIntent.getActivity(
            context,
            1,
            launchIntent,
            PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE,
        )

        widgetIds.forEach { widgetId ->
            val views = RemoteViews(context.packageName, R.layout.progress_widget)
            val config = SmokingConfig(
                languagePreference = state.language,
                hasStartedTracking = state.hasStartedTracking,
                quitDateTime = state.quitDateTime,
                packsPerDay = state.packsPerDay,
                packPriceUah = state.packPriceUah,
            )
            val now = Instant.now()
            val savedAmount = SavingsCalculator.calculateSavedAmount(config, now)

            views.setTextViewText(R.id.widgetSavedAmount, SavingsCalculator.formatWholeCurrency(savedAmount, state.language))
            views.setTextViewText(R.id.widgetElapsed, SavingsCalculator.formatElapsedDaysHours(config, now))
            views.setOnClickPendingIntent(R.id.widgetRoot, pendingIntent)
            appWidgetManager.updateAppWidget(widgetId, views)
        }
    }
}
