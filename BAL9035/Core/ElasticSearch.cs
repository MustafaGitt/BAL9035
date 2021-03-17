using BAL9035.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace BAL9035.Core
{
    /* Contains the method which needs to add the SE Logs */
    public class ElasticSearch
    {
        // NLOG Logger
        Logger logger = LogManager.GetLogger("ES");
        AppSettingsValues appKeys = GetAppSettings.GetAppSettingsValues();
        // Add Starting point for 9035 in ES
        public HttpStatusCode AddESLog(string caseMatterNumber, string idNo, out string errorMessage)
        {
            errorMessage = "";

            //Do Search to get AVG Execution Time and Waiting Time
            //POST - https://vpc-bal-es-uipath-stg-qbceejar3ywxu54v7vnedxfdtu.us-west-2.es.amazonaws.com/bal_uipath/_search
            //REQUEST:
            //{
            //    "aggs": {
            //        "avg_WT": {
            //            "avg": {
            //                "field": "WaitingTime"
            //            }
            //        },
            //        "avg_ET": {
            //                            "avg": {
            //                                "field": "ExecutionTime"
            //                            }
            //                        }
            //                    },
            //      "size": 0,
            //      "_source": {
            //                        "excludes": []
            //        },
            //      "stored_fields": [
            //        "*"
            //      ],
            //      "script_fields": {},
            //      "query": {
            //        "bool": {
            //          "must": [
            //            {
            //              "match_all": {}
            //            },
            //            {
            //              "match_phrase": {
            //                "QueueName.keyword": {
            //                  "query": "H1B-Request"
            //                }
            //              }
            //            }
            //          ]
            //        }
            //      }
            //    }
            //RESPONSE:
            //{
            //    "took": 1,
            //    "timed_out": false,
            //    "_shards": {
            //                    "total": 5,
            //        "successful": 5,
            //        "skipped": 0,
            //        "failed": 0
            //    },
            //    "hits": {
            //                    "total": 1,
            //        "max_score": 0.0,
            //        "hits": []
            //    },
            //    "aggregations": {
            //        "avg_ET": {
            //            "value": 360.0
            //        },
            //        "avg_WT": {
            //            "value": 20.0
            //        }
            //    }
            //}

            //Then Add log - TimeSTamp=Current Datetime + 7 hours
            //PUT - https://vpc-bal-es-uipath-stg-qbceejar3ywxu54v7vnedxfdtu.us-west-2.es.amazonaws.com/default-2019.10/bal_uipath/_doc/<SN_URL_IDNO>
            //REQUEST:            
            //{
            //    "TimeStamp": "2019-11-06T09:15:21.5201345",
            //    "QueueId": null,
            //    "QueueName": "PackUpload-Request",
            //    "MachineId": null,
            //    "MachineName":null,
            //    "ReleaseId": null,
            //    "ReleaseName": null,
            //    "PackageName": null,
            //    "EnvironmentName": null,
            //    "EnvironmentId": null,
            //    "RobotId": null,
            //    "WindowsIdentity": null,
            //    "OutputData": null,
            //    "Status": "Waiting",
            //    "Priority": "Normal",
            //    "NoOfItems":1,  
            //    "SuccessItems":0,
            //    "SpecificContent": {},
            //    "Progress": null,
            //    "Output": null,
            //    "ProcessingExceptionType": null,
            //    "ProcessingException": null,
            //    "RetryNumber": 0,
            //    "DueDate": null,
            //    "DeferDate": null,
            //    "CreationTime": "2019-11-06T09:15:21.5201345",
            //    "WaitingTime": 300,
            //    "EstimatedStartProcessing": "2019-10-24T19:20:21.5201345",
            //    "EstimatedEndProcessing": "2019-10-24T19:30:21.5201345",
            //    "StartProcessing": null,
            //    "EndProcessing": null,
            //    "ExecutionTime": null,
            //    "AvgPerRecord":null
            //}
            //RESPONSE: 
            //{
            //    "_index": "default-2019.10",
            //    "_type": "logEvent",
            //    "_id": "ZR10y20B4zdbPNkGAs25",
            //    "_version": 1,
            //    "result": "created",
            //    "_shards": {
            //                    "total": 2,
            //        "successful": 2,
            //        "failed": 0
            //    },
            //    "_seq_no": 683010,
            //    "_primary_term": 1
            //}

            string body = "";
            string authority = appKeys.ElasticSearch_Authority;
            try
            {
                using (var client = new HttpClient())
                {
                    //1- Get Averages
                    body = "{" +
                        "\"aggs\": {" +
                            "\"avg_WT\": { \"avg\": { \"field\": \"WaitingTime\"} }," +
                            "\"avg_ET\": { \"avg\": { \"field\": \"ExecutionTime\"} }" +
                          "}," +
                          "\"size\": 0," +
                          "\"_source\": { \"excludes\": []" +
                          "}," +
                          "\"stored_fields\": [\"*\"]," +
                          "\"script_fields\": { }," +
                          "\"query\": {" +
                            "\"bool\": {" +
                              "\"must\": [" +
                                "{" +
                                  "\"match_all\": { }" +
                                "}," +
                                "{" +
                                  "\"match_phrase\": {" +
                                    "\"QueueName.keyword\": {" +
                                      "\"query\": \"" + appKeys.QueueName + "\"" +
                                    "}" +
                                  "}" +
                                "}" +
                              "]" +
                            "}" +
                          "}" +
                        "}";
                    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage response = client.PostAsync(authority + "bal_uipath/_search", content).Result;

                    //errorMessage = response.Content.ToString();
                    Double awt_d = 0;
                    Double aet_d = 0;
                    Int64 awt = 0;
                    Int64 aet = 0;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        logger.Log(LogLevel.Info, responseString);
                        var obj = JsonConvert.DeserializeObject<JObject>(responseString);
                        Double.TryParse("0" + obj["aggregations"]["avg_ET"]["value"], out aet_d);
                        Double.TryParse("0" + obj["aggregations"]["avg_WT"]["value"], out awt_d);
                        awt = Convert.ToInt64(awt_d);
                        aet = Convert.ToInt64(aet_d);
                        logger.Log(LogLevel.Info, "AET: " + aet_d + ", AWT:" + awt_d);
                        logger.Log(LogLevel.Info, "URL:" + authority + "bal_uipath/_search" + ", ES Request Body:" + body + ", Response:" + responseString + ", AET:" + aet + ", AWT:" + awt);
                    }
                    else
                    {
                        errorMessage = "ES Request Body:" + body + ", ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_uipath/";
                        logger.Log(LogLevel.Error, errorMessage);
                        return HttpStatusCode.BadRequest;
                    }


                    //2- Add log
                    //DateTime dt = DateTime.Now.AddHours(8);
                    DateTime dt = DateTime.UtcNow;
                    body = "{" +
                        "\"TimeStamp\": \"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"QueueId\": null," +
                        "\"QueueName\": \"" + appKeys.QueueName + "\"," +
                        "\"MachineId\": null," +
                        "\"MachineName\":null," +
                        "\"ReleaseId\": null," +
                        "\"ReleaseName\": null," +
                        "\"PackageName\": null," +
                        "\"EnvironmentName\": null," +
                        "\"EnvironmentId\": null," +
                        "\"RobotId\": null," +
                        "\"WindowsIdentity\": null," +
                        "\"OutputData\": null," +
                        "\"Status\": \"Waiting\"," +
                        "\"Priority\": \"Normal\"," +
                        "\"NoOfItems\":1,  " +
                        "\"SuccessItems\":0," +
                        "\"SpecificContent\": { }," +
                        "\"Progress\": null," +
                        "\"Output\": null," +
                        "\"ProcessingExceptionType\": null," +
                        "\"ProcessingException\": null," +
                        "\"RetryNumber\": 0," +
                        "\"DueDate\": null," +
                        "\"DeferDate\": null," +
                        "\"CreationTime\": \"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"WaitingTime\": null," +
                        "\"EstimatedStartProcessing\": \"" + dt.AddSeconds(awt).ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"EstimatedEndProcessing\": \"" + dt.AddSeconds(awt + aet).ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"StartProcessingTime\": null," +
                        "\"EndProcessingTime\": null," +
                        "\"ExecutionTime\": null," +
                        "\"AvgPerRecord\": null," +
                        "\"HandlingTime\": null," +
                        "\"OnTime\": null," +
                        "\"DisplayName\": null," +
                        "\"ProcessDescription\": null," +
                        "\"ProcessUnit\": null," +
                        "\"ManualTimePerUnit\": 0," +
                        "\"ManualCostPerHour\": 0," +
                        "\"ManualCostPerSecond\": 0," +
                        "\"TotalUnitsPerYear\":0}";
                    content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    logger.Log(LogLevel.Info, body);
                    // HTTP PUT
                    response = client.PutAsync(authority + "bal_uipath/_doc/" + idNo, content).Result;
                    //errorMessage = body+","+ response.ReasonPhrase+","+ authority + "bal_uipath/";
                    if (response.IsSuccessStatusCode)
                        return HttpStatusCode.OK;
                    else
                    {
                        errorMessage = "ES Request Body:" + body + ", ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_uipath/";
                        logger.Log(LogLevel.Error, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "ES Request Body:" + body + ", Error:" + ex.Message + ", ES URL:" + authority + "bal_uipath/";
                logger.Log(LogLevel.Error, errorMessage);
            }
            return HttpStatusCode.BadRequest;
        }
        // Get Secret Key of a User
        public string[] GetESKey(string uid, string keyType, out string errorMessage)
        {
            errorMessage = "";
            if (String.IsNullOrEmpty(keyType))
                keyType = "DOL";
            //Do Search to get Keys for the User ID
            //GET - https://vpc-bal-es-uipath-stg-qbceejar3ywxu54v7vnedxfdtu.us-west-2.es.amazonaws.com/bal_key/_search?q=UID:Test
            //REQUEST:            
            //RESPONSE:
            //{
            //    "took": 2,
            //    "timed_out": false,
            //    "_shards": {
            //              "total": 5,
            //              "successful": 5,
            //              "skipped": 0,
            //              "failed": 0
            //},
            //  "hits": {
            //      "total": 1,
            //      "max_score": 0.2876821,
            //      "hits": [
            //       {
            //          "_index": "bal_key",
            //          "_type": "_doc",
            //          "_id": "qVoHknABrc4FKEqxj0pp",
            //          "_score": 0.2876821,
            //          "_source": {
            //          "TimeStamp": "2020-29-02T22:16:04.835",
            //          "UID": "Test",
            //          "Key": "H1B-Request",
            //          "Type": "USCIS"
            //        }
            //      ]
            // }
            //}
            string authority = appKeys.ElasticSearch_Authority;
            try
            {
                using (var client = new HttpClient())
                {
                    //1- HTTP Get User ID Keys                    
                    HttpResponseMessage response = client.GetAsync(authority + "bal_key/_search?q=UID:" + uid.Replace("\\", "#")).Result;

                    //errorMessage = response.Content.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        logger.Log(LogLevel.Info, responseString);
                        JObject obj = JsonConvert.DeserializeObject<JObject>(responseString);
                        foreach (JObject o in obj["hits"]["hits"])
                        {
                            //ConfigurationManager.AppSettings["LookupKey"]
                            if (o["_source"]["Type"].ToString() == keyType && o["_source"]["UID"].ToString().ToLower() == uid.Replace("\\", "#").Trim().ToLower())
                            {
                                String[] results = new string[2];
                                results[0] = EncryptionUtil.Decrypt(o["_source"]["Key"].ToString(), "ES");
                                results[1] = o["_id"].ToString();
                                return results;
                            }
                        }
                        //logger.Log(LogLevel.Info, "AET: " + aet_d + ", AWT:" + awt_d);
                        //logger.Log(LogLevel.Info, "URL:" + authority + "bal_key/_search" + ", ES Request Body:" + body + ", Response:" + responseString + ", AET:" + aet + ", AWT:" + awt);
                        return null;
                    }
                    else
                    {
                        errorMessage = "ES Request UID:" + uid + ", ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_key/";
                        logger.Log(LogLevel.Error, errorMessage);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "ES Request UID:" + uid + ", Error:" + ex.Message + ", ES URL:" + authority + "bal_key/";
                logger.Log(LogLevel.Error, errorMessage);
            }
            return null;
        }
        // Insert or Update the Secret Key in ES
        public HttpStatusCode SetESKey(string keyIdNo, string uid, string key, string keyType, out string errorMessage, out string keyId)
        {
            errorMessage = "";
            keyId = "";
            if (String.IsNullOrEmpty(keyType))
                keyType = "DOL";

            //Add - TimeSTamp=Current Datetime + 7 hours
            //PUT/POST - https://vpc-bal-es-uipath-stg-qbceejar3ywxu54v7vnedxfdtu.us-west-2.es.amazonaws.com/default-2019.10/bal_key/_doc/<SN_URL_IDNO>
            //REQUEST:            
            //{
            //    "TimeStamp": "2020-29-02T22:16:04.835",
            //    "UID": "USABAL#...",
            //    "Key": "H1B-Request",
            //    "Type":"USCIS"           
            //}
            //RESPONSE: 
            //{
            //    "_index": "bal_key",
            //    "_type": "_doc",
            //    "_id": "HQmoknABgoUNEt4YvOpM",
            //    "_version": 1,
            //    "result": "created",
            //    "_shards": {
            //        "total": 2,
            //        "successful": 2,
            //        "failed": 0
            //    },
            //    "_seq_no": 683010,
            //    "_primary_term": 1
            //}

            string body = "";
            string authority = appKeys.ElasticSearch_Authority;
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    //Delete if available
                    if (!String.IsNullOrEmpty(keyIdNo))
                    {
                        response = client.DeleteAsync(authority + "bal_key/_doc/" + keyIdNo).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            errorMessage = "Delete ES Request, ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_key/";
                            logger.Log(LogLevel.Error, errorMessage);
                            return HttpStatusCode.BadRequest;
                        }
                    }

                    //Add new
                    //DateTime dt = DateTime.Now.AddHours(8);
                    DateTime dt = DateTime.UtcNow;
                    body = "{" +
                        "\"TimeStamp\": \"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"UID\": \"" + uid.Replace("\\", "#") + "\"," +
                        "\"Key\": \"" + EncryptionUtil.Encrypt(key, "ES") + "\"," +
                        "\"Type\": \"" + appKeys.LookupKey + "\"}";

                    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    logger.Log(LogLevel.Info, body);
                    response = client.PostAsync(authority + "bal_key/_doc/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        logger.Log(LogLevel.Info, responseString);
                        JObject obj = JsonConvert.DeserializeObject<JObject>(responseString);
                        keyId = obj["_id"].ToString();
                        return HttpStatusCode.OK;
                    }
                    else
                    {
                        errorMessage = "ES Request Body:" + body + ", ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_key/";
                        logger.Log(LogLevel.Error, errorMessage);
                        return HttpStatusCode.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "ES Request Body:" + body + ", Error:" + ex.Message + ", ES URL:" + authority + "bal_key/";
                logger.Log(LogLevel.Error, errorMessage);
            }
            return HttpStatusCode.BadRequest;
        }
        public HttpStatusCode AddErrorESLog(string idNo, string exceptionType, string exceptionMessage, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(idNo))
                idNo = "BOT0000000";
            // Add Error log - TimeSTamp=Current Datetime + 7 hours
            //PUT - https://vpc-bal-es-uipath-stg-qbceejar3ywxu54v7vnedxfdtu.us-west-2.es.amazonaws.com/default-2019.10/bal_uipath/_doc/<SN_URL_IDNO>
            //REQUEST:            
            //{
            //    "TimeStamp": "2019-11-06T09:15:21.5201345",
            //    "QueueId": null,
            //    "QueueName": "PackUpload-Request",
            //    "MachineId": null,
            //    "MachineName":null,
            //    "ReleaseId": null,
            //    "ReleaseName": null,
            //    "PackageName": null,
            //    "EnvironmentName": null,
            //    "EnvironmentId": null,
            //    "RobotId": null,
            //    "WindowsIdentity": null,
            //    "OutputData": null,
            //    "Status": "Waiting",
            //    "Priority": "Normal",
            //    "NoOfItems":1,  
            //    "SuccessItems":0,
            //    "SpecificContent": {},
            //    "Progress": null,
            //    "Output": null,
            //    "ProcessingExceptionType": "Business Error",
            //    "ProcessingException": "No Record Found in SQL",
            //    "RetryNumber": 0,
            //    "DueDate": null,
            //    "DeferDate": null,
            //    "CreationTime": "2019-11-06T09:15:21.5201345",
            //    "WaitingTime": 300,
            //    "EstimatedStartProcessing": null,
            //    "EstimatedEndProcessing": null,
            //    "StartProcessing": null,
            //    "EndProcessing": null,
            //    "ExecutionTime": null,
            //    "AvgPerRecord":null
            //}
            //RESPONSE: 
            //{
            //    "_index": "default-2019.10",
            //    "_type": "logEvent",
            //    "_id": "ZR10y20B4zdbPNkGAs25",
            //    "_version": 1,
            //    "result": "created",
            //    "_shards": {
            //                    "total": 2,
            //        "successful": 2,
            //        "failed": 0
            //    },
            //    "_seq_no": 683010,
            //    "_primary_term": 1
            //}

            string body = "";
            string authority = appKeys.ElasticSearch_Authority;
            try
            {
                using (var client = new HttpClient())
                {

                    //1- Add Error log
                    //DateTime dt = DateTime.Now.AddHours(8);
                    DateTime dt = DateTime.UtcNow;
                    body = "{" +
                        "\"TimeStamp\": \"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"QueueId\": null," +
                        "\"QueueName\": \"" + appKeys.ResponseQueueName + "\"," +
                        "\"MachineId\": null," +
                        "\"MachineName\":null," +
                        "\"ReleaseId\": null," +
                        "\"ReleaseName\": null," +
                        "\"PackageName\": null," +
                        "\"EnvironmentName\": null," +
                        "\"EnvironmentId\": null," +
                        "\"RobotId\": null," +
                        "\"WindowsIdentity\": null," +
                        "\"OutputData\": null," +
                        "\"Status\": \"Waiting\"," +
                        "\"Priority\": \"Normal\"," +
                        "\"NoOfItems\":1,  " +
                        "\"SuccessItems\":0," +
                        "\"SpecificContent\": { }," +
                        "\"Progress\": null," +
                        "\"Output\": null," +
                        "\"ProcessingExceptionType\": \"" + exceptionType + "\"," +
                        "\"ProcessingException\": \"" + exceptionMessage + "\"," +
                        "\"RetryNumber\": 0," +
                        "\"DueDate\": null," +
                        "\"DeferDate\": null," +
                        "\"CreationTime\": \"" + dt.ToString("yyyy-MM-ddTHH:mm:ss.FFF") + "\"," +
                        "\"WaitingTime\": null," +
                        "\"EstimatedStartProcessing\": null," +
                        "\"EstimatedEndProcessing\": null," +
                        "\"StartProcessingTime\": null," +
                        "\"EndProcessingTime\": null," +
                        "\"ExecutionTime\": null," +
                        "\"AvgPerRecord\":null}";
                    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    logger.Log(LogLevel.Info, body);
                    // HTTP PUT
                    var response = client.PutAsync(authority + "bal_uipath/_doc/" + idNo, content).Result;
                    if (response.IsSuccessStatusCode)
                        return HttpStatusCode.OK;
                    else
                    {
                        errorMessage = "ES Request Body:" + body + ", ES Response:" + response.ReasonPhrase + ", ES URL:" + authority + "bal_uipath/";
                        logger.Log(LogLevel.Error, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "ES Request Body:" + body + ", Error:" + ex.Message + ", ES URL:" + authority + "bal_uipath/";
                logger.Log(LogLevel.Error, errorMessage);
            }
            return HttpStatusCode.BadRequest;
        }
    }
}