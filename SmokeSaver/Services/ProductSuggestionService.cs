using SmokeSaver.Models;

namespace SmokeSaver.Services;

public sealed class ProductSuggestionService
{
    private readonly IReadOnlyList<CatalogProduct> _catalogProducts;

    public ProductSuggestionService(IReadOnlyList<CatalogProduct> catalogProducts)
    {
        _catalogProducts = catalogProducts;
    }

    public IReadOnlyList<SuggestedProduct> GetSuggestions(decimal budgetUah)
    {
        if (_catalogProducts.Count == 0)
        {
            return Array.Empty<SuggestedProduct>();
        }

        var minimumPrice = _catalogProducts.Min(product => product.PriceUah);
        var effectiveBudget = Math.Max(budgetUah, minimumPrice);

        var eligibleProducts = _catalogProducts
            .Where(product => product.PriceUah <= effectiveBudget)
            .OrderBy(product => product.PriceUah)
            .ToArray();

        if (eligibleProducts.Length == 0)
        {
            eligibleProducts = _catalogProducts.Take(3).ToArray();
        }

        var selectedProducts = new List<CatalogProduct>();
        var usedCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var topTier = eligibleProducts.Where(product => product.PriceUah >= effectiveBudget * 0.72m).ToArray();
        var middleTier = eligibleProducts.Where(product => product.PriceUah >= effectiveBudget * 0.35m && product.PriceUah < effectiveBudget * 0.72m).ToArray();
        var smallTier = eligibleProducts.Where(product => product.PriceUah < effectiveBudget * 0.35m).ToArray();

        TryPickProduct(topTier, selectedProducts, usedCategories);
        TryPickProduct(middleTier, selectedProducts, usedCategories);
        TryPickProduct(smallTier, selectedProducts, usedCategories);

        while (selectedProducts.Count < 3)
        {
            if (!TryPickProduct(eligibleProducts, selectedProducts, usedCategories))
            {
                break;
            }
        }

        return selectedProducts
            .DistinctBy(product => product.Id)
            .Take(3)
            .Select(product => new SuggestedProduct
            {
                Title = product.Title,
                PriceUah = product.PriceUah,
                Category = product.Category,
                ShortDescription = product.ShortDescription
            })
            .ToArray();
    }

    private static bool TryPickProduct(
        IReadOnlyList<CatalogProduct> source,
        ICollection<CatalogProduct> selectedProducts,
        ISet<string> usedCategories)
    {
        var usedIds = selectedProducts.Select(product => product.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var freshCategoryCandidates = source
            .Where(product => !usedIds.Contains(product.Id) && !usedCategories.Contains(product.Category))
            .ToArray();

        var fallbackCandidates = source
            .Where(product => !usedIds.Contains(product.Id))
            .ToArray();

        var candidatePool = freshCategoryCandidates.Length > 0 ? freshCategoryCandidates : fallbackCandidates;
        if (candidatePool.Length == 0)
        {
            return false;
        }

        var pick = candidatePool[Random.Shared.Next(candidatePool.Length)];
        selectedProducts.Add(pick);
        usedCategories.Add(pick.Category);
        return true;
    }
}