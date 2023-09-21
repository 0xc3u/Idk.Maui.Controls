using CommunityToolkit.Mvvm.ComponentModel;
using Idk.Maui.Controls.Sample.Interfaces;
using Idk.Maui.Controls.Sample.Services;

namespace Idk.Maui.Controls.Sample.ViewModels;

public partial class BaseViewModel : ObservableObject, IViewModel
{
	public INavigationService Navigationservice { get; }

	[ObservableProperty]
	bool isBusy;

	public BaseViewModel(INavigationService navigationService)
	{
		Navigationservice = navigationService;
	}

	public virtual void OnAppearing(object param) { }

	public virtual Task RefreshAsync()
	{
		return Task.CompletedTask;
	}
}
