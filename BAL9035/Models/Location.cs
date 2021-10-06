using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class Location
    {
        public int? LocationId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string LocWageFrom { get; set; }
        public string LocWageTo { get; set; }
        public string SecondEntityName { get; set; }
        public SectionFModal FmodalObject { get; set; }
    }
}