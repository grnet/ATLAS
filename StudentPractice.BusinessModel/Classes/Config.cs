using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace StudentPractice.BusinessModel
{
    public static class Config
    {
        private static bool? _providerRegistrationAllowed = null;
        public static bool ProviderRegistrationAllowed
        {
            get
            {
                if (_providerRegistrationAllowed == null)
                    _providerRegistrationAllowed = Convert.ToBoolean(ConfigurationManager.AppSettings["ProviderRegistrationAllowed"]);

                return _providerRegistrationAllowed.Value;
            }
        }

        private static bool? _officeRegistrationAllowed = null;
        public static bool OfficeRegistrationAllowed
        {
            get
            {
                if (_officeRegistrationAllowed == null)
                    _officeRegistrationAllowed = Convert.ToBoolean(ConfigurationManager.AppSettings["OfficeRegistrationAllowed"]);

                return _officeRegistrationAllowed.Value;
            }
        }

        private static bool? _studentLoginAllowed = null;
        public static bool StudentLoginAllowed
        {
            get
            {
                if (_studentLoginAllowed == null)
                    _studentLoginAllowed = Convert.ToBoolean(ConfigurationManager.AppSettings["StudentLoginAllowed"]);

                return _studentLoginAllowed.Value;
            }
        }

        private static bool? _enableAPI = null;
        public static bool EnableAPI
        {
            get
            {
                if (_enableAPI == null)
                    _enableAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAPI"]);

                return _enableAPI.Value;
            }
        }

        private static string _studentLoginMessage = null;
        public static string StudentLoginMessage
        {
            get
            {
                if (_studentLoginMessage == null)
                    _studentLoginMessage = ConfigurationManager.AppSettings["StudentLoginMessage"];

                return _studentLoginMessage;
            }
        }

        private static bool? _internshipPositionCreationAllowed = null;
        public static bool InternshipPositionCreationAllowed
        {
            get
            {
                if (_internshipPositionCreationAllowed == null)
                    _internshipPositionCreationAllowed = Convert.ToBoolean(ConfigurationManager.AppSettings["InternshipPositionCreationAllowed"]);

                return _internshipPositionCreationAllowed.Value;
            }
        }

        private static bool? _preAssignmentAllowed = null;
        public static bool PreAssignmentAllowed
        {
            get
            {
                if (_preAssignmentAllowed == null)
                    _preAssignmentAllowed = Convert.ToBoolean(ConfigurationManager.AppSettings["PreAssignmentAllowed"]);

                return _preAssignmentAllowed.Value;
            }
        }

        private static string _preAssignmentMessage = null;
        public static string PreAssignmentMessage
        {
            get
            {
                if (_preAssignmentMessage == null)
                    _preAssignmentMessage = ConfigurationManager.AppSettings["PreAssignmentMessage"];

                return _preAssignmentMessage;
            }
        }

        private static bool? _enableSMS = null;
        public static bool EnableSMS
        {
            get
            {
                if (_enableSMS == null)
                    _enableSMS = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSMS"]);

                return _enableSMS.Value;
            }
        }

        private static int? _maxSMSAllowed = null;
        public static int MaxSMSAllowed
        {
            get
            {
                if (_maxSMSAllowed == null)
                    _maxSMSAllowed = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSMSAllowed"]);

                return _maxSMSAllowed.Value;
            }
        }

        private static bool? _isSSL = null;
        public static bool IsSSL
        {
            get
            {
                if (_isSSL == null)
                    _isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);

                return _isSSL.Value;
            }
        }

        private static bool? _fakeShib = null;
        public static bool FakeShib
        {
            get
            {
                if (_fakeShib == null)
                    _fakeShib = Convert.ToBoolean(ConfigurationManager.AppSettings["FakeShib"]);

                return _fakeShib.Value;
            }
        }

        private static string _academicIDServiceURL = null;
        public static string AcademicIDServiceURL
        {
            get
            {
                if (_academicIDServiceURL == null)
                    _academicIDServiceURL = ConfigurationManager.AppSettings["AcademicIDServiceURL"];

                return _academicIDServiceURL;
            }
        }

        private static string _studentPracticePortalURL = null;
        public static string StudentPracticePortalURL
        {
            get
            {
                if (_studentPracticePortalURL == null)
                    _studentPracticePortalURL = ConfigurationManager.AppSettings["StudentPracticePortalURL"];

                return _studentPracticePortalURL;
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

        private static bool? _allowCyprusRegistration = null;
        public static bool AllowCyprusRegistration
        {
            get
            {
                if (_allowCyprusRegistration == null)
                    _allowCyprusRegistration = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowCyprusRegistration"]);

                return _allowCyprusRegistration.Value;
            }
        }

        private static bool? _allowForeignRegistration = null;
        public static bool AllowForeignRegistration
        {
            get
            {
                if (_allowForeignRegistration == null)
                    _allowForeignRegistration = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowForeignRegistration"]);

                return _allowForeignRegistration.Value;
            }
        }

        private static bool? _enableServerSync = null;
        public static bool EnableServerSync
        {
            get
            {
                if (_enableServerSync == null)
                    _enableServerSync = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableServerSync"]);

                return _enableServerSync.Value;
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
    }
}
