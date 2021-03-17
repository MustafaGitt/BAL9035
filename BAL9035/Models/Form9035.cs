using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class Form9035
    {
        public bool? isSubmit { get; set; }
        public SectionB SectionB { get; set; }
        public string E14 { get; set; }
        public SectionF SectionF { get; set; }
        public SectionH SectionH { get; set; }
        public string I1 { get; set; }
        public SectionJ SectionJ { get; set; }
        public SectionK SectionK { get; set; }
        public string otherValues { get; set; }

        public Form9035()
        {
            SectionB = new SectionB();
            SectionF = new SectionF();
            SectionH = new SectionH();
            SectionJ = new SectionJ();
            SectionK = new SectionK();
        }

    }
    public enum NaSelection
    {
        Yes = 1,
        No = 2,
        NA = 3
    }
}