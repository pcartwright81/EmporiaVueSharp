using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using EmporiaVue.Api.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmporiaVue.Api
{
    public class VueClient : IVueClient
    {
        /// <summary>
        ///     The root url for the api.
        /// </summary>
        private const string RootUrl = "https://api.emporiaenergy.com";


        public VueClient(IConfiguration configuration)
        {
            UserName = configuration["email"];
            Password = configuration["password"];
        }

        public VueClient(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        ///     The username that is set by user secrets.
        /// </summary>
        private string UserName { get; }

        /// <summary>
        ///     The password that is set by user secrets.
        /// </summary>
        private string Password { get; }

        /// <summary>
        ///     The authentication result.
        /// </summary>
        private AuthenticationResultType AuthenticationResult { get; set; }

        /// <summary>
        ///     Logs into the api.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Login()
        {
            // ReSharper disable StringLiteralTypo
            const string clientId = "4qte47jbstod8apnfic0bunmrq";
            // ReSharper restore StringLiteralTypo
            const string poolId = "us-east-2_ghlOXVLi1";
            var provider =
                new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.USEast2);
            var userPool = new CognitoUserPool(poolId, clientId, provider);
            var user = new CognitoUser(UserName, clientId, userPool, provider);
            var authResponse = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest
            {
                Password = Password
            }).ConfigureAwait(false);
            AuthenticationResult = authResponse.AuthenticationResult;
            return AuthenticationResult != null;
        }

        /// <summary>
        ///     Checks the api to see if it is in maintenance.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsMaintenanceAsync()
        {
            const string url = "https://s3.amazonaws.com/com.emporiaenergy.manual.ota/maintenance/maintenance.json";
            var client = new HttpClient();
            using var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Gets the customer information from the API.
        /// </summary>
        /// <param name="emailAddress">The email address of the user.</param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerInfoAsync(string emailAddress)
        {
            var customer = await MakeRequest<Customer>($"/customers?email={emailAddress}");
            return customer;
        }

        /// <summary>
        ///     Gets the customer information from the API including the devices.
        /// </summary>
        /// <param name="customerGid">The identifier for the customer.</param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerWithDevicesAsync(long customerGid)
        {
            var customer =
                await MakeRequest<Customer>("/customers/devices");
            return customer;
        }

        /// <summary>
        ///     Makes the request to a given path and returns the object with the type.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="path">The path of the request url.</param>
        /// <returns></returns>
        private async Task<T> MakeRequest<T>(string path)
        {
            if (AuthenticationResult == null) throw new Exception("Must login before calling any methods.");

            var url = $"{RootUrl}{path}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("authtoken", AuthenticationResult.IdToken);
            using var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode}:{response.ReasonPhrase}");
            }
            using var content = response.Content;
            var json = await content.ReadAsStringAsync();
            var objReturn = JsonConvert.DeserializeObject<T>(json);
            return objReturn;
        }

        /// <summary>
        ///     Gets the location info of a device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <returns></returns>
        public async Task<DeviceLocation> GetDeviceLocationInfoAsync(long deviceGid)
        {
            var deviceLocation = await MakeRequest<DeviceLocation>($"/devices/{deviceGid}/locationProperties");
            return deviceLocation;
        }

        /// <summary>
        ///     Gets the usage over a range of time.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <param name="channels">The channels to pull.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H</param>
        /// <param name="unit">USD, WATTS, TREES, GALLONSGAS, MILESDRIVEN, MILESFLOWN</param>
        /// <returns></returns>
        public async Task<ChartUsage> GetChartUsageAsync(long deviceGid, List<int> channels, DateTime startDate,
            DateTime endDate, string scale, string unit)
        {
            var url =
                $"/AppAPI?apiMethod=getChartUsage&deviceGid={deviceGid}&channel={string.Join(",", channels)}&start={startDate:yyyy-MM-ddTHH:mm:ssZ}&end={endDate:yyyy-MM-ddTHH:mm:ssZ}&scale={scale}&energyUnit={unit}";
            url = url.Replace(":", "%3A");
            var totalUsage = await MakeRequest<ChartUsage>(url);
            return totalUsage;
        }


        /// <summary>
        ///     Gets the device usage for a specified date range.
        /// </summary>
        /// <param name="deviceGids">The id of the devices.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="scale">1S, 1MIN, 1H, 1D, 1W, 1MON, 1Y</param>
        /// <param name="unit">KilowattHours, Dollars, AmpHours, Trees, GallonsOfGas, MilesDriven, Carbon</param>
        /// <returns></returns>
        public async Task<DeviceListUsage> DeviceListUsages(List<long> deviceGids, DateTime dateToCheck,
            string scale, string unit)
        {
            var deviceIds = string.Empty;
            for (var index = 0; index < deviceGids.Count; index++)
            {
                var deviceId = deviceGids[index];
                deviceIds += "{" + deviceId + "}";
                if (index < deviceGids.Count - 1)
                {
                    deviceIds += "+";
                }
            }

            var url =
                $"/AppAPI?apiMethod=getDeviceListUsages&deviceGids={deviceIds}&instant={dateToCheck:yyyy-MM-ddTHH:mm:ssZ}&scale={scale}&energyUnit={unit}";
            url = url.Replace(":", "%3A");
            var recentUsage = await MakeRequest<DeviceListUsage>(url);
            return recentUsage;
        }

        /// <summary>
        ///     Gets the total estimated usage since the last bill.
        /// </summary>
        /// <param name="deviceGid">The device gid.</param>
        /// <param name="billDay">The day the bill is given.</param>
        /// <param name="costPerKwHour">The cost per kw.</param>
        /// <returns></returns>
        public async Task<NextBillEstimate> EstimateNextBill(long deviceGid, int billDay,
            long costPerKwHour)
        {
            var dtNow = DateTime.UtcNow;
            var billDate = GetLastBillDate(billDay);
            var usage = new NextBillEstimate();
            var usageByTime = await GetChartUsageAsync(deviceGid, new List<int>{1,2,3}, billDate, dtNow, "1H", "KilowattHours");
            usage.Usage = usageByTime.UsageList.Sum();
            usage.UsageCost = usage.Usage * costPerKwHour / 100;
            usage.UsagePerDay = usage.Usage / (dtNow - billDate).TotalDays; //get the total days since last bill
            var totalBillDays = (billDate.AddMonths(1) - billDate).TotalDays;
            usage.EstimatedUsage = usage.UsagePerDay * totalBillDays;
            usage.EstimatedCost = usage.EstimatedUsage * costPerKwHour / 100;
            return usage;
        }

        public DateTime GetLastBillDate(int day)
        {
            var dtNow = DateTime.UtcNow;
            var month = dtNow.Month;
            if (dtNow.Day <= day)
            {
                month = dtNow.Month - 1;
            }

            var year = dtNow.Year;
            if (month != 0) return new DateTime(year, month, day);
            month = 12;
            year -= 1;

            return new DateTime(year, month, day);
        }
    }
}