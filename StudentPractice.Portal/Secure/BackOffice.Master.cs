using StudentPractice.BusinessModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal
{
    public partial class BackOffice : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var reporter = new ReporterRepository().FindByUsername(Page.User.Identity.Name);
            if (reporter != null)
            {
                reporter.SaveReporterIDToCurrentContext();
                if (reporter.MustChangePassword)
                {
                    Response.Redirect("~/Common/ChangePassword.aspx");
                }
            }

            Page.ClientScript.RegisterStartupScript(GetType(), "Resources", string.Format("Resources.Init({0});", new JavaScriptSerializer().Serialize(GetResources())), true);
            lnkChangePassword.Attributes["onclick"] = string.Format("popUp.show('../../Common/AlterPassword.aspx?r=true&username={0}','Αλλαγή Κωδικού Πρόσβασης');", Page.User.Identity.Name);
        }

        protected bool ShowNode(SiteMapNode node)
        {
            if (node.Roles.Count == 0)
                return true;

            foreach (string r in Roles.GetRolesForUser())
            {
                if (node.Roles.Cast<string>().Contains(r, StringComparer.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        protected void LoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            LoginStatus.LogoutPageUrl = Server.MapPath("~/Default.aspx");
        }

        private IEnumerable GetResources()
        {
            foreach (DictionaryEntry item in Resources.JavaScriptResource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                yield return new
                {
                    Key = item.Key,
                    Value = item.Value.ToString().Replace("\\n", "\n")
                };
            }
        }
    }
}