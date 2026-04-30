namespace SmokeSaver.Models;

public sealed class CatalogProduct
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public decimal PriceUah { get; set; }

    public string Category { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;
}