package com.nitkotin.android.widget

import android.appwidget.AppWidgetManager
import android.appwidget.AppWidgetProvider
import android.content.Context
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.ui.MainUiState

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
