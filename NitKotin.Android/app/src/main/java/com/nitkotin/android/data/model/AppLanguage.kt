package com.nitkotin.android.data.model

enum class AppLanguage(val code: String) {
    ENGLISH("en"),
    UKRAINIAN("uk");

    companion object {
        fun fromCode(code: String?): AppLanguage {
            return entries.firstOrNull { it.code.equals(code, ignoreCase = true) } ?: ENGLISH
        }
    }
}
