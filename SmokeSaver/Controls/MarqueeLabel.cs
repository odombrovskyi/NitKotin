using System.Drawing.Text;

namespace SmokeSaver.Controls;

public sealed class MarqueeLabel : Control
{
    private float _offsetX;
    private int _textWidth;
    private string _marqueeText = string.Empty;

    public MarqueeLabel()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint
            | ControlStyles.OptimizedDoubleBuffer
            | ControlStyles.ResizeRedraw
            | ControlStyles.UserPaint,
            true);

        DoubleBuffered = true;
        BackColor = Color.FromArgb(255, 248, 235);
        ForeColor = Color.FromArgb(146, 64, 14);
        Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        Padding = new Padding(18, 0, 18, 0);
        GapWidth = 64;
        ScrollStep = 1.3f;
    }

    public int GapWidth { get; set; }

    public float ScrollStep { get; set; }

    public string MarqueeText
    {
        get => _marqueeText;
        set
        {
            var normalized = value?.Trim() ?? string.Empty;
            if (string.Equals(_marqueeText, normalized, StringComparison.Ordinal))
            {
                return;
            }

            _marqueeText = normalized;
            RecalculateMetrics(resetOffset: true);
            Invalidate();
        }
    }

    public void Advance()
    {
        if (string.IsNullOrWhiteSpace(_marqueeText) || _textWidth <= 0 || Width <= 0)
        {
            return;
        }

        _offsetX -= ScrollStep;
        var cycleWidth = _textWidth + GapWidth;
        if (_offsetX <= -cycleWidth)
        {
            _offsetX += cycleWidth;
        }

        Invalidate();
    }

    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        RecalculateMetrics(resetOffset: false);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        if (_offsetX == 0 && !string.IsNullOrWhiteSpace(_marqueeText))
        {
            _offsetX = Width;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.Clear(BackColor);

        if (string.IsNullOrWhiteSpace(_marqueeText))
        {
            return;
        }

        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        using var brush = new SolidBrush(ForeColor);
        var textY = (Height - Font.Height) / 2f;

        if (_textWidth <= Width)
        {
            e.Graphics.DrawString(_marqueeText, Font, brush, Padding.Left, textY);
            return;
        }

        var cycleWidth = _textWidth + GapWidth;
        e.Graphics.DrawString(_marqueeText, Font, brush, _offsetX, textY);
        e.Graphics.DrawString(_marqueeText, Font, brush, _offsetX + cycleWidth, textY);
    }

    private void RecalculateMetrics(bool resetOffset)
    {
        _textWidth = string.IsNullOrWhiteSpace(_marqueeText)
            ? 0
            : TextRenderer.MeasureText(_marqueeText, Font, new Size(int.MaxValue, Height), TextFormatFlags.NoPadding).Width;

        if (resetOffset || _offsetX == 0)
        {
            _offsetX = Width > 0 ? Width : 1;
        }
    }
}