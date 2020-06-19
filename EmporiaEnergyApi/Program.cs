using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EmporiaEnergyApi
{
    internal class Program
    {
        private static async Task Main()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
            var config = builder.Build();
            var api = new EmporiaApi(config);
            var login = await api.Login();
            if (!login)
            {
                Console.WriteLine("Login Failed");
            }
            var customer = await api.GetCustomerInfoAsync(config["email"]);
            var customerWithDevices = await api.GetCustomerWithDevicesAsync(customer.CustomerGid);
            var billDate = new DateTime(2020, 05, 27);
            var dtNow = DateTime.Now;
            var usageByTime = await api.GetUsageByTimeRangeAsync(customerWithDevices.Devices[0].DeviceGid, billDate,
                dtNow, "1H", "WATTS");
            var usageSinceLastBill = usageByTime.Usage.Sum() / 1000; //add all and convert to KW
            var usagePerDay = usageSinceLastBill / (DateTime.UtcNow - billDate).TotalDays; //get the total days since last bill
            const double kwCost = .09;
            var totalBillDays = (billDate.AddMonths(1) - billDate).TotalDays;
            var estimatedUsage = usagePerDay * totalBillDays;
            Console.WriteLine($"Usage since last bill is {usageSinceLastBill:F}");
            Console.WriteLine($"Estimated usage is {estimatedUsage:F}");
            Console.WriteLine($"Total estimated bill is {estimatedUsage * kwCost:F}");
        }
    }
}