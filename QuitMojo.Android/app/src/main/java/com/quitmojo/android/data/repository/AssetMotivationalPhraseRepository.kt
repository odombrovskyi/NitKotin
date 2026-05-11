package com.quitmojo.android.data.repository

import android.content.Context
import com.quitmojo.android.data.model.AppLanguage
import com.quitmojo.android.data.model.MotivationalPhrase
import com.quitmojo.android.domain.LocalizationService
import kotlinx.serialization.json.Json

class AssetMotivationalPhraseRepository(private val context: Context) {
    private val json = Json { ignoreUnknownKeys = true }

    suspend fun load(language: AppLanguage): List<MotivationalPhrase> {
        return runCatching {
            val assetName = LocalizationService.phrasesAssetName(language)
            context.assets.open(assetName).bufferedReader().use { reader ->
                val loaded = json.decodeFromString<List<MotivationalPhrase>>(reader.readText())
                loaded.filter { it.text.isNotBlank() }
            }
        }.getOrElse {
            LocalizationService.fallbackPhrases(language)
        }.ifEmpty {
            LocalizationService.fallbackPhrases(language)
        }
    }
}
