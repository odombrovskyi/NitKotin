package com.nitkotin.android.ui

import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.hasSetTextAction
import androidx.compose.ui.test.hasText
import androidx.compose.ui.test.junit4.createComposeRule
import androidx.compose.ui.test.onNodeWithTag
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.performClick
import androidx.compose.ui.test.performScrollToNode
import androidx.compose.ui.test.performTextInput
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
    fun firstRunState_showsStartTrackingAndNotificationPrompt() {
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
                    showNotificationPermissionPrompt = true,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("I quit smoking!").assertIsDisplayed()
        composeRule.onNodeWithText("Enable").assertIsDisplayed()
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
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = { selectedLanguage = it },
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
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
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

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
                    showNotificationPermissionPrompt = true,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Я кинув палити!").assertIsDisplayed()
        composeRule.onNodeWithText("Увімкнути").assertIsDisplayed()
        composeRule.onNodeWithTag("main_screen_list")
            .performScrollToNode(hasText("Поточний етап"))
        composeRule.onNodeWithText("Поточний етап").assertIsDisplayed()
    }

    @Test
    fun callbacks_fire_forRefreshAndNotificationPermission() {
        var refreshCount = 0
        var permissionCount = 0

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
                    showNotificationPermissionPrompt = true,
                    onEnableNotifications = { permissionCount++ },
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = { refreshCount++ },
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithText("Enable").performClick()
        composeRule.onNodeWithTag("main_screen_list")
            .performScrollToNode(hasText("Refresh picks"))
        composeRule.onNodeWithText("Refresh picks").performClick()

        assertEquals(1, permissionCount)
        assertEquals(1, refreshCount)
    }

    @Test
    fun inputFields_acceptUpdatedValues() {
        composeRule.setContent {
            NitKotinTheme {
                MainScreen(
                    state = englishTrackedState(),
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onAllNodes(hasSetTextAction())[0].performTextInput("3")
        composeRule.onAllNodes(hasSetTextAction())[1].performTextInput("200")

        composeRule.onNodeWithText("Packs per day").assertIsDisplayed()
        composeRule.onNodeWithText("Pack price, UAH").assertIsDisplayed()
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
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
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
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

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
                    showNotificationPermissionPrompt = false,
                    onEnableNotifications = {},
                    onLanguageChanged = {},
                    onQuitDateChanged = {},
                    onPacksChanged = {},
                    onPriceChanged = {},
                    onRefreshProducts = {},
                    onStartTracking = {},
                )
            }
        }

        composeRule.onNodeWithTag("main_screen_list").performScrollToNode(hasText("Completed"))
        composeRule.onNodeWithText("Completed").assertIsDisplayed()
        composeRule.onNodeWithTag("main_screen_list").performScrollToNode(hasText("Current"))
        composeRule.onNodeWithText("Current").assertIsDisplayed()
        composeRule.onNodeWithTag("main_screen_list").performScrollToNode(hasText("Coming up"))
        composeRule.onNodeWithText("Coming up").assertIsDisplayed()
    }
}
