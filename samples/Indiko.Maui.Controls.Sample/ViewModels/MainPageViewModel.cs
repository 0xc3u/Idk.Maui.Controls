using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Indiko.Maui.Controls.Sample.Services;

namespace Indiko.Maui.Controls.Sample.ViewModels;
public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<ChipItem> chips;

    public MainPageViewModel(INavigationService navigationService) : base(navigationService)
	{
	}

	[RelayCommand]
	Task OpenView(Models.MenuItem menu)
	{
		return Navigationservice.NavigateToViewAsync(menu.ViewType.Name);
	}

    [RelayCommand]
    Task DisplayContextHelp(object param)
    {
        return Task.CompletedTask;
    }

    [ObservableProperty]
    byte[] avatar;

    public override async void OnAppearing(object param)
	{
        Chips = new ObservableCollection<ChipItem>
        {
            new ChipItem{ Text ="Custom Icons", IsSelectedable = true, 
                            IsCloseable = true, Icon = "house.svg", CloseIcon = "close.svg", BackgroundColor=Colors.LightGray, 
                            SelectedBackgroundColor=Colors.DarkGray, TextColor=Colors.Black},
            new ChipItem{ Text ="Custom Icon, No Close", IsSelectedable = true, IsCloseable = false, Icon ="user.svg", BackgroundColor=Colors.LightGray, SelectedBackgroundColor=Colors.DarkGray, TextColor=Colors.Black},
            new ChipItem{ Text ="Default Close", IsSelectedable = true, IsCloseable = true, BackgroundColor=Colors.DarkViolet, SelectedBackgroundColor=Colors.DarkViolet, TextColor=Colors.White},
            new ChipItem{ Text ="Disabled", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.LightGrey, SelectedBackgroundColor=Colors.Grey, DisabledBackgroundColor=Colors.LightGrey,  TextColor=Colors.Grey},
            new ChipItem{ Text ="#MAUI", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},
            new ChipItem{ Text ="#DOTNET", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},
            new ChipItem{ Text ="#AWESOME", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},
        };

        Avatar = await RandomAvatar();
    }

    public static async Task<byte[]> RandomAvatar()
    {
        var avatar = default(byte[]);

        using (var client = new HttpClient())
        {
            avatar = await client.GetByteArrayAsync("https://i.pravatar.cc/150");
        }

        return avatar;
    }

}
