package com.nitkotin.android.domain

import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.MotivationalPhrase
import com.nitkotin.android.data.model.RecoveryMilestone
import java.time.Duration

object LocalizationService {
    private val englishStrings = mapOf(
        "app_title" to "No! kitten motivator",
        "quit_now" to "I quit smoking!",
        "quit_date" to "Quit date and time",
        "change_quit_date" to "Change quit date and time",
        "packs_per_day" to "Packs per day",
        "pack_price" to "Pack price, UAH",
        "saved_caption" to "Saved on cigarettes",
        "elapsed_caption" to "Smoke-free",
        "products_header" to "What you can already buy",
        "products_refresh" to "Refresh picks",
        "products_updated" to "Updated: %s",
        "recovery_header" to "Recovery timeline",
        "motivation_header" to "Motivation",
        "motivation_rotating" to "Rotates automatically",
        "enable_notifications" to "Enable",
        "language_en" to "English",
        "language_uk" to "Українська",
        "currency_major" to "UAH",
        "currency_minor" to "kop",
        "elapsed_days" to "d.",
        "elapsed_hours" to "hrs.",
        "elapsed_minutes" to "min.",
        "elapsed_seconds" to "sec.",
        "elapsed_not_started" to "Quit time has not started yet",
        "first_run_message" to "Press \"I quit smoking!\" to start tracking.",
        "products_empty" to "A little more savings and product ideas will appear.",
        "dashboard_hint" to "The dashboard updates every second and keeps inputs saved locally.",
        "notification_channel_name" to "NitKotin progress",
        "notification_channel_description" to "Shows your current smoke-free progress and savings.",
        "notification_title" to "NitKotin progress",
        "widget_title" to "NitKotin",
        "recovery_completed" to "Completed",
        "recovery_current" to "Current",
        "recovery_upcoming" to "Coming up"
    )

    private val ukrainianStrings = mapOf(
        "app_title" to "Ні! котину мотиватор",
        "quit_now" to "Я кинув палити!",
        "quit_date" to "Дата й час відмови від куріння",
        "change_quit_date" to "Змінити дату й час відмови",
        "packs_per_day" to "Пачок на день",
        "pack_price" to "Ціна однієї пачки, грн",
        "saved_caption" to "Заощаджено на сигаретах",
        "elapsed_caption" to "Без куріння",
        "products_header" to "Що вже можна купити",
        "products_refresh" to "Оновити добірку",
        "products_updated" to "Оновлено: %s",
        "recovery_header" to "Таймлайн відновлення",
        "motivation_header" to "Мотивація",
        "motivation_rotating" to "Фрази обертаються автоматично",
        "enable_notifications" to "Увімкнути",
        "language_en" to "English",
        "language_uk" to "Українська",
        "currency_major" to "грн",
        "currency_minor" to "коп",
        "elapsed_days" to "дн.",
        "elapsed_hours" to "год.",
        "elapsed_minutes" to "хв.",
        "elapsed_seconds" to "сек.",
        "elapsed_not_started" to "Відмова ще не почалася",
        "first_run_message" to "Натисни \"Я кинув палити!\", щоб почати відлік.",
        "products_empty" to "Ще трохи заощаджень і з'являться нові ідеї покупок.",
        "dashboard_hint" to "Дашборд оновлюється щосекунди і зберігає зміни локально.",
        "notification_channel_name" to "Прогрес NitKotin",
        "notification_channel_description" to "Показує поточний прогрес без куріння та заощадження.",
        "notification_title" to "Прогрес NitKotin",
        "widget_title" to "NitKotin",
        "recovery_completed" to "Вже пройдено",
        "recovery_current" to "Поточний етап",
        "recovery_upcoming" to "Попереду"
    )

    private val englishFallbackPhrases = listOf(
        MotivationalPhrase("Whiter teeth", "Health"),
        MotivationalPhrase("Better stamina and breathing", "Health"),
        MotivationalPhrase("More money left over", "Finance"),
        MotivationalPhrase("More energy for the day", "Energy")
    )

    private val ukrainianFallbackPhrases = listOf(
        MotivationalPhrase("Світліші зуби", "Здоров'я"),
        MotivationalPhrase("Краща витривалість і дихання", "Здоров'я"),
        MotivationalPhrase("Заощадження коштів", "Фінанси"),
        MotivationalPhrase("Більше енергії на день", "Енергія")
    )

    private val englishMilestones = listOf(
        RecoveryMilestone(Duration.ofMinutes(20), "20 min", "Your body is switching gears", "Pulse and blood pressure move closer to normal."),
        RecoveryMilestone(Duration.ofHours(8), "8-12 hrs", "Your blood gets more oxygen", "Carbon monoxide drops noticeably."),
        RecoveryMilestone(Duration.ofDays(1), "1 day", "First day without smoke", "Your body clears remaining carbon monoxide."),
        RecoveryMilestone(Duration.ofDays(30), "1 month", "Lungs clear the damage", "Breathing often feels easier and cleaner."),
        RecoveryMilestone(Duration.ofDays(365), "1 year", "Cardiovascular risk drops", "Heart disease risk is about half that of a smoker."),
    )

    private val ukrainianMilestones = listOf(
        RecoveryMilestone(Duration.ofMinutes(20), "20 хв", "Організм переключається", "Пульс і тиск знижуються ближче до норми."),
        RecoveryMilestone(Duration.ofHours(8), "8–12 год", "Кров отримує більше кисню", "Рівень чадного газу помітно падає."),
        RecoveryMilestone(Duration.ofDays(1), "1 доба", "Перша доба без диму", "Організм виводить залишки чадного газу."),
        RecoveryMilestone(Duration.ofDays(30), "1 місяць", "Легені прибирають наслідки диму", "Кашель і важкість у грудях часто слабшають."),
        RecoveryMilestone(Duration.ofDays(365), "1 рік", "Серцевий ризик уже нижчий", "Ризик ішемічної хвороби серця приблизно вдвічі нижчий."),
    )

    fun getString(language: AppLanguage, key: String): String {
        val dictionary = if (language == AppLanguage.UKRAINIAN) ukrainianStrings else englishStrings
        return dictionary[key] ?: englishStrings[key] ?: key
    }

    fun format(language: AppLanguage, key: String, vararg args: Any): String {
        return getString(language, key).format(*args)
    }

    fun fallbackPhrases(language: AppLanguage): List<MotivationalPhrase> {
        return if (language == AppLanguage.UKRAINIAN) ukrainianFallbackPhrases else englishFallbackPhrases
    }

    fun phrasesAssetName(language: AppLanguage): String {
        return if (language == AppLanguage.UKRAINIAN) "data/motivational-phrases.ua.json" else "data/motivational-phrases.en.json"
    }

    fun catalogAssetName(language: AppLanguage): String {
        return if (language == AppLanguage.UKRAINIAN) "data/product-catalog.ua.json" else "data/product-catalog.en.json"
    }

    fun recoveryMilestones(language: AppLanguage): List<RecoveryMilestone> {
        return if (language == AppLanguage.UKRAINIAN) ukrainianMilestones else englishMilestones
    }
}
