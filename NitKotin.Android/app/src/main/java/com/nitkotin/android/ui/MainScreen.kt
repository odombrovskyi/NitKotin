package com.nitkotin.android.ui

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.WindowInsets
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.rounded.AutoAwesome
import androidx.compose.material.icons.rounded.CalendarMonth
import androidx.compose.material.icons.rounded.Sell
import androidx.compose.material.icons.rounded.CheckCircle
import androidx.compose.material.icons.rounded.HourglassTop
import androidx.compose.material.icons.rounded.Refresh
import androidx.compose.material.icons.rounded.Settings
import androidx.compose.material.icons.rounded.Savings
import androidx.compose.material.icons.rounded.Timelapse
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.AssistChip
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.FilterChip
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.nitkotin.android.R
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.RecoveryMilestoneSnapshot
import com.nitkotin.android.data.model.RecoveryMilestoneState
import com.nitkotin.android.data.model.SuggestedProduct
import com.nitkotin.android.domain.LocalizationService
import kotlinx.coroutines.launch
import java.time.Instant
import java.time.ZoneId
import java.time.format.DateTimeFormatter

private enum class MainTab(val index: Int) {
    Products(0),
    Overview(1),
    Recovery(2),
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainScreen(
    state: MainUiState,
    showNotificationPermissionPrompt: Boolean,
    onEnableNotifications: () -> Unit,
    onLanguageChanged: (AppLanguage) -> Unit,
    onSaveSettings: (Instant, Double, Double) -> Unit,
    onRefreshProducts: () -> Unit,
    onStartTracking: () -> Unit,
) {
    val language = state.language
    val pagerState = rememberPagerState(initialPage = MainTab.Overview.index) { MainTab.entries.size }
    val coroutineScope = rememberCoroutineScope()
    var isSettingsOpen by rememberSaveable { mutableStateOf(false) }

    if (isSettingsOpen) {
        SettingsDialog(
            state = state,
            onDismiss = { isSettingsOpen = false },
            onSave = { quitDateTime, packsPerDay, packPrice ->
                onSaveSettings(quitDateTime, packsPerDay, packPrice)
                isSettingsOpen = false
            },
        )
    }

    Scaffold(
        contentWindowInsets = WindowInsets(0, 0, 0, 0),
        topBar = {
            TopAppBar(
                title = { Text(LocalizationService.getString(language, "app_title")) },
                actions = {
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(8.dp),
                    ) {
                        FilterChip(
                            selected = language == AppLanguage.ENGLISH,
                            onClick = { onLanguageChanged(AppLanguage.ENGLISH) },
                            label = { Text("EN") },
                            leadingIcon = {
                                Image(
                                    painter = painterResource(id = R.drawable.flag_us),
                                    contentDescription = LocalizationService.getString(language, "language_en"),
                                    modifier = Modifier.size(18.dp),
                                )
                            },
                        )
                        FilterChip(
                            selected = language == AppLanguage.UKRAINIAN,
                            onClick = { onLanguageChanged(AppLanguage.UKRAINIAN) },
                            label = { Text("UK") },
                            leadingIcon = {
                                Image(
                                    painter = painterResource(id = R.drawable.flag_ua),
                                    contentDescription = LocalizationService.getString(language, "language_uk"),
                                    modifier = Modifier.size(18.dp),
                                )
                            },
                        )
                        IconButton(onClick = { isSettingsOpen = true }) {
                            Icon(
                                imageVector = Icons.Rounded.Settings,
                                contentDescription = LocalizationService.getString(language, "settings_title"),
                            )
                        }
                    }
                },
            )
        },
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues),
        ) {
            TabRow(selectedTabIndex = pagerState.currentPage) {
                MainTab.entries.forEach { tab ->
                    Tab(
                        selected = pagerState.currentPage == tab.index,
                        onClick = {
                            coroutineScope.launch {
                                pagerState.animateScrollToPage(tab.index)
                            }
                        },
                        text = {
                            Text(
                                when (tab) {
                                    MainTab.Products -> LocalizationService.getString(language, "tab_products")
                                    MainTab.Overview -> LocalizationService.getString(language, "tab_overview")
                                    MainTab.Recovery -> LocalizationService.getString(language, "tab_recovery")
                                },
                            )
                        },
                    )
                }
            }
            HorizontalPager(
                state = pagerState,
                modifier = Modifier
                    .fillMaxSize()
                    .testTag("main_screen_pager"),
            ) { page ->
                when (page) {
                    MainTab.Products.index -> ProductsPage(
                        language = language,
                        productUpdatedAtText = state.productUpdatedAtText,
                        productSuggestions = state.productSuggestions,
                        onRefreshProducts = onRefreshProducts,
                    )

                    MainTab.Overview.index -> OverviewPage(
                        state = state,
                        showNotificationPermissionPrompt = showNotificationPermissionPrompt,
                        onEnableNotifications = onEnableNotifications,
                        onStartTracking = onStartTracking,
                    )

                    MainTab.Recovery.index -> RecoveryPage(
                        language = language,
                        recoveryMilestones = state.recoveryMilestones,
                    )
                }
            }
        }
    }
}

@Composable
private fun OverviewPage(
    state: MainUiState,
    showNotificationPermissionPrompt: Boolean,
    onEnableNotifications: () -> Unit,
    onStartTracking: () -> Unit,
) {
    val language = state.language
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .testTag("overview_page")
            .padding(16.dp),
        contentPadding = PaddingValues(bottom = 24.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp),
    ) {
        if (showNotificationPermissionPrompt) {
            item {
                Card(modifier = Modifier.fillMaxWidth()) {
                    Row(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp),
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.SpaceBetween,
                    ) {
                        Column(modifier = Modifier.weight(1f)) {
                            Text(LocalizationService.getString(language, "notification_title"), style = MaterialTheme.typography.titleMedium)
                            Text(LocalizationService.getString(language, "notification_channel_description"), style = MaterialTheme.typography.bodyMedium)
                        }
                        Button(onClick = onEnableNotifications) {
                            Text(LocalizationService.getString(language, "enable_notifications"))
                        }
                    }
                }
            }
        }
        if (!state.hasStartedTracking) {
            item {
                Card(modifier = Modifier.fillMaxWidth()) {
                    Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                        Text(state.firstRunMessage, style = MaterialTheme.typography.bodyLarge)
                        Button(onClick = onStartTracking) {
                            Text(LocalizationService.getString(language, "quit_now"))
                        }
                    }
                }
            }
        }
        item {
            InfoCard(
                title = LocalizationService.getString(language, "motivation_header"),
                body = state.currentPhrase,
                footer = LocalizationService.getString(language, "motivation_rotating"),
                icon = Icons.Rounded.AutoAwesome,
                containerColor = MaterialTheme.colorScheme.tertiaryContainer,
                bodyStyle = MaterialTheme.typography.headlineSmall,
            )
        }
        item {
            InfoCard(
                title = LocalizationService.getString(language, "saved_caption"),
                body = state.savedAmountText,
                icon = Icons.Rounded.Savings,
                containerColor = MaterialTheme.colorScheme.primaryContainer,
                bodyStyle = MaterialTheme.typography.displaySmall,
            )
        }
        item {
            InfoCard(
                title = LocalizationService.getString(language, "elapsed_caption"),
                body = state.elapsedText,
                footer = state.dashboardHint,
                icon = Icons.Rounded.Timelapse,
                containerColor = MaterialTheme.colorScheme.secondaryContainer,
                bodyStyle = MaterialTheme.typography.displaySmall,
            )
        }
    }
}

@Composable
private fun ProductsPage(
    language: AppLanguage,
    productUpdatedAtText: String,
    productSuggestions: List<SuggestedProduct>,
    onRefreshProducts: () -> Unit,
) {
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .testTag("products_page")
            .padding(16.dp),
        contentPadding = PaddingValues(bottom = 24.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp),
    ) {
        item {
            Card(
                modifier = Modifier.fillMaxWidth(),
                colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.secondaryContainer),
            ) {
                Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.SpaceBetween,
                    ) {
                        Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                            Icon(Icons.Rounded.Sell, contentDescription = null)
                            Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
                                Text(LocalizationService.getString(language, "products_header"), style = MaterialTheme.typography.titleLarge)
                                Text(productUpdatedAtText, style = MaterialTheme.typography.labelMedium)
                            }
                        }
                        AssistChip(
                            onClick = {},
                            enabled = false,
                            label = { Text(productSuggestions.size.toString()) },
                            leadingIcon = {
                                Icon(Icons.Rounded.AutoAwesome, contentDescription = null)
                            },
                        )
                    }
                    Button(onClick = onRefreshProducts, modifier = Modifier.align(Alignment.Start)) {
                        Icon(Icons.Rounded.Refresh, contentDescription = null)
                        Text(LocalizationService.getString(language, "products_refresh"), modifier = Modifier.padding(start = 8.dp))
                    }
                }
            }
        }
        if (productSuggestions.isEmpty()) {
            item {
                Card(modifier = Modifier.fillMaxWidth()) {
                    Text(
                        LocalizationService.getString(language, "products_empty"),
                        modifier = Modifier.padding(16.dp),
                    )
                }
            }
        } else {
            itemsIndexed(productSuggestions) { index, product ->
                ProductSuggestionCard(
                    product = product,
                    language = language,
                    modifier = Modifier.fillMaxWidth(),
                    index = index,
                )
            }
        }
    }
}

@Composable
private fun RecoveryPage(
    language: AppLanguage,
    recoveryMilestones: List<RecoveryMilestoneSnapshot>,
) {
    LazyColumn(
        modifier = Modifier
            .fillMaxSize()
            .testTag("recovery_page")
            .padding(16.dp),
        contentPadding = PaddingValues(bottom = 24.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp),
    ) {
        item {
            Text(LocalizationService.getString(language, "recovery_header"), style = MaterialTheme.typography.titleMedium)
        }
        items(recoveryMilestones) { milestone ->
            val milestoneColors = milestoneColors(milestone.state)
            Card(
                modifier = Modifier.fillMaxWidth(),
                colors = CardDefaults.cardColors(containerColor = milestoneColors.containerColor),
            ) {
                Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
                    AssistChip(
                        onClick = {},
                        enabled = false,
                        label = {
                            Text(
                                when (milestone.state) {
                                    RecoveryMilestoneState.Completed -> LocalizationService.getString(language, "recovery_completed")
                                    RecoveryMilestoneState.Current -> LocalizationService.getString(language, "recovery_current")
                                    RecoveryMilestoneState.Upcoming -> LocalizationService.getString(language, "recovery_upcoming")
                                }
                            )
                        },
                        leadingIcon = {
                            Icon(
                                imageVector = when (milestone.state) {
                                    RecoveryMilestoneState.Completed -> Icons.Rounded.CheckCircle
                                    RecoveryMilestoneState.Current -> Icons.Rounded.Timelapse
                                    RecoveryMilestoneState.Upcoming -> Icons.Rounded.HourglassTop
                                },
                                contentDescription = null,
                            )
                        },
                    )
                    Text(milestone.milestone.timeframeLabel, style = MaterialTheme.typography.labelLarge)
                    Text(milestone.milestone.title, style = MaterialTheme.typography.titleMedium)
                    Text(milestone.milestone.description, style = MaterialTheme.typography.bodyMedium)
                }
            }
        }
    }
}

@Composable
private fun InfoCard(
    title: String,
    body: String,
    footer: String? = null,
    icon: androidx.compose.ui.graphics.vector.ImageVector,
    containerColor: Color,
    bodyStyle: androidx.compose.ui.text.TextStyle,
) {
    Card(
        modifier = Modifier.fillMaxWidth(),
        colors = CardDefaults.cardColors(containerColor = containerColor),
    ) {
        Column(modifier = Modifier.padding(18.dp), verticalArrangement = Arrangement.spacedBy(10.dp)) {
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                Icon(imageVector = icon, contentDescription = null)
                Text(title, style = MaterialTheme.typography.titleMedium)
            }
            Text(body, style = bodyStyle, fontWeight = FontWeight.SemiBold)
            if (!footer.isNullOrBlank()) {
                Text(footer, style = MaterialTheme.typography.labelMedium, textAlign = TextAlign.Start)
            }
        }
    }
}

private data class MilestoneColors(
    val containerColor: Color,
)

@Composable
private fun milestoneColors(state: RecoveryMilestoneState): MilestoneColors {
    return when (state) {
        RecoveryMilestoneState.Completed -> MilestoneColors(MaterialTheme.colorScheme.primaryContainer)
        RecoveryMilestoneState.Current -> MilestoneColors(MaterialTheme.colorScheme.tertiaryContainer)
        RecoveryMilestoneState.Upcoming -> MilestoneColors(MaterialTheme.colorScheme.surfaceVariant)
    }
}

@Composable
private fun ProductSuggestionCard(
    product: SuggestedProduct,
    language: AppLanguage,
    modifier: Modifier = Modifier,
    index: Int = 0,
) {
    val containerColor = when (index % 3) {
        0 -> MaterialTheme.colorScheme.primaryContainer
        1 -> MaterialTheme.colorScheme.tertiaryContainer
        else -> MaterialTheme.colorScheme.surfaceVariant
    }

    Card(
        modifier = modifier,
        colors = CardDefaults.cardColors(containerColor = containerColor),
    ) {
        Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(10.dp)) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.Top,
            ) {
                Column(
                    modifier = Modifier.weight(1f),
                    verticalArrangement = Arrangement.spacedBy(4.dp),
                ) {
                    Text(product.title, style = MaterialTheme.typography.titleLarge, fontWeight = FontWeight.Bold)
                    Text(product.shortDescription, style = MaterialTheme.typography.bodyMedium)
                }
                AssistChip(
                    onClick = {},
                    enabled = false,
                    label = { Text("#${index + 1}") },
                    leadingIcon = {
                        Icon(Icons.Rounded.AutoAwesome, contentDescription = null)
                    },
                )
            }

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically,
            ) {
                AssistChip(
                    onClick = {},
                    enabled = false,
                    label = { Text(product.category) },
                )
                Text(
                    text = "${product.priceUah.toInt()} ${LocalizationService.getString(language, "currency_major")}",
                    style = MaterialTheme.typography.headlineSmall,
                    fontWeight = FontWeight.ExtraBold,
                )
            }
        }
    }
}

@Composable
private fun SettingsDialog(
    state: MainUiState,
    onDismiss: () -> Unit,
    onSave: (Instant, Double, Double) -> Unit,
) {
    val context = LocalContext.current
    val language = state.language
    val formatter = remember {
        DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm").withZone(ZoneId.systemDefault())
    }
    var tempQuitDateTime by remember(state.quitDateTime) { mutableStateOf(state.quitDateTime) }
    var packsInput by remember(state.packsPerDay) { mutableStateOf(state.packsPerDay.toString()) }
    var priceInput by remember(state.packPriceUah) { mutableStateOf(state.packPriceUah.toString()) }

    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text(LocalizationService.getString(language, "settings_title")) },
        text = {
            Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {
                Button(
                    onClick = {
                        showDateTimePicker(context, tempQuitDateTime) {
                            tempQuitDateTime = it
                        }
                    },
                    modifier = Modifier.align(Alignment.Start),
                ) {
                    Icon(Icons.Rounded.CalendarMonth, contentDescription = null)
                    Text(formatter.format(tempQuitDateTime), modifier = Modifier.padding(start = 8.dp))
                }
                OutlinedTextField(
                    value = packsInput,
                    onValueChange = { packsInput = it },
                    label = { Text(LocalizationService.getString(language, "packs_per_day")) },
                )
                OutlinedTextField(
                    value = priceInput,
                    onValueChange = { priceInput = it },
                    label = { Text(LocalizationService.getString(language, "pack_price")) },
                )
            }
        },
        confirmButton = {
            TextButton(
                onClick = {
                    val packs = packsInput.toDoubleOrNull() ?: state.packsPerDay
                    val price = priceInput.toDoubleOrNull() ?: state.packPriceUah
                    onSave(tempQuitDateTime, packs, price)
                },
            ) {
                Text(LocalizationService.getString(language, "settings_save"))
            }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) {
                Text(LocalizationService.getString(language, "settings_cancel"))
            }
        },
    )
}

private fun showDateTimePicker(
    context: android.content.Context,
    currentValue: Instant,
    onQuitDateChanged: (Instant) -> Unit,
) {
    val zonedDateTime = currentValue.atZone(ZoneId.systemDefault())
    DatePickerDialog(
        context,
        { _, year, month, dayOfMonth ->
            TimePickerDialog(
                context,
                { _, hourOfDay, minute ->
                    val updated = zonedDateTime
                        .withYear(year)
                        .withMonth(month + 1)
                        .withDayOfMonth(dayOfMonth)
                        .withHour(hourOfDay)
                        .withMinute(minute)
                        .withSecond(0)
                        .withNano(0)
                    onQuitDateChanged(updated.toInstant())
                },
                zonedDateTime.hour,
                zonedDateTime.minute,
                true,
            ).show()
        },
        zonedDateTime.year,
        zonedDateTime.monthValue - 1,
        zonedDateTime.dayOfMonth,
    ).show()
}
