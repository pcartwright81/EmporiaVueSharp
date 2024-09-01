namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

    public class DeviceListUsage
    {
        [JsonProperty("deviceListUsages")]
        public DeviceListUsages DeviceListUsages { get; set; }
    }
}