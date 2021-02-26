using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class ChannelUsage
    {
        [JsonProperty("deviceGid")]
        public int DeviceGid { get; set; }

        [JsonProperty("channelNum")]
        public string ChannelNum { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }
}