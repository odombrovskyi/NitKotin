package com.nitkotin.android.domain

import com.nitkotin.android.data.model.SmokingConfig
import java.math.BigDecimal
import java.math.RoundingMode
import java.time.Duration
import java.time.Instant

object SavingsCalculator {
    private val secondsPerDay = BigDecimal(24 * 60 * 60)

    fun calculateSavedAmount(config: SmokingConfig, currentTime: Instant): BigDecimal {
        if (config.packsPerDay <= 0.0 || config.packPriceUah <= 0.0 || !config.hasStartedTracking || !config.quitDateTime.isBefore(currentTime)) {
            return BigDecimal.ZERO.setScale(2, RoundingMode.HALF_UP)
        }

        val elapsed = Duration.between(config.quitDateTime, currentTime)
        val dailySpend = BigDecimal.valueOf(config.packsPerDay).multiply(BigDecimal.valueOf(config.packPriceUah))
        val spendPerSecond = dailySpend.divide(secondsPerDay, 12, RoundingMode.HALF_UP)
        val savedAmount = spendPerSecond.multiply(BigDecimal.valueOf(elapsed.seconds).add(BigDecimal.valueOf(elapsed.nano.toLong(), 9)))
        return savedAmount.setScale(2, RoundingMode.HALF_UP)
    }

    fun formatCurrency(amount: BigDecimal, language: com.nitkotin.android.data.model.AppLanguage): String {
        val normalized = amount.setScale(2, RoundingMode.HALF_UP)
        val major = normalized.setScale(0, RoundingMode.DOWN)
        val minor = normalized.subtract(major).movePointRight(2).setScale(0, RoundingMode.HALF_UP)
        val fixedMajor = if (minor == BigDecimal(100)) major + BigDecimal.ONE else major
        val fixedMinor = if (minor == BigDecimal(100)) BigDecimal.ZERO else minor
        return "%s %s %02d %s".format(
            fixedMajor.toPlainString(),
            LocalizationService.getString(language, "currency_major"),
            fixedMinor.toInt(),
            LocalizationService.getString(language, "currency_minor")
        )
    }

    fun formatWholeCurrency(amount: BigDecimal, language: com.nitkotin.android.data.model.AppLanguage): String {
        val normalized = amount.setScale(0, RoundingMode.DOWN)
        return "%s %s".format(
            normalized.toPlainString(),
            LocalizationService.getString(language, "currency_major")
        )
    }

    fun formatElapsed(config: SmokingConfig, currentTime: Instant): String {
        val language = config.languagePreference
        if (!config.hasStartedTracking || !config.quitDateTime.isBefore(currentTime)) {
            return LocalizationService.getString(language, "elapsed_not_started")
        }

        val elapsed = Duration.between(config.quitDateTime, currentTime)
        val totalMinutes = elapsed.toMinutes()
        val days = totalMinutes / (24 * 60)
        val hours = (totalMinutes % (24 * 60)) / 60
        val minutes = totalMinutes % 60
        return "%d %s %02d %s %02d %s".format(
            days,
            LocalizationService.getString(language, "elapsed_days"),
            hours,
            LocalizationService.getString(language, "elapsed_hours"),
            minutes,
            LocalizationService.getString(language, "elapsed_minutes")
        )
    }

    fun formatElapsedDaysHours(config: SmokingConfig, currentTime: Instant): String {
        val language = config.languagePreference
        if (!config.hasStartedTracking || !config.quitDateTime.isBefore(currentTime)) {
            return LocalizationService.getString(language, "elapsed_not_started")
        }

        val elapsed = Duration.between(config.quitDateTime, currentTime)
        val totalHours = elapsed.toHours()
        val days = totalHours / 24
        val hours = totalHours % 24

        return "%d %s %02d %s".format(
            days,
            LocalizationService.getString(language, "elapsed_days"),
            hours,
            LocalizationService.getString(language, "elapsed_hours")
        )
    }
}
