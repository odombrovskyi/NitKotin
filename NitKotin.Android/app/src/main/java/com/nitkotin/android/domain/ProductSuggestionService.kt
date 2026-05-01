package com.nitkotin.android.domain

import com.nitkotin.android.data.model.CatalogProduct
import com.nitkotin.android.data.model.SuggestedProduct
import kotlin.math.max
import kotlin.random.Random

class ProductSuggestionService(
    private val catalogProducts: List<CatalogProduct>,
    private val random: Random = Random.Default,
) {
    fun getSuggestions(budgetUah: Double): List<SuggestedProduct> {
        if (catalogProducts.isEmpty()) {
            return emptyList()
        }

        val minimumPrice = catalogProducts.minOf { it.priceUah }
        val effectiveBudget = max(budgetUah, minimumPrice)

        var eligibleProducts = catalogProducts
            .filter { it.priceUah <= effectiveBudget }
            .sortedBy { it.priceUah }

        if (eligibleProducts.isEmpty()) {
            eligibleProducts = catalogProducts.take(3)
        }

        val selectedProducts = mutableListOf<CatalogProduct>()
        val usedCategories = linkedSetOf<String>()

        val topTier = eligibleProducts.filter { it.priceUah >= effectiveBudget * 0.72 }
        val middleTier = eligibleProducts.filter { it.priceUah >= effectiveBudget * 0.35 && it.priceUah < effectiveBudget * 0.72 }
        val smallTier = eligibleProducts.filter { it.priceUah < effectiveBudget * 0.35 }

        tryPickProduct(topTier, selectedProducts, usedCategories)
        tryPickProduct(middleTier, selectedProducts, usedCategories)
        tryPickProduct(smallTier, selectedProducts, usedCategories)

        while (selectedProducts.size < 3) {
            if (!tryPickProduct(eligibleProducts, selectedProducts, usedCategories)) {
                break
            }
        }

        return selectedProducts
            .distinctBy { it.id }
            .take(3)
            .map {
                SuggestedProduct(
                    title = it.title,
                    priceUah = it.priceUah,
                    category = it.category,
                    shortDescription = it.shortDescription,
                )
            }
    }

    private fun tryPickProduct(
        source: List<CatalogProduct>,
        selectedProducts: MutableList<CatalogProduct>,
        usedCategories: MutableSet<String>,
    ): Boolean {
        val usedIds = selectedProducts.map { it.id.lowercase() }.toSet()
        val freshCategoryCandidates = source.filter { it.id.lowercase() !in usedIds && it.category !in usedCategories }
        val fallbackCandidates = source.filter { it.id.lowercase() !in usedIds }
        val candidatePool = if (freshCategoryCandidates.isNotEmpty()) freshCategoryCandidates else fallbackCandidates
        if (candidatePool.isEmpty()) {
            return false
        }

        val pick = candidatePool[random.nextInt(candidatePool.size)]
        selectedProducts += pick
        usedCategories += pick.category
        return true
    }
}
