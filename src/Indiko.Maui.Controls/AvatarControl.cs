using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace Indiko.Maui.Controls;

public class AvatarControl : SKCanvasView
{
    private SKBitmap cachedBitmap;

    public static BindableProperty AvatarBackgroundColorProperty = BindableProperty.Create(nameof(AvatarBackgroundColor),
     typeof(Color), typeof(AvatarControl), Colors.LightGray, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the background color of the avatar control.
    /// </summary>
    /// <value>The background color of the avatar control.</value>
    public Color AvatarBackgroundColor
    {
        get => (Color)GetValue(AvatarBackgroundColorProperty);
        set => SetValue(AvatarBackgroundColorProperty, value);
    }

    public static BindableProperty InitialsTextColorProperty = BindableProperty.Create(nameof(InitialsTextColor),
              typeof(Color), typeof(AvatarControl), Colors.Black, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the text color for the initials displayed in the avatar control.
    /// </summary>
    /// <value>The text color of the initials.</value>
    public Color InitialsTextColor
    {
        get => (Color)GetValue(InitialsTextColorProperty);
        set => SetValue(InitialsTextColorProperty, value);
    }

    public static BindableProperty AvatarControlImageProperty = BindableProperty.Create(nameof(AvatarControlImage),
               typeof(byte[]), typeof(AvatarControl), null, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the image data to be used as the avatar image.
    /// </summary>
    /// <value>The byte array representing the avatar image.</value>
    public byte[] AvatarControlImage
    {
        get => (byte[])GetValue(AvatarControlImageProperty);
        set => SetValue(AvatarControlImageProperty, value);
    }

    public static BindableProperty InitialsProperty = BindableProperty.Create(nameof(Initials),
      typeof(string), typeof(AvatarControl), string.Empty, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the initials to be displayed in the avatar control.
    /// </summary>
    /// <value>The string representing the initials.</value>
    public string Initials
    {
        get => (string)GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }

    public static BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily),
      typeof(string), typeof(AvatarControl), "OpenSansRegular", propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the font family used for rendering the initials in the avatar control.
    /// </summary>
    /// <value>The name of the font family.</value>
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
        typeof(Color), typeof(AvatarControl), Colors.LightGrey, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the border color of the avatar control.
    /// </summary>
    /// <value>The border color.</value>
    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BorderSizeProperty = BindableProperty.Create(nameof(BorderSize),
        typeof(int), typeof(AvatarControl), 2, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets the size of the border in the avatar control.
    /// </summary>
    /// <value>The size of the border.</value>
    public int BorderSize
    {
        get => (int)GetValue(BorderSizeProperty);
        set => SetValue(BorderSizeProperty, value);
    }

    public static readonly BindableProperty HasBorderProperty = BindableProperty.Create(nameof(HasBorder),
    typeof(bool), typeof(AvatarControl), true, propertyChanged: OnPropertyChanged);

    /// <summary>
    /// Gets or sets a value indicating whether the avatar control should have a border.
    /// </summary>
    /// <value><c>true</c> if this avatar control has a border; otherwise, <c>false</c>.</value>
    public bool HasBorder
    {
        get => (bool)GetValue(HasBorderProperty);
        set => SetValue(HasBorderProperty, value);
    }

    public AvatarControl()
    {
        PaintSurface += OnPaintSurface;
    }

    ~AvatarControl()
    {
        PaintSurface -= OnPaintSurface;
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (AvatarControl)bindable;
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
            DrawAvatarControl(bitmapCanvas, info);
        }
        canvas.DrawBitmap(cachedBitmap, 0, 0);
    }

    private void DrawAvatarControl(SKCanvas canvas, SKImageInfo info)
    {
        canvas.Clear();

        if (AvatarControlImage != null && AvatarControlImage.Length > 0)
        {
            using (var stream = new SKMemoryStream(AvatarControlImage))
            using (var bitmap = SKBitmap.Decode(stream))
            {
                if (bitmap != null)
                {
                    var scaledBitmap = bitmap.Resize(new SKImageInfo(info.Width, info.Height), SKFilterQuality.High);
                    var adjustedBorderSize = HasBorder ? BorderSize / 2f : 0;
                    var circleRect = new SKRect(adjustedBorderSize, adjustedBorderSize, info.Width - adjustedBorderSize, info.Height - adjustedBorderSize);

                    var centerX = circleRect.MidX;
                    var centerY = circleRect.MidY;
                    var radius = Math.Min(circleRect.Width, circleRect.Height) / 2;

                    using (var path = new SKPath())
                    {
                        path.AddCircle(centerX, centerY, radius);
                        canvas.ClipPath(path, SKClipOperation.Intersect);
                        canvas.DrawBitmap(scaledBitmap, circleRect);
                    }
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(Initials))
            {
                DrawBackground(canvas, info);
                canvas.Save();
                using (var textPaint = new SKPaint())
                {
                    textPaint.Color = InitialsTextColor.ToSKColor();
                    textPaint.TextAlign = SKTextAlign.Center;
                    textPaint.TextSize = Math.Min(info.Width, info.Height) / 2;

                    var textBounds = new SKRect();
                    textPaint.MeasureText(Initials, ref textBounds);

                    var textX = info.Rect.MidX;
                    var textY = info.Rect.MidY - (textBounds.Top + textBounds.Bottom) / 2;

                    canvas.DrawText(Initials, textX, textY, textPaint);
                }
                canvas.Restore();
            }
        }

        if (HasBorder)
        {
            canvas.Save();
            DrawBorder(canvas, info);
            canvas.Restore();
        }
    }

    private void DrawBackground(SKCanvas canvas, SKImageInfo info)
    {
        var circleRect = SKRect.Create(info.Rect.Left, info.Rect.Top, info.Rect.Right, info.Rect.Bottom);
        var centerX = circleRect.MidX;
        var centerY = circleRect.MidY;
        var radius = Math.Min(circleRect.Width, circleRect.Height) / 2;

        using var backgroundPaint = new SKPaint();
        backgroundPaint.Style = SKPaintStyle.Fill;
        backgroundPaint.Color = AvatarBackgroundColor.ToSKColor();
        backgroundPaint.IsAntialias = true;
        canvas.DrawCircle(centerX, centerY, radius, backgroundPaint);
    }

    private void DrawBorder(SKCanvas canvas, SKImageInfo info)
    {
        var adjustedBorderSize = BorderSize / 2f;
        var circleRect = new SKRect(adjustedBorderSize, adjustedBorderSize, info.Width - adjustedBorderSize, info.Height - adjustedBorderSize);
        var radius = Math.Min(circleRect.Width, circleRect.Height) / 2;

        using var borderPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = BorderColor.ToSKColor(),
            StrokeWidth = BorderSize,
            IsAntialias = true
        };

        canvas.DrawCircle(circleRect.MidX, circleRect.MidY, radius, borderPaint);
    }
}