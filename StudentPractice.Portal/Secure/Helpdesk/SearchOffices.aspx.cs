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
using Microsoft.Data.Extensions;
using StudentPractice.Portal.UserControls.GenericControls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchOffices : BaseEntityPortalPage<object>
    {
        private bool _isSuperHelpdesk;

        protected override void Fill()
        {
            _isSuperHelpdesk = System.Web.Security.Roles.IsUserInRole(RoleNames.SuperHelpdesk);
        }

        protected bool HideFilters
        {
            get
            {
                bool hideFilters;

                bool.TryParse(Request.QueryString["hideFilters"], out hideFilters);

                return hideFilters;
            }
        }

        protected bool IsChildAcount
        {
            get
            {
                bool isChildAcount;

                bool.TryParse(Request.QueryString["isChildAcount"], out isChildAcount);

                return isChildAcount;
            }
        }

        private InternshipOffice _childOffice = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HideFilters)
            {
                tblFilters.Visible = false;
                btnSearch.Visible = false;
            }

            int officeID = 0;
            if (int.TryParse(Request.QueryString["oID"], out officeID) && officeID > 0)
            {
                if (IsChildAcount)
                    _childOffice = new InternshipOfficeRepository(UnitOfWork).Load(officeID);
                else
                    txtOfficeID.Text = officeID.ToString();
            }

            if (!Page.IsPostBack)
            {
                txtUserName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtEmail.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtCertificationNumber.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                deCertificationDate.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
            }
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlOfficeType_Init(object sender, EventArgs e)
        {
            ddlOfficeType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (enOfficeType item in Enum.GetValues(typeof(enOfficeType)))
            {
                ddlOfficeType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvOffices.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvOffices.PageIndex = 0;
            gvOffices.DataBind();
        }

        protected void OnEmailSending(object sender, EmailFormSendingEventArgs e)
        {
            if (!e.SendToLoggedInUser)
            {
                int _pageSize = gvOffices.SettingsPager.PageSize;

                gvOffices.SettingsPager.PageSize = int.MaxValue;
                gvOffices.DataBind();

                if (_offices != null && _offices.Count > 0)
                {
                    e.EmailRecepients.AddRange(_offices.Select(x => x.Email).Distinct());
                    e.EmailRecepients.AddRange(_offices.Select(x => x.ContactEmail).Distinct());
                    e.EmailRecepients.AddRange(_offices.Select(x => x.AlternateContactEmail).Distinct());
                    e.EmailRecepients = e.EmailRecepients.Distinct().ToList();
                    e.InfoMessage = string.Format("Η μαζική αποστολή ξεκίνησε για {0} μοναδικά Email χρηστών, {1} μοναδικά Email επικοινωνίας Γραφείων Πρακτικής Άσκησης και {2} μοναδικά Email επικοινωνίας αναπληρωτών από {3} συνολικά γραφεία.",
                        _offices.Select(x => x.Email).Distinct().Count(),
                        _offices.Select(x => x.ContactEmail).Distinct().Except(_offices.Select(x => x.Email).Distinct()).Count(),
                        _offices.Select(x => x.AlternateContactEmail).Distinct().Count(),
                        _offices.Count);
                }
                else
                {
                    e.Cancel = true;
                    e.InfoMessage = string.Format("Η μαζική αποστολή δεν πραγματοποιήθηκε.");
                }
                gvOffices.SettingsPager.PageSize = _pageSize;

            }
            else
            {
                e.InfoMessage = string.Format("Η δοκιμαστική αποστολή ολοκληρώθηκε. Παρακαλούμε ελέξτε το Email σας.");
            }
        }

        List<InternshipOffice> _offices = null;
        protected void odsOffices_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is List<InternshipOffice>)
                _offices = e.ReturnValue as List<InternshipOffice>;
        }

        protected void odsOffices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipOffice> criteria = new Criteria<InternshipOffice>();

            criteria.Include(x => x.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);
            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, true);

            if (IsChildAcount && _childOffice != null)
                criteria.Expression = criteria.Expression.Where(x => x.ID, _childOffice.MasterAccountID);

            int verificationStatus;
            if (int.TryParse(ddlVerificationStatus.SelectedItem.Value, out verificationStatus) && verificationStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.VerificationStatus, (enVerificationStatus)verificationStatus);
            }

            int approvalStatus;
            if (int.TryParse(ddlApprovalStatus.SelectedItem.Value, out approvalStatus) && approvalStatus >= 0)
            {
                if (approvalStatus == 1)
                {
                    criteria.Expression = criteria.Expression.Where(x => x.IsApproved, true);
                }
                else
                {
                    criteria.Expression = criteria.Expression.Where(x => x.IsApproved, false);
                }
            }

            int officeID;
            if (int.TryParse(txtOfficeID.Text.ToNull(), out officeID) && officeID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, officeID);
            }

            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.UserName, txtUserName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Email, txtEmail.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtCertificationNumber.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CertificationNumber, Convert.ToInt32(txtCertificationNumber.Text.ToNull()));
            }

            if (deCertificationDate.Value != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CertificationDate, deCertificationDate.Date, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                criteria.Expression = criteria.Expression.Where(x => x.CertificationDate, deCertificationDate.Date.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThanEquals);
            }

            //int accountType;
            //if (int.TryParse(ddlAccountType.SelectedItem.Value, out accountType) && accountType > 0)
            //{
            //    if (accountType == 1)
            //    {
            //        criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, true);
            //    }
            //    else if (accountType == 2)
            //    {
            //        criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);
            //    }
            //}

            int officeType;
            if (int.TryParse(ddlOfficeType.SelectedItem.Value, out officeType) && officeType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.OfficeTypeInt, officeType);
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InstitutionID, institutionID);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvOffices_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipOffice office = (InternshipOffice)gvOffices.GetRow(e.VisibleIndex);

            if (office != null)
            {
                switch (office.VerificationStatus)
                {
                    case enVerificationStatus.NotVerified:
                        e.Row.BackColor = Color.DarkGray;
                        break;
                    case enVerificationStatus.Verified:
                        e.Row.BackColor = Color.LightGreen;
                        break;
                    case enVerificationStatus.CannotBeVerified:
                        e.Row.BackColor = Color.Tomato;
                        break;
                    default:
                        break;
                }
            }
        }

        protected string GetOfficeDetails(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string officeDetails = string.Empty;

            var institution = CacheManager.Institutions.Get(office.InstitutionID.Value);

            switch (office.OfficeType)
            {
                case enOfficeType.None:
                    officeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <span style='color: Red'>-</span>", institution.Name);
                    break;
                case enOfficeType.Institutional:
                    if (office.CanViewAllAcademics.GetValueOrDefault())
                        officeDetails = string.Format("Ίδρυμα: {0}", institution.Name);
                    else
                        officeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", institution.Name, office.ID);
                    break;
                case enOfficeType.Departmental:
                    var academic = office.Academics.ToList()[0];

                    officeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήμα: {1}", institution.Name, academic.Department);
                    break;
                case enOfficeType.MultipleDepartmental:
                    officeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", institution.Name, office.ID);
                    break;
                default:
                    break;
            }

            return officeDetails;
        }

        protected string GetSearchIncidentReportLink(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string link = string.Empty;

            link = string.Format("~/Secure/Helpdesk/SearchIncidentReports.aspx?hideFilters=true&rID={0}&showAllAccounts=true", office.ID);

            return link;
        }

        protected bool CanShowOfficeUsers(InternshipOffice office)
        {
            if (office == null)
                return false;

            return office.IsMasterAccount;
        }

        protected bool CanEditOffice(InternshipOffice office)
        {
            if (office == null)
                return false;

            return office.IsMasterAccount && office.VerificationStatus == enVerificationStatus.Verified;
        }

        protected bool CanViewVerificationLogs(InternshipOffice office)
        {
            if (office == null)
                return false;

            return office.IsMasterAccount;
        }

        protected bool CanEditOfficeAcademics(InternshipOffice office)
        {
            return (office.OfficeType == enOfficeType.Institutional && _isSuperHelpdesk);
        }
    }
}