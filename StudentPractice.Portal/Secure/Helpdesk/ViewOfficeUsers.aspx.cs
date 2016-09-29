using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewOfficeUsers : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            int officeID;
            if (int.TryParse(Request.QueryString["oID"], out officeID) && officeID > 0)
            {
                Entity = new InternshipOfficeRepository(UnitOfWork).Load(officeID, x => x.Academics);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvOfficeUsers.DataBind();
        }

        protected void odsOfficeUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipOffice> criteria = new Criteria<InternshipOffice>();

            criteria.Include(x => x.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.MasterAccountID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvOfficeUsers_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipOffice provider = (InternshipOffice)gvOfficeUsers.GetRow(e.VisibleIndex);

            if (provider != null)
            {
                if (provider.IsApproved)
                {
                    e.Row.BackColor = Color.LightGreen;
                }
                else
                {
                    e.Row.BackColor = Color.Tomato;
                }
            }
        }

        protected string GetApprovalStatus(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string officeDetails = string.Empty;

            if (office.IsApproved)
            {
                officeDetails = "Ενεργός";
            }
            else
            {
                officeDetails = "Ανενεργός";
            }

            return officeDetails;
        }

        protected string GetContactDetails(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string contactDetails = string.Empty;

            contactDetails = string.Format("{0}<br/>{1}<br/>{2}<br/>{3}", office.ContactName, office.ContactPhone, office.ContactMobilePhone, office.ContactEmail);

            return contactDetails;
        }

        protected string GetOfficeAcademics(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string academics = string.Empty;

            if (office.Academics.Count() == Entity.Academics.Count())
            {
                academics = "ΟΛΑ";
            }
            else
            {
                foreach (var academic in office.Academics.OrderBy(x => x.Department))
                {
                    academics += academic.Department + "<br/>";
                }
            }

            return academics;
        }
    }
}