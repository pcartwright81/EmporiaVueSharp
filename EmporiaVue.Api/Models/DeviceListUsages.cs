namespace EmporiaVue.Api.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DeviceListUsages
    {
        [JsonProperty("instant")]
        public DateTime Instant { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }

        [JsonProperty("devices")]
        public List<DeviceUsage> Devices { get; set; }

        [JsonProperty("energyUnit")]
        public string EnergyUnit { get; set; }
    }
}
