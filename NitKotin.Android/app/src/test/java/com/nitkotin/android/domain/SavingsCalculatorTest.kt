package com.nitkotin.android.domain

import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.SmokingConfig
import org.junit.Assert.assertEquals
import org.junit.Test
import java.time.Instant

class SavingsCalculatorTest {
    @Test
    fun calculateSavedAmount_returnsZero_whenTrackingNotStarted() {
        val config = SmokingConfig(hasStartedTracking = false)

        val result = SavingsCalculator.calculateSavedAmount(config, Instant.parse("2026-05-01T12:00:00Z"))

        assertEquals("0.00", result.toPlainString())
    }

    @Test
    fun calculateSavedAmount_matchesExpectedRoundedValue() {
        val config = SmokingConfig(
            languagePreference = AppLanguage.ENGLISH,
            hasStartedTracking = true,
            quitDateTime = Instant.parse("2026-05-01T00:00:00Z"),
            packsPerDay = 2.0,
            packPriceUah = 160.0,
        )

        val result = SavingsCalculator.calculateSavedAmount(config, Instant.parse("2026-05-01T12:00:00Z"))

        assertEquals("160.00", result.toPlainString())
    }

    @Test
    fun formatElapsed_returnsLocalizedNotStarted_whenQuitTimeNotReached() {
        val config = SmokingConfig(
            languagePreference = AppLanguage.UKRAINIAN,
            hasStartedTracking = true,
            quitDateTime = Instant.parse("2026-05-02T00:00:00Z"),
        )

        val result = SavingsCalculator.formatElapsed(config, Instant.parse("2026-05-01T12:00:00Z"))

        assertEquals("Відмова ще не почалася", result)
    }
}