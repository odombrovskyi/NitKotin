using NitKotin.Models;

namespace NitKotin.Services;

public static class LocalizationService
{
    public const string English = "en";
    public const string Ukrainian = "uk";

    private static readonly IReadOnlyDictionary<string, string> EnglishStrings = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["AppTitle"] = "No! kitten motivator",
        ["TrayExit"] = "Exit",
        ["TrayHideOverlay"] = "Hide overlay",
        ["TrayShowOverlay"] = "Show overlay",
        ["TrayHideMainWindow"] = "Hide main window",
        ["TrayShowMainWindow"] = "Show main window",
        ["MotivationLoading"] = "Loading motivation",
        ["MotivationFallback"] = "You are already doing something important for yourself.",
        ["ConfigStatusFirstRun"] = "Press \"I quit smoking!\" to start tracking. Config will be saved to: {0}",
        ["ConfigStatusLoaded"] = "Config loaded: {0}",
        ["ConfigStatusWillBeSaved"] = "Config will be saved to: {0}",
        ["StatusPendingFirstRun"] = "Settings are ready. Press \"I quit smoking!\" to start tracking and save the config.",
        ["StatusNeedFirstRunButton"] = "Press \"I quit smoking!\" first to lock in your quit time.",
        ["StatusChangesDetected"] = "Changes detected. Autosaving...",
        ["StatusAutoSaved"] = "Autosaved",
        ["StatusQuitStartedSaved"] = "Tracking start saved",
        ["StatusSavedFormat"] = "{0}: {1}",
        ["StatusSaveError"] = "Save error: {0}",
        ["StatusAccessError"] = "No access to config: {0}",
        ["StatusProductsTooSoon"] = "Product refresh just ran. Try again in a few seconds.",
        ["StatusProductsAutoRefreshed"] = "Product picks refreshed automatically.",
        ["StatusProductsManualRefreshed"] = "Product picks refreshed manually.",
        ["StatusProductsLoaded"] = "Loaded picks from the local catalog.",
        ["HintFirstRun"] = "Press \"I quit smoking!\" to store the current time and start tracking.",
        ["HintFutureDate"] = "You selected a future date. Savings will start counting after that moment.",
        ["HintNeedPositiveValues"] = "To see the amount, set packs per day and pack price above zero.",
        ["HintRealtime"] = "The amount updates automatically in real time.",
        ["QuitDateLabel"] = "Quit date and time",
        ["PacksPerDayLabel"] = "Packs per day",
        ["PackPriceLabel"] = "Pack price, UAH",
        ["AutosaveLabel"] = "Changes are saved automatically after a short delay.",
        ["SavedCaption"] = "Saved on cigarettes",
        ["SmokeFreeCaption"] = "Smoke-free",
        ["ProductsHeader"] = "What you can already buy",
        ["ProductsNeverUpdated"] = "Updated: not yet",
        ["ProductsUpdatedAtFormat"] = "Updated at {0:HH:mm}",
        ["ProductsRefreshButton"] = "Refresh picks",
        ["CatalogUnavailableTitle"] = "Catalog unavailable",
        ["CatalogUnavailableDescription"] = "Could not load local products.",
        ["CatalogTryLaterTitle"] = "Try again later",
        ["CatalogTryLaterDescription"] = "Check the JSON catalog in the app directory.",
        ["CatalogEmptyTitle"] = "No picks yet",
        ["CatalogEmptyDescription"] = "The main tracker still works, but the catalog is empty.",
        ["CatalogGoalTitle"] = "A little more and a new goal appears",
        ["CatalogGoalCategory"] = "Motivation",
        ["CatalogGoalDescription"] = "As the amount grows, the catalog will show a wider selection.",
        ["RecoveryHeader"] = "Recovery timeline",
        ["RecoveryLegend"] = "Green means done, orange is your current stage, neutral means what comes next.",
        ["RecoveryLegendFirstRun"] = "Recovery stages will appear after you press the start button.",
        ["RecoveryStateCompleted"] = "Completed",
        ["RecoveryStateCurrent"] = "Current stage",
        ["RecoveryStateUpcoming"] = "Coming up",
        ["ElapsedNotStarted"] = "Tracking has not started yet",
        ["QuitNowButton"] = "I quit smoking!",
        ["LanguageEnglish"] = "English",
        ["LanguageUkrainian"] = "Українська",
        ["CurrencyMajor"] = "UAH",
        ["CurrencyMinor"] = "kop",
        ["ElapsedDays"] = "d.",
        ["ElapsedHours"] = "hrs.",
        ["ElapsedMinutes"] = "min.",
        ["ElapsedSeconds"] = "sec.",
        ["ElapsedNotStartedShort"] = "Quit time has not started yet",
        ["OverlayHours"] = "hrs"
    };

    private static readonly IReadOnlyDictionary<string, string> UkrainianStrings = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["AppTitle"] = "Ні! котину мотиватор",
        ["TrayExit"] = "Вихід",
        ["TrayHideOverlay"] = "Сховати оверлей",
        ["TrayShowOverlay"] = "Показати оверлей",
        ["TrayHideMainWindow"] = "Сховати основне вікно",
        ["TrayShowMainWindow"] = "Показати основне вікно",
        ["MotivationLoading"] = "Мотивація завантажується",
        ["MotivationFallback"] = "Ти вже робиш щось важливе для себе.",
        ["ConfigStatusFirstRun"] = "Натисни \"Я кинув палити!\", щоб почати відлік. Конфіг буде збережено в: {0}",
        ["ConfigStatusLoaded"] = "Конфіг завантажено: {0}",
        ["ConfigStatusWillBeSaved"] = "Конфіг буде збережено в: {0}",
        ["StatusPendingFirstRun"] = "Налаштування підготовлено. Натисни \"Я кинув палити!\", щоб почати відлік і зберегти конфіг.",
        ["StatusNeedFirstRunButton"] = "Спочатку натисни \"Я кинув палити!\", щоб зафіксувати час відмови.",
        ["StatusChangesDetected"] = "Виявлено зміни. Автозбереження...",
        ["StatusAutoSaved"] = "Автозбережено",
        ["StatusQuitStartedSaved"] = "Початок відліку збережено",
        ["StatusSavedFormat"] = "{0}: {1}",
        ["StatusSaveError"] = "Помилка збереження: {0}",
        ["StatusAccessError"] = "Немає доступу до конфігу: {0}",
        ["StatusProductsTooSoon"] = "Оновлення товарів щойно запускалося. Спробуй ще за кілька секунд.",
        ["StatusProductsAutoRefreshed"] = "Добірку товарів оновлено автоматично.",
        ["StatusProductsManualRefreshed"] = "Добірку товарів оновлено вручну.",
        ["StatusProductsLoaded"] = "Підібрано товари з локального каталогу.",
        ["HintFirstRun"] = "Натисни \"Я кинув палити!\", щоб зафіксувати поточний час і почати відлік.",
        ["HintFutureDate"] = "Вказано майбутню дату. Економія почне рахуватися після цього моменту.",
        ["HintNeedPositiveValues"] = "Щоб побачити суму, вкажіть кількість пачок і ціну більше нуля.",
        ["HintRealtime"] = "Сума оновлюється автоматично в реальному часі.",
        ["QuitDateLabel"] = "Дата й час відмови від куріння",
        ["PacksPerDayLabel"] = "Пачок на день",
        ["PackPriceLabel"] = "Ціна однієї пачки, грн",
        ["AutosaveLabel"] = "Зміни зберігаються автоматично після короткої затримки.",
        ["SavedCaption"] = "Заощаджено на сигаретах",
        ["SmokeFreeCaption"] = "Без куріння",
        ["ProductsHeader"] = "Що вже можна купити",
        ["ProductsNeverUpdated"] = "Оновлення: ще не виконувалось",
        ["ProductsUpdatedAtFormat"] = "Оновлено о {0:HH:mm}",
        ["ProductsRefreshButton"] = "Оновити добірку",
        ["CatalogUnavailableTitle"] = "Каталог недоступний",
        ["CatalogUnavailableDescription"] = "Не вдалося завантажити локальні товари.",
        ["CatalogTryLaterTitle"] = "Спробуй пізніше",
        ["CatalogTryLaterDescription"] = "Перевір JSON-каталог у директорії застосунку.",
        ["CatalogEmptyTitle"] = "Поки без добірки",
        ["CatalogEmptyDescription"] = "Основний лічильник працює, але каталог порожній.",
        ["CatalogGoalTitle"] = "Ще трохи і буде нова ціль",
        ["CatalogGoalCategory"] = "Мотивація",
        ["CatalogGoalDescription"] = "Коли сума підросте, каталог покаже ширший вибір.",
        ["RecoveryHeader"] = "Таймлайн відновлення",
        ["RecoveryLegend"] = "Зелене — уже позаду, помаранчеве — твій поточний етап, нейтральне — наступні зміни.",
        ["RecoveryLegendFirstRun"] = "Етапи відновлення з'являться після натискання кнопки старту.",
        ["RecoveryStateCompleted"] = "Вже пройдено",
        ["RecoveryStateCurrent"] = "Поточний етап",
        ["RecoveryStateUpcoming"] = "Попереду",
        ["ElapsedNotStarted"] = "Відлік ще не почався",
        ["QuitNowButton"] = "Я кинув палити!",
        ["LanguageEnglish"] = "English",
        ["LanguageUkrainian"] = "Українська",
        ["CurrencyMajor"] = "грн",
        ["CurrencyMinor"] = "коп",
        ["ElapsedDays"] = "дн.",
        ["ElapsedHours"] = "год.",
        ["ElapsedMinutes"] = "хв.",
        ["ElapsedSeconds"] = "сек.",
        ["ElapsedNotStartedShort"] = "Відмова ще не почалася",
        ["OverlayHours"] = "год"
    };

    private static readonly IReadOnlyList<MotivationalPhrase> EnglishFallbackPhrases =
    [
        new() { Text = "Whiter teeth", Category = "Health" },
        new() { Text = "Better stamina and breathing", Category = "Health" },
        new() { Text = "No blocked nose", Category = "Comfort" },
        new() { Text = "No morning cough", Category = "Health" },
        new() { Text = "Less eye irritation", Category = "Health" },
        new() { Text = "More money left over", Category = "Finance" },
        new() { Text = "Fewer headaches", Category = "Health" },
        new() { Text = "More stable blood pressure", Category = "Health" },
        new() { Text = "Better gym endurance", Category = "Sport" },
        new() { Text = "A fresher car interior", Category = "Lifestyle" },
        new() { Text = "No attachment to cigarettes", Category = "Freedom" },
        new() { Text = "A calmer overall state", Category = "Well-being" },
        new() { Text = "A better example for your child", Category = "Family" },
        new() { Text = "Less smell on clothes", Category = "Comfort" },
        new() { Text = "More energy for the day", Category = "Energy" },
        new() { Text = "A lower chance of early death", Category = "Health" }
    ];

    private static readonly IReadOnlyList<MotivationalPhrase> UkrainianFallbackPhrases =
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

    private static readonly IReadOnlyList<RecoveryMilestone> EnglishRecoveryMilestones =
    [
        new() { Threshold = TimeSpan.FromMinutes(20), TimeframeLabel = "20 min", Title = "Your body is switching gears", Description = "• Pulse and blood pressure move closer to normal.\n• Blood flow in the extremities starts to level out." },
        new() { Threshold = TimeSpan.FromHours(8), TimeframeLabel = "8-12 hrs", Title = "Your blood gets more oxygen", Description = "• Carbon monoxide drops noticeably.\n• Your blood carries more oxygen than in the first hours." },
        new() { Threshold = TimeSpan.FromHours(24), TimeframeLabel = "1 day", Title = "First day without smoke", Description = "• Your body clears the remaining carbon monoxide.\n• Your lungs start removing mucus and irritation more effectively." },
        new() { Threshold = TimeSpan.FromHours(48), TimeframeLabel = "2 days", Title = "Nicotine is behind you", Description = "• Nicotine is almost completely out of your system.\n• Taste and smell often become sharper." },
        new() { Threshold = TimeSpan.FromHours(72), TimeframeLabel = "3 days", Title = "Breathing feels easier", Description = "• Bronchial spasm eases.\n• Breathing often feels freer and energy can rise." },
        new() { Threshold = TimeSpan.FromDays(14), TimeframeLabel = "2-12 weeks", Title = "Your heart and vessels recover", Description = "• Blood circulation improves noticeably.\n• Walking fast, taking stairs and training often feel easier." },
        new() { Threshold = TimeSpan.FromDays(30), TimeframeLabel = "1 month", Title = "Your lungs clear the damage", Description = "• Cough and chest heaviness often fade.\n• Airways remove mucus and residue better." },
        new() { Threshold = TimeSpan.FromDays(270), TimeframeLabel = "1-9 months", Title = "More air and more energy", Description = "• Shortness of breath becomes rarer or weaker.\n• Your overall energy level often rises noticeably." },
        new() { Threshold = TimeSpan.FromDays(365), TimeframeLabel = "1 year", Title = "Cardiovascular risk drops", Description = "• Coronary heart disease risk is about half that of a smoker.\n• Endurance and recovery from exercise often improve." },
        new() { Threshold = TimeSpan.FromDays(365 * 5), TimeframeLabel = "5 years", Title = "Blood vessels stay calmer", Description = "• Stroke risk drops significantly.\n• Risks for the mouth, throat and larynx also continue to fall." },
        new() { Threshold = TimeSpan.FromDays(365 * 10), TimeframeLabel = "10 years", Title = "A long-term win for the lungs", Description = "• Lung cancer risk is about half that of a smoker.\n• Risks for the kidneys, esophagus and bladder are also lower." },
        new() { Threshold = TimeSpan.FromDays(365 * 15), TimeframeLabel = "15 years", Title = "Almost like a non-smoker", Description = "• Coronary heart disease risk is close to that of a person who does not smoke.\n• Your heart and vessels live under less constant stress." }
    ];

    private static readonly IReadOnlyList<RecoveryMilestone> UkrainianRecoveryMilestones =
    [
        new() { Threshold = TimeSpan.FromMinutes(20), TimeframeLabel = "20 хв", Title = "Організм переключається", Description = "• Пульс і тиск знижуються ближче до норми.\n• Кровообіг у кінцівках починає вирівнюватися." },
        new() { Threshold = TimeSpan.FromHours(8), TimeframeLabel = "8–12 год", Title = "Кров отримує більше кисню", Description = "• Рівень чадного газу помітно падає.\n• Кисню в крові стає більше, ніж у перші години." },
        new() { Threshold = TimeSpan.FromHours(24), TimeframeLabel = "1 доба", Title = "Перша доба без диму", Description = "• Організм виводить залишки чадного газу.\n• Легені починають краще відводити слиз і продукти подразнення." },
        new() { Threshold = TimeSpan.FromHours(48), TimeframeLabel = "2 доби", Title = "Нікотин уже позаду", Description = "• Нікотин майже повністю виходить з організму.\n• Смак і нюх часто стають чіткішими." },
        new() { Threshold = TimeSpan.FromHours(72), TimeframeLabel = "3 доби", Title = "Дихати стає легше", Description = "• Спазм бронхів слабшає.\n• Дихання відчувається вільнішим, а енергії може бути більше." },
        new() { Threshold = TimeSpan.FromDays(14), TimeframeLabel = "2–12 тижнів", Title = "Серце й судини підтягуються", Description = "• Кровообіг помітно покращується.\n• Ходити швидко, підійматися сходами й тренуватися часто легше." },
        new() { Threshold = TimeSpan.FromDays(30), TimeframeLabel = "1 місяць", Title = "Легені прибирають наслідки диму", Description = "• Кашель і важкість у грудях часто слабшають.\n• Дихальні шляхи краще прибирають слиз і бруд." },
        new() { Threshold = TimeSpan.FromDays(270), TimeframeLabel = "1–9 місяців", Title = "Більше повітря й енергії", Description = "• Задишка стає рідшою або слабшою.\n• Загальний рівень енергії часто відчутно зростає." },
        new() { Threshold = TimeSpan.FromDays(365), TimeframeLabel = "1 рік", Title = "Серцевий ризик уже нижчий", Description = "• Ризик ішемічної хвороби серця приблизно вдвічі нижчий, ніж у курця.\n• Витривалість і відновлення після навантаження часто кращі." },
        new() { Threshold = TimeSpan.FromDays(365 * 5), TimeframeLabel = "5 років", Title = "Судини працюють спокійніше", Description = "• Ризик інсульту суттєво знижується.\n• Ризики для рота, горла та гортані теж ідуть вниз." },
        new() { Threshold = TimeSpan.FromDays(365 * 10), TimeframeLabel = "10 років", Title = "Довгий виграш для легень", Description = "• Ризик раку легень приблизно вдвічі нижчий, ніж у курця.\n• Також нижчі ризики для нирок, стравоходу й сечового міхура." },
        new() { Threshold = TimeSpan.FromDays(365 * 15), TimeframeLabel = "15 років", Title = "Майже як у некурця", Description = "• Ризик ішемічної хвороби серця близький до рівня людини, яка не курить.\n• Серце й судини менше живуть у режимі постійного стресу." }
    ];

    public static string NormalizeLanguage(string? languageCode)
    {
        return string.Equals(languageCode, Ukrainian, StringComparison.OrdinalIgnoreCase)
            ? Ukrainian
            : English;
    }

    public static string GetString(string languageCode, string key)
    {
        var dictionary = GetStringDictionary(languageCode);
        if (dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        return EnglishStrings.TryGetValue(key, out var fallback)
            ? fallback
            : key;
    }

    public static string Format(string languageCode, string key, params object[] args)
    {
        return string.Format(GetString(languageCode, key), args);
    }

    public static IReadOnlyList<MotivationalPhrase> GetFallbackPhrases(string languageCode)
    {
        return NormalizeLanguage(languageCode) == Ukrainian
            ? UkrainianFallbackPhrases
            : EnglishFallbackPhrases;
    }

    public static string GetMotivationalPhrasesFileName(string languageCode)
    {
        return NormalizeLanguage(languageCode) == Ukrainian
            ? "motivational-phrases.ua.json"
            : "motivational-phrases.en.json";
    }

    public static IReadOnlyList<RecoveryMilestone> GetRecoveryMilestones(string languageCode)
    {
        return NormalizeLanguage(languageCode) == Ukrainian
            ? UkrainianRecoveryMilestones
            : EnglishRecoveryMilestones;
    }

    private static IReadOnlyDictionary<string, string> GetStringDictionary(string languageCode)
    {
        return NormalizeLanguage(languageCode) == Ukrainian
            ? UkrainianStrings
            : EnglishStrings;
    }
}