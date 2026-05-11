package com.quitmojo.android

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.runtime.getValue
import androidx.core.app.NotificationManagerCompat
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.activity.viewModels
import com.quitmojo.android.ui.MainScreen
import com.quitmojo.android.ui.MainViewModel
import com.quitmojo.android.ui.theme.QuitMojoTheme

class MainActivity : ComponentActivity() {
    private val mainViewModel: MainViewModel by viewModels()

    private companion object {
        const val LegacyProgressNotificationId = 1001
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        NotificationManagerCompat.from(this).cancel(LegacyProgressNotificationId)
        enableEdgeToEdge()
        setContent {
            QuitMojoTheme {
                val state by mainViewModel.uiState.collectAsStateWithLifecycle()
                MainScreen(
                    state = state,
                    onLanguageChanged = mainViewModel::setLanguage,
                    onSaveSettings = mainViewModel::updateSettings,
                    onRefreshProducts = mainViewModel::refreshProducts,
                    onStartTracking = mainViewModel::startTrackingNow,
                )
            }
        }
    }

    override fun onStart() {
        super.onStart()
        mainViewModel.setScreenActive(true)
    }

    override fun onStop() {
        mainViewModel.setScreenActive(false)
        super.onStop()
    }
}
