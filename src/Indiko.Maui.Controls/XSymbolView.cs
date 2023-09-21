using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Indiko.Maui.Controls;
public class XSymbolView : SKCanvasView
{
    public static readonly BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(XSymbolView), Colors.Black, propertyChanged: RedrawCanvas);

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    private static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as XSymbolView;
        view?.InvalidateSurface();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);
        SKImageInfo info = e.Info;
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;

        float padding = 2;  // Add padding value here

        canvas.Clear();

        using SKPaint paint = new();
        paint.Style = SKPaintStyle.Stroke;
        paint.Color = LineColor.ToSKColor();
        paint.StrokeWidth = 3;
        paint.IsAntialias = true;

        // Draw first line from top-left to bottom-right with padding
        canvas.DrawLine(padding, padding, info.Width - padding, info.Height - padding, paint);

        // Draw second line from top-right to bottom-left with padding
        canvas.DrawLine(info.Width - padding, padding, padding, info.Height - padding, paint);

    }
}