namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

    public class Usage
    {
        [JsonProperty("Timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}