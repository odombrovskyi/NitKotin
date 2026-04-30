using System.Text.Json;
using NitKotin.Models;

namespace NitKotin.Services;

public sealed class MotivationalPhraseService
{
    private static readonly MotivationalPhrase[] FallbackPhrases =
    [
        new() { Text = "Світліші зуби", Category = "Здоров'я" },
        new() { Text = "Краща потенція і відчуття", Category = "Здоров'я" },
        new() { Text = "Не закладений ніс", Category = "Здоров'я" },
        new() { Text = "Немає ранкового кашлю", Category = "Здоров'я" },
        new() { Text = "Немає подразнення очей", Category = "Здоров'я" },
        new() { Text = "Можливо знову спробувати контактні лінзи", Category = "Комфорт" },
        new() { Text = "Заощадження коштів", Category = "Фінанси" },
        new() { Text = "Немає головного болю", Category = "Здоров'я" },
        new() { Text = "Стабільніший тиск", Category = "Здоров'я" },
        new() { Text = "Краща витривалість у спортзалі", Category = "Спорт" },
        new() { Text = "Дружина не називає вонючкою", Category = "Фан" },
        new() { Text = "У дитини буде гарний приклад", Category = "Сім'я" },
        new() { Text = "Свіжий запах в авто", Category = "Побут" },
        new() { Text = "Немає прив'язаності до цигарок", Category = "Свобода" },
        new() { Text = "Менш нервовий стан", Category = "Самопочуття" },
        new() { Text = "Менший шанс передчасної смерті", Category = "Здоров'я" }
    ];

    private readonly string _phrasesPath;

    public MotivationalPhraseService(string? phrasesPath = null)
    {
        _phrasesPath = phrasesPath ?? Path.Combine(AppContext.BaseDirectory, "Data", "motivational-phrases.ua.json");
    }

    public IReadOnlyList<MotivationalPhrase> LoadPhrases()
    {
        if (!File.Exists(_phrasesPath))
        {
            return FallbackPhrases;
        }

        try
        {
            var json = File.ReadAllText(_phrasesPath);
            var phrases = JsonSerializer.Deserialize<List<MotivationalPhrase>>(json);
            var validPhrases = phrases?
                .Where(phrase => !string.IsNullOrWhiteSpace(phrase.Text))
                .ToArray();

            return validPhrases is { Length: > 0 }
                ? validPhrases
                : FallbackPhrases;
        }
        catch (JsonException)
        {
            return FallbackPhrases;
        }
        catch (IOException)
        {
            return FallbackPhrases;
        }
    }
}