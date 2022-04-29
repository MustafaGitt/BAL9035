using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using BAL9035.Models;
using Newtonsoft.Json;
using BAL9035.Core;
using _9035BL;
using System.Web.Configuration;
using Accelirate.ElasticSearch.Common;
using System.Text;

namespace BAL9035.Controllers
{
    /*
     * API WEB CONTROLLER
     * This Controller has Actions which require to Save the UIPATH Data Assets and Add a Queue Item
     */
    public class Save9035Controller : ApiController
    {
        ElasticSearch es = new ElasticSearch();
        // CREATE Data Assets on UIPATH Orchestrator
        [HttpPost]
        public IHttpActionResult CreateAsset([FromBody] PostSaveData bodyModel)
        {
            string assetResponse = "";
            string logMsg = "";
            Response outResponse = new Response();
            OrchestratorAPI api = new OrchestratorAPI();
           // var test = new Accelirate.ElasticSearch.Common.ElasticBasicClient(,);
          
            string errorMessage = "";
            try
            {
                // Validate the input parameters
                if (string.IsNullOrEmpty(bodyModel.JsonString) && string.IsNullOrEmpty(bodyModel.Sysid) && string.IsNullOrEmpty(bodyModel.BalNumber))
                {
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : JsonString  parameter required";
                    Log.Info(logMsg);
                    assetResponse = JsonConvert.SerializeObject(outResponse);
                    es.AddErrorESLog(bodyModel.Sysid, "Technical", "Data Asset Parameter Missing", out errorMessage);
                    api.AddErrorQueueItem(bodyModel.Sysid, "Data Asset Parameter Missing", "Technical", "Data Asset Parameter Missing", "In Progress");
                    return Json(assetResponse);
                }
                // configuration getting from Appsetting
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
                //Set which tenant to use
                appKeys.tenancyName = bodyModel.tenantName.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
                ElasticResponse response;
                var key = Encoding.ASCII.GetBytes(appKeys.SecretKey);
                var client9035 = new ElasticSearchOps(appKeys.ElasticSearch_Authority, "9035_assets", null, null, key);

                //create FormData on ElasticSearch
                response = client9035.SetAsset("FormData", bodyModel.Sysid, bodyModel.JsonString, true);
                if (!response.Success)
                {
                    throw response.OriginalException;
                }
                logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : FormData Asset has been created on ES successfully";
                Log.Info(logMsg);


                response = client9035.SetAsset("Lists", bodyModel.Sysid, bodyModel.ListJsonString, true);
                if (!response.Success)
                {
                    throw response.OriginalException;
                }
                logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Lists Asset has been created on ES successfully";
                Log.Info(logMsg);
               
                // In Case of submit sends the message
                if (response.Success && bodyModel.isSubmit == true)
                {
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Submit 9035 has been executed.";
                    outResponse.success = true;
                    outResponse.message = "Your 9035 will be drafted on the DOL ETA site and a copy will be uploaded to Cobalt.  You will receive a notification from ServiceNow when it is complete.  If you have any questions, contact #automationinfo.";
                }
                // in case of save Later
                else if (response.Success && bodyModel.isSubmit == false)
                {
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Save Later has been executed";
                    outResponse.success = true;
                    outResponse.message = @"Your progress has been saved.  To access your draft 9035 in the future, click on the ""Access My Draft 9035"" button in your ServiceNow ticket and you will be redirected to the form.";
                }
                // In case of any error
                else
                {
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Some error occurred while returning an asset";
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";

                }
                Log.Info(logMsg);
            }
            catch (Exception ex)
            {
                outResponse.success = false;
                outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                Log.Error(ex, bodyModel.BalNumber);
                es.AddErrorESLog(bodyModel.Sysid, "Technical", ex.Message, out errorMessage);
                api.AddErrorQueueItem(bodyModel.Sysid, ex.Message, "Technical", ex.Message, "In Progress");

                if (WebConfigurationManager.AppSettings["EnvironmentName"] == "prod")
                {
                    //create jira exception ticket for production environment
                    try
                    {
                        JiraTicket jira = new JiraTicket();
                        jira.CreateJiraException(bodyModel.Sysid, ex.Message);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception, bodyModel.BalNumber);
                    }
                }
            }
            assetResponse = JsonConvert.SerializeObject(outResponse);
            return Json(assetResponse);
        }

        [HttpPost]
        public IHttpActionResult AddQueueItem([FromBody] PostSaveData bodyModel)
        {
            string assetResponse = "";
            Response outResponse = new Response();
            OrchestratorAPI api = new OrchestratorAPI();
           
           
            string logMsg = "";
            string errorMessage = "";
            try
            {
                if (string.IsNullOrEmpty(bodyModel.Sysid) && string.IsNullOrEmpty(bodyModel.BalNumber) && string.IsNullOrEmpty(bodyModel.Email))
                {
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                    logMsg = "Bal Number : " + bodyModel.BalNumber + ", Process : Add Queue Item, Message : JsonString  parameter required";
                    Log.Info(logMsg);
                    assetResponse = JsonConvert.SerializeObject(outResponse);
                    es.AddErrorESLog(bodyModel.Sysid, "Technical", "QueueItem API Parameter Missing", out errorMessage);
                    api.AddErrorQueueItem(bodyModel.Sysid, "QueueItem API Parameter Missing", "Technical", "QueueItem API Parameter Missing", "In Progress");
                    return Json(assetResponse);
                }
                string filter = "?$filter=Name eq '" + bodyModel.BalNumber + "'";
                // configuration
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
               
                //Set which tenant to use
                appKeys.tenancyName = bodyModel.tenantName.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
                string token = api.Authentication(appKeys);
               
                var key = Encoding.ASCII.GetBytes(appKeys.SecretKey);
                var client9035 = new ElasticSearchOps(appKeys.ElasticSearch_Authority, "9035_assets", null, null, key);
                ElasticResponse FormDataResponse = client9035.GetAsset("FormData", bodyModel.Sysid, out object formDataResult);

               // AssetModel assetModel = api.GetAsset(token, filter);
                if (FormDataResponse.Success)
                {
                    SpecificContent content = new SpecificContent();
                    content.ID = bodyModel.Sysid;
                    content.Err_Message = null;
                    content.Err_Type = null;
                    content.Note = null;
                    content.Status = null;
                    content.Data = "[{'CaseNo':'" + bodyModel.BalNumber + "','Email':'" + bodyModel.Email + "','EsIdNo':'" + bodyModel.EsIdNo + "', 'ExtraAsset':''}]";
                    ItemData itemData = new ItemData();
                    itemData.Name = appKeys.QueueName;
                    itemData.Priority = "Normal";
                    itemData.Reference = bodyModel.Sysid;
                    itemData.SpecificContent = content;

                    RootObject queueItemBody = new RootObject();
                    queueItemBody.itemData = itemData;

                    string strQueue = JsonConvert.SerializeObject(queueItemBody);
                    string message = api.AddQueueItem(token, strQueue);

                    var messageResult = JsonConvert.DeserializeObject<ItemData>(message).SpecificContent;
                    if (messageResult != null && !string.IsNullOrEmpty(messageResult.ID))
                    {
                        // Entry point on ES
                        //  if (es.AddESLog(bodyModel.BalNumber, bodyModel.Sysid, out errorMessage) == HttpStatusCode.BadRequest)
                        //    Log.Info(bodyModel.BalNumber + ": Process : Draft9089 -  Error Occurred while Calling ES. See ES Log File for further details");
                    }
                    outResponse.success = true;
                    outResponse.message = "Your 9035 will be drafted on FLAG and a copy will be updated to Cobalt. You will receive a notification from ServiceNow when it is complete. if you have any questions, contact #automationinfo";
                    logMsg = "Bal Number : " + bodyModel.BalNumber + ", Process : Add Queue Item, Message : Queueitem has been added.";
                    Log.Info(logMsg);
                }
            }
            catch (Exception ex)
            {
                outResponse.success = false;
                outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                Log.Error(ex, bodyModel.BalNumber);
                es.AddErrorESLog(bodyModel.Sysid, "Technical", ex.Message, out errorMessage);
                api.AddErrorQueueItem(bodyModel.Sysid, ex.Message, "Technical", ex.Message, "In Progress");

                if (WebConfigurationManager.AppSettings["EnvironmentName"] == "prod")
                {
                    //create jira exception ticket for production environment
                    try
                    {
                        JiraTicket jira = new JiraTicket();
                        jira.CreateJiraException(bodyModel.Sysid, ex.Message);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception, bodyModel.BalNumber);
                    }
                }
            }
            assetResponse = JsonConvert.SerializeObject(outResponse);
            return Json(assetResponse);
        }


    }
}
