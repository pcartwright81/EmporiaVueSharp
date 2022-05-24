using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class DeviceListUsage
    {
        [JsonProperty("deviceListUsages")]
        public DeviceListUsages DeviceListUsages { get; set; }
    }
}