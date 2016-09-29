using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using System.Drawing;
using StudentPractice.Portal.UserControls;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Mails;
using Imis.Domain;
using System.Threading;
using StudentPractice.Portal.UserControls.GenericControls;
using System.Text;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ServiceLogs : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvLogs.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvLogs.PageIndex = 0;
            gvLogs.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gveLogs.FileName = string.Format("ServiceLogs_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveLogs.WriteXlsxToResponse(true);
        }

        protected void odsStudentPracticeApiLog_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<StudentPracticeApiLog> criteria = new Criteria<StudentPracticeApiLog>();
            criteria.UsePaging = true;

            criteria.Expression = criteria.Expression.Where(x => x.ServiceCaller, (int)enServiceCaller.Office);
            int officeID;
            if (int.TryParse(txtOfficeID.Text.ToNull(), out officeID) && officeID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ServiceCallerID, officeID);
            }

            int successInt;
            if (int.TryParse(ddlSuccess.SelectedItem.Value, out successInt) && successInt > 0)
            {
                switch (successInt)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.Success, true);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.Success, false);
                        break;
                    default:
                        break;
                }
            }

            int submissionDate;
            if (int.TryParse(ddlSubmissionDate.SelectedItem.Value, out submissionDate) && submissionDate > 0)
            {
                switch (submissionDate)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, DateTime.Today.AddDays(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, DateTime.Today.AddDays(-7), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 3:
                        criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, DateTime.Today.AddMonths(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 4:
                        criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, DateTime.Today.AddMonths(-3), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var startDate = deCalledAtFrom.GetDate();
                if (startDate != null)
                    criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, startDate.Value, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);

                var endDate = deCalledAtTo.GetDate();
                if (endDate != null)
                    criteria.Expression = criteria.Expression.Where(x => x.ServiceCalledAtDateOnly, endDate.Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            string srvName = ddlServiceName.GetText();
            if (!string.IsNullOrEmpty(srvName))
                criteria.Expression = criteria.Expression.Where(x => x.ServiceMethodCalled, srvName);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvLogs_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            StudentPracticeApiLog log = (StudentPracticeApiLog)gvLogs.GetRow(e.VisibleIndex);
            if (log.Success.HasValue && log.Success.Value)
            {
                e.Row.BackColor = Color.LightGreen;
            }
            else
            {
                e.Row.BackColor = Color.Tomato;
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvLogs.DataBind();
        }

    }
}