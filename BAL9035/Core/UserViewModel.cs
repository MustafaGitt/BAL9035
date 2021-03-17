using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using _9035BL;

namespace BAL9035.Core
{
    /* NOT IN USE */
    public class UserViewModel
    {

        public static string CurrentUser()
        {
            string cacheUser = "";
            if (HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null
                && !String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
            {
                cacheUser = HttpContext.Current.User.Identity.Name;
            }
            return cacheUser;
        }
        public static List<string> GetEnvVars()
        {
            List<string> vars = new List<string>();
            vars.Add(Environment.MachineName);
            Log.Info("The MachineName  is : " + Environment.MachineName);
            vars.Add(Environment.UserName);
            Log.Info("The UserName  is : " + Environment.UserName);
            vars.Add(Environment.UserDomainName);
            Log.Info("The Userdomain Name is : " + Environment.UserDomainName);
            // IHttpContextAccessor _contextAccessor
            var hostname = HttpContext.Current.Request.UserHostName;
            Log.Info("The hostname  is : " + hostname);
            string ip = HttpContext.Current.Request.UserHostAddress;
            Log.Info("The ip is : " + ip);
            System.Net.IPHostEntry host = new System.Net.IPHostEntry();
            host = System.Net.Dns.GetHostEntry(ip);//Request.ServerVariables["REMOTE_HOST"]
            Log.Info("The HostEntry works ");
            //Log.Info("The HostEntry is : " + hostEntry.HostName.ToString());
            //var Name = hostEntry.HostName;
            //Log.Info("The HostName is : " + Name);
            return vars;

        }
    }
}
