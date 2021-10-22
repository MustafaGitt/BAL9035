using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class SpecificContent
    {
        public string ID { get; set; }
        public string Err_Type { get; set; }
        public string Err_Message { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
    }

    public class ItemData
    {
        public string Name { get; set; }
        public string Priority { get; set; }
        public string Reference { get; set; }
        public SpecificContent SpecificContent { get; set; }
    }

    public class RootObject
    {
        public ItemData itemData { get; set; }
    }
}