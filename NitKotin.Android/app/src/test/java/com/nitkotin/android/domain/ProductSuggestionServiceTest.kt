package com.nitkotin.android.domain

import com.nitkotin.android.data.model.CatalogProduct
import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import kotlin.random.Random

class ProductSuggestionServiceTest {
    @Test
    fun getSuggestions_returnsThreeDistinctProducts_whenCatalogAllowsIt() {
        val products = listOf(
            CatalogProduct("a", "A", 100.0, "Books", "A"),
            CatalogProduct("b", "B", 200.0, "Sport", "B"),
            CatalogProduct("c", "C", 300.0, "Home", "C"),
            CatalogProduct("d", "D", 320.0, "Home", "D"),
        )

        val result = ProductSuggestionService(products, Random(42)).getSuggestions(350.0)

        assertEquals(3, result.size)
        assertEquals(3, result.map { it.title }.distinct().size)
    }

    @Test
    fun getSuggestions_usesMinimumPrice_whenBudgetTooLow() {
        val products = listOf(
            CatalogProduct("a", "A", 100.0, "Books", "A"),
            CatalogProduct("b", "B", 200.0, "Sport", "B"),
        )

        val result = ProductSuggestionService(products, Random(1)).getSuggestions(1.0)

        assertTrue(result.isNotEmpty())
        assertEquals("A", result.first().title)
    }
}