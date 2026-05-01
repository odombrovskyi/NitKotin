package com.nitkotin.android.data.model

import kotlinx.serialization.Serializable
import kotlinx.serialization.SerialName

@Serializable
data class MotivationalPhrase(
    @SerialName("Text")
    val text: String,
    @SerialName("Category")
    val category: String = "",
)
