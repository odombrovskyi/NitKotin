package com.nitkotin.android.ui

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.ExperimentalLayoutApi
import androidx.compose.foundation.layout.FlowRow
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.WindowInsets
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.FilterChip
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.rounded.CalendarMonth
import androidx.compose.material.icons.rounded.Refresh
import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.platform.LocalConfiguration
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.nitkotin.android.R
import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.domain.LocalizationService
import java.time.Instant
import java.time.ZoneId
import java.time.format.DateTimeFormatter

@OptIn(ExperimentalLayoutApi::class, ExperimentalMaterial3Api::class)
@Composable
fun MainScreen(
    state: MainUiState,
    showNotificationPermissionPrompt: Boolean,
    onEnableNotifications: () -> Unit,
    onLanguageChanged: (AppLanguage) -> Unit,
    onQuitDateChanged: (Instant) -> Unit,
    onPacksChanged: (Double) -> Unit,
    onPriceChanged: (Double) -> Unit,
    onRefreshProducts: () -> Unit,
    onStartTracking: () -> Unit,
) {
    val language = state.language
    val context = LocalContext.current
    val configuration = LocalConfiguration.current
    val isTabletLike = configuration.screenWidthDp >= 840
    val formattedQuitDate = remember(state.quitDateTime) {
        DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")
            .withZone(ZoneId.systemDefault())
            .format(state.quitDateTime)
    }
    Scaffold(
        contentWindowInsets = WindowInsets(0, 0, 0, 0),
        topBar = {
            TopAppBar(
                title = { Text(LocalizationService.getString(language, "app_title")) },
                actions = {
                    Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
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
                    }
                },
            )
        },
    ) { paddingValues ->
        LazyColumn(
            modifier = Modifier
                .fillMaxSize()
                .testTag("main_screen_list")
                .padding(paddingValues)
                .padding(16.dp),
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
            item {
                if (isTabletLike) {
                    Row(modifier = Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(16.dp)) {
                        Card(modifier = Modifier.weight(1f)) {
                            Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                Text(LocalizationService.getString(language, "saved_caption"), style = MaterialTheme.typography.labelLarge)
                                Text(state.savedAmountText, style = MaterialTheme.typography.headlineMedium, fontWeight = FontWeight.Bold)
                                Text(LocalizationService.getString(language, "elapsed_caption"), style = MaterialTheme.typography.labelLarge)
                                Text(state.elapsedText, style = MaterialTheme.typography.bodyLarge)
                                Text(state.dashboardHint, style = MaterialTheme.typography.bodyMedium)
                                if (!state.hasStartedTracking) {
                                    Text(state.firstRunMessage, style = MaterialTheme.typography.bodyMedium)
                                    Button(onClick = onStartTracking) {
                                        Text(LocalizationService.getString(language, "quit_now"))
                                    }
                                }
                            }
                        }
                        Card(modifier = Modifier.weight(1f)) {
                            Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                Text(LocalizationService.getString(language, "quit_date"), style = MaterialTheme.typography.labelLarge)
                                Button(
                                    onClick = {
                                        showDateTimePicker(context, state.quitDateTime, onQuitDateChanged)
                                    },
                                ) {
                                    Icon(Icons.Rounded.CalendarMonth, contentDescription = null)
                                    Text(formattedQuitDate, modifier = Modifier.padding(start = 8.dp))
                                }
                                FlowRow(horizontalArrangement = Arrangement.spacedBy(12.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                    OutlinedTextField(
                                        value = state.packsPerDay.toString(),
                                        onValueChange = { it.toDoubleOrNull()?.let(onPacksChanged) },
                                        label = { Text(LocalizationService.getString(language, "packs_per_day")) },
                                    )
                                    OutlinedTextField(
                                        value = state.packPriceUah.toString(),
                                        onValueChange = { it.toDoubleOrNull()?.let(onPriceChanged) },
                                        label = { Text(LocalizationService.getString(language, "pack_price")) },
                                    )
                                }
                            }
                        }
                    }
                } else {
                    Column(verticalArrangement = Arrangement.spacedBy(16.dp)) {
                        Card(modifier = Modifier.fillMaxWidth()) {
                            Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                Text(LocalizationService.getString(language, "saved_caption"), style = MaterialTheme.typography.labelLarge)
                                Text(state.savedAmountText, style = MaterialTheme.typography.headlineMedium, fontWeight = FontWeight.Bold)
                                Text(LocalizationService.getString(language, "elapsed_caption"), style = MaterialTheme.typography.labelLarge)
                                Text(state.elapsedText, style = MaterialTheme.typography.bodyLarge)
                                Text(state.dashboardHint, style = MaterialTheme.typography.bodyMedium)
                                if (!state.hasStartedTracking) {
                                    Text(state.firstRunMessage, style = MaterialTheme.typography.bodyMedium)
                                    Button(onClick = onStartTracking) {
                                        Text(LocalizationService.getString(language, "quit_now"))
                                    }
                                }
                            }
                        }
                        Card(modifier = Modifier.fillMaxWidth()) {
                            Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                Text(LocalizationService.getString(language, "quit_date"), style = MaterialTheme.typography.labelLarge)
                                Button(
                                    onClick = {
                                        showDateTimePicker(context, state.quitDateTime, onQuitDateChanged)
                                    },
                                ) {
                                    Icon(Icons.Rounded.CalendarMonth, contentDescription = null)
                                    Text(formattedQuitDate, modifier = Modifier.padding(start = 8.dp))
                                }
                                FlowRow(horizontalArrangement = Arrangement.spacedBy(12.dp), verticalArrangement = Arrangement.spacedBy(12.dp)) {
                                    OutlinedTextField(
                                        value = state.packsPerDay.toString(),
                                        onValueChange = { it.toDoubleOrNull()?.let(onPacksChanged) },
                                        label = { Text(LocalizationService.getString(language, "packs_per_day")) },
                                    )
                                    OutlinedTextField(
                                        value = state.packPriceUah.toString(),
                                        onValueChange = { it.toDoubleOrNull()?.let(onPriceChanged) },
                                        label = { Text(LocalizationService.getString(language, "pack_price")) },
                                    )
                                }
                            }
                        }
                    }
                }
            }
            item {
                Card(modifier = Modifier.fillMaxWidth()) {
                    Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(8.dp)) {
                        Text(LocalizationService.getString(language, "motivation_header"), style = MaterialTheme.typography.titleMedium)
                        Text(state.currentPhrase, style = MaterialTheme.typography.bodyLarge)
                        Text(LocalizationService.getString(language, "motivation_rotating"), style = MaterialTheme.typography.labelMedium)
                    }
                }
            }
            item {
                Row(modifier = Modifier.fillMaxWidth(), verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.SpaceBetween) {
                    Column {
                        Text(LocalizationService.getString(language, "products_header"), style = MaterialTheme.typography.titleMedium)
                        Text(state.productUpdatedAtText, style = MaterialTheme.typography.labelMedium)
                    }
                    Button(onClick = onRefreshProducts) {
                        Icon(Icons.Rounded.Refresh, contentDescription = null)
                        Text(LocalizationService.getString(language, "products_refresh"), modifier = Modifier.padding(start = 8.dp))
                    }
                }
            }
            if (state.productSuggestions.isEmpty()) {
                item {
                    Text(LocalizationService.getString(language, "products_empty"))
                }
            } else {
                items(state.productSuggestions) { product ->
                    Card(modifier = Modifier.fillMaxWidth()) {
                        Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(4.dp)) {
                            Text(product.title, style = MaterialTheme.typography.titleMedium)
                            Text("${product.priceUah.toInt()} ${LocalizationService.getString(language, "currency_major")}")
                            Text(product.category, style = MaterialTheme.typography.labelMedium)
                            Text(product.shortDescription, style = MaterialTheme.typography.bodyMedium)
                        }
                    }
                }
            }
            item {
                Text(LocalizationService.getString(language, "recovery_header"), style = MaterialTheme.typography.titleMedium)
            }
            items(state.recoveryMilestones) { milestone ->
                Card(modifier = Modifier.fillMaxWidth()) {
                    Column(modifier = Modifier.padding(16.dp), verticalArrangement = Arrangement.spacedBy(4.dp)) {
                        Text(milestone.milestone.timeframeLabel, style = MaterialTheme.typography.labelLarge)
                        Text(milestone.milestone.title, style = MaterialTheme.typography.titleMedium)
                        Text(milestone.milestone.description, style = MaterialTheme.typography.bodyMedium)
                        Text(
                            when (milestone.state) {
                                com.nitkotin.android.data.model.RecoveryMilestoneState.Completed -> LocalizationService.getString(language, "recovery_completed")
                                com.nitkotin.android.data.model.RecoveryMilestoneState.Current -> LocalizationService.getString(language, "recovery_current")
                                com.nitkotin.android.data.model.RecoveryMilestoneState.Upcoming -> LocalizationService.getString(language, "recovery_upcoming")
                            },
                            style = MaterialTheme.typography.labelMedium,
                            fontWeight = FontWeight.Bold,
                        )
                    }
                }
            }
        }
    }
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
