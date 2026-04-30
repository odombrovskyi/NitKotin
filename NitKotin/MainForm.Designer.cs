namespace NitKotin;

using NitKotin.Controls;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        headerPanel = new Panel();
        subtitleLabel = new Label();
        titleLabel = new Label();
        motivationMarqueeLabel = new MarqueeLabel();
        configPanel = new Panel();
        autosaveLabel = new Label();
        packPriceNumericUpDown = new NumericUpDown();
        packPriceLabel = new Label();
        packsPerDayNumericUpDown = new NumericUpDown();
        packsPerDayLabel = new Label();
        quitSmokingNowButton = new Button();
        quitDateTimePicker = new DateTimePicker();
        quitDateLabel = new Label();
        savingsPanel = new Panel();
        savedAmountLabel = new Label();
        savedCaptionLabel = new Label();
        smokeFreePanel = new Panel();
        elapsedValueLabel = new Label();
        elapsedCaptionLabel = new Label();
        productsPanel = new Panel();
        productCardPanel3 = new Panel();
        product3DescriptionLabel = new Label();
        product3PriceLabel = new Label();
        product3TitleLabel = new Label();
        productCardPanel2 = new Panel();
        product2DescriptionLabel = new Label();
        product2PriceLabel = new Label();
        product2TitleLabel = new Label();
        productCardPanel1 = new Panel();
        product1DescriptionLabel = new Label();
        product1PriceLabel = new Label();
        product1TitleLabel = new Label();
        productsUpdatedLabel = new Label();
        refreshProductsButton = new Button();
        productsHeaderLabel = new Label();
        recoveryTimelinePanel = new Panel();
        recoveryCardPanel5 = new Panel();
        recoveryDescriptionLabel5 = new Label();
        recoveryTitleLabel5 = new Label();
        recoveryTimeLabel5 = new Label();
        recoveryStateLabel5 = new Label();
        recoveryCardPanel4 = new Panel();
        recoveryDescriptionLabel4 = new Label();
        recoveryTitleLabel4 = new Label();
        recoveryTimeLabel4 = new Label();
        recoveryStateLabel4 = new Label();
        recoveryCardPanel3 = new Panel();
        recoveryDescriptionLabel3 = new Label();
        recoveryTitleLabel3 = new Label();
        recoveryTimeLabel3 = new Label();
        recoveryStateLabel3 = new Label();
        recoveryCardPanel2 = new Panel();
        recoveryDescriptionLabel2 = new Label();
        recoveryTitleLabel2 = new Label();
        recoveryTimeLabel2 = new Label();
        recoveryStateLabel2 = new Label();
        recoveryCardPanel1 = new Panel();
        recoveryDescriptionLabel1 = new Label();
        recoveryTitleLabel1 = new Label();
        recoveryTimeLabel1 = new Label();
        recoveryStateLabel1 = new Label();
        recoveryTimelineLegendLabel = new Label();
        recoveryTimelineHeaderLabel = new Label();
        hintLabel = new Label();
        statusLabel = new Label();
        headerPanel.SuspendLayout();
        configPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)packPriceNumericUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)packsPerDayNumericUpDown).BeginInit();
        savingsPanel.SuspendLayout();
        smokeFreePanel.SuspendLayout();
        productsPanel.SuspendLayout();
        productCardPanel3.SuspendLayout();
        productCardPanel2.SuspendLayout();
        productCardPanel1.SuspendLayout();
        recoveryTimelinePanel.SuspendLayout();
        recoveryCardPanel5.SuspendLayout();
        recoveryCardPanel4.SuspendLayout();
        recoveryCardPanel3.SuspendLayout();
        recoveryCardPanel2.SuspendLayout();
        recoveryCardPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // headerPanel
        // 
        headerPanel.BackColor = Color.FromArgb(28, 44, 54);
        headerPanel.Controls.Add(subtitleLabel);
        headerPanel.Controls.Add(titleLabel);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Margin = new Padding(3, 4, 3, 4);
        headerPanel.Name = "headerPanel";
        headerPanel.Size = new Size(1280, 80);
        headerPanel.TabIndex = 0;
        // 
        // subtitleLabel
        // 
        subtitleLabel.AutoSize = true;
        subtitleLabel.Font = new Font("Segoe UI", 10F);
        subtitleLabel.ForeColor = Color.FromArgb(213, 223, 229);
        subtitleLabel.Location = new Point(35, 92);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(801, 23);
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "Фіксуй дату відмови, стеж за сумою та часом без сигарет, а застосунок сам збереже налаштування.";
        subtitleLabel.Visible = false;
        // 
        // titleLabel
        // 
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
        titleLabel.ForeColor = Color.White;
        titleLabel.Location = new Point(32, 15);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(360, 46);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "Ні! котину мотиватор";
        // 
        // motivationMarqueeLabel
        // 
        motivationMarqueeLabel.BackColor = Color.FromArgb(255, 248, 235);
        motivationMarqueeLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        motivationMarqueeLabel.ForeColor = Color.FromArgb(146, 64, 14);
        motivationMarqueeLabel.GapWidth = 84;
        motivationMarqueeLabel.Location = new Point(32, 91);
        motivationMarqueeLabel.Margin = new Padding(3, 4, 3, 4);
        motivationMarqueeLabel.MarqueeText = "Мотивація завантажується";
        motivationMarqueeLabel.Name = "motivationMarqueeLabel";
        motivationMarqueeLabel.Padding = new Padding(21, 0, 21, 0);
        motivationMarqueeLabel.ScrollStep = 1.35F;
        motivationMarqueeLabel.Size = new Size(1216, 40);
        motivationMarqueeLabel.TabIndex = 1;
        // 
        // configPanel
        // 
        configPanel.BackColor = Color.White;
        configPanel.Controls.Add(autosaveLabel);
        configPanel.Controls.Add(packPriceNumericUpDown);
        configPanel.Controls.Add(packPriceLabel);
        configPanel.Controls.Add(packsPerDayNumericUpDown);
        configPanel.Controls.Add(packsPerDayLabel);
        configPanel.Controls.Add(quitSmokingNowButton);
        configPanel.Controls.Add(quitDateTimePicker);
        configPanel.Controls.Add(quitDateLabel);
        configPanel.Location = new Point(32, 149);
        configPanel.Margin = new Padding(3, 4, 3, 4);
        configPanel.Name = "configPanel";
        configPanel.Size = new Size(297, 315);
        configPanel.TabIndex = 2;
        // 
        // autosaveLabel
        // 
        autosaveLabel.AutoSize = true;
        autosaveLabel.ForeColor = Color.FromArgb(71, 85, 105);
        autosaveLabel.Location = new Point(25, 258);
        autosaveLabel.MaximumSize = new Size(263, 0);
        autosaveLabel.Name = "autosaveLabel";
        autosaveLabel.Size = new Size(245, 40);
        autosaveLabel.TabIndex = 8;
        autosaveLabel.Text = "Зміни зберігаються автоматично після короткої затримки.";
        autosaveLabel.Visible = true;
        // 
        // packPriceNumericUpDown
        // 
        packPriceNumericUpDown.DecimalPlaces = 2;
        packPriceNumericUpDown.Font = new Font("Segoe UI", 10F);
        packPriceNumericUpDown.Increment = new decimal(new int[] { 50, 0, 0, 131072 });
        packPriceNumericUpDown.Location = new Point(25, 211);
        packPriceNumericUpDown.Margin = new Padding(3, 4, 3, 4);
        packPriceNumericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        packPriceNumericUpDown.Name = "packPriceNumericUpDown";
        packPriceNumericUpDown.Size = new Size(137, 30);
        packPriceNumericUpDown.TabIndex = 7;
        packPriceNumericUpDown.Value = new decimal(new int[] { 160, 0, 0, 0 });
        // 
        // packPriceLabel
        // 
        packPriceLabel.AutoSize = true;
        packPriceLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        packPriceLabel.Location = new Point(25, 179);
        packPriceLabel.Name = "packPriceLabel";
        packPriceLabel.Size = new Size(186, 23);
        packPriceLabel.TabIndex = 6;
        packPriceLabel.Text = "Ціна однієї пачки, грн";
        // 
        // packsPerDayNumericUpDown
        // 
        packsPerDayNumericUpDown.DecimalPlaces = 2;
        packsPerDayNumericUpDown.Font = new Font("Segoe UI", 10F);
        packsPerDayNumericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
        packsPerDayNumericUpDown.Location = new Point(25, 131);
        packsPerDayNumericUpDown.Margin = new Padding(3, 4, 3, 4);
        packsPerDayNumericUpDown.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        packsPerDayNumericUpDown.Name = "packsPerDayNumericUpDown";
        packsPerDayNumericUpDown.Size = new Size(137, 30);
        packsPerDayNumericUpDown.TabIndex = 5;
        packsPerDayNumericUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // packsPerDayLabel
        // 
        packsPerDayLabel.AutoSize = true;
        packsPerDayLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        packsPerDayLabel.Location = new Point(25, 99);
        packsPerDayLabel.Name = "packsPerDayLabel";
        packsPerDayLabel.Size = new Size(128, 23);
        packsPerDayLabel.TabIndex = 4;
        packsPerDayLabel.Text = "Пачок на день";
        // 
        // quitSmokingNowButton
        // 
        quitSmokingNowButton.BackColor = Color.FromArgb(22, 163, 74);
        quitSmokingNowButton.FlatStyle = FlatStyle.Flat;
        quitSmokingNowButton.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        quitSmokingNowButton.ForeColor = Color.White;
        quitSmokingNowButton.Location = new Point(25, 27);
        quitSmokingNowButton.Margin = new Padding(3, 4, 3, 4);
        quitSmokingNowButton.Name = "quitSmokingNowButton";
        quitSmokingNowButton.Size = new Size(244, 54);
        quitSmokingNowButton.TabIndex = 3;
        quitSmokingNowButton.Text = "Я кинув палити!";
        quitSmokingNowButton.UseVisualStyleBackColor = false;
        quitSmokingNowButton.Visible = false;
        quitSmokingNowButton.Click += quitSmokingNowButton_Click;
        // 
        // quitDateTimePicker
        // 
        quitDateTimePicker.CustomFormat = "dd.MM.yyyy HH:mm";
        quitDateTimePicker.Font = new Font("Segoe UI", 10F);
        quitDateTimePicker.Format = DateTimePickerFormat.Custom;
        quitDateTimePicker.Location = new Point(25, 51);
        quitDateTimePicker.Margin = new Padding(3, 4, 3, 4);
        quitDateTimePicker.Name = "quitDateTimePicker";
        quitDateTimePicker.ShowUpDown = true;
        quitDateTimePicker.Size = new Size(244, 30);
        quitDateTimePicker.TabIndex = 3;
        // 
        // quitDateLabel
        // 
        quitDateLabel.AutoSize = true;
        quitDateLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        quitDateLabel.Location = new Point(25, 19);
        quitDateLabel.Name = "quitDateLabel";
        quitDateLabel.Size = new Size(258, 23);
        quitDateLabel.TabIndex = 2;
        quitDateLabel.Text = "Дата й час відмови від куріння";
        // 
        // savingsPanel
        // 
        savingsPanel.BackColor = Color.FromArgb(235, 248, 240);
        savingsPanel.Controls.Add(savedAmountLabel);
        savingsPanel.Controls.Add(savedCaptionLabel);
        savingsPanel.Location = new Point(341, 149);
        savingsPanel.Margin = new Padding(3, 4, 3, 4);
        savingsPanel.Name = "savingsPanel";
        savingsPanel.Size = new Size(448, 315);
        savingsPanel.TabIndex = 3;
        // 
        // savedAmountLabel
        // 
        savedAmountLabel.AutoSize = true;
        savedAmountLabel.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        savedAmountLabel.ForeColor = Color.FromArgb(22, 101, 52);
        savedAmountLabel.Location = new Point(23, 96);
        savedAmountLabel.Name = "savedAmountLabel";
        savedAmountLabel.Size = new Size(286, 60);
        savedAmountLabel.TabIndex = 1;
        savedAmountLabel.Text = "0 грн 00 коп";
        // 
        // savedCaptionLabel
        // 
        savedCaptionLabel.AutoSize = true;
        savedCaptionLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        savedCaptionLabel.ForeColor = Color.FromArgb(39, 85, 55);
        savedCaptionLabel.Location = new Point(30, 29);
        savedCaptionLabel.Name = "savedCaptionLabel";
        savedCaptionLabel.Size = new Size(244, 25);
        savedCaptionLabel.TabIndex = 0;
        savedCaptionLabel.Text = "Заощаджено на сигаретах";
        // 
        // smokeFreePanel
        // 
        smokeFreePanel.BackColor = Color.FromArgb(239, 246, 255);
        smokeFreePanel.Controls.Add(elapsedValueLabel);
        smokeFreePanel.Controls.Add(elapsedCaptionLabel);
        smokeFreePanel.Location = new Point(800, 149);
        smokeFreePanel.Margin = new Padding(3, 4, 3, 4);
        smokeFreePanel.Name = "smokeFreePanel";
        smokeFreePanel.Size = new Size(448, 315);
        smokeFreePanel.TabIndex = 4;
        // 
        // elapsedValueLabel
        // 
        elapsedValueLabel.AutoSize = true;
        elapsedValueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        elapsedValueLabel.ForeColor = Color.FromArgb(30, 41, 59);
        elapsedValueLabel.Location = new Point(23, 85);
        elapsedValueLabel.MaximumSize = new Size(389, 0);
        elapsedValueLabel.Name = "elapsedValueLabel";
        elapsedValueLabel.Size = new Size(337, 82);
        elapsedValueLabel.TabIndex = 1;
        elapsedValueLabel.Text = "0 дн. 00 год. 00 хв. 00 сек.";
        // 
        // elapsedCaptionLabel
        // 
        elapsedCaptionLabel.AutoSize = true;
        elapsedCaptionLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        elapsedCaptionLabel.ForeColor = Color.FromArgb(30, 64, 175);
        elapsedCaptionLabel.Location = new Point(30, 29);
        elapsedCaptionLabel.Name = "elapsedCaptionLabel";
        elapsedCaptionLabel.Size = new Size(115, 25);
        elapsedCaptionLabel.TabIndex = 0;
        elapsedCaptionLabel.Text = "Без куріння";
        // 
        // productsPanel
        // 
        productsPanel.BackColor = Color.White;
        productsPanel.Controls.Add(productCardPanel3);
        productsPanel.Controls.Add(productCardPanel2);
        productsPanel.Controls.Add(productCardPanel1);
        productsPanel.Controls.Add(productsUpdatedLabel);
        productsPanel.Controls.Add(refreshProductsButton);
        productsPanel.Controls.Add(productsHeaderLabel);
        productsPanel.Location = new Point(32, 474);
        productsPanel.Margin = new Padding(3, 4, 3, 4);
        productsPanel.Name = "productsPanel";
        productsPanel.Size = new Size(1216, 193);
        productsPanel.TabIndex = 5;
        // 
        // productCardPanel3
        // 
        productCardPanel3.BackColor = Color.FromArgb(248, 250, 252);
        productCardPanel3.Controls.Add(product3DescriptionLabel);
        productCardPanel3.Controls.Add(product3PriceLabel);
        productCardPanel3.Controls.Add(product3TitleLabel);
        productCardPanel3.Location = new Point(825, 58);
        productCardPanel3.Margin = new Padding(3, 4, 3, 4);
        productCardPanel3.Name = "productCardPanel3";
        productCardPanel3.Size = new Size(366, 117);
        productCardPanel3.TabIndex = 6;
        // 
        // product3DescriptionLabel
        // 
        product3DescriptionLabel.AutoSize = true;
        product3DescriptionLabel.ForeColor = Color.FromArgb(71, 85, 105);
        product3DescriptionLabel.Location = new Point(18, 72);
        product3DescriptionLabel.MaximumSize = new Size(327, 0);
        product3DescriptionLabel.Name = "product3DescriptionLabel";
        product3DescriptionLabel.Size = new Size(260, 40);
        product3DescriptionLabel.TabIndex = 2;
        product3DescriptionLabel.Text = "Гаджети · Одна з перших приємних серйозних покупок.";
        // 
        // product3PriceLabel
        // 
        product3PriceLabel.AutoSize = true;
        product3PriceLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        product3PriceLabel.ForeColor = Color.FromArgb(22, 101, 52);
        product3PriceLabel.Location = new Point(18, 43);
        product3PriceLabel.Name = "product3PriceLabel";
        product3PriceLabel.Size = new Size(117, 32);
        product3PriceLabel.TabIndex = 1;
        product3PriceLabel.Text = "2490 грн";
        // 
        // product3TitleLabel
        // 
        product3TitleLabel.AutoSize = true;
        product3TitleLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        product3TitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
        product3TitleLabel.Location = new Point(18, 16);
        product3TitleLabel.MaximumSize = new Size(327, 0);
        product3TitleLabel.Name = "product3TitleLabel";
        product3TitleLabel.Size = new Size(216, 25);
        product3TitleLabel.TabIndex = 0;
        product3TitleLabel.Text = "Бездротові навушники";
        // 
        // productCardPanel2
        // 
        productCardPanel2.BackColor = Color.FromArgb(248, 250, 252);
        productCardPanel2.Controls.Add(product2DescriptionLabel);
        productCardPanel2.Controls.Add(product2PriceLabel);
        productCardPanel2.Controls.Add(product2TitleLabel);
        productCardPanel2.Location = new Point(425, 58);
        productCardPanel2.Margin = new Padding(3, 4, 3, 4);
        productCardPanel2.Name = "productCardPanel2";
        productCardPanel2.Size = new Size(366, 117);
        productCardPanel2.TabIndex = 5;
        // 
        // product2DescriptionLabel
        // 
        product2DescriptionLabel.AutoSize = true;
        product2DescriptionLabel.ForeColor = Color.FromArgb(71, 85, 105);
        product2DescriptionLabel.Location = new Point(18, 72);
        product2DescriptionLabel.MaximumSize = new Size(327, 0);
        product2DescriptionLabel.Name = "product2DescriptionLabel";
        product2DescriptionLabel.Size = new Size(311, 40);
        product2DescriptionLabel.TabIndex = 2;
        product2DescriptionLabel.Text = "Здоров'я · Добра дрібниця для щоденного режиму.";
        // 
        // product2PriceLabel
        // 
        product2PriceLabel.AutoSize = true;
        product2PriceLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        product2PriceLabel.ForeColor = Color.FromArgb(22, 101, 52);
        product2PriceLabel.Location = new Point(18, 43);
        product2PriceLabel.Name = "product2PriceLabel";
        product2PriceLabel.Size = new Size(103, 32);
        product2PriceLabel.TabIndex = 1;
        product2PriceLabel.Text = "720 грн";
        // 
        // product2TitleLabel
        // 
        product2TitleLabel.AutoSize = true;
        product2TitleLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        product2TitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
        product2TitleLabel.Location = new Point(18, 16);
        product2TitleLabel.MaximumSize = new Size(327, 0);
        product2TitleLabel.Name = "product2TitleLabel";
        product2TitleLabel.Size = new Size(173, 25);
        product2TitleLabel.TabIndex = 0;
        product2TitleLabel.Text = "Металева пляшка";
        // 
        // productCardPanel1
        // 
        productCardPanel1.BackColor = Color.FromArgb(248, 250, 252);
        productCardPanel1.Controls.Add(product1DescriptionLabel);
        productCardPanel1.Controls.Add(product1PriceLabel);
        productCardPanel1.Controls.Add(product1TitleLabel);
        productCardPanel1.Location = new Point(25, 58);
        productCardPanel1.Margin = new Padding(3, 4, 3, 4);
        productCardPanel1.Name = "productCardPanel1";
        productCardPanel1.Size = new Size(366, 117);
        productCardPanel1.TabIndex = 4;
        // 
        // product1DescriptionLabel
        // 
        product1DescriptionLabel.AutoSize = true;
        product1DescriptionLabel.ForeColor = Color.FromArgb(71, 85, 105);
        product1DescriptionLabel.Location = new Point(18, 72);
        product1DescriptionLabel.MaximumSize = new Size(327, 0);
        product1DescriptionLabel.Name = "product1DescriptionLabel";
        product1DescriptionLabel.Size = new Size(314, 40);
        product1DescriptionLabel.TabIndex = 2;
        product1DescriptionLabel.Text = "Книги · Невелика, але корисна покупка для нового старту.";
        // 
        // product1PriceLabel
        // 
        product1PriceLabel.AutoSize = true;
        product1PriceLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        product1PriceLabel.ForeColor = Color.FromArgb(22, 101, 52);
        product1PriceLabel.Location = new Point(18, 43);
        product1PriceLabel.Name = "product1PriceLabel";
        product1PriceLabel.Size = new Size(103, 32);
        product1PriceLabel.TabIndex = 1;
        product1PriceLabel.Text = "100 грн";
        // 
        // product1TitleLabel
        // 
        product1TitleLabel.AutoSize = true;
        product1TitleLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        product1TitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
        product1TitleLabel.Location = new Point(18, 16);
        product1TitleLabel.MaximumSize = new Size(327, 0);
        product1TitleLabel.Name = "product1TitleLabel";
        product1TitleLabel.Size = new Size(169, 25);
        product1TitleLabel.TabIndex = 0;
        product1TitleLabel.Text = "Книга про звички";
        // 
        // productsUpdatedLabel
        // 
        productsUpdatedLabel.AutoSize = true;
        productsUpdatedLabel.ForeColor = Color.FromArgb(100, 116, 139);
        productsUpdatedLabel.Location = new Point(25, 64);
        productsUpdatedLabel.Name = "productsUpdatedLabel";
        productsUpdatedLabel.Size = new Size(237, 20);
        productsUpdatedLabel.TabIndex = 2;
        productsUpdatedLabel.Text = "Оновлення: ще не виконувалось";
        productsUpdatedLabel.Visible = false;
        // 
        // refreshProductsButton
        // 
        refreshProductsButton.BackColor = Color.FromArgb(14, 165, 233);
        refreshProductsButton.FlatStyle = FlatStyle.Flat;
        refreshProductsButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        refreshProductsButton.ForeColor = Color.White;
        refreshProductsButton.Location = new Point(1054, 11);
        refreshProductsButton.Margin = new Padding(3, 4, 3, 4);
        refreshProductsButton.Name = "refreshProductsButton";
        refreshProductsButton.Size = new Size(137, 40);
        refreshProductsButton.TabIndex = 1;
        refreshProductsButton.Text = "Оновити добірку";
        refreshProductsButton.TextAlign = ContentAlignment.MiddleCenter;
        refreshProductsButton.UseVisualStyleBackColor = false;
        refreshProductsButton.Click += refreshProductsButton_Click;
        // 
        // productsHeaderLabel
        // 
        productsHeaderLabel.AutoSize = true;
        productsHeaderLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        productsHeaderLabel.ForeColor = Color.FromArgb(30, 41, 59);
        productsHeaderLabel.Location = new Point(25, 13);
        productsHeaderLabel.Name = "productsHeaderLabel";
        productsHeaderLabel.Size = new Size(243, 28);
        productsHeaderLabel.TabIndex = 0;
        productsHeaderLabel.Text = "Що можна купити зараз";
        // 
        // recoveryTimelinePanel
        // 
        recoveryTimelinePanel.BackColor = Color.White;
        recoveryTimelinePanel.Controls.Add(recoveryCardPanel5);
        recoveryTimelinePanel.Controls.Add(recoveryCardPanel4);
        recoveryTimelinePanel.Controls.Add(recoveryCardPanel3);
        recoveryTimelinePanel.Controls.Add(recoveryCardPanel2);
        recoveryTimelinePanel.Controls.Add(recoveryCardPanel1);
        recoveryTimelinePanel.Controls.Add(recoveryTimelineLegendLabel);
        recoveryTimelinePanel.Controls.Add(recoveryTimelineHeaderLabel);
        recoveryTimelinePanel.Location = new Point(32, 681);
        recoveryTimelinePanel.Margin = new Padding(3, 4, 3, 4);
        recoveryTimelinePanel.Name = "recoveryTimelinePanel";
        recoveryTimelinePanel.Size = new Size(1216, 251);
        recoveryTimelinePanel.TabIndex = 6;
        // 
        // recoveryCardPanel5
        // 
        recoveryCardPanel5.BackColor = Color.FromArgb(248, 250, 252);
        recoveryCardPanel5.Controls.Add(recoveryDescriptionLabel5);
        recoveryCardPanel5.Controls.Add(recoveryTitleLabel5);
        recoveryCardPanel5.Controls.Add(recoveryTimeLabel5);
        recoveryCardPanel5.Controls.Add(recoveryStateLabel5);
        recoveryCardPanel5.Location = new Point(958, 13);
        recoveryCardPanel5.Margin = new Padding(3, 4, 3, 4);
        recoveryCardPanel5.Name = "recoveryCardPanel5";
        recoveryCardPanel5.Size = new Size(219, 221);
        recoveryCardPanel5.TabIndex = 6;
        // 
        // recoveryDescriptionLabel5
        // 
        recoveryDescriptionLabel5.AutoSize = true;
        recoveryDescriptionLabel5.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryDescriptionLabel5.Location = new Point(16, 117);
        recoveryDescriptionLabel5.MaximumSize = new Size(192, 0);
        recoveryDescriptionLabel5.Name = "recoveryDescriptionLabel5";
        recoveryDescriptionLabel5.Size = new Size(148, 60);
        recoveryDescriptionLabel5.TabIndex = 3;
        recoveryDescriptionLabel5.Text = "Кашель і задишка у багатьох стають слабшими.";
        // 
        // recoveryTitleLabel5
        // 
        recoveryTitleLabel5.AutoSize = true;
        recoveryTitleLabel5.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTitleLabel5.ForeColor = Color.FromArgb(15, 23, 42);
        recoveryTitleLabel5.Location = new Point(16, 72);
        recoveryTitleLabel5.MaximumSize = new Size(192, 0);
        recoveryTitleLabel5.Name = "recoveryTitleLabel5";
        recoveryTitleLabel5.Size = new Size(144, 23);
        recoveryTitleLabel5.TabIndex = 2;
        recoveryTitleLabel5.Text = "Дихати простіше";
        // 
        // recoveryTimeLabel5
        // 
        recoveryTimeLabel5.AutoSize = true;
        recoveryTimeLabel5.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTimeLabel5.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimeLabel5.Location = new Point(16, 45);
        recoveryTimeLabel5.Name = "recoveryTimeLabel5";
        recoveryTimeLabel5.Size = new Size(73, 23);
        recoveryTimeLabel5.TabIndex = 1;
        recoveryTimeLabel5.Text = "3 місяці";
        // 
        // recoveryStateLabel5
        // 
        recoveryStateLabel5.AutoSize = true;
        recoveryStateLabel5.BackColor = Color.FromArgb(226, 232, 240);
        recoveryStateLabel5.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        recoveryStateLabel5.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryStateLabel5.Location = new Point(16, 19);
        recoveryStateLabel5.Name = "recoveryStateLabel5";
        recoveryStateLabel5.Padding = new Padding(8, 4, 8, 4);
        recoveryStateLabel5.Size = new Size(96, 27);
        recoveryStateLabel5.TabIndex = 0;
        recoveryStateLabel5.Text = "Попереду";
        // 
        // recoveryCardPanel4
        // 
        recoveryCardPanel4.BackColor = Color.FromArgb(248, 250, 252);
        recoveryCardPanel4.Controls.Add(recoveryDescriptionLabel4);
        recoveryCardPanel4.Controls.Add(recoveryTitleLabel4);
        recoveryCardPanel4.Controls.Add(recoveryTimeLabel4);
        recoveryCardPanel4.Controls.Add(recoveryStateLabel4);
        recoveryCardPanel4.Location = new Point(725, 13);
        recoveryCardPanel4.Margin = new Padding(3, 4, 3, 4);
        recoveryCardPanel4.Name = "recoveryCardPanel4";
        recoveryCardPanel4.Size = new Size(219, 221);
        recoveryCardPanel4.TabIndex = 5;
        // 
        // recoveryDescriptionLabel4
        // 
        recoveryDescriptionLabel4.AutoSize = true;
        recoveryDescriptionLabel4.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryDescriptionLabel4.Location = new Point(16, 117);
        recoveryDescriptionLabel4.MaximumSize = new Size(192, 0);
        recoveryDescriptionLabel4.Name = "recoveryDescriptionLabel4";
        recoveryDescriptionLabel4.Size = new Size(131, 60);
        recoveryDescriptionLabel4.TabIndex = 3;
        recoveryDescriptionLabel4.Text = "Часто стає легше переносити навантаження.";
        // 
        // recoveryTitleLabel4
        // 
        recoveryTitleLabel4.AutoSize = true;
        recoveryTitleLabel4.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTitleLabel4.ForeColor = Color.FromArgb(15, 23, 42);
        recoveryTitleLabel4.Location = new Point(16, 72);
        recoveryTitleLabel4.MaximumSize = new Size(192, 0);
        recoveryTitleLabel4.Name = "recoveryTitleLabel4";
        recoveryTitleLabel4.Size = new Size(137, 23);
        recoveryTitleLabel4.TabIndex = 2;
        recoveryTitleLabel4.Text = "Легше рухатися";
        // 
        // recoveryTimeLabel4
        // 
        recoveryTimeLabel4.AutoSize = true;
        recoveryTimeLabel4.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTimeLabel4.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimeLabel4.Location = new Point(16, 45);
        recoveryTimeLabel4.Name = "recoveryTimeLabel4";
        recoveryTimeLabel4.Size = new Size(68, 23);
        recoveryTimeLabel4.TabIndex = 1;
        recoveryTimeLabel4.Text = "2 тижні";
        // 
        // recoveryStateLabel4
        // 
        recoveryStateLabel4.AutoSize = true;
        recoveryStateLabel4.BackColor = Color.FromArgb(226, 232, 240);
        recoveryStateLabel4.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        recoveryStateLabel4.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryStateLabel4.Location = new Point(16, 19);
        recoveryStateLabel4.Name = "recoveryStateLabel4";
        recoveryStateLabel4.Padding = new Padding(8, 4, 8, 4);
        recoveryStateLabel4.Size = new Size(96, 27);
        recoveryStateLabel4.TabIndex = 0;
        recoveryStateLabel4.Text = "Попереду";
        // 
        // recoveryCardPanel3
        // 
        recoveryCardPanel3.BackColor = Color.FromArgb(248, 250, 252);
        recoveryCardPanel3.Controls.Add(recoveryDescriptionLabel3);
        recoveryCardPanel3.Controls.Add(recoveryTitleLabel3);
        recoveryCardPanel3.Controls.Add(recoveryTimeLabel3);
        recoveryCardPanel3.Controls.Add(recoveryStateLabel3);
        recoveryCardPanel3.Location = new Point(491, 13);
        recoveryCardPanel3.Margin = new Padding(3, 4, 3, 4);
        recoveryCardPanel3.Name = "recoveryCardPanel3";
        recoveryCardPanel3.Size = new Size(219, 221);
        recoveryCardPanel3.TabIndex = 4;
        // 
        // recoveryDescriptionLabel3
        // 
        recoveryDescriptionLabel3.AutoSize = true;
        recoveryDescriptionLabel3.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryDescriptionLabel3.Location = new Point(16, 117);
        recoveryDescriptionLabel3.MaximumSize = new Size(192, 0);
        recoveryDescriptionLabel3.Name = "recoveryDescriptionLabel3";
        recoveryDescriptionLabel3.Size = new Size(163, 40);
        recoveryDescriptionLabel3.TabIndex = 3;
        recoveryDescriptionLabel3.Text = "Смак і запах можуть ставати виразнішими.";
        // 
        // recoveryTitleLabel3
        // 
        recoveryTitleLabel3.AutoSize = true;
        recoveryTitleLabel3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTitleLabel3.ForeColor = Color.FromArgb(15, 23, 42);
        recoveryTitleLabel3.Location = new Point(16, 72);
        recoveryTitleLabel3.MaximumSize = new Size(192, 0);
        recoveryTitleLabel3.Name = "recoveryTitleLabel3";
        recoveryTitleLabel3.Size = new Size(152, 23);
        recoveryTitleLabel3.TabIndex = 2;
        recoveryTitleLabel3.Text = "Нікотин виходить";
        // 
        // recoveryTimeLabel3
        // 
        recoveryTimeLabel3.AutoSize = true;
        recoveryTimeLabel3.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTimeLabel3.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimeLabel3.Location = new Point(16, 45);
        recoveryTimeLabel3.Name = "recoveryTimeLabel3";
        recoveryTimeLabel3.Size = new Size(81, 23);
        recoveryTimeLabel3.TabIndex = 1;
        recoveryTimeLabel3.Text = "48 годин";
        // 
        // recoveryStateLabel3
        // 
        recoveryStateLabel3.AutoSize = true;
        recoveryStateLabel3.BackColor = Color.FromArgb(226, 232, 240);
        recoveryStateLabel3.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        recoveryStateLabel3.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryStateLabel3.Location = new Point(16, 19);
        recoveryStateLabel3.Name = "recoveryStateLabel3";
        recoveryStateLabel3.Padding = new Padding(8, 4, 8, 4);
        recoveryStateLabel3.Size = new Size(96, 27);
        recoveryStateLabel3.TabIndex = 0;
        recoveryStateLabel3.Text = "Попереду";
        // 
        // recoveryCardPanel2
        // 
        recoveryCardPanel2.BackColor = Color.FromArgb(248, 250, 252);
        recoveryCardPanel2.Controls.Add(recoveryDescriptionLabel2);
        recoveryCardPanel2.Controls.Add(recoveryTitleLabel2);
        recoveryCardPanel2.Controls.Add(recoveryTimeLabel2);
        recoveryCardPanel2.Controls.Add(recoveryStateLabel2);
        recoveryCardPanel2.Location = new Point(258, 13);
        recoveryCardPanel2.Margin = new Padding(3, 4, 3, 4);
        recoveryCardPanel2.Name = "recoveryCardPanel2";
        recoveryCardPanel2.Size = new Size(219, 221);
        recoveryCardPanel2.TabIndex = 3;
        // 
        // recoveryDescriptionLabel2
        // 
        recoveryDescriptionLabel2.AutoSize = true;
        recoveryDescriptionLabel2.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryDescriptionLabel2.Location = new Point(16, 117);
        recoveryDescriptionLabel2.MaximumSize = new Size(192, 0);
        recoveryDescriptionLabel2.Name = "recoveryDescriptionLabel2";
        recoveryDescriptionLabel2.Size = new Size(163, 40);
        recoveryDescriptionLabel2.TabIndex = 3;
        recoveryDescriptionLabel2.Text = "Кров отримує більше кисню.";
        // 
        // recoveryTitleLabel2
        // 
        recoveryTitleLabel2.AutoSize = true;
        recoveryTitleLabel2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTitleLabel2.ForeColor = Color.FromArgb(15, 23, 42);
        recoveryTitleLabel2.Location = new Point(16, 72);
        recoveryTitleLabel2.MaximumSize = new Size(192, 0);
        recoveryTitleLabel2.Name = "recoveryTitleLabel2";
        recoveryTitleLabel2.Size = new Size(177, 23);
        recoveryTitleLabel2.TabIndex = 2;
        recoveryTitleLabel2.Text = "Менше чадного газу";
        // 
        // recoveryTimeLabel2
        // 
        recoveryTimeLabel2.AutoSize = true;
        recoveryTimeLabel2.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTimeLabel2.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimeLabel2.Location = new Point(16, 45);
        recoveryTimeLabel2.Name = "recoveryTimeLabel2";
        recoveryTimeLabel2.Size = new Size(91, 23);
        recoveryTimeLabel2.TabIndex = 1;
        recoveryTimeLabel2.Text = "24 години";
        // 
        // recoveryStateLabel2
        // 
        recoveryStateLabel2.AutoSize = true;
        recoveryStateLabel2.BackColor = Color.FromArgb(226, 232, 240);
        recoveryStateLabel2.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        recoveryStateLabel2.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryStateLabel2.Location = new Point(16, 19);
        recoveryStateLabel2.Name = "recoveryStateLabel2";
        recoveryStateLabel2.Padding = new Padding(8, 4, 8, 4);
        recoveryStateLabel2.Size = new Size(96, 27);
        recoveryStateLabel2.TabIndex = 0;
        recoveryStateLabel2.Text = "Попереду";
        // 
        // recoveryCardPanel1
        // 
        recoveryCardPanel1.BackColor = Color.FromArgb(248, 250, 252);
        recoveryCardPanel1.Controls.Add(recoveryDescriptionLabel1);
        recoveryCardPanel1.Controls.Add(recoveryTitleLabel1);
        recoveryCardPanel1.Controls.Add(recoveryTimeLabel1);
        recoveryCardPanel1.Controls.Add(recoveryStateLabel1);
        recoveryCardPanel1.Location = new Point(25, 13);
        recoveryCardPanel1.Margin = new Padding(3, 4, 3, 4);
        recoveryCardPanel1.Name = "recoveryCardPanel1";
        recoveryCardPanel1.Size = new Size(219, 221);
        recoveryCardPanel1.TabIndex = 2;
        // 
        // recoveryDescriptionLabel1
        // 
        recoveryDescriptionLabel1.AutoSize = true;
        recoveryDescriptionLabel1.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryDescriptionLabel1.Location = new Point(16, 117);
        recoveryDescriptionLabel1.MaximumSize = new Size(192, 0);
        recoveryDescriptionLabel1.Name = "recoveryDescriptionLabel1";
        recoveryDescriptionLabel1.Size = new Size(157, 40);
        recoveryDescriptionLabel1.TabIndex = 3;
        recoveryDescriptionLabel1.Text = "Перші помітні зміни після відмови.";
        // 
        // recoveryTitleLabel1
        // 
        recoveryTitleLabel1.AutoSize = true;
        recoveryTitleLabel1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTitleLabel1.ForeColor = Color.FromArgb(15, 23, 42);
        recoveryTitleLabel1.Location = new Point(16, 72);
        recoveryTitleLabel1.MaximumSize = new Size(192, 0);
        recoveryTitleLabel1.Name = "recoveryTitleLabel1";
        recoveryTitleLabel1.Size = new Size(160, 46);
        recoveryTitleLabel1.TabIndex = 2;
        recoveryTitleLabel1.Text = "Перший фізичний відгук";
        // 
        // recoveryTimeLabel1
        // 
        recoveryTimeLabel1.AutoSize = true;
        recoveryTimeLabel1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        recoveryTimeLabel1.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimeLabel1.Location = new Point(16, 45);
        recoveryTimeLabel1.Name = "recoveryTimeLabel1";
        recoveryTimeLabel1.Size = new Size(51, 23);
        recoveryTimeLabel1.TabIndex = 1;
        recoveryTimeLabel1.Text = "20 хв";
        // 
        // recoveryStateLabel1
        // 
        recoveryStateLabel1.AutoSize = true;
        recoveryStateLabel1.BackColor = Color.FromArgb(226, 232, 240);
        recoveryStateLabel1.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        recoveryStateLabel1.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryStateLabel1.Location = new Point(16, 19);
        recoveryStateLabel1.Name = "recoveryStateLabel1";
        recoveryStateLabel1.Padding = new Padding(8, 4, 8, 4);
        recoveryStateLabel1.Size = new Size(96, 27);
        recoveryStateLabel1.TabIndex = 0;
        recoveryStateLabel1.Text = "Попереду";
        // 
        // recoveryTimelineLegendLabel
        // 
        recoveryTimelineLegendLabel.AutoSize = true;
        recoveryTimelineLegendLabel.ForeColor = Color.FromArgb(71, 85, 105);
        recoveryTimelineLegendLabel.Location = new Point(25, 59);
        recoveryTimelineLegendLabel.MaximumSize = new Size(1120, 0);
        recoveryTimelineLegendLabel.Name = "recoveryTimelineLegendLabel";
        recoveryTimelineLegendLabel.Size = new Size(636, 20);
        recoveryTimelineLegendLabel.TabIndex = 1;
        recoveryTimelineLegendLabel.Text = "Зелене — уже позаду, помаранчеве — твій поточний етап, нейтральне — наступні зміни.";
        recoveryTimelineLegendLabel.Visible = false;
        // 
        // recoveryTimelineHeaderLabel
        // 
        recoveryTimelineHeaderLabel.AutoSize = true;
        recoveryTimelineHeaderLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        recoveryTimelineHeaderLabel.ForeColor = Color.FromArgb(30, 41, 59);
        recoveryTimelineHeaderLabel.Location = new Point(25, 21);
        recoveryTimelineHeaderLabel.Name = "recoveryTimelineHeaderLabel";
        recoveryTimelineHeaderLabel.Size = new Size(313, 28);
        recoveryTimelineHeaderLabel.TabIndex = 0;
        recoveryTimelineHeaderLabel.Text = "Як організм відновлюється далі";
        recoveryTimelineHeaderLabel.Visible = false;
        // 
        // hintLabel
        // 
        hintLabel.AutoSize = true;
        hintLabel.Font = new Font("Segoe UI", 10F);
        hintLabel.ForeColor = Color.FromArgb(51, 65, 85);
        hintLabel.Location = new Point(32, 1471);
        hintLabel.MaximumSize = new Size(611, 0);
        hintLabel.Name = "hintLabel";
        hintLabel.Size = new Size(411, 23);
        hintLabel.TabIndex = 6;
        hintLabel.Text = "Сума оновлюється автоматично в реальному часі.";
        hintLabel.Visible = false;
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = true;
        statusLabel.ForeColor = Color.FromArgb(71, 85, 105);
        statusLabel.Location = new Point(32, 1508);
        statusLabel.MaximumSize = new Size(965, 0);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(41, 20);
        statusLabel.TabIndex = 7;
        statusLabel.Text = "Стан";
        statusLabel.Visible = false;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        ClientSize = new Size(1280, 959);
        Controls.Add(recoveryTimelinePanel);
        Controls.Add(productsPanel);
        Controls.Add(smokeFreePanel);
        Controls.Add(savingsPanel);
        Controls.Add(configPanel);
        Controls.Add(motivationMarqueeLabel);
        Controls.Add(headerPanel);
        Controls.Add(statusLabel);
        Controls.Add(hintLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(3, 4, 3, 4);
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Ні! котину мотиватор";
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        configPanel.ResumeLayout(false);
        configPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)packPriceNumericUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)packsPerDayNumericUpDown).EndInit();
        savingsPanel.ResumeLayout(false);
        savingsPanel.PerformLayout();
        smokeFreePanel.ResumeLayout(false);
        smokeFreePanel.PerformLayout();
        productsPanel.ResumeLayout(false);
        productsPanel.PerformLayout();
        productCardPanel3.ResumeLayout(false);
        productCardPanel3.PerformLayout();
        productCardPanel2.ResumeLayout(false);
        productCardPanel2.PerformLayout();
        productCardPanel1.ResumeLayout(false);
        productCardPanel1.PerformLayout();
        recoveryTimelinePanel.ResumeLayout(false);
        recoveryTimelinePanel.PerformLayout();
        recoveryCardPanel5.ResumeLayout(false);
        recoveryCardPanel5.PerformLayout();
        recoveryCardPanel4.ResumeLayout(false);
        recoveryCardPanel4.PerformLayout();
        recoveryCardPanel3.ResumeLayout(false);
        recoveryCardPanel3.PerformLayout();
        recoveryCardPanel2.ResumeLayout(false);
        recoveryCardPanel2.PerformLayout();
        recoveryCardPanel1.ResumeLayout(false);
        recoveryCardPanel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel headerPanel;
    private Label titleLabel;
    private Label subtitleLabel;
    private MarqueeLabel motivationMarqueeLabel;
    private Panel configPanel;
    private Label quitDateLabel;
    private Button quitSmokingNowButton;
    private DateTimePicker quitDateTimePicker;
    private Label packsPerDayLabel;
    private NumericUpDown packsPerDayNumericUpDown;
    private Label packPriceLabel;
    private NumericUpDown packPriceNumericUpDown;
    private Label autosaveLabel;
    private Panel savingsPanel;
    private Label savedCaptionLabel;
    private Label savedAmountLabel;
    private Panel smokeFreePanel;
    private Label elapsedCaptionLabel;
    private Label elapsedValueLabel;
    private Panel productsPanel;
    private Label productsHeaderLabel;
    private Button refreshProductsButton;
    private Label productsUpdatedLabel;
    private Panel productCardPanel1;
    private Label product1DescriptionLabel;
    private Label product1PriceLabel;
    private Label product1TitleLabel;
    private Panel productCardPanel2;
    private Label product2DescriptionLabel;
    private Label product2PriceLabel;
    private Label product2TitleLabel;
    private Panel productCardPanel3;
    private Label product3DescriptionLabel;
    private Label product3PriceLabel;
    private Label product3TitleLabel;
    private Panel recoveryTimelinePanel;
    private Label recoveryTimelineHeaderLabel;
    private Label recoveryTimelineLegendLabel;
    private Panel recoveryCardPanel1;
    private Label recoveryStateLabel1;
    private Label recoveryTimeLabel1;
    private Label recoveryTitleLabel1;
    private Label recoveryDescriptionLabel1;
    private Panel recoveryCardPanel2;
    private Label recoveryStateLabel2;
    private Label recoveryTimeLabel2;
    private Label recoveryTitleLabel2;
    private Label recoveryDescriptionLabel2;
    private Panel recoveryCardPanel3;
    private Label recoveryStateLabel3;
    private Label recoveryTimeLabel3;
    private Label recoveryTitleLabel3;
    private Label recoveryDescriptionLabel3;
    private Panel recoveryCardPanel4;
    private Label recoveryStateLabel4;
    private Label recoveryTimeLabel4;
    private Label recoveryTitleLabel4;
    private Label recoveryDescriptionLabel4;
    private Panel recoveryCardPanel5;
    private Label recoveryStateLabel5;
    private Label recoveryTimeLabel5;
    private Label recoveryTitleLabel5;
    private Label recoveryDescriptionLabel5;
    private Label hintLabel;
    private Label statusLabel;
}
