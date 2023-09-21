namespace Indiko.Maui.Controls.Sample.Interfaces;
interface IViewModel
{
	bool IsBusy { get; set; }
	void OnAppearing(object param);
	Task RefreshAsync();
}
