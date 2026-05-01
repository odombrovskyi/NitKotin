package com.nitkotin.android.data.repository

import android.content.Context
import androidx.datastore.preferences.core.booleanPreferencesKey
import androidx.datastore.preferences.core.doublePreferencesKey
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.core.stringPreferencesKey
import androidx.datastore.preferences.preferencesDataStore
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.SmokingConfig
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map
import java.time.Instant

private val Context.dataStore by preferencesDataStore(name = "nitkotin_prefs")

class PreferencesRepository(private val context: Context) {
    private object Keys {
        val quitDateTime = stringPreferencesKey("quit_date_time")
        val packsPerDay = doublePreferencesKey("packs_per_day")
        val packPriceUah = doublePreferencesKey("pack_price_uah")
        val languagePreference = stringPreferencesKey("language_preference")
        val hasStartedTracking = booleanPreferencesKey("has_started_tracking")
    }

    val config: Flow<SmokingConfig> = context.dataStore.data.map { preferences ->
        SmokingConfig(
            languagePreference = AppLanguage.fromCode(preferences[Keys.languagePreference]),
            hasStartedTracking = preferences[Keys.hasStartedTracking] ?: false,
            quitDateTime = preferences[Keys.quitDateTime]?.let(Instant::parse) ?: Instant.now(),
            packsPerDay = preferences[Keys.packsPerDay] ?: 2.0,
            packPriceUah = preferences[Keys.packPriceUah] ?: 160.0,
        )
    }

    suspend fun setLanguage(language: AppLanguage) {
        context.dataStore.edit { it[Keys.languagePreference] = language.code }
    }

    suspend fun updateInputs(quitDateTime: Instant, packsPerDay: Double, packPriceUah: Double) {
        context.dataStore.edit {
            it[Keys.quitDateTime] = quitDateTime.toString()
            it[Keys.packsPerDay] = packsPerDay
            it[Keys.packPriceUah] = packPriceUah
        }
    }

    suspend fun setQuitDateTime(quitDateTime: Instant) {
        context.dataStore.edit {
            it[Keys.quitDateTime] = quitDateTime.toString()
        }
    }

    suspend fun startTrackingNow(now: Instant) {
        context.dataStore.edit {
            it[Keys.hasStartedTracking] = true
            it[Keys.quitDateTime] = now.toString()
        }
    }
}
