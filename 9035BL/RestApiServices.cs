using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace _9035BL
{
    /*
     *This class has all methods which we need to consume the RESTful API's 
     */
    public class RestApiServices
    {
        private static readonly HttpClient client = new HttpClient();
        // Post Method (Token is optional)
        public string Post(string url, string body, string token = "")
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                //if (!response.StatusCode.ToString().Contains("20"))
                //{
                //    throw new Exception(responseString);
                //}
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string PostCloud(string url, string body, bool tenantNameExist)
        {
            try
            {
                if (tenantNameExist == true)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-UIPATH-TenantName", "DefaultTenant");
                }
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseString);
                }
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // GET Method (Token and Filter is optional)
        public string Get(string url, string token = "", string filter = "")
        {
            try
            {
                if (filter != "")
                {
                    url = url + filter;
                }
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                var responseString = client.GetStringAsync(url).Result;
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string GetCloud(string url, string token = "", string filter = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    url = url + filter;
                }
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-UIPATH-TenantName", "DefaultTenant");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("X-UIPATH-OrganizationUnitId", "1204340");
                }
                var responseString = client.GetStringAsync(url).Result;
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        // PUT Method (Token is optional)
        public string Put(string url, string body, string token = "")
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        // DELETE Method (Token is optional)
        public string Delete(string url, string token = "")
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                var response = client.DeleteAsync(url).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                //if (!response.StatusCode.ToString().Contains("20"))
                //{
                //    throw new Exception(responseString);
                //}
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}