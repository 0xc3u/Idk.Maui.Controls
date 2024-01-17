using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Indiko.Maui.Controls;
public class QuestionMarkView : SKCanvasView
{
    public static readonly BindableProperty LineColorProperty =
        BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(QuestionMarkView), Colors.Black);

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
    {
        base.OnPaintSurface(args);

        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        float centerX = info.Width / 2f;
        float centerY = info.Height / 2f;
        float radius = Math.Min(centerX, centerY) * 0.85f;

        using (SKPaint paint = new SKPaint())
        {
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = LineColor.ToSKColor();
            paint.StrokeWidth = 6;
            paint.IsAntialias = true;
            
            canvas.DrawCircle(centerX, centerY, radius, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.StrokeWidth = 0;
            paint.TextSize = (radius * 1.45f);
            SKRect textBounds = new();
            paint.MeasureText("?", ref textBounds);
            float textX = (centerX -2) - textBounds.Width / 2;
            float textY = centerY + textBounds.Height / 2;
            canvas.DrawText("?", textX, textY, paint);
        }
    }

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		// Invalidate the surface to redraw the control when size changes
		InvalidateSurface();
	}
}