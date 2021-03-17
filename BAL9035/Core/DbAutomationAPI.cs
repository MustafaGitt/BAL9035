using _9035BL;
using BAL9035.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BAL9035.Core
{
    public class DbAutomationAPI
    {
        RestApiServices api = new RestApiServices();

        public DataTable GetDataFromDBAPI(string BALMatterNo)
        {
            try
            {
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
                string queryString = "api/SQL9035?BALMatterNo=" + BALMatterNo;
                string response = api.Get(appKeys.DbAutomation_Authority, "", queryString);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(response, (typeof(DataTable)));
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}