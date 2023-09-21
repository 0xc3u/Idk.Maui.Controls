﻿namespace Indiko.Maui.Controls.Sample;

public partial class App : Application
{
	public static IServiceProvider ServiceProvider { get; private set; }

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		ServiceProvider = serviceProvider;

		MainPage = new AppShell();
	}
}