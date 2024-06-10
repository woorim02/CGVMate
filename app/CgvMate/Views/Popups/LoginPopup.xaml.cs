using CommunityToolkit.Maui.Views;
namespace CgvMate.Views.Popups;

public partial class LoginPopup : Popup
{
    CgvService service;
	public LoginPopup(CgvService service)
	{
		InitializeComponent();
        this.service = service;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if(string.IsNullOrWhiteSpace(IdEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("로그인", "아이디를 입력해 주세요.", "확인");
        }
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("로그인", "비밀번호를 입력해 주세요.", "확인");
        }
        var result = await service.Auth.LoginAsync(IdEntry.Text, PasswordEntry.Text);
        if (result)
        {
            await CloseAsync(result);
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("로그인", "로그인에 실패하였습니다.\r\n ID, 비밀번호를 확인해 주세요", "확인");
        }
    }
}