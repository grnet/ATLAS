using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using System.Web.Services;
using Imis.Domain;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewAccountDetails : BaseEntityPortalPage<object>
    {
        #region [ Private Fields and Properties ]

        private enReporterType ReporterType
        {
            get
            {
                int i;
                if (!int.TryParse(Request.QueryString["t"], out i))
                {
                    InvalidatePage();
                    return enReporterType.Other;
                }
                return (enReporterType)i;
            }
        }
        private int ReporterID
        {
            get
            {
                int id;
                if (!int.TryParse(Request.QueryString["rid"], out id))
                    InvalidatePage();
                return id;
            }
        }
        private Reporter _Reporter = null;
        private MembershipUser _User = null;
        private InternshipOffice _Office = null;

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            LoadData();
            Bind();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", string.Format(@"var _USERNAME = '{0}';", _User.UserName), true);
            base.OnLoad(e);
        }

        private void InvalidatePage()
        {
            Response.Clear();
            Response.Write("<html><body><script type='text/javascript'>window.close();</script></body></html>");
            Response.End();
        }

        #region [ Data Methods ]

        private void LoadData()
        {
            ReporterRepository rep = new ReporterRepository(UnitOfWork);
            switch (ReporterType)
            {
                case enReporterType.InternshipProvider:
                    _Reporter = rep.FindByID<InternshipProvider>(ReporterID);
                    break;
                case enReporterType.InternshipOffice:
                    _Reporter = rep.FindByID<InternshipOffice>(ReporterID);
                    _Office = new InternshipOfficeRepository(UnitOfWork).Load(ReporterID);
                    break;
                case enReporterType.Student:
                    _Reporter = rep.FindByID<Student>(ReporterID);
                    break;
            }
            if (_Reporter != null)
            {
                _User = Membership.GetUser(_Reporter.UserName);
            }
        }

        private void Bind()
        {
            if (_Reporter == null || _User == null)
                return;


            ltrEmail.Text = _User.Email;
            ltrIsLockedOut.Text = _User.IsLockedOut ? "Ναι" : "Όχι";
            ltrUsername.Text = _User.UserName;

            phIsLocked.Visible = _User.IsLockedOut;
            if (ReporterType == enReporterType.InternshipOffice)
                OfficeBind();
        }

        private void OfficeBind()
        {
            rowAPI.Visible = true;
            btnSubmit.Visible = !(_Office.IsApiAppoved.GetValueOrDefault());
            btnCancel.Visible = _Office.IsApiAppoved.GetValueOrDefault();
            ltrApiApproved.Text = _Office.IsApiAppoved.GetValueOrDefault() ? "Ενεργή" : "Μη ενεργή";
            ltrApiApproved.ForeColor = _Office.IsApiAppoved.GetValueOrDefault() ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
        }
        
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            _Office.IsApiAppoved = true;
            UnitOfWork.Commit();
            OfficeBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _Office.IsApiAppoved = false;
            UnitOfWork.Commit();
            OfficeBind();
        }
    }
}
