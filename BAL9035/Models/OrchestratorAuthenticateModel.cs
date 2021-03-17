using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class OrchestratorAuthenticateModel
    {
        public string result { get; set; }
        public string targetUrl { get; set; }
        public string success { get; set; }
        public string error { get; set; }
        public string unAuthorizedRequest { get; set; }
        public string __abp { get; set; }
        public string access_token { get; set; }
    }
}