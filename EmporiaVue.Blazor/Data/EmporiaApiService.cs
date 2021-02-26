using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmporiaVue.Api;
using Microsoft.Extensions.Configuration;

namespace EmporiaVue.Blazor.Data
{
    public class EmporiaApiService
    {
        public EmporiaApiService(IVueClient vueClient, IConfiguration configuration)
        {
            Client = vueClient;
            Configuration = configuration;
        }

        private IVueClient Client { get; }

        private IConfiguration Configuration { get; }


        public async Task<List<EmporiaUsage>> GetUsageData(string scale)
        {
            await Client.Login();
            var customer = await Client.GetCustomerInfoAsync(Configuration["email"]);
            var customerWithDevices = await Client.GetCustomerWithDevicesAsync(customer.CustomerGid);
            var usageList = await Client.GetChartUsageAsync(customerWithDevices.Devices[0].DeviceGid, new List<int>{1,2,3},
                DateTime.UtcNow.AddDays(-2).Date, DateTime.Now.ToUniversalTime(), scale, "KilowattHours");
            var listReturn = new List<EmporiaUsage>();
            var counter =
                TimeZoneInfo.ConvertTime(usageList.FirstUsageInstant, TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"));
            foreach (var usage in usageList.UsageList)
            {
                listReturn.Add(new EmporiaUsage(counter, usage));

                counter = scale switch
                {
                    "1S" => counter.AddSeconds(1),
                    "1MIN" => counter.AddMinutes(1),
                    "15MIN" => counter.AddMinutes(15),
                    "1H" => counter.AddHours(1),
                    _ => counter
                };
            }

            return listReturn;
        }
    }
}