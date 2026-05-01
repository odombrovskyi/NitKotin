namespace NitKotin;

using NitKotin.Controls;
using NitKotin.Models;
using NitKotin.Services;

public partial class MainForm : Form
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
    private readonly ProductCatalogService _productCatalogService;
    private ProductSuggestionService _productSuggestionService;
    private readonly RecoveryTimelineService _recoveryTimelineService;
    private readonly System.Windows.Forms.Timer _productsRefreshTimer;
    private readonly System.Windows.Forms.Timer _refreshTimer;
    private readonly Image? _englishFlagImage;
    private readonly Image? _ukrainianFlagImage;
    private decimal _animatedSavedAmount;
    private bool _isInitializing;
    private bool _isExiting;
    private bool _isAwaitingFirstQuitConfirmation;
    private DateTime _lastProductsRefreshLocal;
    private DateTime _lastProductsRefreshRequestUtc = DateTime.MinValue;
    private SmokingConfig _config;
    private string _currentLanguage = LocalizationService.English;
    private decimal _targetSavedAmount;

    public MainForm()
    {
        _configService = new ConfigService();
        _productCatalogService = new ProductCatalogService();
        _motivationalPhraseService = new MotivationalPhraseService();
        _recoveryTimelineService = new RecoveryTimelineService();
        _config = _configService.Load();
        _currentLanguage = LocalizationService.NormalizeLanguage(_config.LanguagePreference);
        _isAwaitingFirstQuitConfirmation = !_config.HasStartedTracking;
        _autoSaveTimer = new System.Windows.Forms.Timer();
        _marqueeTimer = new System.Windows.Forms.Timer();
        _productsRefreshTimer = new System.Windows.Forms.Timer();
        _productSuggestionService = new ProductSuggestionService(_productCatalogService.LoadCatalog(_currentLanguage));
        _refreshTimer = new System.Windows.Forms.Timer();

        InitializeComponent();
        _englishFlagImage = LoadFlagImage("flag-us.png");
        _ukrainianFlagImage = LoadFlagImage("flag-ua.png");
        UpdateQuitDateInputMode();
        _appIcon = LoadAppIcon();
        Icon = _appIcon;
        _overlayForm = new OverlayForm(_config);
        _overlayForm.PositionCommitted += OverlayForm_PositionCommitted;
        _overlayForm.OverlayDoubleClicked += OverlayForm_OverlayDoubleClicked;
        _overlayForm.VisibleChanged += OverlayForm_VisibleChanged;

        var trayMenu = new ContextMenuStrip();
        trayMenu.Opening += TrayMenu_Opening;
        _toggleOverlayMenuItem = new ToolStripMenuItem();
        _toggleOverlayMenuItem.Click += ToggleOverlayMenuItem_Click;
        _toggleMainWindowMenuItem = new ToolStripMenuItem();
        _toggleMainWindowMenuItem.Click += ToggleMainWindowMenuItem_Click;
        var exitMenuItem = new ToolStripMenuItem(T("TrayExit"));
        exitMenuItem.Click += ExitMenuItem_Click;
        trayMenu.Items.AddRange([_toggleOverlayMenuItem, _toggleMainWindowMenuItem, new ToolStripSeparator(), exitMenuItem]);

        _trayIcon = new NotifyIcon
        {
            ContextMenuStrip = trayMenu,
            Icon = _appIcon,
            Text = T("AppTitle"),
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

        Load += MainForm_Load;
        Shown += MainForm_Shown;
        FormClosing += MainForm_FormClosing;
        FormClosed += MainForm_FormClosed;
        Resize += MainForm_Resize;
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        _isInitializing = true;
        quitDateTimePicker.Value = NormalizeDateTime(_config.QuitDateTime);
        packsPerDayNumericUpDown.Value = ClampDecimal(_config.PacksPerDay, packsPerDayNumericUpDown.Minimum, packsPerDayNumericUpDown.Maximum);
        packPriceNumericUpDown.Value = ClampDecimal(_config.PackPriceUah, packPriceNumericUpDown.Minimum, packPriceNumericUpDown.Maximum);
        _isInitializing = false;
        UpdateQuitDateInputMode();
        ApplyLocalizedStaticTexts();

        statusLabel.Text = _isAwaitingFirstQuitConfirmation
            ? F("ConfigStatusFirstRun", _configService.ConfigPath)
            : File.Exists(_configService.ConfigPath)
                ? F("ConfigStatusLoaded", _configService.ConfigPath)
                : F("ConfigStatusWillBeSaved", _configService.ConfigPath);

        ConfigureMotivationMarquee();
        UpdateSavingsDisplay();
        UpdateOverlayDisplay();
        RefreshProductSuggestions(forceRefresh: true, reason: T("StatusProductsLoaded"));
        _overlayForm.Show();
        _trayIcon.Visible = true;
        UpdateTrayMenuLabels();
        _marqueeTimer.Start();
        _productsRefreshTimer.Start();
        _refreshTimer.Start();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        PersistOverlayPosition();
        _trayIcon.Visible = false;

        if (!_overlayForm.IsDisposed)
        {
            _overlayForm.AllowClose();
            _overlayForm.Close();
        }
    }

    private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _autoSaveTimer.Stop();
        _marqueeTimer.Stop();
        _productsRefreshTimer.Stop();
        _refreshTimer.Stop();
        _trayIcon.Dispose();
        _appIcon.Dispose();
        _englishFlagImage?.Dispose();
        _ukrainianFlagImage?.Dispose();
    }

    private void MainForm_Shown(object? sender, EventArgs e)
    {
        EnsureMainWindowVisible();
    }

    private static Icon LoadAppIcon()
    {
        var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "icon.ico");
        return File.Exists(iconPath)
            ? new Icon(iconPath)
            : SystemIcons.Application;
    }

    private static Image? LoadFlagImage(string fileName)
    {
        var imagePath = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);
        if (!File.Exists(imagePath))
        {
            return null;
        }

        using var source = Image.FromFile(imagePath);
        return new Bitmap(source);
    }

    private void quitSmokingNowButton_Click(object? sender, EventArgs e)
    {
        _autoSaveTimer.Stop();
        _isInitializing = true;
        quitDateTimePicker.Value = NormalizeDateTime(DateTime.Now);
        _isInitializing = false;
        _isAwaitingFirstQuitConfirmation = false;
        UpdateQuitDateInputMode();
        SaveCurrentConfig(T("StatusQuitStartedSaved"));
        UpdateOverlayDisplay();
    }

    private void languageEnglishButton_Click(object? sender, EventArgs e)
    {
        SetLanguage(LocalizationService.English);
    }

    private void languageUkrainianButton_Click(object? sender, EventArgs e)
    {
        SetLanguage(LocalizationService.Ukrainian);
    }

    private void Input_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing)
        {
            return;
        }

        if (_isAwaitingFirstQuitConfirmation)
        {
            statusLabel.Text = T("StatusPendingFirstRun");
            UpdateSavingsDisplay(forceRefreshDependentViews: true);
            UpdateAnimatedAmountDisplay();
            UpdateOverlayDisplay();
            return;
        }

        statusLabel.Text = T("StatusChangesDetected");
        UpdateSavingsDisplay(forceRefreshDependentViews: true);
        UpdateAnimatedAmountDisplay();
        UpdateOverlayDisplay();
        _autoSaveTimer.Stop();
        _autoSaveTimer.Start();
    }

    private void AutoSaveTimer_Tick(object? sender, EventArgs e)
    {
        _autoSaveTimer.Stop();
        SaveCurrentConfig(T("StatusAutoSaved"));
    }

    private void SaveCurrentConfig(string statusPrefix)
    {
        if (_isAwaitingFirstQuitConfirmation)
        {
            statusLabel.Text = T("StatusNeedFirstRunButton");
            UpdateSavingsDisplay();
            return;
        }

        _config = BuildCurrentConfig();

        try
        {
            _configService.Save(_config);
            statusLabel.Text = F("StatusSavedFormat", statusPrefix, _configService.ConfigPath);
            UpdateSavingsDisplay();
        }
        catch (IOException ex)
        {
            statusLabel.Text = F("StatusSaveError", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            statusLabel.Text = F("StatusAccessError", ex.Message);
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
        RefreshProductSuggestions(forceRefresh: true, reason: T("StatusProductsAutoRefreshed"));
    }

    private void refreshProductsButton_Click(object? sender, EventArgs e)
    {
        if (DateTime.UtcNow - _lastProductsRefreshRequestUtc < TimeSpan.FromSeconds(3))
        {
            statusLabel.Text = T("StatusProductsTooSoon");
            return;
        }

        _lastProductsRefreshRequestUtc = DateTime.UtcNow;
        RefreshProductSuggestions(forceRefresh: true, reason: T("StatusProductsManualRefreshed"));
    }

    private void UpdateSavingsDisplay(bool forceRefreshDependentViews = false)
    {
        if (_isAwaitingFirstQuitConfirmation)
        {
            _targetSavedAmount = 0m;
            _animatedSavedAmount = 0m;
            savedAmountLabel.Text = SavingsCalculator.FormatCurrency(0m, _currentLanguage);
            elapsedValueLabel.Text = T("ElapsedNotStarted");
            hintLabel.Text = T("HintFirstRun");
            recoveryTimelineLegendLabel.Text = T("RecoveryLegendFirstRun");
            HideRecoveryTimelineCards();
            RefreshProductSuggestions(forceRefresh: forceRefreshDependentViews, reason: null);
            return;
        }

        var liveConfig = new SmokingConfig
        {
            QuitDateTime = quitDateTimePicker.Value,
            PacksPerDay = packsPerDayNumericUpDown.Value,
            PackPriceUah = packPriceNumericUpDown.Value
        };

        var now = DateTime.Now;
        _targetSavedAmount = SavingsCalculator.CalculateSavedAmount(liveConfig, now);
        elapsedValueLabel.Text = SavingsCalculator.FormatElapsed(liveConfig.QuitDateTime, now, _currentLanguage);
        UpdateRecoveryTimeline(liveConfig.QuitDateTime, now);

        if (liveConfig.QuitDateTime > now)
        {
            hintLabel.Text = T("HintFutureDate");
        }
        else if (liveConfig.PacksPerDay <= 0 || liveConfig.PackPriceUah <= 0)
        {
            hintLabel.Text = T("HintNeedPositiveValues");
        }
        else
        {
            hintLabel.Text = T("HintRealtime");
        }

        RefreshProductSuggestions(forceRefresh: forceRefreshDependentViews, reason: null);
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

        savedAmountLabel.Text = SavingsCalculator.FormatCurrency(_animatedSavedAmount, _currentLanguage);
    }

    private void UpdateOverlayDisplay()
    {
        if (_overlayForm.IsDisposed)
        {
            return;
        }

        if (_isAwaitingFirstQuitConfirmation)
        {
            _overlayForm.UpdateValues(FormatOverlaySavings(0m, _currentLanguage), FormatOverlayHours(0, _currentLanguage));
            UpdateTrayMenuLabels();
            return;
        }

        var smokeFreeHours = GetSmokeFreeHours(quitDateTimePicker.Value, DateTime.Now);
        _overlayForm.UpdateValues(FormatOverlaySavings(_animatedSavedAmount, _currentLanguage), FormatOverlayHours(smokeFreeHours, _currentLanguage));
        UpdateTrayMenuLabels();
    }

    private void UpdateQuitDateInputMode()
    {
        quitSmokingNowButton.Visible = _isAwaitingFirstQuitConfirmation;
        quitDateLabel.Visible = !_isAwaitingFirstQuitConfirmation;
        quitDateTimePicker.Visible = !_isAwaitingFirstQuitConfirmation;
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
            ApplySuggestionToCard(product1TitleLabel, product1PriceLabel, product1DescriptionLabel, T("CatalogUnavailableTitle"), string.Empty, T("CatalogUnavailableDescription"));
            ApplySuggestionToCard(product2TitleLabel, product2PriceLabel, product2DescriptionLabel, T("CatalogTryLaterTitle"), string.Empty, T("CatalogTryLaterDescription"));
            ApplySuggestionToCard(product3TitleLabel, product3PriceLabel, product3DescriptionLabel, T("CatalogEmptyTitle"), string.Empty, T("CatalogEmptyDescription"));
            return;
        }

        _lastProductsRefreshLocal = DateTime.Now;

        var paddedSuggestions = suggestions
            .Concat(Enumerable.Repeat(new SuggestedProduct
            {
                Title = T("CatalogGoalTitle"),
                PriceUah = 0m,
                Category = T("CatalogGoalCategory"),
                ShortDescription = T("CatalogGoalDescription")
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
            productsUpdatedLabel.Text = T("ProductsNeverUpdated");
            return;
        }

        productsUpdatedLabel.Text = F("ProductsUpdatedAtFormat", _lastProductsRefreshLocal);
    }

    private static void ApplySuggestionToCard(Label titleLabel, Label priceLabel, Label descriptionLabel, string title, string price, string description)
    {
        titleLabel.Text = title;
        priceLabel.Text = price;
        descriptionLabel.Text = description;
    }

    private string FormatProductPrice(decimal priceUah)
    {
        return priceUah <= 0m ? string.Empty : $"{priceUah:0} {T("CurrencyMajor")}";
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
            LanguagePreference = _currentLanguage,
            HasStartedTracking = !_isAwaitingFirstQuitConfirmation,
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

    private static string FormatOverlaySavings(decimal amount, string languageCode)
    {
        var roundedAmount = decimal.Round(amount, 0, MidpointRounding.AwayFromZero);
        return $"{roundedAmount:0} {LocalizationService.GetString(languageCode, "CurrencyMajor")}";
    }

    private static string FormatOverlayHours(int smokeFreeHours, string languageCode)
    {
        return $"{smokeFreeHours:0} {LocalizationService.GetString(languageCode, "OverlayHours")}";
    }

    private void PersistOverlayPosition()
    {
        if (_isAwaitingFirstQuitConfirmation)
        {
            return;
        }

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

    private void OverlayForm_OverlayDoubleClicked(object? sender, EventArgs e)
    {
        EnsureMainWindowVisible();
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
        EnsureMainWindowVisible();
    }

    private void TrayMenu_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        UpdateTrayMenuLabels();
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized && !_isExiting)
        {
            Hide();
            UpdateTrayMenuLabels();
            return;
        }

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

    private void EnsureMainWindowVisible()
    {
        if (!Visible)
        {
            Show();
        }

        WindowState = FormWindowState.Normal;
        Activate();
        BringToFront();
        UpdateTrayMenuLabels();
    }

    private void UpdateTrayMenuLabels()
    {
        _toggleOverlayMenuItem.Text = _overlayForm.Visible
            ? T("TrayHideOverlay")
            : T("TrayShowOverlay");

        var isMainVisible = Visible && WindowState != FormWindowState.Minimized;
        _toggleMainWindowMenuItem.Text = isMainVisible
            ? T("TrayHideMainWindow")
            : T("TrayShowMainWindow");
    }

    private void ConfigureMotivationMarquee()
    {
        var phrases = _motivationalPhraseService.LoadPhrases(_currentLanguage);
        var text = string.Join("   •   ", phrases.Select(phrase => phrase.Text.Trim()).Where(text => !string.IsNullOrWhiteSpace(text)));

        motivationMarqueeLabel.MarqueeText = string.IsNullOrWhiteSpace(text)
            ? T("MotivationFallback")
            : text;
    }

    private void UpdateRecoveryTimeline(DateTime quitDateTime, DateTime now)
    {
        recoveryTimelineLegendLabel.Text = T("RecoveryLegend");
        var snapshots = _recoveryTimelineService.GetVisibleMilestones(now - quitDateTime, 5, _currentLanguage);
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

    private void HideRecoveryTimelineCards()
    {
        recoveryCardPanel1.Visible = false;
        recoveryCardPanel2.Visible = false;
        recoveryCardPanel3.Visible = false;
        recoveryCardPanel4.Visible = false;
        recoveryCardPanel5.Visible = false;
    }

    private void ApplyRecoveryCard(
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
                stateLabel.Text = T("RecoveryStateCompleted");
                timeLabel.ForeColor = Color.FromArgb(21, 128, 61);
                titleLabel.ForeColor = Color.FromArgb(20, 83, 45);
                descriptionLabel.ForeColor = Color.FromArgb(63, 98, 18);
                break;

            case RecoveryMilestoneState.Current:
                panel.BackColor = Color.FromArgb(255, 247, 237);
                stateLabel.BackColor = Color.FromArgb(234, 88, 12);
                stateLabel.ForeColor = Color.White;
                stateLabel.Text = T("RecoveryStateCurrent");
                timeLabel.ForeColor = Color.FromArgb(194, 65, 12);
                titleLabel.ForeColor = Color.FromArgb(124, 45, 18);
                descriptionLabel.ForeColor = Color.FromArgb(154, 52, 18);
                break;

            default:
                panel.BackColor = Color.FromArgb(248, 250, 252);
                stateLabel.BackColor = Color.FromArgb(226, 232, 240);
                stateLabel.ForeColor = Color.FromArgb(71, 85, 105);
                stateLabel.Text = T("RecoveryStateUpcoming");
                timeLabel.ForeColor = Color.FromArgb(71, 85, 105);
                titleLabel.ForeColor = Color.FromArgb(15, 23, 42);
                descriptionLabel.ForeColor = Color.FromArgb(71, 85, 105);
                break;
        }
    }

    private void ApplyLocalizedStaticTexts()
    {
        Text = T("AppTitle");
        _trayIcon.Text = T("AppTitle");
        titleLabel.Text = T("AppTitle");
        motivationMarqueeLabel.MarqueeText = T("MotivationLoading");
        quitDateLabel.Text = T("QuitDateLabel");
        packsPerDayLabel.Text = T("PacksPerDayLabel");
        packPriceLabel.Text = T("PackPriceLabel");
        autosaveLabel.Text = T("AutosaveLabel");
        quitSmokingNowButton.Text = T("QuitNowButton");
        savedCaptionLabel.Text = T("SavedCaption");
        elapsedCaptionLabel.Text = T("SmokeFreeCaption");
        productsHeaderLabel.Text = T("ProductsHeader");
        refreshProductsButton.Text = T("ProductsRefreshButton");
        recoveryTimelineHeaderLabel.Text = T("RecoveryHeader");
        languageEnglishButton.BackgroundImage = _englishFlagImage;
        languageEnglishButton.BackgroundImageLayout = ImageLayout.Zoom;
        languageEnglishButton.Text = string.Empty;
        languageUkrainianButton.BackgroundImage = _ukrainianFlagImage;
        languageUkrainianButton.BackgroundImageLayout = ImageLayout.Zoom;
        languageUkrainianButton.Text = string.Empty;
        UpdateLanguageButtonStyles();
    }

    private void SetLanguage(string languageCode)
    {
        var normalizedLanguage = LocalizationService.NormalizeLanguage(languageCode);
        if (_currentLanguage == normalizedLanguage)
        {
            UpdateLanguageButtonStyles();
            return;
        }

        _currentLanguage = normalizedLanguage;
        _productSuggestionService = new ProductSuggestionService(_productCatalogService.LoadCatalog(_currentLanguage));
        ApplyLocalizedStaticTexts();
        statusLabel.Text = _isAwaitingFirstQuitConfirmation
            ? F("ConfigStatusFirstRun", _configService.ConfigPath)
            : F("ConfigStatusLoaded", _configService.ConfigPath);
        ConfigureMotivationMarquee();
        UpdateSavingsDisplay(forceRefreshDependentViews: true);
        UpdateAnimatedAmountDisplay();
        UpdateOverlayDisplay();
        UpdateTrayMenuLabels();
        PersistLanguagePreference();
    }

    private void PersistLanguagePreference()
    {
        try
        {
            _config = BuildCurrentConfig();
            _configService.Save(_config);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private void UpdateLanguageButtonStyles()
    {
        ApplyLanguageButtonStyle(languageEnglishButton, _currentLanguage == LocalizationService.English);
        ApplyLanguageButtonStyle(languageUkrainianButton, _currentLanguage == LocalizationService.Ukrainian);
    }

    private static void ApplyLanguageButtonStyle(Button button, bool isSelected)
    {
        button.FlatAppearance.BorderSize = isSelected ? 2 : 1;
        button.FlatAppearance.BorderColor = isSelected
            ? Color.FromArgb(251, 191, 36)
            : Color.FromArgb(71, 85, 105);
    }

    private string T(string key)
    {
        return LocalizationService.GetString(_currentLanguage, key);
    }

    private string F(string key, params object[] args)
    {
        return LocalizationService.Format(_currentLanguage, key, args);
    }
}
