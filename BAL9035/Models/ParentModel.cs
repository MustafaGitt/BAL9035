using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class ParentModel
    {
        public Form9035 Form9035 { get; set; }
        public Lists Lists { get; set; }

        public ParentModel()
        {
            Form9035 = new Form9035();
            Lists = new Lists();
        }
    }
}