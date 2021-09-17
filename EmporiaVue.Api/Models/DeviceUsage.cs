namespace EmporiaVue.Api.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class DeviceUsage
    {
        [JsonProperty("deviceGid")]
        public int DeviceGid { get; set; }

        [JsonProperty("channelUsages")]
        public List<ChannelUsage> ChannelUsages { get; set; }
    }
}