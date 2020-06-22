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
            var usageList = await Client.GetUsageByTimeRangeAsync(customerWithDevices.Devices[0].DeviceGid,
                DateTime.UtcNow.AddDays(-2).Date, DateTime.Now.ToUniversalTime(), scale, "WATTS");
            var listReturn = new List<EmporiaUsage>();
            var counter =
                TimeZoneInfo.ConvertTime(usageList.Start, TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"));
            foreach (var usage in usageList.Usage)
            {
                listReturn.Add(new EmporiaUsage(counter, usage));

                counter = usageList.Scale switch
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