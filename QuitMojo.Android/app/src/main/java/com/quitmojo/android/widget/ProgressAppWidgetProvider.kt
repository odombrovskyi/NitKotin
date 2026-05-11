package com.quitmojo.android.widget

import android.appwidget.AppWidgetManager
import android.appwidget.AppWidgetProvider
import android.content.Context
import com.quitmojo.android.data.model.AppLanguage
import com.quitmojo.android.ui.MainUiState

class ProgressAppWidgetProvider : AppWidgetProvider() {
    override fun onUpdate(
        context: Context,
        appWidgetManager: AppWidgetManager,
        appWidgetIds: IntArray,
    ) {
        ProgressWidgetUpdater.update(
            context = context,
            state = MainUiState(language = AppLanguage.ENGLISH),
        )
    }
}
