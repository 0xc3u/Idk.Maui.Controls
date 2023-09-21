![](nuget.png)
# idk.Maui.Controls

`idk.Maui.Controls` provides assorted maui ui controls for your NET MAUI application.


## Build Status
![ci](https://github.com/0xc3u/Idk.Maui.Controls/actions/workflows/ci.yml/badge.svg)


## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/Idk.Maui.Controls.svg?label=NuGet)](https://www.nuget.org/packages/Idk.Maui.Controls/)

Available on [NuGet](http://www.nuget.org/packages/Idk.Maui.Controls).

Install with the dotnet CLI: `dotnet add package idk.Maui.Controls`, or through the NuGet Package Manager in Visual Studio.


### Platform supported

| Platform | Minimum Version Supported |
|----------|--------------------------|
| iOS      |   14.2+         |
| Android  |   21+   |



### Dependency Injection

You will first need to register the `idk.maui.controls` with the `MauiAppBuilder` following the same pattern that the .NET MAUI Essentials libraries follow.

```csharp
builder.UseMauiApp<App>()
	   .UseIndikoControls();
```


### Controls available

#### Line Control
A simple Line control that can be used to separate content.
_This control is rendered using Skiasharp_

**Bindable Properties**
- LineColor `Color`
- Stroke `int`

#### Context Help Control 
A simple ContextHelpControl that can be used to show a help icon with a caption, when the user tabs on it a command is propagated for instance to show a popup with further help content.

**Bindable Properties**
- Icon `ImageSource`
- IconSize `Double`
- Caption `String`
- CaptionLineBreakMode `LineBreakMode`
- CaptionFontSize `double`
- CaptionFontFamily `string`
- CaptionForegroundColor `Color`
- CaptionUnderline `bool`
- Animate `bool`
- Command `ICommand`


#### Chips Control
A simple Chips control that can be used to show a list of items that can be selected or not.

**Bindable Properties**
- ItemsSource `IEnumerable`
- SelectedItems `IEnumerable`
