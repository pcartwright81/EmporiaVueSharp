namespace EmporiaVue.Api.Models
{
    using Newtonsoft.Json;

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

        [JsonProperty("locationSqFt")]
        public long LocationSqFt { get; set; }

        [JsonProperty("hotTub")]
        public bool HotTub { get; set; }

        [JsonProperty("latitudeLongitude")]
        public LatitudeLongitude LatitudeLongitude { get;set;}
    }
}