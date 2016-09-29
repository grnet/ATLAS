using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewVerificationLogs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvVerificationLogs.DataBind();
        }

        protected void odsVerificationLogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<VerificationLog> criteria = new Criteria<VerificationLog>();

            int reporterID = int.Parse(Page.Request.QueryString["rID"]);

            criteria.Expression = criteria.Expression.Where(x => x.ReporterID, reporterID);
            criteria.Expression = criteria.Expression.Where(x => x.Reporter.IsMasterAccount, true);

            e.InputParameters["criteria"] = criteria;
        }

        protected string GetAction(VerificationLog vLog)
        {
            if (vLog.OldVerificationStatus == enVerificationStatus.NotVerified && vLog.NewVerificationStatus == enVerificationStatus.Verified)
                return "Πιστοποίηση";
            else if (vLog.OldVerificationStatus == enVerificationStatus.Verified && vLog.NewVerificationStatus == enVerificationStatus.NotVerified)
                return "Αποπιστοποίηση";
            else if ((vLog.OldVerificationStatus == enVerificationStatus.NotVerified || vLog.OldVerificationStatus == enVerificationStatus.Verified)
                && vLog.NewVerificationStatus == enVerificationStatus.CannotBeVerified)
                return "Ακύρωση";
            else
                return string.Empty;
        }
    }
}