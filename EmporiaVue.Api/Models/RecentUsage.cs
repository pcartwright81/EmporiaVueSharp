using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class RecentUsage
    {
        [JsonProperty("channelUsages")]
        public List<ChannelUsage> ChannelUsages { get; set; }
    }
}