using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace EmporiaVue.CurrentBill
{
    public class App : Application
    {
        public static bool IsUserLoggedIn
        {
            get
            {
                try
                {
                    var secureStorage = SecureStorage.GetAsync("UserName").Result;
                    return secureStorage != null;
                }
                catch
                {
                    return false;
                }


            }
        }

        public App()
        {
            MainPage = !IsUserLoggedIn ? new NavigationPage(new LoginPage()) : new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

