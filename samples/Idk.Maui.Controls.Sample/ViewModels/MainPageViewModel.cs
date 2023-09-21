using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Idk.Maui.Controls.Sample.Services;

namespace Idk.Maui.Controls.Sample.ViewModels;
public partial class MainPageViewModel : BaseViewModel
{
	[ObservableProperty]
	ObservableCollection<Models.MenuItem> menuItems;


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

    public override void OnAppearing(object param)
	{

        //MenuItems = new ObservableCollection<Models.MenuItem>
        //{
        //	new Models.MenuItem("Body Measurements", typeof(BodyMeasurementsView)),
        //	new Models.MenuItem("Vitamins", typeof(VitaminsView))
        //};

        Chips = new ObservableCollection<ChipItem>
        {
            new ChipItem{ Text ="Custom Icons", IsSelectedable = true, 
                            IsCloseable = true, Icon = "house.svg", CloseIcon = "close.svg", BackgroundColor=Colors.LightGray, 
                            SelectedBackgroundColor=Colors.DarkGray, TextColor=Colors.Black},
            new ChipItem{ Text ="Custom Icon, No Close", IsSelectedable = true, IsCloseable = false, Icon ="user.svg", BackgroundColor=Colors.LightGray, SelectedBackgroundColor=Colors.DarkGray, TextColor=Colors.Black},
            new ChipItem{ Text ="Default Close", IsSelectedable = true, IsCloseable = true, BackgroundColor=Colors.DarkViolet, SelectedBackgroundColor=Colors.DarkViolet, TextColor=Colors.White},
            new ChipItem{ Text ="#MAUI", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},
            new ChipItem{ Text ="#DOTNET", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},
            new ChipItem{ Text ="#AWESOME", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, TextColor=Colors.White},

            new ChipItem{ Text ="Disabled", IsDisabled = true, IsCloseable = false, BackgroundColor=Colors.DarkSlateBlue, SelectedBackgroundColor=Colors.DarkSlateBlue, DisabledBackgroundColor=Colors.LightGrey,  TextColor=Colors.Black},
        };


    }
}
