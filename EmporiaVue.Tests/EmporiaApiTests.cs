using System;
using System.Threading.Tasks;
using EmporiaVue.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmporiaVue.Tests
{
    [TestClass]
    public class EmporiaApiTests
    {
        private IVueClient Client { get; }

        private readonly IConfiguration _configuration;

        public EmporiaApiTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<EmporiaApiTests>();
            _configuration = builder.Build();
            Client = new VueClient(_configuration);
        }

        [TestMethod]
        public async Task TestIsMaintenance()
        {
            var maintenance = await Client.IsMaintenanceAsync();
            Assert.IsFalse(maintenance);

        }

        [TestMethod]
        public async Task TestCustomerInfo()
        {
            var customer = await GetCustomerId();
            Assert.AreNotEqual(customer,0);
        }

        private async Task<long> GetCustomerId()
        {
            var b = await Client.Login();
            Assert.IsTrue(b, "Login Failed");
            var customer = await Client.GetCustomerInfoAsync(_configuration["email"]);
            return customer.CustomerGid;
        }

        [TestMethod]
        public async Task TestCustomerWithDeviceInfo()
        {
            var b = await Client.Login();
            Assert.IsTrue(b, "Login Failed");
            var customer = await Client.GetCustomerInfoAsync(_configuration["email"]);
            var customerWithDevices = await Client.GetCustomerWithDevicesAsync(customer.CustomerGid);
            Assert.IsNotNull(customerWithDevices.Email);
        }

        [TestMethod]
        public async Task TestDeviceLocation()
        {
            var firstDeviceId = await GetFirstDeviceId();
            var location = await Client.GetDeviceLocationInfoAsync(firstDeviceId);
            Assert.IsNotNull(location.DeviceGid);
        }

        private async Task<long> GetFirstDeviceId()
        {
            var b = await Client.Login();
            Assert.IsTrue(b, "Login Failed");
            var customer = await Client.GetCustomerInfoAsync(_configuration["email"]);
            var customerWithDevices = await Client.GetCustomerWithDevicesAsync(customer.CustomerGid);
            return customerWithDevices.Devices[0].DeviceGid;
        }

        [TestMethod]
        public async Task TestTotalUsage()
        {
            var firstDeviceId = await GetFirstDeviceId();
            var totalUsage = await Client.GetTotalUsageAsync(firstDeviceId);
            Assert.IsNotNull(totalUsage.DeviceGid);
        }

        [TestMethod]
        public async Task TestUsageByTimeRange()
        {
            var firstDeviceId = await GetFirstDeviceId();
            var usageByTimeRange = await Client.GetUsageByTimeRangeAsync(firstDeviceId, DateTime.Now.AddDays(-15), DateTime.Now, "1H", "WATTS");
            Assert.IsNotNull(usageByTimeRange.DeviceGid);
        }

        [TestMethod]
        public async Task TestRecentUsage()
        {
            var customerId = await GetCustomerId();
            var recentUsage = await Client.GetRecentDeviceUsageAsync(customerId, DateTime.Now, "1MON", "WATTS");
            Assert.IsNotNull(recentUsage.CustomerGid);
        }

        [TestMethod]
        public async Task TestEstimatedUsage()
        {
            var firstDeviceId = await GetFirstDeviceId();
            var location = await Client.GetDeviceLocationInfoAsync(firstDeviceId);
            var recentUsage = await Client.EstimateNextBill(firstDeviceId, 27, location.UsageCentPerKwHour);
            Assert.AreNotEqual(0, recentUsage.EstimatedCost);
        }

        [TestMethod]
        public void TestLastBillDate()
        {
            const int day = 27;
            var dtNow = DateTime.UtcNow;
            var month = dtNow.Month;
            if (dtNow.Day <= day)
            {
                month = dtNow.Month - 1;
            }

            var billDate = new DateTime(2020, month, day);
            var testBillDate = Client.GetLastBillDate(day);
            Assert.AreEqual(billDate, testBillDate);
        }
    }
}