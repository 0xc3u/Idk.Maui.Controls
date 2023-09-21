using System.Windows.Input;

namespace Indiko.Maui.Controls;

/// <summary>
/// Represents a custom view that provides contextual help features.
/// </summary>
/// <remarks>
/// This control allows customization of various properties including icon, caption, and actions when tapped.
/// </remarks>
public partial class ContextHelpControl : ContentView
{
    private readonly TapGestureRecognizer _tapGestureRecognizer;

    public static readonly BindableProperty HelpIdProperty = BindableProperty.Create(nameof(HelpId), typeof(string), typeof(ContextHelpControl), defaultValue: string.Empty, propertyChanged: OnHelpIdChanged);
    /// <summary>
    /// Gets or sets the HelpId of the context help control.
    /// </summary>
    public string HelpId
    {
        get => (string)GetValue(HelpIdProperty);
        set => SetValue(HelpIdProperty, value);
    }

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(ContextHelpControl), null, propertyChanged: OnIconChanged);

    /// <summary>
    /// Gets or sets the icon image source for the control.
    /// </summary>
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(double), typeof(ContextHelpControl), defaultValue: 24d, propertyChanged: OnIconSizeChanged);

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly BindableProperty CaptionProperty = BindableProperty.Create(nameof(Caption), typeof(string), typeof(ContextHelpControl),
        defaultValue: string.Empty, propertyChanged: OnCaptionChanged);

    /// <summary>
    /// Gets or sets the caption text of the control.
    /// </summary>
    public string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    public static readonly BindableProperty CaptionLineBreakModeProperty = BindableProperty.Create(nameof(CaptionLineBreakMode),
        typeof(LineBreakMode), typeof(ContextHelpControl), defaultValue: LineBreakMode.TailTruncation, propertyChanged: OnCaptionLineBraeakModeChanged);

    /// <summary>
    /// Gets or sets the line break mode of the caption.
    /// </summary>
    public LineBreakMode CaptionLineBreakMode
    {
        get => (LineBreakMode)GetValue(CaptionLineBreakModeProperty);
        set => SetValue(CaptionLineBreakModeProperty, value);
    }

    public static readonly BindableProperty CaptionFontSizeProperty = BindableProperty.Create(nameof(CaptionFontSize),
        typeof(double), typeof(ContextHelpControl), defaultValue: 18d, propertyChanged: OnCaptionFontSizeChanged);

    /// <summary>
    /// Gets or sets the font size of the caption.
    /// </summary>
    public double CaptionFontSize
    {
        get => (double)GetValue(CaptionFontSizeProperty);
        set => SetValue(CaptionFontSizeProperty, value);
    }

    public static readonly BindableProperty CaptionUnderlineProperty = BindableProperty.Create(nameof(CaptionUnderline),
       typeof(bool), typeof(ContextHelpControl), defaultValue: true, propertyChanged: OnCaptionUnderlineChanged);

    /// <summary>
    /// Gets or sets a value indicating whether to underline the caption.
    /// </summary>
    public bool CaptionUnderline
    {
        get => (bool)GetValue(CaptionUnderlineProperty);
        set => SetValue(CaptionUnderlineProperty, value);
    }

    public static readonly BindableProperty CaptionFontFamilyProperty = BindableProperty.Create(nameof(CaptionFontFamily),
       typeof(string), typeof(ContextHelpControl), defaultValue: string.Empty, propertyChanged: OnCaptionFontFamilyChanged);

    /// <summary>
    /// Gets or sets the font family of the caption.
    /// </summary>
    public string CaptionFontFamily
    {
        get => (string)GetValue(CaptionFontFamilyProperty);
        set => SetValue(CaptionFontFamilyProperty, value);
    }

    public static readonly BindableProperty CaptionForegroundColorProperty = BindableProperty.Create(nameof(CaptionForegroundColor),
        typeof(Color), typeof(ContextHelpControl), defaultValue: Colors.Black, propertyChanged: OnCaptionForegroundColorChanged);

    /// <summary>
    /// Gets or sets the foreground color of the caption.
    /// </summary>
    public Color CaptionForegroundColor
    {
        get => (Color)GetValue(CaptionForegroundColorProperty);
        set => SetValue(CaptionForegroundColorProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContextHelpControl), propertyChanged: OnCommandUpdated);

    /// <summary>
    /// Gets or sets the command to execute when the control is tapped.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty AnimateProperty = BindableProperty.Create(nameof(Animate), typeof(bool), typeof(ContextHelpControl), defaultValue: true);

    /// <summary>
    /// Gets or sets a value indicating whether the tap action should invoke an animation.
    /// </summary>
    public bool Animate
    {
        get => (bool)GetValue(AnimateProperty);
        set => SetValue(AnimateProperty, value);
    }

    public ContextHelpControl()
    {
        InitializeComponent();
        _tapGestureRecognizer = new TapGestureRecognizer();
        _tapGestureRecognizer.Tapped += OnTapped;
        GestureRecognizers.Add(_tapGestureRecognizer);
    }

    private async void OnTapped(object sender, EventArgs args)
    {
        if (Command?.CanExecute(HelpId) == true)
        {
            Command?.Execute(HelpId);
        }

        if (Animate)
        {
            await this.ScaleTo(0.95, 50);
            await this.ScaleTo(1.0, 50);
        }
    }

    ~ContextHelpControl()
    {
        if (_tapGestureRecognizer != null)
        {
            _tapGestureRecognizer.Tapped -= OnTapped;
            GestureRecognizers.Remove(_tapGestureRecognizer);
        }
    }

    private static void OnCommandUpdated(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control._tapGestureRecognizer.Command = (ICommand)newValue;
        }
    }

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.IconImage.Source = (ImageSource)newValue;

            if(control.IconImage.Source == null)
            {
                control.IconImage.IsVisible = false;
            }
            else
            {
                control.IconImage.IsVisible = true;
            }
        }
    }

    private static void OnIconSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.IconImage.WidthRequest = (double)newValue;
            control.IconImage.HeightRequest = (double)newValue;

            control.questionMarkView.WidthRequest = (double)newValue;
            control.questionMarkView.HeightRequest = (double)newValue;

        }
    }

    private static void OnCaptionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.Text = (string)newValue;
        }
    }

    private static void OnCaptionFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.FontSize = (double)newValue;
        }
    }

    private static void OnCaptionFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.FontFamily = (string)newValue;
        }
    }

    private static void OnCaptionForegroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.TextColor = (Color)newValue;
            control.questionMarkView.LineColor = (Color)newValue;
        }
    }

    private static void OnCaptionLineBraeakModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.LineBreakMode = (LineBreakMode)newValue;
        }
    }

    private static void OnCaptionUnderlineChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control.QuestionLabel.TextDecorations = ((bool)newValue) ? TextDecorations.Underline : TextDecorations.None;
        }
    }

    private static void OnHelpIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var control = (ContextHelpControl)bindable;
            control._tapGestureRecognizer.CommandParameter = newValue;
        }
    }
}