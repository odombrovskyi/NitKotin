namespace SmokeSaver.Services;

using SmokeSaver.Models;

public sealed class RecoveryTimelineService
{
    private readonly RecoveryMilestone[] _milestones =
    [
        new()
        {
            Threshold = TimeSpan.FromMinutes(20),
            TimeframeLabel = "20 хв",
            Title = "Організм переключається",
            Description = "• Пульс і тиск знижуються ближче до норми.\n• Кровообіг у кінцівках починає вирівнюватися."
        },
        new()
        {
            Threshold = TimeSpan.FromHours(8),
            TimeframeLabel = "8–12 год",
            Title = "Кров отримує більше кисню",
            Description = "• Рівень чадного газу помітно падає.\n• Кисню в крові стає більше, ніж у перші години."
        },
        new()
        {
            Threshold = TimeSpan.FromHours(24),
            TimeframeLabel = "1 доба",
            Title = "Перша доба без диму",
            Description = "• Організм виводить залишки чадного газу.\n• Легені починають краще відводити слиз і продукти подразнення."
        },
        new()
        {
            Threshold = TimeSpan.FromHours(48),
            TimeframeLabel = "2 доби",
            Title = "Нікотин уже позаду",
            Description = "• Нікотин майже повністю виходить з організму.\n• Смак і нюх часто стають чіткішими."
        },
        new()
        {
            Threshold = TimeSpan.FromHours(72),
            TimeframeLabel = "3 доби",
            Title = "Дихати стає легше",
            Description = "• Спазм бронхів слабшає.\n• Дихання відчувається вільнішим, а енергії може бути більше."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(14),
            TimeframeLabel = "2–12 тижнів",
            Title = "Серце й судини підтягуються",
            Description = "• Кровообіг помітно покращується.\n• Ходити швидко, підійматися сходами й тренуватися часто легше."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(30),
            TimeframeLabel = "1 місяць",
            Title = "Легені прибирають наслідки диму",
            Description = "• Кашель і важкість у грудях часто слабшають.\n• Дихальні шляхи краще прибирають слиз і бруд."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(270),
            TimeframeLabel = "1–9 місяців",
            Title = "Більше повітря й енергії",
            Description = "• Задишка стає рідшою або слабшою.\n• Загальний рівень енергії часто відчутно зростає."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(365),
            TimeframeLabel = "1 рік",
            Title = "Серцевий ризик уже нижчий",
            Description = "• Ризик ішемічної хвороби серця приблизно вдвічі нижчий, ніж у курця.\n• Витривалість і відновлення після навантаження часто кращі."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(365 * 5),
            TimeframeLabel = "5 років",
            Title = "Судини працюють спокійніше",
            Description = "• Ризик інсульту суттєво знижується.\n• Ризики для рота, горла та гортані теж ідуть вниз."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(365 * 10),
            TimeframeLabel = "10 років",
            Title = "Довгий виграш для легень",
            Description = "• Ризик раку легень приблизно вдвічі нижчий, ніж у курця.\n• Також нижчі ризики для нирок, стравоходу й сечового міхура."
        },
        new()
        {
            Threshold = TimeSpan.FromDays(365 * 15),
            TimeframeLabel = "15 років",
            Title = "Майже як у некурця",
            Description = "• Ризик ішемічної хвороби серця близький до рівня людини, яка не курить.\n• Серце й судини менше живуть у режимі постійного стресу."
        }
    ];

    public IReadOnlyList<RecoveryMilestoneSnapshot> GetVisibleMilestones(TimeSpan smokeFreeDuration, int maxItems = 5)
    {
        if (maxItems <= 0)
        {
            return Array.Empty<RecoveryMilestoneSnapshot>();
        }

        var safeDuration = smokeFreeDuration < TimeSpan.Zero
            ? TimeSpan.Zero
            : smokeFreeDuration;

        var currentIndex = 0;
        for (var index = 0; index < _milestones.Length; index++)
        {
            if (safeDuration >= _milestones[index].Threshold)
            {
                currentIndex = index;
                continue;
            }

            currentIndex = index;
            break;
        }

        if (safeDuration >= _milestones[^1].Threshold)
        {
            currentIndex = _milestones.Length - 1;
        }

        var startIndex = Math.Max(0, currentIndex - 2);
        if (startIndex + maxItems > _milestones.Length)
        {
            startIndex = Math.Max(0, _milestones.Length - maxItems);
        }

        var snapshots = new List<RecoveryMilestoneSnapshot>(maxItems);
        for (var index = startIndex; index < _milestones.Length && snapshots.Count < maxItems; index++)
        {
            snapshots.Add(new RecoveryMilestoneSnapshot
            {
                Milestone = _milestones[index],
                State = ResolveState(index, safeDuration)
            });
        }

        return snapshots;
    }

    private RecoveryMilestoneState ResolveState(int index, TimeSpan smokeFreeDuration)
    {
        var milestone = _milestones[index];
        if (smokeFreeDuration < milestone.Threshold)
        {
            return index == 0 || smokeFreeDuration >= _milestones[index - 1].Threshold
                ? RecoveryMilestoneState.Current
                : RecoveryMilestoneState.Upcoming;
        }

        if (index == _milestones.Length - 1 || smokeFreeDuration < _milestones[index + 1].Threshold)
        {
            return RecoveryMilestoneState.Current;
        }

        return RecoveryMilestoneState.Completed;
    }
}