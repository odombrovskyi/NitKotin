package com.nitkotin.android.ui.theme

import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable

private val LightColors = lightColorScheme(
    primary = Slate900,
    secondary = Slate700,
    tertiary = Amber500,
    surface = SurfaceLight,
)

private val DarkColors = darkColorScheme(
    primary = SurfaceLight,
    secondary = Amber500,
    tertiary = Mint500,
)

@Composable
fun NitKotinTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = LightColors,
        typography = Typography,
        content = content,
    )
}
