﻿using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Idk.Maui.Controls;
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
            paint.StrokeWidth = 4;
            paint.IsAntialias = true;
            
            canvas.DrawCircle(centerX, centerY, radius, paint);

            paint.TextSize = (radius + 0.25f);
            SKRect textBounds = new();
            paint.MeasureText("?", ref textBounds);
            float textX = centerX - textBounds.Width / 2;
            float textY = centerY + textBounds.Height / 2;
            canvas.DrawText("?", textX, textY, paint);
        }
    }
}