using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal
{
    public static class Extensions
    {
        private const string s_CurrentProvider = "_currentProvider";
        private const string s_CurrentOffice = "_currentOffice";
        private const string s_CurrentPosition = "_currentPosition";
        private const string s_CurrentStudent = "_currentStudent";
        private const string s_CurrentHelpdeskUser = "_currentHelpdeskUser";
        private const string s_CurrentReporter = "_currentReporter";
        private const string s_CurrentReporterID = "_currentReporterID";
        private const string s_CurrentMasterCountryID = "_currentMasterCountryID";
        private const string s_ProviderUserCountryID = "_currentProviderUserCountryID";

        /// <summary>
        /// Replace \r,\n with &lt;br /&gt;
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CrLfToBr(this string s)
        {
            return s != null ? s.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br />") : "";
        }

        public static string[] SplitLines(this string s)
        {
            return s.Replace("\r", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string AddZeroPadding(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            int sLength = s.Length;
            int numberOfZeros = maxLength - sLength;

            string sPadded = "";

            for (int i = 0; i < numberOfZeros; i++)
                sPadded += "0";

            sPadded += s;

            return sPadded;
        }

        public static void SaveToCurrentContext(this InternshipProvider provider)
        {
            if (HttpContext.Current != null && provider != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentProvider))
                    HttpContext.Current.Items[s_CurrentProvider] = provider;
                else
                    HttpContext.Current.Items.Add(s_CurrentProvider, provider);
            }
        }

        public static InternshipProvider LoadProvider(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentProvider))
            {
                return context.Items[s_CurrentProvider] as InternshipProvider;
            }
            return null;
        }

        public static void SaveToCurrentContext(this InternshipPosition position)
        {
            if (HttpContext.Current != null && position != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentPosition))
                    HttpContext.Current.Items[s_CurrentPosition] = position;
                else
                    HttpContext.Current.Items.Add(s_CurrentPosition, position);
            }
        }

        public static InternshipPosition LoadPosition(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentPosition))
            {
                return context.Items[s_CurrentPosition] as InternshipPosition;
            }
            return null;
        }

        public static void SaveToCurrentContext(this InternshipOffice office)
        {
            if (HttpContext.Current != null && office != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentOffice))
                    HttpContext.Current.Items[s_CurrentOffice] = office;
                else
                    HttpContext.Current.Items.Add(s_CurrentOffice, office);
            }
        }

        public static InternshipOffice LoadOffice(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentOffice))
            {
                return context.Items[s_CurrentOffice] as InternshipOffice;
            }
            return null;
        }

        public static void SaveToCurrentContext(this Student student)
        {
            if (HttpContext.Current != null && student != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentStudent))
                    HttpContext.Current.Items[s_CurrentStudent] = student;
                else
                    HttpContext.Current.Items.Add(s_CurrentStudent, student);
            }
        }

        public static Student LoadStudent(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentStudent))
            {
                return context.Items[s_CurrentStudent] as Student;
            }
            return null;
        }

        public static void SaveToCurrentContext(this HelpdeskUser helpdeskUser)
        {
            if (HttpContext.Current != null && helpdeskUser != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentHelpdeskUser))
                    HttpContext.Current.Items[s_CurrentHelpdeskUser] = helpdeskUser;
                else
                    HttpContext.Current.Items.Add(s_CurrentHelpdeskUser, helpdeskUser);
            }
        }

        public static HelpdeskUser LoadHelpdeskUser(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentHelpdeskUser))
            {
                return context.Items[s_CurrentHelpdeskUser] as HelpdeskUser;
            }
            return null;
        }

        public static void SaveToCurrentContext(this Reporter reporter)
        {
            if (HttpContext.Current != null && reporter != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentReporter))
                    HttpContext.Current.Items[s_CurrentReporter] = reporter;
                else
                    HttpContext.Current.Items.Add(s_CurrentReporter, reporter);
            }
        }

        public static Reporter LoadReporter(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentReporter))
            {
                return context.Items[s_CurrentReporter] as Reporter;
            }
            return null;
        }

        public static void SaveReporterIDToCurrentContext(this Reporter reporter)
        {
            if (HttpContext.Current != null && reporter != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentReporterID))
                    HttpContext.Current.Items[s_CurrentReporterID] = reporter.ID;
                else
                    HttpContext.Current.Items.Add(s_CurrentReporterID, reporter.ID);
            }
        }

        public static int LoadReporterID(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentReporterID))
            {
                return (int)context.Items[s_CurrentReporterID];
            }
            return 0;
        }

        public static void SaveMasterCountryIDToCurrentContext(this InternshipProvider provider)
        {
            if (HttpContext.Current != null && provider != null)
            {
                if (HttpContext.Current.Items.Contains(s_CurrentMasterCountryID))
                    HttpContext.Current.Items[s_CurrentMasterCountryID] = provider.CountryID;
                else
                    HttpContext.Current.Items.Add(s_CurrentMasterCountryID, provider.CountryID);
            }
        }

        public static int? LoadMasterCountryID(this HttpContext context)
        {
            if (context.Items.Contains(s_CurrentMasterCountryID))
            {
                return (int?)context.Items[s_CurrentMasterCountryID];
            }
            return null;
        }

        public static void SaveProviderUserSelectedCountry(int countryID)
        {
            if (HttpContext.Current != null && countryID != 0)
            {
                if (HttpContext.Current.Items.Contains(s_ProviderUserCountryID))
                    HttpContext.Current.Items[s_ProviderUserCountryID] = countryID;
                else
                    HttpContext.Current.Items.Add(s_ProviderUserCountryID, countryID);
            }
        }

        public static int? LoadProviderUserSelectedCountry(this HttpContext context)
        {
            if (context.Items.Contains(s_ProviderUserCountryID))
            {
                return (int?)context.Items[s_ProviderUserCountryID];
            }
            return null;
        }

    }
}
