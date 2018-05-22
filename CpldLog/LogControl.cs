using System;
using log4net;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log.config", Watch = true)]

namespace CpldLog
{
    public class LogControl
    {
        public static readonly ILog Log = LogManager.GetLogger("CPLD.Logging");

        public static void LogError(Exception ex)
        {
           Log.Error("Error", ex);
        }

        public static void LogError(string msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void LogError(string msg)
        {
            Log.Error(msg);
        }

        public static void LogInfo(string msg)
        {
            Log.Info(msg);//写入一条新log           
        }

    }
}
