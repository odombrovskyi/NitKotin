package com.nitkotin.android.integration

import androidx.test.core.app.ApplicationProvider
import androidx.test.ext.junit.runners.AndroidJUnit4
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.notification.ProgressNotificationManager
import com.nitkotin.android.ui.MainUiState
import com.nitkotin.android.widget.ProgressWidgetUpdater
import org.junit.Test
import org.junit.runner.RunWith
import java.time.Instant

@RunWith(AndroidJUnit4::class)
class BackgroundSurfacesSmokeTest {
    @Test
    fun notificationManager_showProgress_doesNotCrash() {
        val context = ApplicationProvider.getApplicationContext<android.content.Context>()
        val manager = ProgressNotificationManager(context)

        manager.showProgress(
            MainUiState(
                language = AppLanguage.ENGLISH,
                hasStartedTracking = true,
                quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                savedAmountText = "120 UAH 00 kop",
                elapsedText = "0 d. 09 hrs. 00 min. 00 sec.",
                dashboardHint = "hint",
            )
        )
    }

    @Test
    fun widgetUpdater_update_doesNotCrashWithoutPinnedWidgets() {
        val context = ApplicationProvider.getApplicationContext<android.content.Context>()

        ProgressWidgetUpdater.update(
            context,
            MainUiState(
                language = AppLanguage.ENGLISH,
                hasStartedTracking = true,
                quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                savedAmountText = "120 UAH 00 kop",
                elapsedText = "0 d. 09 hrs. 00 min. 00 sec.",
                dashboardHint = "hint",
            )
        )
    }
}
