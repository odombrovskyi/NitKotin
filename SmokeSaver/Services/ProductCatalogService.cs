using System.Text.Json;
using SmokeSaver.Models;

namespace SmokeSaver.Services;

public sealed class ProductCatalogService
{
    private readonly string _catalogPath;

    public ProductCatalogService(string? catalogPath = null)
    {
        _catalogPath = catalogPath ?? Path.Combine(AppContext.BaseDirectory, "Data", "product-catalog.ua.json");
    }

    public IReadOnlyList<CatalogProduct> LoadCatalog()
    {
        if (!File.Exists(_catalogPath))
        {
            return Array.Empty<CatalogProduct>();
        }

        try
        {
            var json = File.ReadAllText(_catalogPath);
            var products = JsonSerializer.Deserialize<List<CatalogProduct>>(json);
            return products?
                .Where(product => !string.IsNullOrWhiteSpace(product.Id)
                    && !string.IsNullOrWhiteSpace(product.Title)
                    && product.PriceUah > 0)
                .OrderBy(product => product.PriceUah)
                .ToArray() ?? Array.Empty<CatalogProduct>();
        }
        catch (JsonException)
        {
            return Array.Empty<CatalogProduct>();
        }
        catch (IOException)
        {
            return Array.Empty<CatalogProduct>();
        }
    }
}