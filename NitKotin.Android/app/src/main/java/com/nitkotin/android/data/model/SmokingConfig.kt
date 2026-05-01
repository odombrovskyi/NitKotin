package com.nitkotin.android.data.model

import java.time.Instant

data class SmokingConfig(
    val languagePreference: AppLanguage = AppLanguage.ENGLISH,
    val hasStartedTracking: Boolean = false,
    val quitDateTime: Instant = Instant.now(),
    val packsPerDay: Double = 2.0,
    val packPriceUah: Double = 160.0,
)
