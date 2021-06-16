using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using _9035BL;
using BAL9035.Core;
using BAL9035.Models;
using Newtonsoft.Json;

namespace BAL9035.Controllers
{
    /*
     * This API Controller Contains the method which create the Credential Asset and Get the key from
     * Elastic Search and Update the Secret key as wellS
     */
    public class AddCredentialsController : ApiController
    {
        // GET: api/AddCredentials
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AddCredentials/username
        // WEB API : Gets the secret key value from ElasticSearch if any exist against the given username
        [HttpGet]
        public IHttpActionResult GetESKey(string username)
        {
            string serializeResponse = "";
            Response response = new Response();
            try
            {
                // Validate inputs
                if (!string.IsNullOrEmpty(username))
                {
                    string errorMessage = "";
                    // Get appsettings
                    AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
                    ElasticSearch es = new ElasticSearch();
                    string secretKey = "";
                    // Gets the key from ElasticSearch
                    string[] esResults = es.GetESKey(username, appKeys.LookupKey, out errorMessage);
                    if (esResults != null)
                    {
                        secretKey = esResults[0];
                    }
                    response.success = true;
                    response.message = secretKey;
                    Log.Info(username + " username, Process : Get ES key, Message : The Get method for Secret Key from Elastic Search has been successfully executed.");
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                Log.Error(ex, username);

            }
            serializeResponse = JsonConvert.SerializeObject(response);
            return Json(serializeResponse);
        }

        // POST: api/AddCredentials
        // Create the Credential Asset on UIPATH Orchestrator
        public IHttpActionResult Post([FromBody]CredentialAsset bodyModel)
        {
            string assetResponse = "";
            Response outResponse = new Response();
            OrchestratorAPI api = new OrchestratorAPI();
            ElasticSearch es = new ElasticSearch();
            string logMsg = "";
            string errorMessage = "";
            try
            {
                // Check if the Input Parameters has all the values or not
                if (string.IsNullOrEmpty(bodyModel.Username) && string.IsNullOrEmpty(bodyModel.Password) && string.IsNullOrEmpty(bodyModel.Name) && string.IsNullOrEmpty(bodyModel.Email))
                {
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                    assetResponse = JsonConvert.SerializeObject(outResponse);
                    logMsg = "Bal Number : " + bodyModel.Name + ", Proecess : Create Credential Asset, Message : Parameter : Username : " + bodyModel.Username + "Password : " + bodyModel.Password + " Name : " + bodyModel.Name + " Email : " + bodyModel.Email + " : Parameter is required.";
                    Log.Info(logMsg);
                    es.AddErrorESLog(bodyModel.Name, "Technical", "Credential Asset Parameter Missing", out errorMessage);
                    api.AddErrorQueueItem(bodyModel.Name, "Credential Asset Parameter Missing","Technical", "Credential Asset Parameter Missing","In Progress");
                    return Json(assetResponse);
                }
                string filter = "";
                //  Get the Config from AppSettings
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
                //Set which tenant to use
                appKeys.tenancyName = bodyModel.Name.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
                // UIPATH TOKEN
                string token = api.Authentication(appKeys);
                // Send error message if the token is null
                if (string.IsNullOrEmpty(token))
                {
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                    logMsg = "Bal Number : "+ bodyModel.Name + ", Proecess : Create Credential Asset, Message : You are not been able to authenticated. Token : " + token;
                    Log.Info(logMsg);
                    es.AddErrorESLog(bodyModel.Name, "Technical", "UIPATH Token not generated. Authentication Failed.", out errorMessage);
                    api.AddErrorQueueItem(bodyModel.Name, "UIPATH Token not generated. Authentication Failed.", "Technical", "UIPATH Token not generated. Authentication Failed.", "In Progress");
                }
                else
                {
                    // get n check if asset exist
                    filter = "?$filter=Name eq '" + bodyModel.Name + "'";
                    AssetModel assetModel = api.GetAsset(token, filter);
                    // create or update asset
                    Dictionary<string, object> assetBody = new Dictionary<string, object>();
                    assetBody.Add("Name", bodyModel.Name);
                    assetBody.Add("ValueScope", "Global");
                    assetBody.Add("ValueType", "Credential");
                    assetBody.Add("StringValue", "no idea");
                    assetBody.Add("CredentialUsername", bodyModel.Username);
                    assetBody.Add("CredentialPassword", bodyModel.Password);
                    assetResponse = api.CreateAsset(assetModel, token, assetBody);
                    // get n check if asset exist
                    assetModel = api.GetAsset(token, filter);
                    if (assetModel.value.Count > 0)
                    {
                        // Gets user old records and sets the new updated key against it
                        string esKeyID = "";
                        // gets the KeyID
                        string[] esResults = es.GetESKey(bodyModel.Email, appKeys.LookupKey, out errorMessage);
                        string initalKeyID = "";
                        if(esResults != null)
                        {
                            initalKeyID = esResults[1];
                        }
                        // Set the SecretKey against the KeyID
                        es.SetESKey(initalKeyID, bodyModel.Email, bodyModel.SecretKey, appKeys.LookupKey, out errorMessage, out esKeyID);

                        outResponse.success = true;
                        outResponse.message = esKeyID;
                        logMsg = "Bal Number : " + bodyModel.Name + " Process : Create Credential Asset, Message :  The method has been successfully exexuted.";
                        Log.Info(logMsg);
                    }
                    else
                    {
                        outResponse.success = false;
                        outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                        logMsg = "Bal Number : "+ bodyModel.Name + " Process : Create Credential Asset, Message :  The asset has not been found";
                        Log.Info(logMsg);
                        es.AddErrorESLog(bodyModel.Name, "Technical", "The Credential Asset has not been created", out errorMessage);
                        api.AddErrorQueueItem(bodyModel.Name, "The Credential Asset has not been created", "Technical", "The Credential Asset has not been created", "In Progress");
                    }
                }
            }
            catch (Exception ex)
            {
                outResponse.success = false;
                outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                Log.Error(ex, bodyModel.Name);
                es.AddErrorESLog(bodyModel.Name, "Technical", ex.Message, out errorMessage);
                api.AddErrorQueueItem(bodyModel.Name, ex.Message, "Technical", ex.Message, "In Progress");
            }
            assetResponse = JsonConvert.SerializeObject(outResponse);
            return Json(assetResponse);
        }

        // DELETE: api/AddCredentials/5
        public void Delete(int id)
        {
        }
    }
}
