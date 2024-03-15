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
            await Application.Current.MainPage.DisplayAlert("�α���", "���̵� �Է��� �ּ���.", "Ȯ��");
        }
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("�α���", "��й�ȣ�� �Է��� �ּ���.", "Ȯ��");
        }
        var result = await service.Auth.LoginAsync(IdEntry.Text, PasswordEntry.Text);
        if (result)
        {
            await CloseAsync(result);
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("�α���", "�α��ο� �����Ͽ����ϴ�.\r\n ID, ��й�ȣ�� Ȯ���� �ּ���", "Ȯ��");
        }
    }
}