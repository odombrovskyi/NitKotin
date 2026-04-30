namespace SmokeSaver.Models;

public sealed class SmokingConfig
{
    public DateTime QuitDateTime { get; set; } = DateTime.Now;

    public decimal PacksPerDay { get; set; } = 3m;

    public decimal PackPriceUah { get; set; } = 160m;

    public int OverlayLocationX { get; set; } = -1;

    public int OverlayLocationY { get; set; } = -1;
}