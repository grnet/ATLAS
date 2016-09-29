using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Utils;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Helpdesk {
    public partial class ViewProviderUsers : BaseEntityPortalPage<object> {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e) {
            gvProviderUsers.DataBind();
        }

        protected void odsProviderUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            int masterProviderID = int.Parse(Page.Request.QueryString["pID"]);

            criteria.Expression = criteria.Expression.Where(x => x.MasterAccountID, masterProviderID);
            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvProviderUsers_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e) {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipProvider provider = (InternshipProvider)gvProviderUsers.GetRow(e.VisibleIndex);

            if (provider != null) {
                if (provider.IsApproved) {
                    e.Row.BackColor = Color.LightGreen;
                }
                else {
                    e.Row.BackColor = Color.Tomato;
                }
            }
        }

        protected string GetApprovalStatus(InternshipProvider provider) {
            if (provider == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (provider.IsApproved) {
                providerDetails = "Ενεργός";
            }
            else {
                providerDetails = "Ανενεργός";
            }

            return providerDetails;
        }

        protected string GetProviderDetails(InternshipProvider provider) {
            if (provider == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (!string.IsNullOrEmpty(provider.TradeName)) {
                providerDetails = string.Format("{0} <br/>{1} <br/>{2}", provider.Name, provider.TradeName, provider.AFM);
            }
            else {
                providerDetails = string.Format("{0} <br/>{1}", provider.Name, provider.AFM);
            }

            return providerDetails;
        }

        protected string GetContactDetails(InternshipProvider provider) {
            if (provider == null)
                return string.Empty;

            string contactDetails = string.Empty;

            contactDetails = string.Format("{0}<br/>{1}<br/>{2}<br/>{3}", provider.ContactName, provider.ContactPhone, provider.ContactMobilePhone, provider.ContactEmail);

            return contactDetails;
        }
    }
}
