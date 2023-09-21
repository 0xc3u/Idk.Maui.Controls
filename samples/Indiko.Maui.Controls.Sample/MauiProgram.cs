using CommunityToolkit.Maui;
using Indiko.Maui.Controls.Sample.Services;
using Indiko.Maui.Controls.Sample.ViewModels;

namespace Indiko.Maui.Controls.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseMauiCommunityToolkit()
			.UseIndikoControls();

        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingletonWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage));
		

		return builder.Build();
    }
}