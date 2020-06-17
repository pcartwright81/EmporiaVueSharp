using System;
using Newtonsoft.Json;

namespace EmporiaEnergyApi.Models
{
    public class UsageByTimeRange
    {
        [JsonProperty("start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("end")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("deviceGid")]
        public long DeviceGid { get; set; }

        [JsonProperty("usage")]
        public double?[] Usage { get; set; }
    }
}