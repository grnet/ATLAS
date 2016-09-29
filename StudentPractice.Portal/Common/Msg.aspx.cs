using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;


namespace StudentPractice.Portal.Common
{
    public partial class Msg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Request.QueryString["key"];
            string title = (string)HttpContext.GetGlobalResourceObject("Messages", key + "Title");
            string body = (string)HttpContext.GetGlobalResourceObject("Messages", key + "Body");

            Page.Title = title;
            litBody.Text = body;
        }
    }
}
