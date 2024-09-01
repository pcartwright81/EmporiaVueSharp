namespace EmporiaVue.Api.Models
{
    using System;
    using Newtonsoft.Json;

    public class Customer
    {
        [JsonProperty("customerGid")]
        public long CustomerGid { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }
}