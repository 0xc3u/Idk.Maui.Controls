using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Windows.Input;

namespace Indiko.Maui.Controls;

public class CheckboxControl : SKCanvasView
{
	private SKBitmap cachedBitmap;
	private readonly TapGestureRecognizer _tapGestureRecognizer;
	private float checkmarkScale = 1.0f;
	public event EventHandler<EventArgs> CheckedChanged;


	public static readonly BindableProperty AnimateProperty = BindableProperty.Create(nameof(Animate), typeof(bool), typeof(CheckboxControl), defaultValue: true);
	public bool Animate
	{
		get => (bool)GetValue(AnimateProperty);
		set => SetValue(AnimateProperty, value);
	}


	public static readonly BindableProperty CheckboxSizeProperty = BindableProperty.Create(nameof(CheckboxSize), typeof(float), typeof(CheckboxControl), defaultValue: 45f, propertyChanged: OnPropertyChanged);
	public float CheckboxSize
	{
		get => (float)GetValue(CheckboxSizeProperty);
		set => SetValue(CheckboxSizeProperty, value);
	}


	public static BindableProperty ForegroundColorProperty = BindableProperty.Create(nameof(ForegroundColor),
		typeof(Color), typeof(CheckboxControl), Colors.LightGray, propertyChanged: OnPropertyChanged);

	public Color ForegroundColor
	{
		get => (Color)GetValue(ForegroundColorProperty);
		set => SetValue(ForegroundColorProperty, value);
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CheckboxControl), propertyChanged: OnCommandUpdated);
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckboxControl), propertyChanged: OnCommandUpdated);
	public object CommandParameter
	{
		get => (object)GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
		typeof(Color), typeof(CheckboxControl), Colors.Black, propertyChanged: OnPropertyChanged);

	public Color TextColor
	{
		get => (Color)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}


	public static new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
		typeof(Color), typeof(CheckboxControl), Colors.LightGray, propertyChanged: OnPropertyChanged);

	public new Color BackgroundColor
	{
		get => (Color)GetValue(BackgroundColorProperty);
		set => SetValue(BackgroundColorProperty, value);
	}

	public static BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke),
		typeof(int), typeof(CheckboxControl), 4, propertyChanged: OnPropertyChanged);

	public int Stroke
	{
		get => (int)GetValue(StrokeProperty);
		set => SetValue(StrokeProperty, value);
	}

	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckboxControl), false);
	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckboxControl), "");
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	private static void OnCommandUpdated(BindableObject bindable, object oldValue, object newValue)
	{
		if (oldValue != newValue)
		{
			var control = (CheckboxControl)bindable;
			control._tapGestureRecognizer.Command = (ICommand)newValue;
		}
	}

	public CheckboxControl()
	{
		_tapGestureRecognizer = new TapGestureRecognizer();
		_tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
		GestureRecognizers.Add(_tapGestureRecognizer);
		PaintSurface += OnPaintSurface;
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		SKTouchEventArgs touchEventArgs = new(int.MaxValue, SKTouchAction.Pressed, default, false);
		OnTouch(touchEventArgs);

		if (Command?.CanExecute(CommandParameter) == true)
		{
			Command?.Execute(CommandParameter);
		}

		if (Animate && IsChecked)
		{
			// Animate the checkmark scale
			await Task.WhenAll(
				ScaleCheckmark(1.2f, 50), // Scale up
				ScaleCheckmark(1.0f, 50)  // Then scale down
			);
		}
	}

	~CheckboxControl()
	{
		_tapGestureRecognizer.Tapped -= TapGestureRecognizer_Tapped;
		GestureRecognizers.Clear();
		PaintSurface -= OnPaintSurface;
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		// Invalidate the surface to redraw the control when size changes
		InvalidateSurface();
	}

	private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var view = (CheckboxControl)bindable;
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

		//if (cachedBitmap == null)
		//{
			cachedBitmap = new SKBitmap(e.Info.Width, e.Info.Height);
			using var bitmapCanvas = new SKCanvas(cachedBitmap);
			DrawCheckbox(bitmapCanvas, info);
		//}

		canvas.DrawBitmap(cachedBitmap, 0, 0);
	}

	private void DrawCheckbox(SKCanvas canvas, SKImageInfo info)
	{
		
		float padding = 10; // Padding between the checkbox and text
		float textOffset = CheckboxSize + padding;

		using var paint = new SKPaint
		{
			Style = SKPaintStyle.Stroke,
			Color = ForegroundColor.ToSKColor(),
			StrokeWidth = Stroke
		};

		// Draw checkbox
		var rect = new SKRect(0, 0, CheckboxSize, CheckboxSize); // Size based on HeightRequest
		canvas.DrawRect(rect, paint);

		// Draw text
		float fontSize = (CheckboxSize * 1.05f); // Adjust this ratio as needed

		using var textPaint = new SKPaint
		{
			Color = TextColor.ToSKColor(),
			TextSize = fontSize
		};
		canvas.DrawText(Text, textOffset, CheckboxSize / 2 + textPaint.TextSize / 2, textPaint);

		// Draw checkmark if checked
		if (IsChecked)
		{
			// Adjust checkmark drawing with scaling
			using var checkmarkPaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = ForegroundColor.ToSKColor(),
				StrokeWidth = Stroke
			};

			float inset = CheckboxSize / 4;
			float centerX = CheckboxSize / 2;
			float centerY = CheckboxSize / 2;

			canvas.Save();
			canvas.Scale(checkmarkScale, checkmarkScale, centerX, centerY); // Scale around the center of the checkbox

			// Draw scaled checkmark
			canvas.DrawLine(inset, centerY, centerX, CheckboxSize - inset, checkmarkPaint);
			canvas.DrawLine(centerX, CheckboxSize - inset, CheckboxSize - inset, inset, checkmarkPaint);

			canvas.Restore();
		}
	}

	protected override void OnTouch(SKTouchEventArgs e)
	{
		if (e.ActionType == SKTouchAction.Pressed)
		{
			IsChecked = !IsChecked;
			CheckedChanged?.Invoke(this, EventArgs.Empty);
			InvalidateSurface(); // Redraw the checkbox
		}
		e.Handled = true;
	}

	private async Task ScaleCheckmark(float targetScale, uint duration)
	{
		float startScale = checkmarkScale;
		await Task.Run(async () =>
		{
			for (float t = 0; t < 1.0f; t += (float)(16.0 / duration)) // 16ms per frame at 60fps
			{
				checkmarkScale = startScale + (targetScale - startScale) * t;
				InvalidateSurface(); // Redraw the checkbox
				await Task.Delay(16); // Wait for the next frame
			}
		});
		checkmarkScale = targetScale;
		InvalidateSurface();
	}
}
