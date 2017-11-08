using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grab.Common.Enum;

namespace Grab.Common
{
    public   class LoggerHelper
    {
        private static string LogVersion = ConfigurationManager.AppSettings["LogVersion"];

        private static ILog logger = null;
        /// <summary>
        /// 一般不要直接调用这个，建议调用此类提供的LoggerDebug或LoggerError方法
        /// </summary>
        public static ILog AppLogger
        {
            get
            {
                if (logger == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    logger = LogManager.GetLogger("AppLogger");
                }
                return logger;
            }
        }

        public static string StrCurDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public static void Logge4netError(object message)
        {
            AppLogger.Error(message);
        }

        public static void Logge4netError(object message, Exception exception)
        {
            AppLogger.Error(message, exception);
        }

        public static void LoggerDebug(string format, params object[] args)
        {
            AppLogger.Debug(string.Format(format, args));
        }

        /// <summary>
        /// 写日志，Error（异常了）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="key"></param>
        public static void LoggerError(string msg, string key = "LogErrorV3")
        {
            AppLogger.Debug(string.Format("\r\n----{0}{2}---\r\n{1}\r\n", StrCurDateTime, msg, key));
        }

        /// <summary>
        /// Console打印，and 写日志
        /// key仅记入日志，用作检索
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="key"></param>
        public static void ConsoleWriteAndLoggerDebug(string msg, string key = "")
        {
            Console.WriteLine(string.Format("\r\n{0}：{1}\r\n", StrCurDateTime, msg));
            AppLogger.Debug(string.Format("\r\n----{0}{2}---\r\n{1}\r\n", StrCurDateTime, msg, key));
        }

        public static void WriteLine(string format, params object[] args)
        {
            switch (LogVersion.ToLower())
            {
                case "console":
                    Console.WriteLine(format, args);
                    break;
                case "log":
                    AppLogger.Debug(string.Format(format, args));
                    break;
                case "all":
                    Console.WriteLine(format, args);
                    AppLogger.Debug(string.Format(format, args));
                    break;
                default:
                    break;
            }
        }

        public static void WriteLine(string format, ConsoleColor color = ConsoleColor.White, params object[] args)
        {
            Console.ForegroundColor = color;
            switch (LogVersion.ToLower())
            {
                case "console":
                    Console.WriteLine(format, args);
                    break;
                case "log":
                    AppLogger.Debug(string.Format(format, args));
                    break;
                case "all":
                    Console.WriteLine(format, args);
                    AppLogger.Debug(string.Format(format, args));
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Print到Console，不需要记录日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ConsoleWrite(string format, params object[] args)
        {
            switch (LogVersion)
            {
                case "console":
                    Console.WriteLine(format, args);
                    break;
                case "all":
                    Console.WriteLine(format, args);
                    break;
                default:
                    break;
            }
        }


    }
}
