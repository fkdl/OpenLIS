using System;
using log4net;

namespace Base.Logger
{
    public static class LogHelper
    {
        public static readonly ILog InfoLog = LogManager.GetLogger("InfoLogger");
        public static readonly ILog ErrorLog = LogManager.GetLogger("ErrorLogger");

        public static void WriteLogInfo(string info)
        {
            if (InfoLog.IsInfoEnabled)
            {
                InfoLog.Info(info);
            }
        }

        public static void WriteLogInfo(string info, Exception ex)
        {
            if (InfoLog.IsInfoEnabled)
            {
                InfoLog.Info(info, ex);
            }
        }

        public static void WriteLogError(string error)
        {
            if (InfoLog.IsErrorEnabled)
            {
                ErrorLog.Info(error);
            }
        }

        public static void WriteLogError(string error, Exception ex)
        {
            if (InfoLog.IsErrorEnabled)
            {
                ErrorLog.Info(error, ex);
            }
        }
    }

}
