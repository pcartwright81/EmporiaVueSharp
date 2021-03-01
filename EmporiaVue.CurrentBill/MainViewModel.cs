using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EmporiaVue.Api;
using EmporiaVue.Api.Models;
using JetBrains.Annotations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmporiaVue.CurrentBill
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private NextBillEstimate _nextBill;

        public MainViewModel()
        {
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
        }

        private CancellationTokenSource TokenSource2 { get; set; }
        public ICommand LogoutCommand => new Command(LogoutAsync);

        public ICommand RefreshCommand => new Command(RefreshAsync);

        public NextBillEstimate NextBill
        {
            get => _nextBill;
            set
            {
                if (value == _nextBill) return;
                _nextBill = value;
                OnPropertyChanged();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void RefreshAsync()
        {
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
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

            var deviceGid = GetDeviceId();
            var (billDay, costPerKwHour) = GetBillingInfo(deviceGid);
            
            NextBill = await ClientVue.EstimateNextBill(deviceGid, billDay, costPerKwHour);
        }

        private Tuple<int, long> GetBillingInfo(long deviceGid)
        {
            const string billDayKey = "BillDay";
            const string kwCostKey = "KWCost";
            long kwCost;
            var billDayStr = SecureStorage.GetAsync(billDayKey).GetAwaiter().GetResult();
            if (!int.TryParse(billDayStr, out var billDay))
            {
                var locationInfo = ClientVue.GetDeviceLocationInfoAsync(deviceGid).GetAwaiter().GetResult();
                billDay = locationInfo.BillingCycleStartDay;
                kwCost = locationInfo.UsageCentPerKwHour;
                SecureStorage.SetAsync(billDayKey, billDay.ToString()).GetAwaiter().GetResult();
                SecureStorage.SetAsync(kwCostKey, kwCost.ToString()).GetAwaiter().GetResult();
                return new Tuple<int, long>(billDay, kwCost);
            }
            var kwCostStr = SecureStorage.GetAsync(kwCostKey).GetAwaiter().GetResult();
            if (long.TryParse(kwCostStr, out kwCost)) return new Tuple<int, long>(billDay, kwCost);
            {
                var locationInfo = ClientVue.GetDeviceLocationInfoAsync(deviceGid).GetAwaiter().GetResult();
                billDay = locationInfo.BillingCycleStartDay;
                kwCost = locationInfo.UsageCentPerKwHour;
                SecureStorage.SetAsync(billDayKey, billDay.ToString()).GetAwaiter().GetResult();
                SecureStorage.SetAsync(kwCostKey, kwCost.ToString()).GetAwaiter().GetResult();
                return new Tuple<int, long>(billDay, kwCost);
            }
        }

        private long GetDeviceId()
        {
            var key = "CustomerGid";
            var customerGidStr = SecureStorage.GetAsync(key).GetAwaiter().GetResult();
            if (!long.TryParse(customerGidStr, out var customerGid))
            {
                var userName = SecureStorage.GetAsync("UserName").GetAwaiter().GetResult();
                var customer = ClientVue.GetCustomerInfoAsync(userName).GetAwaiter().GetResult();
                customerGid = customer.CustomerGid;
                SecureStorage.SetAsync(key, customerGid.ToString()).GetAwaiter().GetResult();
            }
            key = "DeviceGid";
            var deviceGidStr = SecureStorage.GetAsync(key).GetAwaiter().GetResult();
            if (long.TryParse(deviceGidStr, out var deviceGid))
            {
                return deviceGid;
            }
            var customerWithDevices = ClientVue.GetCustomerWithDevicesAsync(customerGid).GetAwaiter().GetResult();
            deviceGid = customerWithDevices.Devices[0].DeviceGid;
            SecureStorage.SetAsync(key, deviceGid.ToString()).GetAwaiter().GetResult();
            return deviceGid;

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