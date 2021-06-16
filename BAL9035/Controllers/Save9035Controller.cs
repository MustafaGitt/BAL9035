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
                string filter = "";
                // configuration getting from Appsetting
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();

                //Set which tenant to use
                appKeys.tenancyName = bodyModel.Sysid.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
                // Saving all keys in a class object
                string token = api.Authentication(appKeys);
                // check if token is generated or not
                if (string.IsNullOrEmpty(token))
                {
                    outResponse.success = false;
                    outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : UIPATH Token has not been generated. Token : " + token;
                    Log.Info(logMsg);
                    es.AddErrorESLog(bodyModel.Sysid, "Technical", "UIPATH Token not generated. Authentication Failed.", out errorMessage);
                    api.AddErrorQueueItem(bodyModel.Sysid, "UIPATH Token not generated. Authentication Failed.", "Technical", "UIPATH Token not generated. Authentication Failed.", "In Progress");
                }
                else
                {
                    // GETS all the Assets against the BAL Number
                    string assetName = bodyModel.BalNumber;
                    filter = "?$filter=Name eq '" + bodyModel.BalNumber + "'";
                    AssetModel assetModel = api.GetAsset(token, filter);
                    // Removing List asset from the assetModel Object
                    if (assetModel.value.Count > 1)
                    {
                        foreach (var item in assetModel.value)
                        {
                            if (item.Name.Contains("Lists"))
                            {
                                assetModel.value.Remove(item);
                                break;
                            }
                        }
                    }
                    // create or update asset
                    Dictionary<string, object> assetBody = new Dictionary<string, object>();
                    assetBody.Add("Name", assetName);
                    assetBody.Add("ValueScope", "Global");
                    assetBody.Add("ValueType", "Text");
                    //Object to JSON
                    //string jsonString = JsonConvert.SerializeObject(bodyModel.JsonString);
                    //Compressing JSON
                    string compressValue = CompressString.Zip(bodyModel.JsonString);
                    //Encoding JSON
                    string encodeValue = SecureData.AesEncryptString(appKeys.SecretKey, compressValue);
                    int chkStrLen = encodeValue.Length;
                    // If length is greater than 4000 Create 2 Assets elsse 1
                    if (chkStrLen > 4000)
                    {
                        assetBody.Add("StringValue", encodeValue.Substring(0, 4000));
                        assetResponse = api.CreateAsset(assetModel, token, assetBody);
                        string secondAsset = encodeValue.Substring(4000);
                        // get n check if asset exist
                        string filter1 = "?$filter=Name eq '" + bodyModel.BalNumber + " A" + "'";
                        assetModel = api.GetAsset(token, filter1);
                        //second asset
                        Dictionary<string, object> asset2Body = new Dictionary<string, object>();
                        asset2Body.Add("ValueScope", "Global");
                        asset2Body.Add("ValueType", "Text");
                        asset2Body.Add("Name", bodyModel.BalNumber + " A");
                        asset2Body.Add("StringValue", secondAsset);
                        assetResponse = api.CreateAsset(assetModel, token, asset2Body);
                        logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : 2 Assets has been created successfully";
                        Log.Info(logMsg);

                    }
                    else
                    {
                        // Delete the current existing assets first
                        if (assetModel.value.Count == 2)
                        {
                            string id = assetModel.value[1].Id.ToString();
                            string deleteAsset = api.DeleteAsset(id, token);
                            logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Extra Asset has been deleted";
                            Log.Info(logMsg);
                        }
                        assetBody.Add("StringValue", encodeValue);
                        assetResponse = api.CreateAsset(assetModel, token, assetBody);
                        logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Data asset has been created";
                        Log.Info(logMsg);
                    }
                    //Compressing JSON
                    compressValue = CompressString.Zip(bodyModel.ListJsonString);
                    //Encoding JSON
                    encodeValue = SecureData.AesEncryptString(appKeys.SecretKey, compressValue);
                    // getting lists assets
                    filter = "?$filter=Name eq '" + bodyModel.BalNumber + ".Lists'";
                    AssetModel ListAssetModel = api.GetAsset(token, filter);
                    // Adding or updating Lits Assets
                    Dictionary<string, object> assetListBody = new Dictionary<string, object>();
                    assetListBody.Add("Name", bodyModel.BalNumber + ".Lists");
                    assetListBody.Add("ValueScope", "Global");
                    assetListBody.Add("ValueType", "Text");
                    assetListBody.Add("StringValue", encodeValue);

                    assetResponse = api.CreateAsset(ListAssetModel, token, assetListBody);
                    // get n check if asset exist
                    logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : List Asset has been created";
                    Log.Info(logMsg);
                    assetModel = api.GetAsset(token, filter);
                    // In Case of submit sends the message
                    if (assetModel.value.Count > 0 && bodyModel.isSubmit == true)
                    {
                        logMsg = "Bal Number" + bodyModel.BalNumber + " , Process : Save9035 Create Asset, Message : Submit 9035 has been executed.";
                        outResponse.success = true;
                        outResponse.message = "Your 9035 will be drafted on the DOL ETA site and a copy will be uploaded to Cobalt.  You will receive a notification from ServiceNow when it is complete.  If you have any questions, contact #automationinfo.";
                    }
                    // in case of save Later
                    else if (assetModel.value.Count > 0 && bodyModel.isSubmit == false)
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
            }
            catch (Exception ex)
            {
                outResponse.success = false;
                outResponse.message = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly.";
                Log.Error(ex, bodyModel.BalNumber);
                es.AddErrorESLog(bodyModel.Sysid, "Technical", ex.Message, out errorMessage);
                api.AddErrorQueueItem(bodyModel.Sysid, ex.Message, "Technical", ex.Message, "In Progress");
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
                appKeys.tenancyName = bodyModel.Sysid.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
                string token = api.Authentication(appKeys);

                AssetModel assetModel = api.GetAsset(token, filter);
                if (assetModel.value.Count > 0)
                {
                    SpecificContent content = new SpecificContent();
                    content.ID = bodyModel.Sysid;
                    content.Err_Message = null;
                    content.Err_Type = null;
                    content.Note = null;
                    content.Status = null;
                    content.Data = "[{'CaseNo':'" + bodyModel.BalNumber + "','Email':'" + bodyModel.Email + "','EsIdNo':'" + bodyModel.EsIdNo + "', 'ExtraAsset':''}]";
                    if (assetModel.value.Count > 2)
                    {
                        content.Data = "[{'CaseNo':'" + bodyModel.BalNumber + "','Email':'" + bodyModel.Email + "','EsIdNo':'" + bodyModel.EsIdNo + "', 'ExtraAsset':'" + bodyModel.BalNumber + " A" + "'}]";
                    }

                    ItemData itemData = new ItemData();
                    itemData.Name = appKeys.QueueName;
                    itemData.Priority = "Normal";
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
            }
            assetResponse = JsonConvert.SerializeObject(outResponse);
            return Json(assetResponse);
        }


    }
}
