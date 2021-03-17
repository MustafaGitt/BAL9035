using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _9035BL
{
    /* NLOG CLASS for adding LOGS*/
    public static class Log
    {
        // NLOG LOGGER
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("App");
        // INFO METHOD
        public static void Info(string message)
        {
            Logger.Info(message);
        }
        // ERROR METHOD
        public static void Error(Exception exception, string balno)
        {
            Logger.Error(exception, balno);
        }
    }
}
