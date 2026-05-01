namespace NitKotin.Services;

using NitKotin.Models;

public sealed class RecoveryTimelineService
{
    public IReadOnlyList<RecoveryMilestoneSnapshot> GetVisibleMilestones(TimeSpan smokeFreeDuration, int maxItems = 5, string languageCode = LocalizationService.English)
    {
        if (maxItems <= 0)
        {
            return Array.Empty<RecoveryMilestoneSnapshot>();
        }

        var milestones = LocalizationService.GetRecoveryMilestones(languageCode);

        var safeDuration = smokeFreeDuration < TimeSpan.Zero
            ? TimeSpan.Zero
            : smokeFreeDuration;

        var currentIndex = 0;
        for (var index = 0; index < milestones.Count; index++)
        {
            if (safeDuration >= milestones[index].Threshold)
            {
                currentIndex = index;
                continue;
            }

            currentIndex = index;
            break;
        }

        if (safeDuration >= milestones[^1].Threshold)
        {
            currentIndex = milestones.Count - 1;
        }

        var startIndex = Math.Max(0, currentIndex - 2);
        if (startIndex + maxItems > milestones.Count)
        {
            startIndex = Math.Max(0, milestones.Count - maxItems);
        }

        var snapshots = new List<RecoveryMilestoneSnapshot>(maxItems);
        for (var index = startIndex; index < milestones.Count && snapshots.Count < maxItems; index++)
        {
            snapshots.Add(new RecoveryMilestoneSnapshot
            {
                Milestone = milestones[index],
                State = ResolveState(milestones, index, safeDuration)
            });
        }

        return snapshots;
    }

    private RecoveryMilestoneState ResolveState(IReadOnlyList<RecoveryMilestone> milestones, int index, TimeSpan smokeFreeDuration)
    {
        var milestone = milestones[index];
        if (smokeFreeDuration < milestone.Threshold)
        {
            return index == 0 || smokeFreeDuration >= milestones[index - 1].Threshold
                ? RecoveryMilestoneState.Current
                : RecoveryMilestoneState.Upcoming;
        }

        if (index == milestones.Count - 1 || smokeFreeDuration < milestones[index + 1].Threshold)
        {
            return RecoveryMilestoneState.Current;
        }

        return RecoveryMilestoneState.Completed;
    }
}