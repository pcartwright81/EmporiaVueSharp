using System;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using EmporiaEnergyApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmporiaEnergyApi
{
    public class EmporiaApi : IEmporiaApi
    {
        /// <summary>
        ///     The root url for the api.
        /// </summary>
        private const string RootUrl = "https://api.emporiaenergy.com";


        public EmporiaApi(IConfiguration configuration)
        {
            UserName = configuration["email"];
            Password = configuration["password"];
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
                await MakeRequest<Customer>($"/customers/{customerGid}/devices?detailed=true&hierarchy=true");
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
        ///     Gets the total usage for the entire timeframe of the device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <returns></returns>
        public async Task<TotalUsageForTimeFrame> GetTotalUsageAsync(long deviceGid)
        {
            // ReSharper disable StringLiteralTypo
            var url = $"/usage/total?deviceGid={deviceGid}&timeframe=ALLTODATE&unit=WATTHOURS&channels=1%2C2%2C3";
            // ReSharper restore StringLiteralTypo
            var totalUsage = await MakeRequest<TotalUsageForTimeFrame>(url);

            return totalUsage;
        }

        /// <summary>
        ///     Gets the usage by time for the current device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H</param>
        /// <param name="unit">USD, WATTS, TREES, GALLONSGAS, MILESDRIVEN, MILESFLOWN</param>
        /// <returns></returns>
        public async Task<UsageByTimeRange> GetUsageByTimeRangeAsync(long deviceGid, DateTime startDate,
            DateTime endDate, string scale, string unit)
        {
            var url =
                $"/usage/time?start={startDate:yyyy-MM-ddTHH:mm:ssZ}&end={endDate:yyyy-MM-ddTHH:mm:ssZ}&type=INSTANT&deviceGid={deviceGid}&scale={scale}&unit={unit}&channels=1%2C2%2C3";
            url = url.Replace(":", "%3A");
            var totalUsage = await MakeRequest<UsageByTimeRange>(url);
            return totalUsage;
        }

        /// <summary>
        ///     Gets the recent device usage for a specified date range.
        /// </summary>
        /// <param name="customerGid">The identifier for the customer.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H, 1D, 1MON, 1W, 1Y</param>
        /// <param name="unit">USD, WATTS, TREES, GALLONSGAS, MILESDRIVEN, MILESFLOWN</param>
        /// <returns></returns>
        public async Task<RecentUsage> GetRecentDeviceUsageAsync(long customerGid, DateTime dateToCheck,
            string scale, string unit)
        {
            var url =
                $"/usage/devices?start={dateToCheck:yyyy-MM-ddTHH:mm:ssZ}&end={dateToCheck.AddSeconds(1):yyyy-MM-ddTHH:mm:ssZ}&scale={scale}&unit={unit}&customerGid={customerGid}";
            url = url.Replace(":", "%3A");
            var recentUsage = await MakeRequest<RecentUsage>(url);
            return recentUsage;
        }
    }
}