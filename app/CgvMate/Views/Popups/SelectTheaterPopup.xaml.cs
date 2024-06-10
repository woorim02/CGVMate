using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace CgvMate.Views.Popups;

public partial class SelectTheaterPopup : Popup
{
	SelectTheaterViewModel viewModel;

	public SelectTheaterPopup(CgvService service)
	{
		InitializeComponent();
		viewModel = new SelectTheaterViewModel(service);
		BindingContext = viewModel;
	}

    private async void Popup_Opened(object sender, PopupOpenedEventArgs e)
    {
		await viewModel.LoadAsync();
    }

    private void TheaterButton_Clicked(object sender, EventArgs e)
    {
		var button = sender as Button;
		CloseAsync(button?.CommandParameter);
    }
}