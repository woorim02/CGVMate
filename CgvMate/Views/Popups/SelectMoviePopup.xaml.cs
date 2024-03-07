using CommunityToolkit.Maui.Views;

namespace CgvMate.Views.Popups;

public partial class SelectMoviePopup : Popup
{
    SelectMovieViewModel viewModel;

	public SelectMoviePopup(CgvService service)
	{
		InitializeComponent();
        viewModel = new SelectMovieViewModel(service);
        BindingContext = viewModel;
	}

    private async void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        await viewModel.LoadAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var movie = e.Parameter as Movie;
        await CloseAsync(movie);
    }
}