using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BAL9035.Core;
using BAL9035.Models;
using Newtonsoft.Json;
using _9035BL;
using System.Net;
using System.Web.Configuration;
using System.Text;
using Accelirate.ElasticSearch.Common;
using Newtonsoft.Json.Linq;
using System.Data.Odbc;

namespace BAL9035.Controllers
{
    /*
     * 9035 WEB APP CONTROLLER (Entry Point of the Application)
     * Populate the 9035 View with the Data and Also Contains The Error message View
     */
    public class Draft9035Controller : Controller
    {
        // POST: Draft9035
        [System.Web.Http.HttpPost]
        // Route Definition
        [System.Web.Mvc.Route("Draft9035")]
        // Return a Model Object with index View and fill the data from Assets / Database
        public ActionResult Index([FromBody] OIDCRequest request)
        {
            // Model Object Declerations
            ParentModel obj = new ParentModel();
            Form9035 form = new Form9035();
            Lists allLists = new Lists();
            CredentialAsset credentialsAsset = new CredentialAsset();
            OrchestratorAPI api = new OrchestratorAPI();
            // Add starting point on ES
            ElasticSearch es = new ElasticSearch();
            string bal_no = "";
            ViewBag.id_no = "";
            string errorMessage = "";
            try
            {
                AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
                string url = Request.Url.ToString();
                var key = Encoding.ASCII.GetBytes(appKeys.SecretKey);
                var client9035 = new ElasticSearchOps(appKeys.ElasticSearch_Authority, "9035_assets", null, null, key);
                // LOCAL USE : Comment this 2 lines whenever you need to publish and push the code these are only for local use
                // add the case no. and the id no. whenever you're debugging and using this as local
                if (url.Contains("localhost"))
                {
                    //localhost
                    //request.State = "'+bal_no=20000.53388.39;id_no=BOT0000090;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
                    request.State = "'+bal_no=A002.369.6;id_no=BOT000012;cobalt=cobaltD;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
                    request.Code = "182635";
                }
                TempData["Error"] = "";
                string dolUserName = string.Empty;
                string dolPassword = string.Empty;

                if ((request.Code != null && request.Code != "") && (request.State != null && request.State != ""))
                {
                    // GET The username which try to access this page
                    string sUserName = null;

                    if (url.Contains("localhost"))
                        sUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    else
                        sUserName = User.Identity.Name;
                    // Extract the username from the identity
                    Log.Info("The User identity Name is : " + sUserName);
                    string[] spliUser = sUserName.Split('\\');
                    sUserName = spliUser[1];
                    Log.Info("The User Name is : " + sUserName);
                    // Split the request string and extract BAL Number and ID Number
                    string stateStr = request.State.ToString().Substring(2);
                    string[] stateArray = stateStr.Split('-');
                    string splitArray = stateArray[0].Replace(";", "&");
                    NameValueCollection queryString = HttpUtility.ParseQueryString(splitArray);
                    bal_no = queryString["bal_no"].ToString();
                    // Save into ViewBags for JS Use
                    ViewBag.id_no = queryString["id_no"];
                    ViewBag.bal_no = bal_no;
                    ViewBag.email_id = sUserName;
                    string tenantName = queryString["cobalt"];
                    ViewBag.cobaltTenant = !string.IsNullOrEmpty(tenantName) && tenantName.ToLower() == "cobaltd" ? "COB" : "BOT";


                    //Set which tenant to use
                    appKeys.tenancyName = ViewBag.cobaltTenant.ToString().StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;

                    if (!ViewBag.cobaltTenant.ToString().StartsWith("COB") && bal_no.StartsWith("1615."))
                    {
                        TempData["Error"] = "You have entered an invalid case matter number. Please submit a new request with the correct matter number";
                        return RedirectToAction("Error");
                    }
                    // Getting UIPATH Token
                    string token = api.Authentication(appKeys);
                    Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  All variables has been initialized successfully.");

                    ElasticResponse FormDataResponse = client9035.GetAsset("FormData", ViewBag.id_no, out object formDataResult);
                    if (FormDataResponse.Success)
                    {
                        form = JsonConvert.DeserializeObject<Form9035>((string)formDataResult);
                        Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Form9035 Data has been decrypted successfully.");
                    }
                    ElasticResponse ListsResponse = client9035.GetAsset("Lists", ViewBag.id_no, out object ListsResult);
                    if (ListsResponse.Success)
                    {
                        allLists = JsonConvert.DeserializeObject<Lists>((string)ListsResult);
                        Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Lists Data has been decrypted successfully.");
                    }
                    ElasticResponse CredentialsResponse = client9035.GetCredential("Credentials", ViewBag.id_no, out dolUserName, out dolPassword);

                    //get Cobalt-D Asset from DataLake
                    if (ViewBag.cobaltTenant.ToString().StartsWith("COB") && !FormDataResponse.Success)
                    {
                        var connnectionString = "DSN=" + appKeys.DataLakeUserName + ";PWD=" + appKeys.DataLakePassword + ";";
                        var cnn = new OdbcConnection(connnectionString);
                        cnn.Open();
                        //var selectSql = "SELECT * FROM hive_metastore.prd_Gold_Automation.d_doleta_9035 WHERE BALNumber = '1615.49792.16'";
                        // var selectSql = "SELECT * FROM doleta_9035 WHERE BALNumber = '" + bal_no + "' ";
                        // SELECT* FROM hive_metastore.prd_Gold_Automation.d_doleta_9035 WHERE BALNumber
                        var selectSql = WebConfigurationManager.AppSettings["EnvironmentName"] == "stg" ?
                             "SELECT * FROM hive_metastore.stg_gold_automation.d_doleta_9035 WHERE BALNumber = '" + bal_no + "'" :
                              "SELECT * FROM hive_metastore.prd_Gold_Automation.d_doleta_9035 WHERE BALNumber = '" + bal_no + "'";
                        var cmd = new OdbcCommand(selectSql, cnn);
                        var da = new OdbcDataAdapter(cmd);
                        var dtResult = new DataTable();
                        da.Fill(dtResult);
                        cnn.Close();
                        if (dtResult.Rows.Count > 0)
                        {
                            MapDataBricks dbData = new MapDataBricks();
                            form = dbData.MapBricksResult(dtResult);
                            allLists = dbData.CreateBricksListsCobaltD(dtResult, bal_no);
                            dbData.AssignValue(form, allLists.parentCaseSubTypes);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Data from SQL Query has been generated successfully.");
                        }
                        else
                        {
                            Log.Info("Bal Number : " + bal_no + ": Process : Draft9035 -  No Data Found from the SQL Query.");
                            TempData["Error"] = "You have entered an invalid case matter number. Please submit a new request with the correct matter number";
                            es.AddErrorESLog(ViewBag.id_no, "Business", "No Data Found from the SQL Query.", out errorMessage);
                            api.AddErrorQueueItem(ViewBag.id_no, TempData["Error"].ToString(), "Business", TempData["Error"].ToString(), "Failed", bal_no);
                            return RedirectToAction("Error");
                        }


                    }

                    // if data not exist check from SQL CobaltB Case
                    if (!ViewBag.cobaltTenant.ToString().StartsWith("COB") && !FormDataResponse.Success)
                    {


                        var connnectionString = "DSN=" + appKeys.DataLakeUserName + ";PWD=" + appKeys.DataLakePassword + ";";
                        var cnn = new OdbcConnection(connnectionString);
                        cnn.Open();
                        var selectSql = WebConfigurationManager.AppSettings["EnvironmentName"] == "stg" ?
                            "SELECT * FROM hive_metastore.stg_gold_automation.b_doleta_9035 WHERE BALNumber = '" + bal_no + "' " :
                             "SELECT * FROM hive_metastore.prd_Gold_Automation.b_doleta_9035 WHERE BALNumber = '" + bal_no + "' ";
                        var cmd = new OdbcCommand(selectSql, cnn);
                        var da = new OdbcDataAdapter(cmd);
                        var dtResult = new DataTable();
                        da.Fill(dtResult);
                        cnn.Close();
                        if (dtResult.Rows.Count > 0)
                        {
                            MapDataBricks dbData = new MapDataBricks();
                            form = dbData.MapBricksResult(dtResult);
                            allLists = dbData.CreateBricksLists(dtResult, bal_no);
                            dbData.AssignValue(form, allLists.parentCaseSubTypes);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Data from SQL Query has been generated successfully.");
                        }
                        else
                        {
                            Log.Info("Bal Number : " + bal_no + ": Process : Draft9035 -  No Data Found from the SQL Query.");
                            TempData["Error"] = "You have entered an invalid case matter number. Please submit a new request with the correct matter number";
                            es.AddErrorESLog(ViewBag.id_no, "Business", "No Data Found from the SQL Query.", out errorMessage);
                            api.AddErrorQueueItem(ViewBag.id_no, TempData["Error"].ToString(), "Business", TempData["Error"].ToString(), "Failed", bal_no);
                            return RedirectToAction("Error");
                        }
                    }

                    // Check Credential Asset and see if it is submit case
                    form.isSubmit = false;
                    if (CredentialsResponse.Success && !string.IsNullOrEmpty(dolUserName) && !string.IsNullOrEmpty(dolPassword))
                    {
                        int queueId = api.GetQueueID(token, appKeys.QueueName);
                        List<QueueItemCount> queueItems = api.GetQueueItemsByID(token, queueId, ViewBag.id_no);
                        if (queueItems.Count > 0)
                        {
                            string itemStatus = queueItems.FirstOrDefault().Status;
                            if (itemStatus.ToLower() == "deleted" || itemStatus.ToLower() == "successful" || itemStatus.ToLower() == "failed")
                            {
                                form.isSubmit = false;
                            }
                            else
                            {
                                form.isSubmit = true;
                            }
                        }
                    }

                    obj.Form9035 = form;
                    obj.Lists = allLists;
                    Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message : Process has been executued successfully returning an object to View.");
                    return View(obj);
                }
                else
                {
                    throw new Exception("HTTP Web Request Failed. The data in the URL is not found.");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly";
                Log.Error(ex, bal_no);
                es.AddErrorESLog(ViewBag.id_no, "Technical", ex.Message, out errorMessage);
                bool isSqlException = ex.GetType().Name == "SqlException" ? true : false;
                if (isSqlException)
                {
                    api.AddErrorQueueItem(ViewBag.id_no, "There is an intermittent database issue that is preventing the bot from proceeding with your 9035. Please try resubmitting your request in 30 minutes. If you have any questions, please contact #automationinfo.", "Business", "There is an intermittent database issue that is preventing the bot from proceeding with your 9035. Please try resubmitting your request in 30 minutes. If you have any questions, please contact #automationinfo.", "Failed", bal_no);
                }
                else
                {
                    api.AddErrorQueueItem(ViewBag.id_no, ex.Message, "Technical", ex.Message, "In Progress", bal_no);
                }

                if (WebConfigurationManager.AppSettings["EnvironmentName"] == "prod")
                {
                    //create jira exception ticket for production environment
                    try
                    {
                        JiraTicket jira = new JiraTicket();
                        jira.CreateJiraException(ViewBag.id_no, ex.Message);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception, bal_no);
                        return RedirectToAction("Error");
                    }
                }
                return RedirectToAction("Error");
            }
        }
        // Sends an Error message and display it to the user if any exception or error occurred
        public ActionResult Error()
        {
            ErrorDetails details = new ErrorDetails();
            if (TempData["Error"] != null)
            {
                if (TempData["Error"].ToString() != "" && TempData["Error"].ToString() != null)
                {
                    details.errorMsg = TempData["Error"].ToString();
                }
            }
            else
            {
                details.errorMsg = "Sorry, there seems to be an issue with your request.  Please send an email to #automationinfo with your ServiceNow ticket number and we will contact you shortly";
            }

            return View(details);
        }
        // Display a static HTML View which have instructions to get the Secret Key
        [System.Web.Mvc.Route("Draft9035/Key", Name = "key")]
        [System.Web.Mvc.HttpGet]
        public ActionResult Key()
        {
            return View("KeyHowTo");
        }


    }
}
