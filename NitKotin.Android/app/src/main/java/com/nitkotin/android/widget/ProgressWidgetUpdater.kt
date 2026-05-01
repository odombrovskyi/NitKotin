package com.nitkotin.android.widget

import android.app.PendingIntent
import android.appwidget.AppWidgetManager
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.widget.RemoteViews
import com.nitkotin.android.MainActivity
import com.nitkotin.android.R
import com.nitkotin.android.domain.LocalizationService
import com.nitkotin.android.ui.MainUiState

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
            views.setTextViewText(R.id.widgetTitle, LocalizationService.getString(state.language, "widget_title"))
            views.setTextViewText(R.id.widgetSavedAmount, state.savedAmountText)
            views.setTextViewText(R.id.widgetElapsed, state.elapsedText)
            views.setImageViewResource(R.id.widgetIcon, R.drawable.ic_launcher_foreground)
            views.setOnClickPendingIntent(R.id.widgetRoot, pendingIntent)
            appWidgetManager.updateAppWidget(widgetId, views)
        }
    }
}
