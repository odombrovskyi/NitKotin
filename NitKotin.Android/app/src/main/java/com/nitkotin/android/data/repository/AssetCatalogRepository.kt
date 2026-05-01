package com.nitkotin.android.data.repository

import android.content.Context
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.CatalogProduct
import com.nitkotin.android.domain.LocalizationService
import kotlinx.serialization.json.Json

class AssetCatalogRepository(private val context: Context) {
    private val json = Json { ignoreUnknownKeys = true }

    suspend fun load(language: AppLanguage): List<CatalogProduct> {
        return runCatching {
            val assetName = LocalizationService.catalogAssetName(language)
            context.assets.open(assetName).bufferedReader().use { reader ->
                json.decodeFromString<List<CatalogProduct>>(reader.readText())
            }
        }.getOrDefault(emptyList())
    }
}
