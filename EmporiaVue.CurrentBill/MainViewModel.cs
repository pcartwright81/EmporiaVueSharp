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

        private async Task GetData()
        {
            var userName = await SecureStorage.GetAsync("UserName");
            var password = await SecureStorage.GetAsync("Password");

            var api = new VueClient(userName, password);
            var login = await api.Login();
            if (!login) Console.WriteLine("Login Failed");

            if (TokenSource2.IsCancellationRequested) return;
            var customer = await api.GetCustomerInfoAsync(userName);
            var customerWithDevices = await api.GetCustomerWithDevicesAsync(customer.CustomerGid);
            if (TokenSource2.IsCancellationRequested) return;
            var dtNow = DateTime.Now;
            var month = dtNow.Month;
            if (dtNow.Day <= 27) month = dtNow.Month - 1;
            var billDate = new DateTime(2020, month, 27);
            if (TokenSource2.IsCancellationRequested) return;
            var usageByTime = await api.GetUsageByTimeRangeAsync(customerWithDevices.Devices[0].DeviceGid, billDate,
                dtNow, "1H", "WATTS");

            var usageSinceLastBill = usageByTime.Usage.Sum() / 1000; //add all and convert to KW
            var usagePerDay =
                usageSinceLastBill / (DateTime.UtcNow - billDate).TotalDays; //get the total days since last bill
            const double kwCost = .09;
            var totalBillDays = (billDate.AddMonths(1) - billDate).TotalDays;
            var estimatedUsage = usagePerDay * totalBillDays;
            Usage = $"{usageSinceLastBill:F}";
            Estimated = $"{estimatedUsage:F}";
            Average = $"{usagePerDay:F}";
            Total = $"{estimatedUsage * kwCost:F}";
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