using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;

namespace EmporiaVue.CurrentBill
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _password;
        private string _userName;

        public ICommand LoginCommand => new Command(LoginAsync);

        public string UserName
        {
            get => _userName;
            set
            {
                if (value == _userName) return;
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private async void LoginAsync()
        {
            if (UserName == null)
            {
                throw new ArgumentNullException(nameof(UserName));
            }
            if (Password == null)
            {
                throw new ArgumentNullException(nameof(Password));
            }
            await SecureStorage.SetAsync("UserName", UserName);
            await SecureStorage.SetAsync("Password", Password);
            var navigation = Application.Current.MainPage.Navigation;
            var homePage = navigation.NavigationStack.First();
            navigation.InsertPageBefore(new MainPage(), homePage);
            await navigation.PopAsync();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}