using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Indiko.Maui.Controls;
public class LineControl : SKCanvasView
{
    private SKBitmap cachedBitmap;

    public static BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor),
        typeof(Color), typeof(LineControl), Colors.LightGray, propertyChanged: OnPropertyChanged);

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    public static BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke),
        typeof(int), typeof(LineControl), 2, propertyChanged: OnPropertyChanged);

    public int Stroke
    {
        get => (int)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public LineControl()
    {
        PaintSurface += OnPaintSurface;
    }

    ~LineControl()
    {
        PaintSurface -= OnPaintSurface;
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (LineControl)bindable;
        view.cachedBitmap?.Dispose();
        view.cachedBitmap = null;
        view.InvalidateSurface();
    }

    protected void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        SKImageInfo info = e.Info;
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;
        canvas.Clear();
        canvas.ClipRect(new SKRect(0, 0, info.Width, info.Height));

        if (cachedBitmap == null)
        {
            cachedBitmap = new SKBitmap(e.Info.Width, e.Info.Height);
            using var bitmapCanvas = new SKCanvas(cachedBitmap);
            DrawLine(bitmapCanvas, info);
        }

        canvas.DrawBitmap(cachedBitmap, 0, 0);
    }

    private void DrawLine(SKCanvas canvas, SKImageInfo info)
    {
        using var linePaint = new SKPaint();
        linePaint.IsAntialias = true;
        linePaint.Color = LineColor.ToSKColor();
        linePaint.Style = SKPaintStyle.Stroke;
        linePaint.StrokeWidth = Stroke;

        canvas.DrawLine(0, 0, info.Width, 0, linePaint);
    }
}