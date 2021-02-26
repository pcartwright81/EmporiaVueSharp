using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class Timestamp
    {
        [JsonProperty("nano")]
        public int Nano { get; set; }

        [JsonProperty("epochSecond")]
        public int EpochSecond { get; set; }
    }
}