﻿using System;
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
                // LOCAL USE : Comment this 2 lines whenever you need to publish and push the code these are only for local use
                // add the case no. and the id no. whenever you're debugging and using this as local
                if (url.Contains("localhost"))
                {
                    //for staging localhost
                    request.State = "'+bal_no=1615.54312.7;id_no=BOT0001711;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
                    //localhost
                    request.State = "'+bal_no=91.147449.2;id_no=43383;-5791a545d45a92763d8216ffb7004e3ebc32226af366113cf24975ea00014d51+";
                    request.Code = "182635";
                }
                TempData["Error"] = "";
                string filter = "";
                string encryptedData = "";
                string decodeData = "";
                string data = "";
                string ListencryptedData = "";
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
                    // Entry point on ES
                    if (es.AddESLog(ViewBag.bal_no, ViewBag.id_no, out errorMessage) == HttpStatusCode.BadRequest)
                        Log.Info(bal_no + ": Process : Draft9089 -  Error Occurred while Calling ES. See ES Log File for further details");
                    // Getting UIPATH Token
                    string token = api.Authentication(appKeys.isDevelopmentEnvironment);
                    Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  All variables has been initialized successfully.");
                    // get n check if asset exist
                    filter = "?$filter=Name eq '" + bal_no + "'";
                    AssetModel assetModel = api.GetAsset(token, filter);
                    // Extracting the Encrypted Data String
                    foreach (var item in assetModel.value)
                    {
                        // 9035 Model Value
                        if (!item.Name.Contains("Lists"))
                        {
                            if (encryptedData == "")
                            {
                                encryptedData = item.Value;
                            }
                            else
                            {
                                encryptedData += item.Value;
                            }
                        }
                        // List Model Value
                        else if (item.Name.Contains("Lists"))
                        {
                            if (ListencryptedData == "")
                            {
                                ListencryptedData = item.Value;
                            }
                            else
                            {
                                ListencryptedData += item.Value;
                            }
                        }
                    }
                    // decode and deserialize data if exists form9035 object
                    if (encryptedData != "")
                    {
                        // 9035 Object Decryption
                        decodeData = SecureData.AesDecryptString(appKeys.SecretKey, encryptedData);
                        if (decodeData != "")
                        {
                            data = CompressString.Unzip(decodeData);
                            form = JsonConvert.DeserializeObject<Form9035>(data);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Form9035 Data has been decrypted successfully.");
                        }
                    }
                    // List Object Data Decryption
                    if (!string.IsNullOrEmpty(ListencryptedData))
                    {
                        decodeData = SecureData.AesDecryptString(appKeys.SecretKey, ListencryptedData);
                        if (decodeData != "")
                        {
                            data = CompressString.Unzip(decodeData);
                            allLists = JsonConvert.DeserializeObject<Lists>(data);
                            Log.Info("Bal Number : " + bal_no + " Process : Draft 9035 Page Loading, Message :  Lists Data has been decrypted successfully.");
                        }
                    }
                    // if data not exist check from SQL
                    if (assetModel.value.Count <= 0)
                    {
                        //string query = @"select * from BAL9035";
                        // string query = @"select * from BAL9035 where BALNumber='20000.50054.51'";
                        string query = @"select c.CompanyName, c.CompanyNumber, c.IsH1BDependent as 'Company H-1B Dependent', ce.EntityName as 'Sponsoring Entity', ce.IsH1BDependent as 'Entity H-1B Dependent',
                                        b.MatterNumber, b.FullName as 'Beneficiary', b.JobPosition as 'Beneficiary Job Title',
                                        cc.BALNumber, cst.CaseSubType, --max of 6
                                        lca.SocCode, lca.SocOccupation, lca.BeginOfValidity, lca.EndOfValidity, lca.WageRangeLow, lca.WageRangeHigh, lca.WageLevel, lca.NumberOfPositions,
                                        bt1.StaffFirstName as 'Attorney First Name', bt1.StaffMiddleName as 'Attorney Middle Name', bt1.StaffLastName as 'Attorney Last Name', bt1.StaffEmail as 'Attorney Email',
                                        bt2.StaffFirstName as 'Assistant First Name', bt2.StaffMiddleName as 'Assistant Middle Name', bt2.StaffLastName as 'Assistant Last Name', bt2.StaffEmail as 'Assistant Email',
                                        bt3.ContactFirstName as 'Signer First Name', bt3.ContactMiddleName as 'Signer Middle Name', bt3.ContactLastName as 'Signer Last Name', bt3.JobTitle as 'Signer Job',
                                        ca.AddressLine1, ca.AddressLine2, ca.Suite, ca.City, ca.State, ca.ZipCode, l.PrevailingWage, l.PrevailingWageSource, l.PrevailingWagePublishedYear, l.PrevailingWageOther,
                                        p.BALNumber as 'Parent Case Number', ps.CaseSubType as 'ParentCaseSubType'
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
                                        where CC.BALNumber = '" + bal_no + "' order by cc.BALNumber";
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
                    // Second Asset check n if it is submit case (NOT IN USE)
                    //ViewBag.isSubmit = false;
                    //filter = "?$filter=Name eq '" + ViewBag.id_no + "'";
                    //AssetModel idModel = api.GetAsset(token, filter);
                    //if (idModel.value.Count > 0 && assetModel.value.Count > 0)
                    //{
                    //    foreach (var item in idModel.value)
                    //    {
                    //        if (item.Name == ViewBag.id_no)
                    //        {
                    //            ViewBag.isSubmit = true;
                    //        }
                    //    }
                    //}
                    //form.isSubmit = false;
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
                api.AddErrorQueueItem(ViewBag.id_no, ex.Message, "Technical", ex.Message, "In Progress", bal_no);
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
