using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmporiaVue.Api;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmporiaVue.CurrentBill
{
    public partial class MainPage
    {
        private CancellationTokenSource TokenSource2 { get; set; }

        public MainPage()
        {
            InitializeComponent();
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            TokenSource2.Cancel();
            SecureStorage.RemoveAll();
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }


        private async Task GetData()
        {
            string userName;
            string password;
            try
            {
                userName = await SecureStorage.GetAsync("UserName");
                password = await SecureStorage.GetAsync("Password");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading:{ex.Message}.");
                return;
            }
            var api = new VueClient(userName, password);
            var login = await api.Login();
            if (!login)
            {
                Console.WriteLine("Login Failed");
            }

            if (TokenSource2.IsCancellationRequested)
            {
                return;
            }
            var customer = await api.GetCustomerInfoAsync(userName);
            var customerWithDevices = await api.GetCustomerWithDevicesAsync(customer.CustomerGid);
            if (TokenSource2.IsCancellationRequested)
            {
                return;
            }
            var dtNow = DateTime.Now;
            var month = dtNow.Month;
            if (dtNow.Day <= 27)
            {
                month = dtNow.Month - 1;
            }
            var billDate = new DateTime(2020, month, 27);
            if (TokenSource2.IsCancellationRequested)
            {
                return;
            }
            var usageByTime = await api.GetUsageByTimeRangeAsync(customerWithDevices.Devices[0].DeviceGid, billDate,
                dtNow, "1H", "WATTS");
           
            var usageSinceLastBill = usageByTime.Usage.Sum() / 1000; //add all and convert to KW
            var usagePerDay = usageSinceLastBill / (DateTime.UtcNow - billDate).TotalDays; //get the total days since last bill
            const double kwCost = .09;
            var totalBillDays = (billDate.AddMonths(1) - billDate).TotalDays;
            var estimatedUsage = usagePerDay * totalBillDays;
            Device.BeginInvokeOnMainThread(() => {
                Usage.Text = $"{usageSinceLastBill:F}";
                Estimated.Text = $"{estimatedUsage:F}";
                Average.Text = $"{usagePerDay:F}";
                Total.Text = $"{estimatedUsage * kwCost:F}";
            });
            
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            TokenSource2 = new CancellationTokenSource();
            Task.Run(GetData, TokenSource2.Token);
        }
    }
}
