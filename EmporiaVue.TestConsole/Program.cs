using System;
using System.Threading.Tasks;
using EmporiaVue.Api;
using Microsoft.Extensions.Configuration;

namespace EmporiaVue.TestConsole
{
    internal class Program
    {
        private static async Task Main()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
            var config = builder.Build();
            var api = new VueClient(config);
            var login = await api.Login();
            if (!login)
            {
                Console.WriteLine("Login Failed");
            }
            var customer = await api.GetCustomerInfoAsync(config["email"]);
            var customerWithDevices = await api.GetCustomerWithDevicesAsync(customer.CustomerGid);
            var deviceGid = customerWithDevices.Devices[0].DeviceGid;
            var location = await api.GetDeviceLocationInfoAsync(deviceGid);
            var nextBill = await api.EstimateNextBill(deviceGid, 27, location.UsageCentPerKwHour);
            Console.WriteLine($"Usage since last bill is {nextBill.UsageSinceDate:F}");
            Console.WriteLine($"Estimated usage is {nextBill.EstimatedUsage:F}");
            Console.WriteLine($"Average usage per day is {nextBill.UsagePerDay:F}");
            Console.WriteLine($"Total estimated bill is {nextBill.EstimatedCost:F}");
        }
    }
}