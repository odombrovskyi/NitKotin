namespace SmokeSaver;

using SmokeSaver.Controls;
using SmokeSaver.Models;
using SmokeSaver.Services;

public partial class Form1 : Form
{
    private readonly Icon _appIcon;
    private readonly ConfigService _configService;
    private readonly System.Windows.Forms.Timer _autoSaveTimer;
    private readonly System.Windows.Forms.Timer _marqueeTimer;
    private readonly OverlayForm _overlayForm;
    private readonly NotifyIcon _trayIcon;
    private readonly ToolStripMenuItem _toggleMainWindowMenuItem;
    private readonly ToolStripMenuItem _toggleOverlayMenuItem;
    private readonly MotivationalPhraseService _motivationalPhraseService;
    private readonly ProductSuggestionService _productSuggestionService;
    private readonly RecoveryTimelineService _recoveryTimelineService;
    private readonly System.Windows.Forms.Timer _productsRefreshTimer;
    private readonly System.Windows.Forms.Timer _refreshTimer;
    private decimal _animatedSavedAmount;
    private bool _isInitializing;
    private bool _isExiting;
    private DateTime _lastProductsRefreshLocal;
    private DateTime _lastProductsRefreshRequestUtc = DateTime.MinValue;
    private SmokingConfig _config;
    private decimal _targetSavedAmount;

    public Form1()
    {
        _configService = new ConfigService();
        var catalogService = new ProductCatalogService();
        _motivationalPhraseService = new MotivationalPhraseService();
        _recoveryTimelineService = new RecoveryTimelineService();
        _config = _configService.Load();
        _autoSaveTimer = new System.Windows.Forms.Timer();
        _marqueeTimer = new System.Windows.Forms.Timer();
        _productsRefreshTimer = new System.Windows.Forms.Timer();
        _productSuggestionService = new ProductSuggestionService(catalogService.LoadCatalog());
        _refreshTimer = new System.Windows.Forms.Timer();

        InitializeComponent();
    _appIcon = LoadAppIcon();
    Icon = _appIcon;
        _overlayForm = new OverlayForm(_config);
        _overlayForm.PositionCommitted += OverlayForm_PositionCommitted;
        _overlayForm.VisibleChanged += OverlayForm_VisibleChanged;

        var trayMenu = new ContextMenuStrip();
        trayMenu.Opening += TrayMenu_Opening;
        _toggleOverlayMenuItem = new ToolStripMenuItem();
        _toggleOverlayMenuItem.Click += ToggleOverlayMenuItem_Click;
        _toggleMainWindowMenuItem = new ToolStripMenuItem();
        _toggleMainWindowMenuItem.Click += ToggleMainWindowMenuItem_Click;
        var exitMenuItem = new ToolStripMenuItem("Вихід");
        exitMenuItem.Click += ExitMenuItem_Click;
        trayMenu.Items.AddRange([_toggleOverlayMenuItem, _toggleMainWindowMenuItem, new ToolStripSeparator(), exitMenuItem]);

        _trayIcon = new NotifyIcon
        {
            ContextMenuStrip = trayMenu,
            Icon = _appIcon,
            Text = "Ні! котину мотиватор",
            Visible = false
        };
        _trayIcon.DoubleClick += TrayIcon_DoubleClick;

        _autoSaveTimer.Interval = 600;
        _autoSaveTimer.Tick += AutoSaveTimer_Tick;

        _marqueeTimer.Interval = 25;
        _marqueeTimer.Tick += MarqueeTimer_Tick;

        _productsRefreshTimer.Interval = 30 * 60 * 1000;
        _productsRefreshTimer.Tick += ProductsRefreshTimer_Tick;

        _refreshTimer.Interval = 33;
        _refreshTimer.Tick += RefreshTimer_Tick;

        quitDateTimePicker.ValueChanged += Input_ValueChanged;
        packsPerDayNumericUpDown.ValueChanged += Input_ValueChanged;
        packPriceNumericUpDown.ValueChanged += Input_ValueChanged;

        Load += Form1_Load;
        FormClosing += Form1_FormClosing;
        FormClosed += Form1_FormClosed;
        Resize += Form1_Resize;
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        _isInitializing = true;
        quitDateTimePicker.Value = NormalizeDateTime(_config.QuitDateTime);
        packsPerDayNumericUpDown.Value = ClampDecimal(_config.PacksPerDay, packsPerDayNumericUpDown.Minimum, packsPerDayNumericUpDown.Maximum);
        packPriceNumericUpDown.Value = ClampDecimal(_config.PackPriceUah, packPriceNumericUpDown.Minimum, packPriceNumericUpDown.Maximum);
        _isInitializing = false;

        statusLabel.Text = File.Exists(_configService.ConfigPath)
            ? $"Конфіг завантажено: {_configService.ConfigPath}"
            : $"Конфіг буде збережено в: {_configService.ConfigPath}";

        ConfigureMotivationMarquee();
        UpdateSavingsDisplay();
        UpdateOverlayDisplay();
        RefreshProductSuggestions(forceRefresh: true, reason: "Підібрано товари з локального каталогу.");
        _overlayForm.Show();
        _trayIcon.Visible = true;
        UpdateTrayMenuLabels();
        _marqueeTimer.Start();
        _productsRefreshTimer.Start();
        _refreshTimer.Start();
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && !_isExiting)
        {
            e.Cancel = true;
            Hide();
            UpdateTrayMenuLabels();
            return;
        }

        PersistOverlayPosition();
        _trayIcon.Visible = false;

        if (!_overlayForm.IsDisposed)
        {
            _overlayForm.AllowClose();
            _overlayForm.Close();
        }
    }

    private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _autoSaveTimer.Stop();
        _marqueeTimer.Stop();
        _productsRefreshTimer.Stop();
        _refreshTimer.Stop();
        _trayIcon.Dispose();
        _appIcon.Dispose();
    }

    private static Icon LoadAppIcon()
    {
        var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "icon.ico");
        return File.Exists(iconPath)
            ? new Icon(iconPath)
            : SystemIcons.Application;
    }

    private void saveButton_Click(object? sender, EventArgs e)
    {
        SaveCurrentConfig("Збережено вручну");
    }

    private void Input_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
        {
            return;
        }

        statusLabel.Text = "Виявлено зміни. Автозбереження...";
        UpdateSavingsDisplay();
        _autoSaveTimer.Stop();
        _autoSaveTimer.Start();
    }

    private void AutoSaveTimer_Tick(object? sender, EventArgs e)
    {
        _autoSaveTimer.Stop();
        SaveCurrentConfig("Автозбережено");
    }

    private void SaveCurrentConfig(string statusPrefix)
    {
        _config = BuildCurrentConfig();

        try
        {
            _configService.Save(_config);
            statusLabel.Text = $"{statusPrefix}: {_configService.ConfigPath}";
            UpdateSavingsDisplay();
        }
        catch (IOException ex)
        {
            statusLabel.Text = $"Помилка збереження: {ex.Message}";
        }
        catch (UnauthorizedAccessException ex)
        {
            statusLabel.Text = $"Немає доступу до конфігу: {ex.Message}";
        }
    }

    private void RefreshTimer_Tick(object? sender, EventArgs e)
    {
        UpdateSavingsDisplay();
        UpdateAnimatedAmountDisplay();
        UpdateOverlayDisplay();
    }

    private void MarqueeTimer_Tick(object? sender, EventArgs e)
    {
        motivationMarqueeLabel.Advance();
    }

    private void ProductsRefreshTimer_Tick(object? sender, EventArgs e)
    {
        RefreshProductSuggestions(forceRefresh: true, reason: "Добірку товарів оновлено автоматично.");
    }

    private void refreshProductsButton_Click(object? sender, EventArgs e)
    {
        if (DateTime.UtcNow - _lastProductsRefreshRequestUtc < TimeSpan.FromSeconds(3))
        {
            statusLabel.Text = "Оновлення товарів щойно запускалося. Спробуй ще за кілька секунд.";
            return;
        }

        _lastProductsRefreshRequestUtc = DateTime.UtcNow;
        RefreshProductSuggestions(forceRefresh: true, reason: "Добірку товарів оновлено вручну.");
    }

    private void UpdateSavingsDisplay()
    {
        var liveConfig = new SmokingConfig
        {
            QuitDateTime = quitDateTimePicker.Value,
            PacksPerDay = packsPerDayNumericUpDown.Value,
            PackPriceUah = packPriceNumericUpDown.Value
        };

        var now = DateTime.Now;
        _targetSavedAmount = SavingsCalculator.CalculateSavedAmount(liveConfig, now);
        elapsedValueLabel.Text = SavingsCalculator.FormatElapsed(liveConfig.QuitDateTime, now);
        UpdateRecoveryTimeline(liveConfig.QuitDateTime, now);

        if (liveConfig.QuitDateTime > now)
        {
            hintLabel.Text = "Вказано майбутню дату. Економія почне рахуватися після цього моменту.";
        }
        else if (liveConfig.PacksPerDay <= 0 || liveConfig.PackPriceUah <= 0)
        {
            hintLabel.Text = "Щоб побачити суму, вкажіть кількість пачок і ціну більше нуля.";
        }
        else
        {
            hintLabel.Text = "Сума оновлюється автоматично в реальному часі.";
        }

        RefreshProductSuggestions(forceRefresh: false, reason: null);
    }

    private void UpdateAnimatedAmountDisplay()
    {
        var delta = _targetSavedAmount - _animatedSavedAmount;

        if (Math.Abs(delta) <= 0.01m)
        {
            _animatedSavedAmount = _targetSavedAmount;
        }
        else
        {
            var step = Math.Max(0.01m, Math.Abs(delta) * 0.18m);
            _animatedSavedAmount += Math.Sign(delta) * step;

            if ((delta > 0 && _animatedSavedAmount > _targetSavedAmount)
                || (delta < 0 && _animatedSavedAmount < _targetSavedAmount))
            {
                _animatedSavedAmount = _targetSavedAmount;
            }
        }

        savedAmountLabel.Text = SavingsCalculator.FormatCurrency(_animatedSavedAmount);
    }

    private void UpdateOverlayDisplay()
    {
        if (_overlayForm.IsDisposed)
        {
            return;
        }

        var smokeFreeHours = GetSmokeFreeHours(quitDateTimePicker.Value, DateTime.Now);
        _overlayForm.UpdateValues(FormatOverlaySavings(_animatedSavedAmount), FormatOverlayHours(smokeFreeHours));
        UpdateTrayMenuLabels();
    }

    private void RefreshProductSuggestions(bool forceRefresh, string? reason)
    {
        if (!forceRefresh && _lastProductsRefreshLocal != default && DateTime.Now - _lastProductsRefreshLocal < TimeSpan.FromMinutes(30))
        {
            UpdateProductsMetaLabels();
            return;
        }

        var suggestions = _productSuggestionService.GetSuggestions(_targetSavedAmount);
        if (suggestions.Count == 0)
        {
            ApplySuggestionToCard(product1TitleLabel, product1PriceLabel, product1DescriptionLabel, "Каталог недоступний", "", "Не вдалося завантажити локальні товари.");
            ApplySuggestionToCard(product2TitleLabel, product2PriceLabel, product2DescriptionLabel, "Спробуй пізніше", "", "Перевір JSON-каталог у директорії застосунку.");
            ApplySuggestionToCard(product3TitleLabel, product3PriceLabel, product3DescriptionLabel, "Поки без добірки", "", "Основний лічильник працює, але каталог порожній.");
            return;
        }

        _lastProductsRefreshLocal = DateTime.Now;

        var paddedSuggestions = suggestions
            .Concat(Enumerable.Repeat(new SuggestedProduct
            {
                Title = "Ще трохи і буде нова ціль",
                PriceUah = 0m,
                Category = "Мотивація",
                ShortDescription = "Коли сума підросте, каталог покаже ширший вибір."
            }, 3))
            .Take(3)
            .ToArray();

        ApplySuggestionToCard(product1TitleLabel, product1PriceLabel, product1DescriptionLabel, paddedSuggestions[0].Title, FormatProductPrice(paddedSuggestions[0].PriceUah), BuildProductDescription(paddedSuggestions[0]));
        ApplySuggestionToCard(product2TitleLabel, product2PriceLabel, product2DescriptionLabel, paddedSuggestions[1].Title, FormatProductPrice(paddedSuggestions[1].PriceUah), BuildProductDescription(paddedSuggestions[1]));
        ApplySuggestionToCard(product3TitleLabel, product3PriceLabel, product3DescriptionLabel, paddedSuggestions[2].Title, FormatProductPrice(paddedSuggestions[2].PriceUah), BuildProductDescription(paddedSuggestions[2]));
                
        UpdateProductsMetaLabels();

        if (!string.IsNullOrWhiteSpace(reason))
        {
            statusLabel.Text = reason;
        }
    }

    private void UpdateProductsMetaLabels()
    {
        if (_lastProductsRefreshLocal == default)
        {
            productsUpdatedLabel.Text = "Оновлення: ще не виконувалось";
            return;
        }

        productsUpdatedLabel.Text = $"Оновлено о {_lastProductsRefreshLocal:HH:mm}";
    }

    private static void ApplySuggestionToCard(Label titleLabel, Label priceLabel, Label descriptionLabel, string title, string price, string description)
    {
        titleLabel.Text = title;
        priceLabel.Text = price;
        descriptionLabel.Text = description;
    }

    private static string FormatProductPrice(decimal priceUah)
    {
        return priceUah <= 0m ? "" : $"{priceUah:0} грн";
    }

    private static string BuildProductDescription(SuggestedProduct product)
    {
        if (string.IsNullOrWhiteSpace(product.Category))
        {
            return product.ShortDescription;
        }

        return $"{product.Category} · {product.ShortDescription}";
    }

    private static DateTime NormalizeDateTime(DateTime value)
    {
        if (value < DateTimePicker.MinimumDateTime)
        {
            return DateTimePicker.MinimumDateTime;
        }

        if (value > DateTimePicker.MaximumDateTime)
        {
            return DateTimePicker.MaximumDateTime;
        }

        return value;
    }

    private static decimal ClampDecimal(decimal value, decimal min, decimal max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }

    private SmokingConfig BuildCurrentConfig()
    {
        var overlayLocation = _overlayForm.IsDisposed
            ? new Point(_config.OverlayLocationX, _config.OverlayLocationY)
            : _overlayForm.Location;

        return new SmokingConfig
        {
            QuitDateTime = quitDateTimePicker.Value,
            PacksPerDay = packsPerDayNumericUpDown.Value,
            PackPriceUah = packPriceNumericUpDown.Value,
            OverlayLocationX = overlayLocation.X,
            OverlayLocationY = overlayLocation.Y
        };
    }

    private static int GetSmokeFreeHours(DateTime quitDateTime, DateTime currentTime)
    {
        if (quitDateTime >= currentTime)
        {
            return 0;
        }

        return Math.Max(0, (int)(currentTime - quitDateTime).TotalHours);
    }

    private static string FormatOverlaySavings(decimal amount)
    {
        var roundedAmount = decimal.Round(amount, 0, MidpointRounding.AwayFromZero);
        return $"{roundedAmount:0} грн";
    }

    private static string FormatOverlayHours(int smokeFreeHours)
    {
        return $"{smokeFreeHours:0} год";
    }

    private void PersistOverlayPosition()
    {
        _config = BuildCurrentConfig();

        try
        {
            _configService.Save(_config);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private void OverlayForm_PositionCommitted(object? sender, EventArgs e)
    {
        PersistOverlayPosition();
    }

    private void OverlayForm_VisibleChanged(object? sender, EventArgs e)
    {
        UpdateTrayMenuLabels();
    }

    private void ToggleOverlayMenuItem_Click(object? sender, EventArgs e)
    {
        ToggleOverlayVisibility();
    }

    private void ToggleMainWindowMenuItem_Click(object? sender, EventArgs e)
    {
        ToggleMainWindowVisibility();
    }

    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        _isExiting = true;
        Close();
    }

    private void TrayIcon_DoubleClick(object? sender, EventArgs e)
    {
        ToggleMainWindowVisibility();
    }

    private void TrayMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        UpdateTrayMenuLabels();
    }

    private void Form1_Resize(object? sender, EventArgs e)
    {
        UpdateTrayMenuLabels();
    }

    private void ToggleOverlayVisibility()
    {
        if (_overlayForm.IsDisposed)
        {
            return;
        }

        if (_overlayForm.Visible)
        {
            PersistOverlayPosition();
            _overlayForm.Hide();
        }
        else
        {
            _overlayForm.Show();
            _overlayForm.BringToFront();
        }

        UpdateTrayMenuLabels();
    }

    private void ToggleMainWindowVisibility()
    {
        if (Visible && WindowState != FormWindowState.Minimized)
        {
            Hide();
        }
        else
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            BringToFront();
        }

        UpdateTrayMenuLabels();
    }

    private void UpdateTrayMenuLabels()
    {
        _toggleOverlayMenuItem.Text = _overlayForm.Visible
            ? "Сховати оверлей"
            : "Показати оверлей";

        var isMainVisible = Visible && WindowState != FormWindowState.Minimized;
        _toggleMainWindowMenuItem.Text = isMainVisible
            ? "Сховати основне вікно"
            : "Показати основне вікно";
    }

    private void ConfigureMotivationMarquee()
    {
        var phrases = _motivationalPhraseService.LoadPhrases();
        var text = string.Join("   •   ", phrases.Select(phrase => phrase.Text.Trim()).Where(text => !string.IsNullOrWhiteSpace(text)));

        motivationMarqueeLabel.MarqueeText = string.IsNullOrWhiteSpace(text)
            ? "Ти вже робиш щось важливе для себе."
            : text;
    }

    private void UpdateRecoveryTimeline(DateTime quitDateTime, DateTime now)
    {
        var snapshots = _recoveryTimelineService.GetVisibleMilestones(now - quitDateTime, 5);
        var cards = new[]
        {
            (recoveryCardPanel1, recoveryStateLabel1, recoveryTimeLabel1, recoveryTitleLabel1, recoveryDescriptionLabel1),
            (recoveryCardPanel2, recoveryStateLabel2, recoveryTimeLabel2, recoveryTitleLabel2, recoveryDescriptionLabel2),
            (recoveryCardPanel3, recoveryStateLabel3, recoveryTimeLabel3, recoveryTitleLabel3, recoveryDescriptionLabel3),
            (recoveryCardPanel4, recoveryStateLabel4, recoveryTimeLabel4, recoveryTitleLabel4, recoveryDescriptionLabel4),
            (recoveryCardPanel5, recoveryStateLabel5, recoveryTimeLabel5, recoveryTitleLabel5, recoveryDescriptionLabel5)
        };

        for (var index = 0; index < cards.Length; index++)
        {
            if (index < snapshots.Count)
            {
                ApplyRecoveryCard(
                    cards[index].Item1,
                    cards[index].Item2,
                    cards[index].Item3,
                    cards[index].Item4,
                    cards[index].Item5,
                    snapshots[index]);
                continue;
            }

            cards[index].Item1.Visible = false;
        }
    }

    private static void ApplyRecoveryCard(
        Panel panel,
        Label stateLabel,
        Label timeLabel,
        Label titleLabel,
        Label descriptionLabel,
        RecoveryMilestoneSnapshot snapshot)
    {
        panel.Visible = true;
        timeLabel.Text = snapshot.Milestone.TimeframeLabel;
        titleLabel.Text = snapshot.Milestone.Title;
        descriptionLabel.Text = snapshot.Milestone.Description;

        switch (snapshot.State)
        {
            case RecoveryMilestoneState.Completed:
                panel.BackColor = Color.FromArgb(236, 253, 245);
                stateLabel.BackColor = Color.FromArgb(22, 163, 74);
                stateLabel.ForeColor = Color.White;
                stateLabel.Text = "Вже пройдено";
                timeLabel.ForeColor = Color.FromArgb(21, 128, 61);
                titleLabel.ForeColor = Color.FromArgb(20, 83, 45);
                descriptionLabel.ForeColor = Color.FromArgb(63, 98, 18);
                break;

            case RecoveryMilestoneState.Current:
                panel.BackColor = Color.FromArgb(255, 247, 237);
                stateLabel.BackColor = Color.FromArgb(234, 88, 12);
                stateLabel.ForeColor = Color.White;
                stateLabel.Text = "Поточний етап";
                timeLabel.ForeColor = Color.FromArgb(194, 65, 12);
                titleLabel.ForeColor = Color.FromArgb(124, 45, 18);
                descriptionLabel.ForeColor = Color.FromArgb(154, 52, 18);
                break;

            default:
                panel.BackColor = Color.FromArgb(248, 250, 252);
                stateLabel.BackColor = Color.FromArgb(226, 232, 240);
                stateLabel.ForeColor = Color.FromArgb(71, 85, 105);
                stateLabel.Text = "Попереду";
                timeLabel.ForeColor = Color.FromArgb(71, 85, 105);
                titleLabel.ForeColor = Color.FromArgb(15, 23, 42);
                descriptionLabel.ForeColor = Color.FromArgb(71, 85, 105);
                break;
        }
    }
}
