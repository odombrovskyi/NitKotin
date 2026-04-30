Add-Type -AssemblyName System.Drawing

$ErrorActionPreference = 'Stop'
$assetsDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$iconPath = Join-Path $assetsDir 'icon.ico'
$previewPath = Join-Path $assetsDir 'icon-preview.png'

function New-RoundedRectPath {
    param(
        [float]$X,
        [float]$Y,
        [float]$Width,
        [float]$Height,
        [float]$Radius
    )

    $path = New-Object System.Drawing.Drawing2D.GraphicsPath
    $diameter = $Radius * 2
    $path.AddArc($X, $Y, $diameter, $diameter, 180, 90)
    $path.AddArc($X + $Width - $diameter, $Y, $diameter, $diameter, 270, 90)
    $path.AddArc($X + $Width - $diameter, $Y + $Height - $diameter, $diameter, $diameter, 0, 90)
    $path.AddArc($X, $Y + $Height - $diameter, $diameter, $diameter, 90, 90)
    $path.CloseFigure()
    return $path
}

function New-IconBitmap {
    param([int]$Size)

    $bitmap = New-Object System.Drawing.Bitmap $Size, $Size
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.Clear([System.Drawing.Color]::Transparent)

    $scale = $Size / 256.0

    $bgBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 22, 52, 71))
    $bodyBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 248, 250, 252))
    $emberBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 249, 115, 22))
    $ashBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(230, 203, 213, 225))
    $slashBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 239, 68, 68))
    $slashHighlightBrush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(180, 255, 245, 245))

    $bgPath = New-RoundedRectPath -X (20 * $scale) -Y (20 * $scale) -Width (216 * $scale) -Height (216 * $scale) -Radius (52 * $scale)
    $graphics.FillPath($bgBrush, $bgPath)

    $cigarettePath = New-RoundedRectPath -X (68 * $scale) -Y (110 * $scale) -Width (120 * $scale) -Height (32 * $scale) -Radius (16 * $scale)
    $graphics.FillPath($bodyBrush, $cigarettePath)

    $emberPath = New-RoundedRectPath -X (177 * $scale) -Y (110 * $scale) -Width (22 * $scale) -Height (32 * $scale) -Radius (10 * $scale)
    $graphics.FillPath($emberBrush, $emberPath)

    $ashPath = New-RoundedRectPath -X (62 * $scale) -Y (118 * $scale) -Width (16 * $scale) -Height (16 * $scale) -Radius (8 * $scale)
    $graphics.FillPath($ashBrush, $ashPath)

    $slashPen = New-Object System.Drawing.Pen $slashBrush.Color, (28 * $scale)
    $slashPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $slashPen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $graphics.DrawLine($slashPen, 52 * $scale, 58 * $scale, 198 * $scale, 204 * $scale)

    $highlightPen = New-Object System.Drawing.Pen $slashHighlightBrush.Color, (8 * $scale)
    $highlightPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $highlightPen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $graphics.DrawLine($highlightPen, 48 * $scale, 54 * $scale, 194 * $scale, 200 * $scale)

    $highlightPen.Dispose()
    $slashPen.Dispose()
    $ashPath.Dispose()
    $emberPath.Dispose()
    $cigarettePath.Dispose()
    $bgPath.Dispose()
    $slashHighlightBrush.Dispose()
    $slashBrush.Dispose()
    $ashBrush.Dispose()
    $emberBrush.Dispose()
    $bodyBrush.Dispose()
    $bgBrush.Dispose()
    $graphics.Dispose()

    return $bitmap
}

$bitmap = New-IconBitmap -Size 256
$bitmap.Save($previewPath, [System.Drawing.Imaging.ImageFormat]::Png)

$iconHandle = $bitmap.GetHicon()
$icon = [System.Drawing.Icon]::FromHandle($iconHandle)
$fileStream = [System.IO.File]::Create($iconPath)
$icon.Save($fileStream)
$fileStream.Dispose()

$icon.Dispose()
$bitmap.Dispose()

Add-Type @"
using System;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool DestroyIcon(IntPtr handle);
}
"@

[NativeMethods]::DestroyIcon($iconHandle) | Out-Null
