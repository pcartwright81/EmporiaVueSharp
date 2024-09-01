namespace EmporiaVue.Api.Models
{
    using System;
    using Newtonsoft.Json;

    public class Device
    {
        [JsonProperty("deviceGid")]
        public long DeviceGid { get; set; }

        [JsonProperty("manufacturerDeviceId")]
        public string ManufacturerDeviceId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("firmware")]
        public string Firmware { get; set; }

        [JsonProperty("devices")]
        public Device[] Devices { get; set; }

        [JsonProperty("channels")]
        public Channel[] Channels { get; set; }

        [JsonProperty("offlineSince")]
        public DateTime OfflineSince { get; set; }
    }
}