using System;
using Newtonsoft.Json;

namespace EmporiaEnergyApi.Models
{
    public class TotalUsageForTimeFrame
    {
        [JsonProperty("timeframeStart")]
        public DateTime TimeFrameStart { get; set; }

        [JsonProperty("timeframe")]
        public string TimeFrame { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("usage")]
        public double Usage { get; set; }

        [JsonProperty("deviceGid")]
        public long DeviceGid { get; set; }

        [JsonProperty("channels")]
        public string[] Channels { get; set; }
    }
}