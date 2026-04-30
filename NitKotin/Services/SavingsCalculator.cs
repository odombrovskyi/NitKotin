using NitKotin.Models;

namespace NitKotin.Services;

public static class SavingsCalculator
{
    private const decimal CigarettesPerPack = 20m;
    private const decimal SecondsPerDay = 24m * 60m * 60m;

    public static decimal CalculateSavedAmount(SmokingConfig config, DateTime currentTime)
    {
        if (config.PacksPerDay <= 0 || config.PackPriceUah <= 0 || config.QuitDateTime >= currentTime)
        {
            return 0m;
        }

        var elapsed = currentTime - config.QuitDateTime;
        var dailySpend = config.PacksPerDay * config.PackPriceUah;
        var spendPerSecond = dailySpend / SecondsPerDay;
        var savedAmount = spendPerSecond * (decimal)elapsed.TotalSeconds;

        return decimal.Round(savedAmount, 2, MidpointRounding.AwayFromZero);
    }

    public static string FormatCurrency(decimal amount)
    {
        var hryvnias = decimal.Truncate(amount);
        var kopecks = decimal.Round((amount - hryvnias) * 100m, 0, MidpointRounding.AwayFromZero);

        if (kopecks == 100m)
        {
            hryvnias += 1m;
            kopecks = 0m;
        }

        return $"{hryvnias:0} грн {kopecks:00} коп";
    }

    public static string FormatElapsed(DateTime quitDateTime, DateTime currentTime)
    {
        if (quitDateTime >= currentTime)
        {
            return "Відмова ще не почалася";
        }

        var elapsed = currentTime - quitDateTime;
        return $"{elapsed.Days} дн. {elapsed.Hours:00} год. {elapsed.Minutes:00} хв. {elapsed.Seconds:00} сек.";
    }
}