using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class PostSaveData
    {
        public string Sysid { get; set; }
        public string BalNumber { get; set; }
        public string JsonString { get; set; }
        public bool isSubmit { get; set; }
        public string Email { get; set; }
        public string EsIdNo { get; set; }
        public string ListJsonString { get; set; }
    }
}