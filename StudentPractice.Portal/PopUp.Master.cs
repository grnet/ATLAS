using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Globalization;


namespace StudentPractice.Portal
{
    public partial class PopUp : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "Resources", string.Format("Resources.Init({0});", new JavaScriptSerializer().Serialize(GetResources())), true);
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
