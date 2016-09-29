using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Configuration;

namespace StudentPractice.BusinessModel
{
    public class ShibDetails
    {
        //ToFix
        public ShibDetails() { }

        #region [ Factory Method ]
        const string k = "__FakeShibAccount";
        private static void SimulateShibLogin(HttpContext context) {
            var details = context.Session[k] as ShibDetails;
            if (details != null)
                return;

            context.Session[k] = details;
        }

        public static void FakeLogin() {
            SimulateShibLogin(HttpContext.Current);
        }

        public static void FakeLogin(ShibDetails shibDetails) {
            SimulateShibLogin(shibDetails);
        }

        private static void SimulateShibLogin(ShibDetails shibDetails) {
            HttpContext.Current.Session[k] = shibDetails;
        }

        public static ShibDetails Create(NameValueCollection serverVariables) {
            ShibDetails details = null;

            if (bool.Parse(ConfigurationManager.AppSettings["FakeShib"])) {
                details = HttpContext.Current.Session[k] as ShibDetails;
                if (details != null) {
                    return details;
                }
                else {
                    HttpContext.Current.Response.Redirect("~/TestShibLogin.aspx");
                }
            }


            details = new ShibDetails {
                Affiliation = serverVariables["HTTP_SHIBAFFILIATION"],
                FullName = serverVariables["HTTP_SHIBCN"].EncodeShibString(),
                FirstName = serverVariables["HTTP_SHIBGIVENNAME"].EncodeShibString(),
                LastName = serverVariables["HTTP_SHIBSN"].EncodeShibString(),
                Email = serverVariables["HTTP_SHIBMAIL"].EncodeShibString(),
                AcademicID = serverVariables["HTTP_SHIBUNDERGRADUATEBRANCH"].EncodeShibString(),
                HomeOrganization = serverVariables["HTTP_SHIBHOMEORGANIZATION"].EncodeShibString(),
                Username = serverVariables["HTTP_SHIBEPPN"].EncodeShibString(),
                PersonalUniqueCode = serverVariables["HTTP_SHIBPERSONALUNIQUECODE"].EncodeShibString()
            };

            string studentCode = serverVariables["HTTP_SHIBPERSONALUNIQUECODE"].EncodeShibString();
            studentCode = studentCode.Replace(string.Format("urn:mace:terena.org:schac:personalUniqueCode:gr:{0}:", details.HomeOrganization), string.Empty);
            studentCode = studentCode.Replace(string.Format("{0}:", details.AcademicID), string.Empty);
            details.StudentCode = studentCode;

            return details;
        }

        #endregion

        #region Properties

        public string PersonalUniqueCode { get; set; }
        public string StudentCode { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Affiliation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AcademicID { get; set; }
        public string HomeOrganization { get; set; }

        public string Username { get; set; }

        #endregion
    }
}
