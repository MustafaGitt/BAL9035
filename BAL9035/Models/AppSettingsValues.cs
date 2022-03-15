using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class AppSettingsValues
    {
        public string OrchestratorUrl { get; set; }
        public string QueueName { get; set; }
        public string CobaltDQueueName { get; set; }
        public string ResponseQueueName { get; set; }
        public string tenancyName { get; set; }
        public string cobaltDtenancyName { get; set; }
        public string usernameOrEmailAddress { get; set; }
        public string password { get; set; }
        public string SecretKey { get; set; }
        public string oidc_Authority { get; set; }
        public string oidc_ClientId { get; set; }
        public string oidc_RedirectUrl { get; set; }
        public string ElasticSearch_Authority { get; set; }
        public string ESSecretKey { get; set; }
        public string LookupKey { get; set; }
        public string DbAutomation_Authority { get; set; }
        public bool isDevelopmentEnvironment { get; set; }
    }
}