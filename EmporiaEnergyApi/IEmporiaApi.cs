using System;
using System.Threading.Tasks;
using EmporiaEnergyApi.Models;

namespace EmporiaEnergyApi
{
    public interface IEmporiaApi
    {
        /// <summary>
        ///     Logs into the api.
        /// </summary>
        /// <returns></returns>
        Task<bool> Login();

        /// <summary>
        ///     Checks the api to see if it is in maintenance.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsMaintenance();

        /// <summary>
        ///     Gets the customer information from the API.
        /// </summary>
        /// <param name="emailAddress">The email address of the user.</param>
        /// <returns></returns>
        Task<Customer> GetCustomerInfoAsync(string emailAddress);

        /// <summary>
        ///     Gets the customer information from the API including the devices.
        /// </summary>
        /// <param name="customerGid">The identifier for the customer.</param>
        /// <returns></returns>
        Task<Customer> GetCustomerWithDevicesAsync(long customerGid);

        /// <summary>
        ///     Gets the location info of a device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <returns></returns>
        Task<DeviceLocation> GetDeviceLocationInfoAsync(long deviceGid);

        /// <summary>
        ///     Gets the total usage for the entire timeframe of the device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <returns></returns>
        Task<TotalUsageForTimeFrame> GetTotalUsageAsync(long deviceGid);


        /// <summary>
        ///     Gets the usage by time for the current device.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H</param>
        /// <param name="unit">USD, WATTS, TREES, GALLONSGAS, MILESDRIVEN, MILESFLOWN</param>
        /// <returns></returns>
        Task<UsageByTimeRange> GetUsageByTimeRangeAsync(long deviceGid, DateTime startDate,
            DateTime endDate, string scale, string unit);

        /// <summary>
        ///     Gets the recent device usage for a specified date range.
        /// </summary>
        /// <param name="customerGid">The identifier for the customer.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H, 1D, 1MON, 1W, 1Y</param>
        /// <param name="unit">USD, WATTS, TREES, GALLONSGAS, MILESDRIVEN, MILESFLOWN</param>
        /// <returns></returns>
        Task<RecentUsage> GetRecentDeviceUsageAsync(long customerGid, DateTime dateToCheck,
            string scale, string unit);
    }
}
