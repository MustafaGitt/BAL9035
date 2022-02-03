using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BAL9035.Core
{
    public class JiraTicket
    {
        public void CreateJiraException(string id_No, string exMessaage)
        {
            //jira exception message creator
            string JEndPoint = "https://accelirate.atlassian.net";
            string JProcessNameField = "customfield_10030";
            string JProcessName = "9035";
            string JType = "Task";
            string JKey = "BET";
            string JTitle = "9035-" + id_No + "-Mustafa ";
            string JText = exMessaage;
            string JUsername = "prod-uirobot@bal.com";
            string JPassword = "JgL4gVauJE0m4aDegxexD400";

            string Body = "{\"fields\":{\"summary\":\"<<summary>>\",\"issuetype\":{\"name\":\"<<type>>\"},\"project\":{\"key\":\"<<key>>\"},\"<<processnamefield>>\":\"<<processname>>\",\"description\":{\"type\":\"doc\",\"version\":1,\"content\":[{\"type\":\"paragraph\",\"content\":[{\"type\":\"text\",\"text\":\"<<text>>\"}]}]}}}".Replace("<<processnamefield>>", JProcessNameField).Replace("<<processname>>", JProcessName).Replace("<<type>>", JType).Replace("<<key>>", JKey).Replace("<<summary>>", JTitle).Replace("<<text>>", JText);
            var request = System.Net.WebRequest.Create(JEndPoint.TrimEnd('/') + "/rest/api/3/issue");
            request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(JUsername + ":" + JPassword)));
            request.Timeout = 60000;
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(Body);
            }
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        }
    }
}