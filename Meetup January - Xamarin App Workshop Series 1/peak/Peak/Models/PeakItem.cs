using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Peak.Models
{
    public class PeakItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Done { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
