using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class Usage
    {
        [JsonProperty("Timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}