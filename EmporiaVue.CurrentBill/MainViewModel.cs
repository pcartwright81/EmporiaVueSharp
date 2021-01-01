using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EmporiaVue.Api;
using JetBrains.Annotations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmporiaVue.CurrentBill
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _average;
        private string _estimated;
        private string _total;
        private string _usage;

        public MainViewModel()
        {
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
        }

        private CancellationTokenSource TokenSource2 { get; set; }
        public ICommand LogoutCommand => new Command(LogoutAsync);

        public ICommand RefreshCommand => new Command(RefreshAsync);

        public string Total
        {
            get => _total;
            set
            {
                if (value == _total) return;
                _total = value;
                OnPropertyChanged();
            }
        }

        public string Average
        {
            get => _average;
            set
            {
                if (value == _average) return;
                _average = value;
                OnPropertyChanged();
            }
        }

        public string Estimated
        {
            get => _estimated;
            set
            {
                if (value == _estimated) return;
                _estimated = value;
                OnPropertyChanged();
            }
        }

        public string Usage
        {
            get => _usage;
            set
            {
                if (value == _usage) return;
                _usage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RefreshAsync()
        {
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
        }

        public long CustomerGid
        {
            get
            {
                const string key = "CustomerGid";
                var customerGidStr = SecureStorage.GetAsync(key).GetAwaiter().GetResult();
                if (long.TryParse(customerGidStr, out var customerGid))
                {
                    return customerGid;
                }

                var userName = SecureStorage.GetAsync("UserName").GetAwaiter().GetResult();
                var customer = ClientVue.GetCustomerInfoAsync(userName).GetAwaiter().GetResult();
                SecureStorage.SetAsync(key, customer.CustomerGid.ToString()).GetAwaiter().GetResult();
                return customer.CustomerGid;
            }
        }

        public long DeviceGid
        {
            get
            {
                const string key = "DeviceGid";
                var customerGidStr = SecureStorage.GetAsync(key).GetAwaiter().GetResult();
                if (long.TryParse(customerGidStr, out var customerGid))
                {
                    return customerGid;
                }
                var customerWithDevices = ClientVue.GetCustomerWithDevicesAsync(CustomerGid).GetAwaiter().GetResult();
                var deviceGid = customerWithDevices.Devices[0].DeviceGid;
                SecureStorage.SetAsync(key, deviceGid.ToString()).GetAwaiter().GetResult();
                return deviceGid;
            }
        }

        public long KwCost
        {
            get
            {
                const string key = "KWCost";
                var customerGidStr = SecureStorage.GetAsync(key).GetAwaiter().GetResult();
                if (long.TryParse(customerGidStr, out var customerGid))
                {
                    return customerGid;
                }
                var locationInfo = ClientVue.GetDeviceLocationInfoAsync(DeviceGid).GetAwaiter().GetResult();
                var kwCost = locationInfo.UsageCentPerKwHour;
                SecureStorage.SetAsync(key, kwCost.ToString()).GetAwaiter().GetResult();
                return kwCost;
            }
        }

        public VueClient ClientVue { get; set; }
        private async Task GetData()
        {
            var userName = await SecureStorage.GetAsync("UserName");
            var password = await SecureStorage.GetAsync("Password");

            ClientVue = new VueClient(userName, password);
            var login = await ClientVue.Login();
            if (!login)
            {
                throw new Exception("Login Failed");
            }

            var nextBill = await ClientVue.EstimateNextBill(DeviceGid, 25, KwCost);
            Usage = $"{nextBill.UsageSinceDate:F}";
            Estimated = $"{nextBill.EstimatedUsage:F}";
            Average = $"{nextBill.UsagePerDay:F}";
            Total = $"{nextBill.EstimatedCost:F}";
        }

        private async void LogoutAsync()
        {
            TokenSource2.Cancel();
            SecureStorage.RemoveAll();
            var navigation = Application.Current.MainPage.Navigation;
            var homePage = navigation.NavigationStack.First();
            navigation.InsertPageBefore(new LoginPage(), homePage);
            await navigation.PopAsync();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}