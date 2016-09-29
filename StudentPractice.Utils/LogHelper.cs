using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using log4net;

namespace StudentPractice.Utils
{
    public static class LogHelper
    {
        public static void LogError<T>(Exception ex)
        {
            LogManager.GetLogger(typeof(T)).Error(FormatDetails(), ex);
        }

        public static void LogError<TSource>(Exception ex, TSource logSource, string message = "") where TSource : class
        {
            string name = string.Empty;
            if (logSource is string)
                name = logSource.ToString();
            else if (logSource == null)
            {
                name = "LogHelper";
            }
            else
                name = logSource.GetType().FullName;
            LogManager.GetLogger(name).Error(!string.IsNullOrWhiteSpace(message) ? FormatDetails(message) : FormatDetails(), ex);
        }

        public static void LogMessage<T>(string message, T logSource = default(T))
        {
            LogManager.GetLogger(typeof(T)).Info(FormatDetails(message));
        }

        #region [ Helpers ]

        private static string FormatDetails(string message = "")
        {
            string template = "\nURL: {1}\nUser: {0}\tBrowser: {2}\nIP: {4}";
            if (!string.IsNullOrWhiteSpace(message))
            {
                template += "\nMessage:\n{3}";
            }
            string url = string.Empty;
            string ip = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
            {
                url = HttpContext.Current.Request.Url.ToString();
                ip = HttpContext.Current.Request.UserHostAddress;
                if (ip == string.Empty)
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            string browser = string.Empty;
            if (!string.IsNullOrWhiteSpace(url))
            {
                HttpBrowserCapabilities currentBrowser = HttpContext.Current.Request.Browser;
                browser = string.Format("{0} ({1})", currentBrowser.Browser, currentBrowser.Version);
            }
            return string.Format(template, Thread.CurrentPrincipal.Identity.Name, url, browser, message, ip);
        }

        private static string GetInnerExceptions(Exception ex)
        {
            StringBuilder sb = new StringBuilder("---> " + ex.Message);
            if (ex.InnerException != null)
            {
                sb.Append("\n");
                sb.Append(GetInnerExceptions(ex.InnerException));
            }
            return sb.ToString();
        }

        private static string GetStackTrace(Exception ex)
        {
            StringBuilder sb = new StringBuilder(ex.StackTrace);
            if (string.IsNullOrWhiteSpace(ex.StackTrace) && ex.InnerException != null)
            {
                sb.Append("\n");
                sb.Append(GetStackTrace(ex.InnerException));
            }
            else
            {
                sb.Append("\n--- End Of Stack Trace ---");
            }
            return sb.ToString();
        }

        #endregion

    }
}