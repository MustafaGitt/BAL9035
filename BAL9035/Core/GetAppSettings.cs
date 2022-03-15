using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using BAL9035.Models;
using _9035BL;

namespace BAL9035.Core
{    
    /*
     *  Get all the Appsettings from config and creates an object
     */
    public static class GetAppSettings
    {
        public static AppSettingsValues GetAppSettingsValues()
        {
            try
            {
                AppSettingsValues keys = new AppSettingsValues();
                var appSettings = ConfigurationManager.GetSection("appSettings") as NameValueCollection;
                keys.OrchestratorUrl = appSettings["OrchestratorUrl"];
                keys.QueueName = SecureData.Base64Decode(appSettings["QueueName"]);
                keys.CobaltDQueueName = SecureData.Base64Decode(appSettings["CobaltDQueueName"]);
                keys.ResponseQueueName = SecureData.Base64Decode(appSettings["ResponseQueueName"]);
                keys.tenancyName = SecureData.Base64Decode(appSettings["tenancyName"]);
                keys.cobaltDtenancyName = SecureData.Base64Decode(appSettings["CobaltDtenancyName"]);
                keys.usernameOrEmailAddress = SecureData.Base64Decode(appSettings["usernameOrEmailAddress"]);
                keys.password = SecureData.Base64Decode(appSettings["password"]);
                keys.SecretKey = SecureData.Base64Decode(appSettings["SecretKey"]);
                keys.oidc_Authority = appSettings["oidc:Authority"];
                keys.oidc_ClientId = appSettings["oidc:ClientId"];
                keys.oidc_RedirectUrl = appSettings["oidc:RedirectUrl"];
                keys.ElasticSearch_Authority = appSettings["ElasticSearch:Authority"];
                keys.ESSecretKey = SecureData.Base64Decode(appSettings["ESSecretKey"]);
                keys.LookupKey = appSettings["LookupKey"];
                keys.DbAutomation_Authority = appSettings["DbAutomation:Authority"];
                keys.isDevelopmentEnvironment = Convert.ToBoolean(appSettings["isDevelopmentEnvironment"]);
                return keys;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}