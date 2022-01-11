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
                    //for staging localhost
                    //request.State = "'+bal_no=1615.54312.7;id_no=BOT0001711;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
                    //localhost
                    request.State = "'+bal_no=20000.50386.18;id_no=BOT0002815;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
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
                    //Set which tenant to use
                    appKeys.tenancyName = ViewBag.id_no.ToString().StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;
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

                    //get Cobalt-D Asset from ElasticSearch
                    if (ViewBag.id_no.ToString().StartsWith("COB") && !FormDataResponse.Success)
                    {
                        var cobaltDclient9035 = new ElasticSearchOps(appKeys.ElasticSearch_Authority, "cobaltd_assets", null, null, key);
                        ElasticResponse cobaltDFormDataResponse = cobaltDclient9035.GetAsset("FormData", ViewBag.id_no, out object cobaltDformDataResult);
                        if (cobaltDFormDataResponse.Success)
                        {
                            form = JsonConvert.DeserializeObject<Form9035>((string)cobaltDformDataResult);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message : Cobalt-D Form9035 Data has been decrypted successfully.");
                        }
                        ElasticResponse CobaltDListsResponse = cobaltDclient9035.GetAsset("Lists", ViewBag.id_no, out object cobaltDListsResult);
                        if (CobaltDListsResponse.Success)
                        {
                            allLists = JsonConvert.DeserializeObject<Lists>((string)cobaltDListsResult);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message : Cobalt-D Lists Data has been decrypted successfully.");
                        }
                    }

                    // if data not exist check from SQL
                    if (!ViewBag.id_no.ToString().StartsWith("COB") && !FormDataResponse.Success)
                    {
                        //string query = @"select * from BAL9035";
                        //string query = @"select * from BAL9035 where BALNumber='20000.50054.51'";
                        string query = @"select c.CompanyName, c.CompanyNumber, c.IsH1BDependent as 'Company H-1B Dependent', ce.EntityName as 'Sponsoring Entity', ce.IsH1BDependent as 'Entity H-1B Dependent',
                        b.MatterNumber, b.FullName as 'Beneficiary', cc.JobPosition as 'Beneficiary Job Title',
                        cc.BALNumber, cst.CaseSubType, --max of 6
                        lca.SocCode, lca.SocOccupation, lca.BeginOfValidity, lca.EndOfValidity, lca.WageRangeLow, lca.WageRangeHigh, lca.WageLevel, lca.NumberOfPositions,
                        bt1.StaffFirstName as 'Attorney First Name', bt1.StaffMiddleName as 'Attorney Middle Name', bt1.StaffLastName as 'Attorney Last Name', bt1.StaffEmail as 'Attorney Email',
                        bt2.StaffFirstName as 'Assistant First Name', bt2.StaffMiddleName as 'Assistant Middle Name', bt2.StaffLastName as 'Assistant Last Name', bt2.StaffEmail as 'Assistant Email',
                        bt3.ContactFirstName as 'Signer First Name', bt3.ContactMiddleName as 'Signer Middle Name', bt3.ContactLastName as 'Signer Last Name', bt3.JobTitle as 'Signer Job',
                        ca.AddressLine1, ca.AddressLine2, ca.Suite, ca.City, ca.State, ca.ZipCode, l.PrevailingWage, l.PrevailingWageSource, l.PrevailingWagePublishedYear, l.PrevailingWageOther,
                        p.BALNumber as 'Parent Case Number', ps.CaseSubType as 'ParentCaseSubType',
                        c1.CountryName as 'Citizenship 1', c2.CountryName as 'Citizenship 2', c3.CountryName as 'Citizenship 3',
                        c4.CountryName as 'Citizenship 4', c5.CountryName as 'Citizenship 5', c6.CountryName as 'Citizenship 6',
                        c7.CountryName as 'Citizenship 7', c8.CountryName as 'Citizenship 8', c9.CountryName as 'Citizenship 9', c10.CountryName as 'Citizenship 10'
                        from ClientCase cc
                        inner join Beneficiary b on b.beneid = cc.BeneId
                        inner join Company c on c.CompanyId = b.CompanyId
                        inner join LCADetail lca on lca.CaseId = cc.CaseId
                        left join CompanyEntity ce on ce.CompanyEntityId = cc.SponsorEntityId
                        inner join casecontacts cc1 (nolock) on cc.caseid = cc1.caseid and cc1.casecontacttype = 'BAL_MANAGER' and cc1.isprimary = 1
                        inner join balteam bt1 (nolock) on cc1.userid = bt1.userid
                        inner join casecontacts cc2 (nolock) on cc.caseid = cc2.caseid and cc2.casecontacttype = 'BAL_ASSISTANT' and cc2.isprimary = 1
                        inner join balteam bt2 (nolock) on cc2.userid = bt2.userid
                        left join casecontacts cc3 (nolock) on cc.caseid = cc3.caseid and cc3.casecontacttype = 'SIGNER'
                        left join CompanyContacts bt3 (nolock) on cc3.userid = bt3.userid
                        --inner join CaseAddress a on a.CaseId = cc.CaseId
                        left join CaseCompanyAddressLink l on l.CaseId = cc.CaseId
                        left join CompanyAddress ca on ca.CompanyAddressId = l.CompanyAddressId
                        inner join CaseSubTypeRel r on r.CaseId = cc.CaseId
                        inner join CaseSubTypes cst on cst.MetaDataId = r.MetaDataId
                        left join ClientCase p on cc.ParentCaseId = p.CaseId
                        left join CaseSubTypeRel pr on pr.CaseId = p.CaseId
                        left join CaseSubTypes ps on ps.MetaDataId = pr.MetaDataId
                        left join Country c1 on c1.CountryCode = b.NationalityCountryCode1
                        left join Country c2 on c2.CountryCode = b.NationalityCountryCode2
                        left join Country c3 on c3.CountryCode = b.NationalityCountryCode3
                        left join Country c4 on c4.CountryCode = b.NationalityCountryCode4
                        left join Country c5 on c5.CountryCode = b.NationalityCountryCode5
                        left join Country c6 on c6.CountryCode = b.NationalityCountryCode6
                        left join Country c7 on c7.CountryCode = b.NationalityCountryCode7
                        left join Country c8 on c8.CountryCode = b.NationalityCountryCode8
                        left join Country c9 on c9.CountryCode = b.NationalityCountryCode9
                        left join Country c10 on c10.CountryCode = b.NationalityCountryCode10
                        where CC.BALNumber ='" + bal_no + "'order by cc.BALNumber";
                        //20000.55.52,20000.50054.51
                        // Calling db object and sending the query and returning the DataTable
                        Database db = new Database();
                        DataTable dt = db.SqlSelect(query);
                        // If Results found map data else send Error message
                        if (dt.Rows.Count > 0)
                        {
                            // Mapping DataTable to the 9035 and Lists object
                            MapDbData dbData = new MapDbData();
                            form = dbData.MapdbResult(dt);
                            allLists = dbData.CreateLists(dt, bal_no);
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
                        int queueId = api.GetQueueID(token);
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
