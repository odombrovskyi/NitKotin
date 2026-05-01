using System.Text.Json;
using NitKotin.Models;

namespace NitKotin.Services;

public sealed class ProductCatalogService
{
    public IReadOnlyList<CatalogProduct> LoadCatalog(string languageCode)
    {
        var normalizedLanguage = LocalizationService.NormalizeLanguage(languageCode);
        var catalogPath = Path.Combine(AppContext.BaseDirectory, "Data", GetCatalogFileName(normalizedLanguage));

        if (!File.Exists(catalogPath))
        {
            return Array.Empty<CatalogProduct>();
        }

        try
        {
            var json = File.ReadAllText(catalogPath);
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

    private static string GetCatalogFileName(string languageCode)
    {
        return languageCode == LocalizationService.Ukrainian
            ? "product-catalog.ua.json"
            : "product-catalog.en.json";
    }
}