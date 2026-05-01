package com.nitkotin.android.domain

import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.RecoveryMilestoneState
import com.nitkotin.android.data.model.RecoveryMilestoneSnapshot
import java.time.Duration
import kotlin.math.max

class RecoveryTimelineService {
    fun getVisibleMilestones(
        smokeFreeDuration: Duration,
        language: AppLanguage,
        maxItems: Int = 5,
    ): List<RecoveryMilestoneSnapshot> {
        if (maxItems <= 0) return emptyList()

        val milestones = LocalizationService.recoveryMilestones(language)
        val safeDuration = if (smokeFreeDuration.isNegative) Duration.ZERO else smokeFreeDuration

        var currentIndex = 0
        for (index in milestones.indices) {
            if (safeDuration >= milestones[index].threshold) {
                currentIndex = index
                continue
            }
            currentIndex = index
            break
        }

        if (safeDuration >= milestones.last().threshold) {
            currentIndex = milestones.lastIndex
        }

        var startIndex = max(0, currentIndex - 2)
        if (startIndex + maxItems > milestones.size) {
            startIndex = max(0, milestones.size - maxItems)
        }

        return buildList {
            for (index in startIndex until milestones.size) {
                if (size >= maxItems) break
                add(
                    RecoveryMilestoneSnapshot(
                        milestone = milestones[index],
                        state = resolveState(milestones.map { it.threshold }, index, safeDuration),
                    )
                )
            }
        }
    }

    private fun resolveState(
        thresholds: List<Duration>,
        index: Int,
        smokeFreeDuration: Duration,
    ): RecoveryMilestoneState {
        val threshold = thresholds[index]
        if (smokeFreeDuration < threshold) {
            return if (index == 0 || smokeFreeDuration >= thresholds[index - 1]) {
                RecoveryMilestoneState.Current
            } else {
                RecoveryMilestoneState.Upcoming
            }
        }

        if (index == thresholds.lastIndex || smokeFreeDuration < thresholds[index + 1]) {
            return RecoveryMilestoneState.Current
        }

        return RecoveryMilestoneState.Completed
    }
}
