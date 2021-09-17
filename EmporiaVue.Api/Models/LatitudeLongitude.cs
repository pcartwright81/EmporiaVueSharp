namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

    public class LatitudeLongitude
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}