using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EmporiaVue.Api.Models
{
    public class DeviceLocation
    {
        [JsonProperty("billingCycleStartDay")]
        public int BillingCycleStartDay { get; set; }

        [JsonProperty("deviceGid")]
        public long DeviceGid { get; set; }

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("zipCode")]
        public long ZipCode { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("usageCentPerKwHour")]
        public long UsageCentPerKwHour { get; set; }

        [JsonProperty("peakDemandDollarPerKw")]
        public long PeakDemandDollarPerKw { get; set; }

        [JsonProperty("solar")]
        public bool Solar { get; set; }

        [JsonProperty("utilityRateGid")]
        public string UtilityRateGid { get; set; }

        [JsonProperty("locationInformation")]
        public LocationInformation LocationInformation { get; set; }
    }
}