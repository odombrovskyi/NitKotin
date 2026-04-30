namespace SmokeSaver.Models;

public enum RecoveryMilestoneState
{
    Completed,
    Current,
    Upcoming
}

public sealed class RecoveryMilestone
{
    public TimeSpan Threshold { get; init; }

    public string TimeframeLabel { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}

public sealed class RecoveryMilestoneSnapshot
{
    public required RecoveryMilestone Milestone { get; init; }

    public RecoveryMilestoneState State { get; init; }
}