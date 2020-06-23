﻿using System;
using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class UsageByTimeRange
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

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