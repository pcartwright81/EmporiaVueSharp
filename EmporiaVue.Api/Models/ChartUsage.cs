namespace EmporiaVue.Api.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ChartUsage
    {
        [JsonProperty("firstUsageInstant")]
        public DateTime FirstUsageInstant { get; set; }

        [JsonProperty("usageList")]
        public List<double> UsageList { get; set; }
    }
}