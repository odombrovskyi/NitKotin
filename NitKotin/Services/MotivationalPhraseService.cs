using System.Text.Json;
using NitKotin.Models;

namespace NitKotin.Services;

public sealed class MotivationalPhraseService
{
    public IReadOnlyList<MotivationalPhrase> LoadPhrases(string languageCode)
    {
        var normalizedLanguage = LocalizationService.NormalizeLanguage(languageCode);
        var phrasesPath = Path.Combine(AppContext.BaseDirectory, "Data", LocalizationService.GetMotivationalPhrasesFileName(normalizedLanguage));
        var fallbackPhrases = LocalizationService.GetFallbackPhrases(normalizedLanguage);

        if (!File.Exists(phrasesPath))
        {
            return fallbackPhrases;
        }

        try
        {
            var json = File.ReadAllText(phrasesPath);
            var phrases = JsonSerializer.Deserialize<List<MotivationalPhrase>>(json);
            var validPhrases = phrases?
                .Where(phrase => !string.IsNullOrWhiteSpace(phrase.Text))
                .ToArray();

            return validPhrases is { Length: > 0 }
                ? validPhrases
                : fallbackPhrases;
        }
        catch (JsonException)
        {
            return fallbackPhrases;
        }
        catch (IOException)
        {
            return fallbackPhrases;
        }
    }
}