using BAL9035.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _9035BL;

namespace BAL9035.Core
{
    /* Contains all the methods which are interacting with UIPATH */
    public class OrchestratorAPI
    {
        RestApiServices api = new RestApiServices();
        AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();

        // GETS Orchestrator Token
        public string Authentication(AppSettingsValues parentAppKeys)
        {
            try
            {
                string Url = "";
                Dictionary<string, string> authBody = new Dictionary<string, string>();
                string body = "";
                string response = "";
                if (parentAppKeys.isDevelopmentEnvironment == false)
                {
                    Url = parentAppKeys.OrchestratorUrl + "api/Account/Authenticate";
                    if (parentAppKeys.tenancyName != "")
                    {
                        authBody.Add("tenancyName", parentAppKeys.tenancyName);
                    }
                    authBody.Add("usernameOrEmailAddress", parentAppKeys.usernameOrEmailAddress);
                    authBody.Add("password", parentAppKeys.password);
                    body = JsonConvert.SerializeObject(authBody);
                    response = api.Post(Url, body);
                    OrchestratorAuthenticateModel authenticateModel = JsonConvert.DeserializeObject<OrchestratorAuthenticateModel>(response);
                    return authenticateModel.result.ToString();
                }
                else
                {
                    Url = "https://account.uipath.com/oauth/token";
                    authBody = new Dictionary<string, string>();
                    authBody.Add("grant_type", "refresh_token");
                    authBody.Add("client_id", "8DEv1AMNXczW3y4U15LL3jYf62jK93n5");
                    authBody.Add("refresh_token", "T_a1NEpX6dShm2BksNE4-eDT_-foVjJLE3ntzcz5-_sUC");
                    body = JsonConvert.SerializeObject(authBody);
                    response = api.PostCloud(Url, body, true);
                    OrchestratorAuthenticateModel authenticateModel = JsonConvert.DeserializeObject<OrchestratorAuthenticateModel>(response);
                    return authenticateModel.access_token.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        // GET Assets and returns the result in string (NOT IN USE)
        public string GetAsset(string url, string token, string filterValue = "")
        {
            try
            {
                if (filterValue != "")
                {
                    url = url + filterValue;
                }
                string response = api.Get(url, token);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // GET ASSET and Returns Asset Model
        public AssetModel GetAsset(string token, string filterValue = "")
        {
            try
            {
                string url = appKeys.OrchestratorUrl + "odata/Assets";
                if (filterValue != "")
                {
                    url = url + filterValue;
                }
                string response = string.Empty;
                if (appKeys.isDevelopmentEnvironment == true)
                {
                    response = api.GetCloud(url, token);
                }
                else
                {
                    response = api.Get(url, token);
                }
                AssetModel assetModel = JsonConvert.DeserializeObject<AssetModel>(response);
                return assetModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Create ASSETS
        public string CreateAsset(AssetModel assetModel, string token, Dictionary<string, object> body)
        {
            try
            {
                string assetResponse = "";
                string jsonBody = "";
                string url = appKeys.OrchestratorUrl + "odata/Assets";
                if (assetModel.value.Count > 0)
                {
                    foreach (var item in assetModel.value)
                    {
                        body.Add("Id", item.Id);
                        url = url + "(" + item.Id.ToString() + ")";
                        jsonBody = JsonConvert.SerializeObject(body);
                        api.Put(url, jsonBody, token);
                        if (assetResponse == "")
                        {
                            assetResponse = "Asset updated successfully.";
                        }
                        break;

                    }
                }
                else
                {
                    jsonBody = JsonConvert.SerializeObject(body);
                    assetResponse = api.Post(url, jsonBody, token);
                }

                return assetResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        // Add a Queue Item
        public string AddQueueItem(string token, string body)
        {
            try
            {
                string url = appKeys.OrchestratorUrl + "odata/Queues/UiPathODataSvc.AddQueueItem";
                string responseString = "";
                responseString = api.Post(url, body, token);
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        // Deletes an Asset
        public string DeleteAsset(string id, string token)
        {
            try
            {
                string responseString = "";
                string url = appKeys.OrchestratorUrl + "odata/Assets(" + id + ")";
                responseString = api.Delete(url, token);
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Get the ID of Queue
        public int GetQueueID(string token,string queueName)
        {
            try
            {
                string url = appKeys.OrchestratorUrl + "odata/QueueDefinitions?$filter=Name eq '" + queueName + "'";
                string response = string.Empty;
                if (appKeys.isDevelopmentEnvironment == true)
                {
                    response = api.GetCloud(url, token);
                }
                else
                {
                    response = api.Get(url, token);
                }

                QueueDefinitionParent queueDetails = JsonConvert.DeserializeObject<QueueDefinitionParent>(response);
                return queueDetails.value.First().Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Get the QueueItems By QueueId
        public List<QueueItemCount> GetQueueItemsByID(string token,int queueId,string ticketNo)
        {
            try
            {
                string url = appKeys.OrchestratorUrl + "odata/QueueItems?$filter=QueueDefinitionId eq " + queueId + " and Reference eq '" + ticketNo + "'";
                string response = string.Empty;
                if (appKeys.isDevelopmentEnvironment == true)
                {
                    response = api.GetCloud(url, token);
                }
                else
                {
                    response = api.Get(url, token);
                }

                QueueItemCountParent queueItems = JsonConvert.DeserializeObject<QueueItemCountParent>(response);
                return queueItems.value.OrderByDescending(q=>q.CreationTime).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Get the Transactions from a Queue Item
        public string GetTransactions(string token, string QueueID, string QueueCreationTime)
        {
            try
            {
                string url = appKeys.OrchestratorUrl + "odata/QueueItems?$filter=QueueDefinitionId%20eq%20" + QueueID + " and Status%20eq%20'New' and CreationTime le " + QueueCreationTime + "";
                string response = api.Get(url, token);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddErrorQueueItem(string ID, string Err_Message, string Err_Type, string Note, string Status, string BALNumber = "")
        {
            try
            {
                if (string.IsNullOrEmpty(ID))
                    ID = "BOT0000000";
                SpecificContent content = new SpecificContent();
                content.ID = ID;
                content.Err_Message = Err_Message;
                content.Err_Type = Err_Type;
                content.Note = Note;
                content.Status = Status;
                content.Data = "[{'CaseNo':'" + BALNumber + "', 'ExtraAsset':'', 'FilePath':'NA'}]";
                ItemData itemData = new ItemData();
                itemData.Name = appKeys.ResponseQueueName;
                itemData.Priority = "Normal";
                itemData.SpecificContent = content;

                //Set which tenant to use
                appKeys.tenancyName = ID.StartsWith("COB") ? appKeys.cobaltDtenancyName : appKeys.tenancyName;

                RootObject queueItemBody = new RootObject();
                queueItemBody.itemData = itemData;
                string token = Authentication(appKeys);
                string strQueue = JsonConvert.SerializeObject(queueItemBody);
                AddQueueItem(token, strQueue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}