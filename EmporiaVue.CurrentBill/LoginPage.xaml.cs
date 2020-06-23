using System;
using Xamarin.Essentials;

namespace EmporiaVue.CurrentBill
{
	public partial class LoginPage
    {
        public LoginPage ()
		{
			InitializeComponent ();
            LoginButton.Clicked += OnLoginButtonClicked;
        }
        
        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await SecureStorage.SetAsync("UserName", UsernameEntry.Text);
            await SecureStorage.SetAsync("Password", PasswordEntry.Text);
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync();
        }
    }
}
