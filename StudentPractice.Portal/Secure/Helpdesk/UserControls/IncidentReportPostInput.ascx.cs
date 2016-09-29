using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportPostInput : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlReportStatus_Init(object sender, EventArgs e)
        {
            foreach (enReportStatus item in Enum.GetValues(typeof(enReportStatus)))
            {
                ddlReportStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }

            ddlReportStatus.SelectedValue = ((int)enReportStatus.Pending).ToString();
        }

        protected void ddlCallType_Init(object sender, EventArgs e)
        {
            ddlCallType.Items.Add(new ListItem("-- επιλέξτε τύπο κλήσης --", ""));

            foreach (enCallType item in Enum.GetValues(typeof(enCallType)))
            {
                ddlCallType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        public void FillIncidentReportPost(IncidentReport ir)
        {
            //if (irPost.IsNew)
            //{
            //    irPost.CreatedBy = Page.User.Identity.Name.ToLower().Trim();
            //}
            //else
            //{
            //    irPost.UpdatedBy = Page.User.Identity.Name.ToLower().Trim();
            //    irPost.UpdatedAt = DateTime.Now;
            //}

            IncidentReportPost irPost = new IncidentReportPost();

            irPost.CallType = (enCallType)Convert.ToInt32(ddlCallType.SelectedValue);
            irPost.PostText = txtPostText.Text.ToNull();
            ir.IncidentReportPosts.Add(irPost);
            ir.LastPost = irPost;
            ir.ReportStatus = (enReportStatus)Convert.ToInt32(ddlReportStatus.SelectedValue);
        }

        public void SetIncidentReportPost(IncidentReport ir)
        {
            ddlReportStatus.SelectedValue = ((int)ir.ReportStatus).ToString();
        }
    }
}