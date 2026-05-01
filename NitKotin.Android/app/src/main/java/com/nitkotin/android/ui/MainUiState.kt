package com.nitkotin.android.ui

import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.RecoveryMilestoneSnapshot
import com.nitkotin.android.data.model.SuggestedProduct
import java.time.Instant

data class MainUiState(
    val language: AppLanguage = AppLanguage.ENGLISH,
    val hasStartedTracking: Boolean = false,
    val quitDateTime: Instant = Instant.now(),
    val packsPerDay: Double = 2.0,
    val packPriceUah: Double = 160.0,
    val savedAmountText: String = "0 UAH 00 kop",
    val elapsedText: String = "",
    val firstRunMessage: String = "",
    val dashboardHint: String = "",
    val currentPhrase: String = "",
    val productUpdatedAtText: String = "",
    val productSuggestions: List<SuggestedProduct> = emptyList(),
    val recoveryMilestones: List<RecoveryMilestoneSnapshot> = emptyList(),
)
