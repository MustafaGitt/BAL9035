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
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
    }


    public class ElasticSearchDto
    {
        public string _id { get; set; }
        public string AssetType { get; set; }
        public string AssetValue_IsEncrypted { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}