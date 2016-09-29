using System;
using System.Configuration;
using System.Globalization;

namespace StudentPractice.Web.Api
{
    public static class ApiConfig
    {
        private static ServicesConfigSection _cur = null;

        static ApiConfig()
        {
            _cur = ConfigurationManager.GetSection("webServices") as ServicesConfigSection;
            if (_cur == null)
                throw new Exception("Web Services Configuration Error: could not find 'webServices' configuration section.");
        }

        public static string AcademicIDServiceURL
        {
            get
            {
                return ConfigurationManager.AppSettings["AcademicIDServiceURL"];
            }
        }

        public static ServicesConfigSection Current { get { return _cur; } }
    }
}
