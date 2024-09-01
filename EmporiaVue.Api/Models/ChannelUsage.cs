namespace EmporiaVue.Api.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ChannelUsage
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("usage")]
        public double Usage { get; set; }

        [JsonProperty("deviceGid")]
        public int DeviceGid { get; set; }

        [JsonProperty("channelNum")]
        public string ChannelNum { get; set; }

        [JsonProperty("percentage")]
        public double Percentage { get; set; }

        [JsonProperty("nestedDevices")]
        public List<object> NestedDevices { get; set; }
    }
}