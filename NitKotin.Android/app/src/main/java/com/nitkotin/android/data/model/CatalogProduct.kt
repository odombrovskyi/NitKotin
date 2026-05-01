package com.nitkotin.android.data.model

import kotlinx.serialization.Serializable

@Serializable
data class CatalogProduct(
    val id: String,
    val title: String,
    val priceUah: Double,
    val category: String,
    val shortDescription: String,
)
