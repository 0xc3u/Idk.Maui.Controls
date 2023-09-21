﻿using Indiko.Maui.Controls.Sample.ViewModels;

namespace Indiko.Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	MainPageViewModel mainPageViewModel;

	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		this.mainPageViewModel = mainPageViewModel;
		BindingContext = mainPageViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		mainPageViewModel.OnAppearing(null);
	}

}
