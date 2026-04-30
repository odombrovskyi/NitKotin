namespace NitKotin;

using NitKotin.Models;

public sealed class OverlayForm : Form
{
    private readonly Button _closeButton;
    private readonly Label _hoursValueLabel;
    private readonly Label _savedAmountValueLabel;
    private bool _allowClose;
    private bool _isDragging;
    private Point _dragCursorOrigin;
    private Point _dragFormOrigin;

    public OverlayForm(SmokingConfig config)
    {
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(19, 34, 48);
        ClientSize = new Size(196, 82);
        DoubleBuffered = true;
        FormBorderStyle = FormBorderStyle.None;
        MaximizeBox = false;
        MinimizeBox = false;
        Opacity = 0.75d;
        Padding = new Padding(2);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        TopMost = true;

        var contentPanel = new Panel
        {
            BackColor = Color.Transparent,
            Dock = DockStyle.Fill,
            Padding = new Padding(6, 4, 24, 4)
        };

        var layout = new TableLayoutPanel
        {
            BackColor = Color.Transparent,
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            RowCount = 2
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        _savedAmountValueLabel = CreateValueLabel("0 грн");
        _hoursValueLabel = CreateValueLabel("0 год");

        _closeButton = new Button
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            BackColor = Color.FromArgb(32, 51, 66),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.FromArgb(226, 232, 240),
            Location = new Point(ClientSize.Width - 18, 4),
            Margin = Padding.Empty,
            Size = new Size(14, 14),
            TabStop = false,
            Text = "x",
            UseVisualStyleBackColor = false
        };
        _closeButton.FlatAppearance.BorderSize = 0;
        _closeButton.Click += CloseButton_Click;

        layout.Controls.Add(_savedAmountValueLabel, 0, 0);
        layout.Controls.Add(_hoursValueLabel, 0, 1);

        contentPanel.Controls.Add(layout);
        Controls.Add(contentPanel);
        Controls.Add(_closeButton);
        _closeButton.BringToFront();

        AttachDragHandlers(this);
        AttachDragHandlers(contentPanel);
        AttachDragHandlers(layout);
        RestorePosition(config);
    }

    public event EventHandler? PositionCommitted;

    public void UpdateValues(string savingsText, string smokeFreeHoursText)
    {
        if (IsDisposed)
        {
            return;
        }

        _savedAmountValueLabel.Text = savingsText;
        _hoursValueLabel.Text = smokeFreeHoursText;
    }

    public void AllowClose()
    {
        _allowClose = true;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_allowClose && e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
            return;
        }

        base.OnFormClosing(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Color.FromArgb(88, 148, 163, 184));
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    private static Label CreateValueLabel(string text)
    {
        return new Label
        {
            AutoEllipsis = true,
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.White,
            Margin = Padding.Empty,
            Text = text,
            TextAlign = ContentAlignment.MiddleCenter
        };
    }

    private void RestorePosition(SmokingConfig config)
    {
        var workingArea = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1920, 1080);
        var defaultLocation = new Point(workingArea.Right - Width - 24, workingArea.Top + 24);

        if (config.OverlayLocationX < 0 || config.OverlayLocationY < 0)
        {
            Location = defaultLocation;
            return;
        }

        var maxX = Math.Max(workingArea.Left, workingArea.Right - Width);
        var maxY = Math.Max(workingArea.Top, workingArea.Bottom - Height);
        var clampedX = Math.Min(Math.Max(config.OverlayLocationX, workingArea.Left), maxX);
        var clampedY = Math.Min(Math.Max(config.OverlayLocationY, workingArea.Top), maxY);
        Location = new Point(clampedX, clampedY);
    }

    private void AttachDragHandlers(Control control)
    {
        control.MouseDown += DragStart;
        control.MouseMove += DragMove;
        control.MouseUp += DragEnd;

        foreach (Control child in control.Controls)
        {
            AttachDragHandlers(child);
        }
    }

    private void DragStart(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _isDragging = true;
        _dragCursorOrigin = Cursor.Position;
        _dragFormOrigin = Location;
    }

    private void DragMove(object? sender, MouseEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        var delta = new Size(Cursor.Position.X - _dragCursorOrigin.X, Cursor.Position.Y - _dragCursorOrigin.Y);
        Location = new Point(_dragFormOrigin.X + delta.Width, _dragFormOrigin.Y + delta.Height);
    }

    private void DragEnd(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || !_isDragging)
        {
            return;
        }

        _isDragging = false;
        PositionCommitted?.Invoke(this, EventArgs.Empty);
    }

    private void CloseButton_Click(object? sender, EventArgs e)
    {
        Hide();
    }
}