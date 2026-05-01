namespace NitKotin.Models;

public sealed class SmokingConfig
{
    public string LanguagePreference { get; set; } = "en";

    public bool HasStartedTracking { get; set; }

    public DateTime QuitDateTime { get; set; } = DateTime.Now;

    public decimal PacksPerDay { get; set; } = 2m;

    public decimal PackPriceUah { get; set; } = 160m;

    public int OverlayLocationX { get; set; } = -1;

    public int OverlayLocationY { get; set; } = -1;
}