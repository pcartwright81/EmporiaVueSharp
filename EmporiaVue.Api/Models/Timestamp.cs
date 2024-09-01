namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

    public class Timestamp
    {
        [JsonProperty("nano")]
        public int Nano { get; set; }

        [JsonProperty("epochSecond")]
        public int EpochSecond { get; set; }
    }
}