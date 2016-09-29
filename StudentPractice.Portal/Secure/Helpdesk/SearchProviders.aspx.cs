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
using StudentPractice.Portal.UserControls.GenericControls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchProviders : BaseEntityPortalPage<object>
    {
        #region [ Databind Methods ]

        List<InternshipProvider> _providers = null;
        List<Country> _countries = null;

        protected override void Fill()
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();
            criteria.UsePaging = false;
            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            int positionCount;
            var positions = new InternshipProviderRepository(UnitOfWork).FindWithCriteria(criteria, out positionCount);
            _countries = positions.Where(x => x.CountryID.HasValue)
                                .Select(x => CacheManager.Countries.Get(x.CountryID.Value))
                                .Distinct()
                                .OrderBy(x => x.Name)
                                .ToList();
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlProviderType_Init(object sender, EventArgs e)
        {
            ddlProviderType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (enProviderType item in Enum.GetValues(typeof(enProviderType)))
            {
                ddlProviderType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            foreach (var item in _countries)
            {
                if (item.ID == StudentPracticeConstants.GreeceCountryID || item.ID == StudentPracticeConstants.CyprusCountryID)
                    ddlCountry.Items.Insert(1, new ListItem(item.Name, item.ID.ToString()));
                else
                    ddlCountry.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        #endregion

        #region [ Page Methods ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtProviderID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtProviderAFM.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtProviderName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtUserName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtEmail.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtCertificationNumber.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                deCertificationDate.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));

                int providerID = 0;
                if (int.TryParse(Request.QueryString["pID"], out providerID) && providerID > 0)
                    txtProviderID.Text = providerID.ToString();
            }
        }

        #endregion

        #region [ Grid Methods ]

        protected void odsProviders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

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

            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.UserName, txtUserName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Email, txtEmail.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            int accountType;
            if (int.TryParse(ddlAccountType.SelectedItem.Value, out accountType) && accountType > 0)
            {
                if (accountType == 1)
                {
                    criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, true);
                }
                else if (accountType == 2)
                {
                    criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);
                }
            }

            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, providerID);
            }

            int providerType;
            if (int.TryParse(ddlProviderType.SelectedItem.Value, out providerType) && providerType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ProviderType, (enProviderType)providerType);
            }

            int countryID;
            if (int.TryParse(ddlCountry.SelectedItem.Value, out countryID) && countryID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CountryID, countryID);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AFM, txtProviderAFM.Text.ToNull());
            }

            if (!string.IsNullOrEmpty(txtProviderName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Name, txtProviderName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
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

            e.InputParameters["criteria"] = criteria;
        }

        protected void odsProviders_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is List<InternshipProvider>)
            {
                _providers = e.ReturnValue as List<InternshipProvider>;
            }
        }

        protected void gvProviders_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipProvider provider = (InternshipProvider)gvProviders.GetRow(e.VisibleIndex);

            if (provider != null && provider.IsMasterAccount)
            {
                switch (provider.VerificationStatus)
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

        protected void OnEmailSending(object sender, EmailFormSendingEventArgs e)
        {
            if (!e.SendToLoggedInUser)
            {
                int _pageSize = gvProviders.SettingsPager.PageSize;

                gvProviders.SettingsPager.PageSize = int.MaxValue;
                gvProviders.DataBind();

                if (_providers != null && _providers.Count > 0)
                {
                    e.EmailRecepients.AddRange(_providers.Select(x => x.Email).Distinct());
                    e.EmailRecepients.AddRange(_providers.Select(x => x.ContactEmail).Distinct());
                    e.EmailRecepients.AddRange(_providers.Select(x => x.AlternateContactEmail).Distinct());
                    e.EmailRecepients = e.EmailRecepients.Distinct().ToList();
                    e.InfoMessage = string.Format("Η μαζική αποστολή ξεκίνησε για {0} μοναδικά Email χρηστών, {1} μοναδικά Email επικοινωνίας Εκδοτών και {2} μοναδικά Email επικοινωνίας αναπληρωτών από {3} συνολικά εκδότες.",
                        _providers.Select(x => x.Email).Distinct().Count(),
                        _providers.Select(x => x.ContactEmail).Distinct().Except(_providers.Select(x => x.Email).Distinct()).Count(),
                        _providers.Select(x => x.AlternateContactEmail).Distinct().Count(),
                        _providers.Count);
                }
                else
                {
                    e.Cancel = true;
                    e.InfoMessage = string.Format("Η μαζική αποστολή δεν πραγματοποιήθηκε.");
                }
                gvProviders.SettingsPager.PageSize = _pageSize;

            }
            else
            {
                e.InfoMessage = string.Format("Η δοκιμαστική αποστολή ολοκληρώθηκε. Παρακαλούμε ελέξτε το Email σας.");
            }
        }

        #endregion

        #region [ Button Methods ]

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvProviders.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvProviders.PageIndex = 0;
            gvProviders.DataBind();
        }

        #endregion

        #region [ Helper Methods ]

        protected string GetProviderDetails(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (!string.IsNullOrEmpty(provider.TradeName))
            {
                providerDetails = string.Format("{0} <br/>{1} <br/>{2}", provider.Name, provider.TradeName, provider.AFM);
            }
            else
            {
                providerDetails = string.Format("{0} <br/>{1}", provider.Name, provider.AFM);
            }

            return providerDetails;
        }

        protected bool CanEditAccountDetails(InternshipProvider provider)
        {
            if (provider == null)
                return false;

            return provider.IsMasterAccount;
        }

        protected bool CanEditProvider(InternshipProvider provider)
        {
            if (provider == null)
                return false;

            return provider.IsMasterAccount && provider.VerificationStatus == enVerificationStatus.Verified;
        }

        protected bool CanShowProviderUsers(InternshipProvider provider)
        {
            if (provider == null)
                return false;

            return provider.IsMasterAccount;
        }

        protected string GetInternshipPositionsLink(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            string link = string.Empty;

            link = string.Format("~/Secure/Helpdesk/SearchPositionGroups.aspx?pID={0}", provider.ID);

            return link;
        }

        protected string GetSearchIncidentReportLink(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            string link = string.Empty;

            link = string.Format("~/Secure/Helpdesk/SearchIncidentReports.aspx?hideFilters=true&rID={0}", provider.ID);

            return link;
        }

        protected bool CanViewVerificationLogs(InternshipProvider provider)
        {
            if (provider == null)
                return false;

            return provider.IsMasterAccount;
        }

        #endregion
    }
}