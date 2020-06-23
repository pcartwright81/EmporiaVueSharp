using System;
using Xamarin.Essentials;

namespace EmporiaVue.CurrentBill
{
	public partial class LoginPage 
    {
        public LoginPage ()
		{
			InitializeComponent ();
        }

        private async void OnLoginButtonClicked (object sender, EventArgs e)
		{
            try
            {
                await SecureStorage.SetAsync("UserName", UsernameEntry.Text);
                await SecureStorage.SetAsync("Password", PasswordEntry.Text);
                await SecureStorage.SetAsync("IsUserLoggedIn", true.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving:{ex.Message}.");
                return;
            }

			Navigation.InsertPageBefore(new MainPage(), this);
			await Navigation.PopAsync();
		}		
	}
}
