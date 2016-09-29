using System;
using System.Configuration;
using StudentPractice.BusinessModel;
using StudentPractice.Utils;
using System.Web.Hosting;

namespace StudentPractice.Portal
{
    public static class Config
    {
        public static bool ProviderRegistrationAllowed
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["ProviderRegistrationAllowed"]);
            }
        }

        public static bool OfficeRegistrationAllowed
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["OfficeRegistrationAllowed"]);
            }
        }

        public static bool StudentLoginAllowed
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["StudentLoginAllowed"]);
            }
        }

        public static bool EnableAPI
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAPI"]);
            }
        }

        public static string StudentLoginMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["StudentLoginMessage"];
            }
        }

        public static bool InternshipPositionCreationAllowed
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["InternshipPositionCreationAllowed"]);
            }
        }

        public static bool PreAssignmentAllowed
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["PreAssignmentAllowed"]);
            }
        }

        public static string PreAssignmentMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["PreAssignmentMessage"];
            }
        }

        public static bool EnableSMS
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSMS"]);
            }
        }

        public static int MaxSMSAllowed
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["MaxSMSAllowed"]);
            }
        }

        public static bool IsSSL
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
            }
        }

        public static bool FakeShib
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["FakeShib"]);
            }
        }

        public static string AcademicIDServiceURL
        {
            get
            {
                return ConfigurationManager.AppSettings["AcademicIDServiceURL"];
            }
        }

        public static string StudentPracticePortalURL
        {
            get
            {
                return ConfigurationManager.AppSettings["StudentPracticePortalURL"];
            }
        }

        private static bool? _isPilotApplication = null;
        public static bool IsPilotApplication
        {
            get
            {
                if (_isPilotApplication == null)
                    _isPilotApplication = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPilotApplication"]);
                return _isPilotApplication.Value;
            }
        }

        public static bool AllowCyprusRegistration
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCyprusRegistration"]);
            }
        }

        public static bool AllowForeignRegistration
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["AllowForeignRegistration"]);
            }
        }

        private static string _reportFilesDirectory = null;
        public static string ReportFilesDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_reportFilesDirectory))
                {
                    var path = ConfigurationManager.AppSettings["ReportFilesDirectory"];
                    if (path.StartsWith("~"))
                        return HostingEnvironment.MapPath(path);
                    else
                        return path;
                }
                return _reportFilesDirectory;
            }
        }

        public static string GoogleAnalyticsUserID
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleAnalyticsUserID"];
            }
        }
    }
}
