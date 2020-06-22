using Newtonsoft.Json;

namespace EmporiaVue.Api.Models
{
    public class LocationInformation
    {
        [JsonProperty("airConditioning")]
        public bool AirConditioning { get; set; }

        [JsonProperty("heatSource")]
        public string HeatSource { get; set; }

        [JsonProperty("numElectricCars")]
        public long NumElectricCars { get; set; }

        [JsonProperty("locationType")]
        public string LocationType { get; set; }

        [JsonProperty("numPeople")]
        public long NumPeople { get; set; }
    }
}