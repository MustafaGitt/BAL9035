using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class Lists
    {
        public string BALNumber { get; set; }
        public List<Location> LocationsList { get; set; }
        public List<string> caseSubTypes { get; set; }
        public List<string> parentCaseSubTypes { get; set; }


        public Lists()
        {
            LocationsList = new List<Location>();
            caseSubTypes = new List<string>();
            parentCaseSubTypes = new List<string>();
        }

    }
}