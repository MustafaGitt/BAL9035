using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BAL9035.Models
{
    public class AssetValueArrayModel
    {
        public string Name { get; set; }
        public bool CanBeDeleted { get; set; }
        public string ValueScope { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public int IntValue { get; set; }
        public string CredentialUsername { get; set; }
        public string CredentialPassword { get; set; }
        public int Id { get; set; }
        public List<object> KeyValueList { get; set; }
    }
}