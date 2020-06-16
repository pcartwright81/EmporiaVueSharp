using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EmporiaEnergyApi
{
    internal class Program
    {

        private static async Task Main()
        {
            var api = new EmporiaApi();
            var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
            var config = builder.Build();
            Console.WriteLine("Hello World!");
            bool b = await api.IsMaintenance();
            var customer = api.GetCustomerInfo(config["UserName"]);
        }
    }
}
