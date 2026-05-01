package com.nitkotin.android.ui

import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.hasSetTextAction
import androidx.compose.ui.test.hasText
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.onNodeWithContentDescription
import androidx.compose.ui.test.performClick
import androidx.compose.ui.test.performScrollToNode
import androidx.compose.ui.test.performTextReplacement
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.RecoveryMilestone
import com.nitkotin.android.data.model.RecoveryMilestoneSnapshot
import com.nitkotin.android.data.model.RecoveryMilestoneState
import com.nitkotin.android.data.model.SuggestedProduct
import com.nitkotin.android.ui.theme.NitKotinTheme
import org.junit.Assert.assertEquals
import org.junit.Rule
import org.junit.Test
import java.time.Duration
import java.time.Instant

class MainScreenTest {
    @get:Rule
    val composeRule = createComposeRule()

    private fun englishTrackedState(
        recoveryMilestones: List<RecoveryMilestoneSnapshot> = emptyList(),
        productSuggestions: List<SuggestedProduct> = emptyList(),
    ) = MainUiState(
        language = AppLanguage.ENGLISH,
        hasStartedTracking = true,
        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
        savedAmountText = "99 UAH 00 kop",
        elapsedText = "0 d. 10 hrs. 00 min. 00 sec.",
        dashboardHint = "hint",
        currentPhrase = "phrase",
        productUpdatedAtText = "Updated: 2026-05-01 12:00",
        recoveryMilestones = recoveryMilestones,
        productSuggestions = productSuggestions,
    )

    @Test
    fun firstRunState_showsStartTrackingAndMotivation() {
        val state = MainUiState(
            language = AppLanguage.ENGLISH,
            hasStartedTracking = false,
            quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
            savedAmountText = "0 UAH 00 kop",
            elapsedText = "Quit time has not started yet",
            firstRunMessage = "Press \"I quit smoking!\" to start tracking.",
            dashboardHint = "The dashboard updates every second and keeps inputs saved locally.",
            currentPhrase = "Whiter teeth",
            productUpdatedAtText = "Updated: 2026-05-01 12:00",
        )

        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = state,
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("I quit smoking!").assertIsDisplayed()
        composeRule.onNodeWithText("Whiter teeth").assertIsDisplayed()
    }

    @Test
    fun languageSwitchCallbacks_areInvoked() {
        var selectedLanguage = AppLanguage.ENGLISH

        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.ENGLISH,
                        hasStartedTracking = true,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "42 UAH 00 kop",
                        elapsedText = "0 d. 01 hrs. 00 min. 00 sec.",
                        dashboardHint = "hint",
                        currentPhrase = "phrase",
                        productUpdatedAtText = "Updated: 2026-05-01 12:00",
                        productSuggestions = listOf(
                            SuggestedProduct("Tea", 120.0, "Drinks", "desc")
                        ),
                        recoveryMilestones = listOf(
                            RecoveryMilestoneSnapshot(
                                milestone = RecoveryMilestone(Duration.ofMinutes(20), "20 min", "Milestone", "Desc"),
                                state = RecoveryMilestoneState.Current,
                            )
                        ),
                    ),
                    onLanguageChanged = { selectedLanguage = it },
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("UK").performClick()

        assertEquals(AppLanguage.UKRAINIAN, selectedLanguage)
    }

    @Test
    fun refreshButton_isDisplayedWithProductCard() {
        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.ENGLISH,
                        hasStartedTracking = true,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "160 UAH 00 kop",
                        elapsedText = "0 d. 12 hrs. 00 min. 00 sec.",
                        dashboardHint = "hint",
                        currentPhrase = "phrase",
                        productUpdatedAtText = "Updated: 2026-05-01 12:00",
                        productSuggestions = listOf(
                            SuggestedProduct("Ceramic mug", 150.0, "Home", "For smoke-free rituals")
                        ),
                        recoveryMilestones = emptyList(),
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Products").performClick()
        composeRule.onNodeWithText("Refresh picks").assertIsDisplayed()
        composeRule.onNodeWithText("Ceramic mug").assertIsDisplayed()
    }

    @Test
    fun ukrainianState_showsLocalizedPromptAndRecoveryState() {
        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.UKRAINIAN,
                        hasStartedTracking = false,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "0 грн 00 коп",
                        elapsedText = "Відмова ще не почалася",
                        firstRunMessage = "Натисни \"Я кинув палити!\", щоб почати відлік.",
                        dashboardHint = "Дашборд оновлюється щосекунди і зберігає зміни локально.",
                        currentPhrase = "Світліші зуби",
                        productUpdatedAtText = "Оновлено: 2026-05-01 12:00",
                        recoveryMilestones = listOf(
                            RecoveryMilestoneSnapshot(
                                milestone = RecoveryMilestone(Duration.ofMinutes(20), "20 хв", "Організм переключається", "Пульс і тиск знижуються ближче до норми."),
                                state = RecoveryMilestoneState.Current,
                            )
                        ),
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Я кинув палити!").assertIsDisplayed()
        composeRule.onNodeWithText("Здоров'я").performClick()
        composeRule.onNodeWithTag("recovery_page")
            .performScrollToNode(hasText("Поточний етап"))
        composeRule.onNodeWithText("Поточний етап").assertIsDisplayed()
    }

    @Test
    fun refreshCallback_fires() {
        var refreshCount = 0

        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.ENGLISH,
                        hasStartedTracking = true,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "99 UAH 00 kop",
                        elapsedText = "0 d. 10 hrs. 00 min. 00 sec.",
                        dashboardHint = "hint",
                        currentPhrase = "phrase",
                        productUpdatedAtText = "Updated: 2026-05-01 12:00",
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = { refreshCount++ },
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Products").performClick()
        composeRule.onNodeWithText("Refresh picks").performClick()

        assertEquals(1, refreshCount)
    }

    @Test
    fun settingsDialog_savesUpdatedValues() {
        var updatedPacks = 0.0
        var updatedPrice = 0.0

        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = englishTrackedState(),
                    onLanguageChanged = {},
                    onSaveSettings = { _, packs, price ->
                        updatedPacks = packs
                        updatedPrice = price
                    },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithContentDescription("Settings").performClick()
        composeRule.onAllNodes(hasSetTextAction())[0].performTextReplacement("3")
        composeRule.onAllNodes(hasSetTextAction())[1].performTextReplacement("200")
        composeRule.onNodeWithText("Save").performClick()

        assertEquals(3.0, updatedPacks, 0.0)
        assertEquals(200.0, updatedPrice, 0.0)
    }

    @Test
    fun startTrackingButton_invokesCallback() {
        var startTrackingCount = 0

        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.ENGLISH,
                        hasStartedTracking = false,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "0 UAH 00 kop",
                        elapsedText = "Quit time has not started yet",
                        firstRunMessage = "Press \"I quit smoking!\" to start tracking.",
                        dashboardHint = "The dashboard updates every second and keeps inputs saved locally.",
                        currentPhrase = "Whiter teeth",
                        productUpdatedAtText = "Updated: 2026-05-01 12:00",
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = { startTrackingCount++ },
                )
            }
        }

        composeRule.onNodeWithText("I quit smoking!").performClick()

        assertEquals(1, startTrackingCount)
    }

    @Test
    fun ukrainianEmptyProductsMessage_isShown() {
        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = MainUiState(
                        language = AppLanguage.UKRAINIAN,
                        hasStartedTracking = true,
                        quitDateTime = Instant.parse("2026-05-01T12:00:00Z"),
                        savedAmountText = "99 грн 00 коп",
                        elapsedText = "0 дн. 10 год. 00 хв. 00 сек.",
                        dashboardHint = "Дашборд оновлюється щосекунди і зберігає зміни локально.",
                        currentPhrase = "Краща витривалість і дихання",
                        productUpdatedAtText = "Оновлено: 2026-05-01 12:00",
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Товари").performClick()
        composeRule.onNodeWithText("Ще трохи заощаджень і з'являться нові ідеї покупок.").assertIsDisplayed()
        composeRule.onNodeWithText("Оновлено: 2026-05-01 12:00").assertIsDisplayed()
    }

    @Test
    fun recoveryStatuses_renderCompletedCurrentAndUpcomingLabels() {
        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = englishTrackedState(
                        recoveryMilestones = listOf(
                            RecoveryMilestoneSnapshot(
                                milestone = RecoveryMilestone(Duration.ofMinutes(20), "20 min", "Milestone 1", "Desc 1"),
                                state = RecoveryMilestoneState.Completed,
                            ),
                            RecoveryMilestoneSnapshot(
                                milestone = RecoveryMilestone(Duration.ofHours(8), "8 hrs", "Milestone 2", "Desc 2"),
                                state = RecoveryMilestoneState.Current,
                            ),
                            RecoveryMilestoneSnapshot(
                                milestone = RecoveryMilestone(Duration.ofDays(1), "1 day", "Milestone 3", "Desc 3"),
                                state = RecoveryMilestoneState.Upcoming,
                            ),
                        ),
                    ),
                    onLanguageChanged = {},
                    onSaveSettings = { _, _, _ -> },
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Health").performClick()
        composeRule.onNodeWithTag("recovery_page").performScrollToNode(hasText("Completed"))
        composeRule.onNodeWithText("Completed").assertIsDisplayed()
        composeRule.onNodeWithTag("recovery_page").performScrollToNode(hasText("Current"))
        composeRule.onNodeWithText("Current").assertIsDisplayed()
        composeRule.onNodeWithTag("recovery_page").performScrollToNode(hasText("Coming up"))
        composeRule.onNodeWithText("Coming up").assertIsDisplayed()
    }
}
