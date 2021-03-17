using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class AssetModel
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string @odatacontext { get; set; }

        [JsonProperty(PropertyName = "@odata.count")]
        public int @odatacount { get; set; }
        public List<AssetValueArrayModel> value { get; set; }
    }
}