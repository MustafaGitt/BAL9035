using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class EsResultObj
    {
        [JsonProperty(PropertyName = "ProcessName")]
        public string ProcessName { get; set; }
        [JsonProperty(PropertyName = "AssetType")]
        public string AssetType { get; set; }
        [JsonProperty(PropertyName = "AssetName")]
        public string AssetName { get; set; }
        [JsonProperty(PropertyName = "AssetValue")]
        public string AssetValue { get; set; }
    }
}