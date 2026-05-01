package com.nitkotin.android.domain

import com.nitkotin.android.data.model.AppLanguage
import com.nitkotin.android.data.model.RecoveryMilestoneState
import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import java.time.Duration

class RecoveryTimelineServiceTest {
    private val service = RecoveryTimelineService()

    @Test
    fun getVisibleMilestones_marksFirstMilestoneCurrent_forNegativeDuration() {
        val result = service.getVisibleMilestones(Duration.ofMinutes(-5), AppLanguage.ENGLISH)

        assertTrue(result.isNotEmpty())
        assertEquals(RecoveryMilestoneState.Current, result.first().state)
    }

    @Test
    fun getVisibleMilestones_marksLastMilestoneCurrent_whenDurationBeyondLastThreshold() {
        val result = service.getVisibleMilestones(Duration.ofDays(800), AppLanguage.ENGLISH)

        assertEquals(RecoveryMilestoneState.Current, result.last().state)
    }
}