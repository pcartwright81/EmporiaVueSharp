using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmporiaVue.Api.Models;

namespace EmporiaVue.Api
{
    public interface IVueClient
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
        Task<bool> IsMaintenanceAsync();

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
        ///     Gets the usage over a range of time.
        /// </summary>
        /// <param name="deviceGid">The id of the device.</param>
        /// <param name="channels">The channels to pull.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scale">1S, 1MIN, 15MIN, 1H</param>
        /// <param name="unit">KilowattHours, Dollars, AmpHours, Trees, GallonsOfGas, MilesDriven, Carbon</param>
        /// <returns></returns>
        Task<ChartUsage> GetChartUsageAsync(long deviceGid, List<int> channels, DateTime startDate,
            DateTime endDate, string scale, string unit);

        /// <summary>
        ///     Gets the device usage for a specified date range.
        /// </summary>
        /// <param name="deviceGids">The id of the devices.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="scale">1S, 1MIN, 1H, 1D, 1W, 1MON, 1Y</param>
        /// <param name="unit">KilowattHours, Dollars, AmpHours, Trees, GallonsOfGas, MilesDriven, Carbon</param>
        /// <returns></returns>
        Task<RecentUsage> GetDevicesUsage(List<long> deviceGids, DateTime dateToCheck,
            string scale, string unit);

        /// <summary>
        ///     Gets the total estimated usage since the last bill.
        /// </summary>
        /// <param name="deviceGid">The device gid.</param>
        /// <param name="billDay">The day the bill is given.</param>
        /// <param name="costPerKwHour">The cost per kw.</param>
        /// <returns></returns>
        Task<NextBillEstimate> EstimateNextBill(long deviceGid, int billDay,
            long costPerKwHour);

        /// <summary>
        ///     Gets the last bill date.
        /// </summary>
        /// <returns></returns>
        DateTime GetLastBillDate(int day);
    }
}