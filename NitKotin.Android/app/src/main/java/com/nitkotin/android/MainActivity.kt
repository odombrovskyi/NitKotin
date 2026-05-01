package com.nitkotin.android

import android.Manifest
import android.content.pm.PackageManager
import android.os.Bundle
import android.os.Build
import androidx.activity.ComponentActivity
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.core.content.ContextCompat
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.activity.viewModels
import com.nitkotin.android.ui.MainScreen
import com.nitkotin.android.ui.MainViewModel
import com.nitkotin.android.ui.theme.NitKotinTheme

class MainActivity : ComponentActivity() {
    private val mainViewModel: MainViewModel by viewModels()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            NitKotinTheme {
                var notificationPermissionGranted by remember {
                    mutableStateOf(
                        Build.VERSION.SDK_INT < Build.VERSION_CODES.TIRAMISU ||
                            ContextCompat.checkSelfPermission(this, Manifest.permission.POST_NOTIFICATIONS) == PackageManager.PERMISSION_GRANTED
                    )
                }
                val notificationPermissionLauncher = rememberLauncherForActivityResult(
                    contract = ActivityResultContracts.RequestPermission(),
                    onResult = { granted ->
                        notificationPermissionGranted = granted
                    },
                )
                LaunchedEffect(Unit) {
                    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU && !notificationPermissionGranted) {
                        notificationPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                    }
                }
                val state by mainViewModel.uiState.collectAsStateWithLifecycle()
                MainScreen(
                    state = state,
                    showNotificationPermissionPrompt = !notificationPermissionGranted,
                    onEnableNotifications = {
                        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
                            notificationPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
                        }
                    },
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
