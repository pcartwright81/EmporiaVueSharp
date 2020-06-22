using System;
using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class RecentUsage
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("scale")]
        public string Scale { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("customerGid")]
        public long CustomerGid { get; set; }

        [JsonProperty("channels")]
        public Channel[] Channels { get; set; }
    }
}