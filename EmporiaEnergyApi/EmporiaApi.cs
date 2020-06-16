using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;

namespace EmporiaEnergyApi
{
    public class EmporiaApi
    {
        private const string RootUrl = "https://api.emporiaenergy.com";

        private static HttpClient HttpClient = new HttpClient();
        public async Task<bool> IsMaintenance()
        {
            const string url = "https://s3.amazonaws.com/com.emporiaenergy.manual.ota/maintenance/maintenance.json";
            using var response = await HttpClient.GetAsync(url);
            using var content = response.Content;
            var json = await content.ReadAsStringAsync();
            return json.Contains("down");
        }

        public async Task<Customer> GetCustomerInfo(string emailAddress)
        {
            return new Customer();
        }


    }
}
