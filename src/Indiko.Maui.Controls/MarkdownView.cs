using Microsoft.Maui.Controls.Shapes;
using System.Text.RegularExpressions;

namespace Indiko.Maui.Controls;
public class MarkdownView : ContentView
{
    // MarkdownText property
    public static readonly BindableProperty MarkdownTextProperty =
        BindableProperty.Create(nameof(MarkdownText), typeof(string), typeof(MarkdownView), default(string), propertyChanged: OnMarkdownTextChanged);

    public string MarkdownText
    {
        get => (string)GetValue(MarkdownTextProperty);
        set => SetValue(MarkdownTextProperty, value);
    }

    public static readonly BindableProperty LineBreakModeTextProperty =
       BindableProperty.Create(nameof(LineBreakModeText), typeof(LineBreakMode), typeof(MarkdownView), LineBreakMode.WordWrap, propertyChanged: OnMarkdownTextChanged);

    public LineBreakMode LineBreakModeText
    {
        get => (LineBreakMode)GetValue(LineBreakModeTextProperty);
        set => SetValue(LineBreakModeTextProperty, value);
    }


    public static readonly BindableProperty LineBreakModeHeaderProperty =
       BindableProperty.Create(nameof(LineBreakModeHeader), typeof(LineBreakMode), typeof(MarkdownView), LineBreakMode.TailTruncation, propertyChanged: OnMarkdownTextChanged);

    public LineBreakMode LineBreakModeHeader
    {
        get => (LineBreakMode)GetValue(LineBreakModeHeaderProperty);
        set => SetValue(LineBreakModeHeaderProperty, value);
    }

    // H1Color property
    public static readonly BindableProperty H1ColorProperty =
        BindableProperty.Create(nameof(H1Color), typeof(Color), typeof(MarkdownView), Colors.Black, propertyChanged: OnMarkdownTextChanged);

    public Color H1Color
    {
        get => (Color)GetValue(H1ColorProperty);
        set => SetValue(H1ColorProperty, value);
    }

    // H2Color property
    public static readonly BindableProperty H2ColorProperty =
        BindableProperty.Create(nameof(H2Color), typeof(Color), typeof(MarkdownView), Colors.DarkGray, propertyChanged: OnMarkdownTextChanged);

    public Color H2Color
    {
        get => (Color)GetValue(H2ColorProperty);
        set => SetValue(H2ColorProperty, value);
    }

    // H3Color property
    public static readonly BindableProperty H3ColorProperty =
        BindableProperty.Create(nameof(H3Color), typeof(Color), typeof(MarkdownView), Colors.Gray, propertyChanged: OnMarkdownTextChanged);

    public Color H3Color
    {
        get => (Color)GetValue(H3ColorProperty);
        set => SetValue(H3ColorProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
       BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MarkdownView), Colors.Black, propertyChanged: OnMarkdownTextChanged);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty CodeBlockBackgroundColorProperty =
       BindableProperty.Create(nameof(CodeBlockBackgroundColor), typeof(Color), typeof(MarkdownView), Colors.LightGray, propertyChanged: OnMarkdownTextChanged);

    public Color CodeBlockBackgroundColor
    {
        get => (Color)GetValue(CodeBlockBackgroundColorProperty);
        set => SetValue(CodeBlockBackgroundColorProperty, value);
    }
    
    public static readonly BindableProperty CodeBlockBorderColorProperty =
       BindableProperty.Create(nameof(CodeBlockBorderColor), typeof(Color), typeof(MarkdownView), Colors.BlueViolet, propertyChanged: OnMarkdownTextChanged);

    public Color CodeBlockBorderColor
    {
        get => (Color)GetValue(CodeBlockBorderColorProperty);
        set => SetValue(CodeBlockBorderColorProperty, value);
    }

    public static readonly BindableProperty CodeBlockTextColorProperty =
       BindableProperty.Create(nameof(CodeBlockTextColor), typeof(Color), typeof(MarkdownView), Colors.BlueViolet, propertyChanged: OnMarkdownTextChanged);

    public Color CodeBlockTextColor
    {
        get => (Color)GetValue(CodeBlockTextColorProperty);
        set => SetValue(CodeBlockTextColorProperty, value);
    }

    private static void OnMarkdownTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (MarkdownView)bindable;
        control.RenderMarkdown();
    }

    private void RenderMarkdown()
    {
        if (string.IsNullOrWhiteSpace(MarkdownText))
            return;

        // Clear existing content
        Content = null;

        var grid = new Grid
        {
            RowSpacing = 2,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = 5 }, // For bullet points
                new ColumnDefinition { Width = GridLength.Star } // For text
            }
        };

        // Split markdown text by new lines correctly
        var lineSplitSymbole = "\n";

        if (MarkdownText.Contains("\r\n"))
        {
            lineSplitSymbole = "\r\n";
        }
        else if (MarkdownText.Contains("\r"))
        {
            lineSplitSymbole = "\\r";
        }
        else
        {
            lineSplitSymbole = "\\n";
        }

        var lines = MarkdownText.Split(new[] { lineSplitSymbole }, StringSplitOptions.RemoveEmptyEntries);

        int gridRow = 0;
        bool isUnorderedListActive = false;


        foreach (var line in lines.Select(line => line.Trim()))
        {
            // Add a new RowDefinition for the content
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            if (line.StartsWith("# ") || line.StartsWith("## ") || line.StartsWith("### "))
            {
                var label = new Label
                {
                    Text = line.Substring(line.IndexOf(' ') + 1).Trim(),
                    TextColor = line.StartsWith("# ") ? H1Color : line.StartsWith("## ") ? H2Color : H3Color,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = line.StartsWith("# ") ? 24 : line.StartsWith("## ") ? 20 : 18,
                    LineBreakMode = LineBreakModeHeader,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };
                grid.Children.Add(label);
                Grid.SetColumnSpan(label, 2);
                Grid.SetRow(label, gridRow++);
            }
            else if (line.StartsWith("!["))
            {
                // Image handling
                int startIndex = line.IndexOf('(') + 1;
                int endIndex = line.IndexOf(')', startIndex);
                string imageUrl = line.Substring(startIndex, endIndex - startIndex);

                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(imageUrl)),
                    Aspect = Aspect.AspectFit
                };
                grid.Children.Add(image);
                Grid.SetColumnSpan(image, 2);
                Grid.SetRow(image, gridRow++);
            }
            else if (line.StartsWith("- ") || line.StartsWith("* "))
            {
                if (!isUnorderedListActive)
                {
                    isUnorderedListActive = true;
                }

                var bulletPoint = new Label
                {
                    Text = "\u2022",
                    FontSize = 14,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(5, 0)
                };

                grid.Children.Add(bulletPoint);
                Grid.SetRow(bulletPoint, gridRow);
                Grid.SetColumn(bulletPoint, 0);

                var listItemText = line.Substring(2); // Remove the "- " or "* " marker
                var formattedString = CreateFormattedString(listItemText, TextColor); // Use a method to handle markdown within the list item

                var listItemLabel = new Label
                {
                    FormattedText = formattedString,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(20, 0, 0, 0) // Indent the list item text
                };

                grid.Children.Add(listItemLabel);
                Grid.SetRow(listItemLabel, gridRow);
                Grid.SetColumn(listItemLabel, 1);

                gridRow++;
            }
            else if (line.StartsWith("```") && line.EndsWith("```"))
            {
                var codeBlock = CreateCodeBlock(line);
                grid.Children.Add(codeBlock);
                Grid.SetRow(codeBlock, gridRow);
                Grid.SetColumnSpan(codeBlock, 2);
                gridRow++;
            }
            else // Regular text
            {
                if (isUnorderedListActive)
                {
                    isUnorderedListActive = false;
                    // Optionally add extra space after a list
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    grid.Children.Add(new BoxView { Color = Colors.Transparent });
                    gridRow++;
                }

                // Handle regular text and potential formatting
                var formattedString = CreateFormattedString(line, TextColor);
                var label = new Label
                {
                    FormattedText = formattedString,
                    LineBreakMode = LineBreakModeText,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start
                };

                grid.Children.Add(label);
                Grid.SetRow(label, gridRow);
                Grid.SetColumn(label, 0);
                Grid.SetColumnSpan(label, 2); // Span across both columns for normal text

                gridRow++;

                // After handling an element, add an empty row for space
                MarkdownView.AddEmptyRow(grid, ref gridRow);
            }
        }

        Content = grid;
    }

    private static void AddEmptyRow(Grid grid, ref int gridRow)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) }); // Adjust the space as needed
        var spacer = new BoxView { Color = Colors.Transparent };
        grid.Children.Add(spacer);
        Grid.SetColumnSpan(spacer, 2);
        Grid.SetRow(spacer, gridRow++);
    }

    private Frame CreateCodeBlock(string codeText)
    {
        return new Frame
        {
            Padding = new Thickness(10),
            CornerRadius = 4,
            BackgroundColor = CodeBlockBackgroundColor,
            BorderColor = CodeBlockBorderColor,
            Content = new Label
            {
                Text = codeText.Trim('`', ' '),
                FontSize = 12,
                FontAutoScalingEnabled = true,
                FontFamily = "Consolas", // Use a monospaced font family
                TextColor = CodeBlockTextColor, // Use a suitable text color for code
                BackgroundColor = Colors.Transparent
            }
        };
    }

    private FormattedString CreateFormattedString(string line, Color textColor)
    {
        // Create a new FormattedString
        var formattedString = new FormattedString();

        // This is a simple parser and will not handle nested or incorrect markdown well.
        // You may want to use or develop a more robust parser for complex markdown.
        var parts = Regex.Split(line, @"(\*\*.*?\*\*|__.*?__|_.*?_|`.*?`)");

        foreach (var part in parts)
        {
            Span span = new Span();

            if (part.StartsWith("**") && part.EndsWith("**"))
            {
                span.Text = part.Trim('*', ' ');
                span.FontAttributes = FontAttributes.Bold;
            }
            else if (part.StartsWith("_") && part.EndsWith("_"))
            {
                span.Text = part.Trim('_', ' ');
                span.FontAttributes = FontAttributes.Italic;
            }
            else if (part.StartsWith("__") && part.EndsWith("__"))
            {
                span.Text = part.Trim('_', ' ');
                span.FontAttributes = FontAttributes.Bold;
                // If you want bold and italic, you can combine flags:
                // span.FontAttributes = FontAttributes.Bold | FontAttributes.Italic;
            }
            else
            {
                span.Text = part;
            }

            // Apply the same text color for all spans here, or customize as needed.
            span.TextColor = TextColor;

            formattedString.Spans.Add(span);
        }

        return formattedString;
    }
}
