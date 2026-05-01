package com.nitkotin.android.data.model

import kotlinx.serialization.Serializable

@Serializable
data class MotivationalPhrase(
    val text: String,
    val category: String = "",
)
