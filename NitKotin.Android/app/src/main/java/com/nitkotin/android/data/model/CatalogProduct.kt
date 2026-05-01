package com.nitkotin.android.data.model

import kotlinx.serialization.Serializable
import kotlinx.serialization.SerialName

@Serializable
data class CatalogProduct(
    @SerialName("Id")
    val id: String,
    @SerialName("Title")
    val title: String,
    @SerialName("PriceUah")
    val priceUah: Double,
    @SerialName("Category")
    val category: String,
    @SerialName("ShortDescription")
    val shortDescription: String,
)
