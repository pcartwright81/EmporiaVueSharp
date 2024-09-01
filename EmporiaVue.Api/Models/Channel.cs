namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

    public class Channel
    {
        [JsonProperty("deviceGid")]
        public long DeviceGid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("channelNum")]
        public string ChannelNum { get; set; }

        [JsonProperty("channelMultiplier")]
        public long ChannelMultiplier { get; set; }

        [JsonProperty("channelTypeGid")]
        public long? ChannelTypeGid { get; set; }

        [JsonProperty("usage")]
        public double Usage { get; set; }
    }
}