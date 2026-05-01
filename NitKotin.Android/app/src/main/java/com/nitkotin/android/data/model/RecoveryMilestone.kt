package com.nitkotin.android.data.model

import java.time.Duration

enum class RecoveryMilestoneState {
    Completed,
    Current,
    Upcoming,
}

data class RecoveryMilestone(
    val threshold: Duration,
    val timeframeLabel: String,
    val title: String,
    val description: String,
)

data class RecoveryMilestoneSnapshot(
    val milestone: RecoveryMilestone,
    val state: RecoveryMilestoneState,
)
