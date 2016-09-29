using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;
using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class InternshipProviders : BaseEntityPortalPage
    {
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
                txtEmail.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                   Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
            }
        }

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
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.GreeceCountryName, StudentPracticeConstants.GreeceCountryID.ToString()));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.CyprusCountryName, StudentPracticeConstants.CyprusCountryID.ToString()));
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvProviders.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvProviders.PageIndex = 0;
            gvProviders.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvProvidersExport.Visible = true;
            gveIntershipProviders.FileName = String.Format("IntershipProviders_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipProviders.WriteXlsxToResponse(true);
        }

        protected void odsProviders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            int verificationStatus;
            if (int.TryParse(ddlVerificationStatus.SelectedItem.Value, out verificationStatus) && verificationStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.VerificationStatus, (enVerificationStatus)verificationStatus);
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

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Email, txtEmail.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AFM, txtProviderAFM.Text.ToNull());
            }

            if (!string.IsNullOrEmpty(txtProviderName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Name, txtProviderName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvProviders_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipProvider provider = (InternshipProvider)gvProviders.GetRow(e.VisibleIndex);

            if (provider != null)
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

        protected string GetProviderDetails(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            string providerDetails = string.Empty;
            if (!string.IsNullOrEmpty(provider.TradeName))
                providerDetails = string.Format("{0} <br />{1} <br />ΑΦΜ: {2}<br />ΔΟΥ: {3}", provider.Name, provider.TradeName, provider.AFM, provider.DOY);
            else
                providerDetails = string.Format("{0} <br />ΑΦΜ: {1}<br />ΔΟΥ: {2}", provider.Name, provider.AFM, provider.DOY);

            return providerDetails;
        }

        protected string GetAddressDetails(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            string country;
            string city = provider.CityID.HasValue ? CacheManager.Cities.Get(provider.CityID.Value).Name : string.Empty;
            string prefecture = provider.PrefectureID.HasValue ? CacheManager.Prefectures.Get(provider.PrefectureID.Value).Name + "<br />" : string.Empty;


            if (provider.CountryID.HasValue && provider.CountryID.Value == StudentPracticeConstants.GreeceCountryID)
                country = StudentPracticeConstants.GreeceCountryName;
            else if (provider.CountryID.HasValue && provider.CountryID.Value == StudentPracticeConstants.CyprusCountryID)
                country = StudentPracticeConstants.CyprusCountryName;
            else
                country = string.Empty;

            return string.Format("{0}, {1} ({2})<br />{3}{4}", provider.Address, city, provider.ZipCode, prefecture, country);
        }

        protected string GetContantDetails(InternshipProvider provider)
        {
            if (provider == null) return string.Empty;

            string contantDetails = "Τηλέφωνο (σταθερό): " + provider.ProviderPhone;
            contantDetails += "<br />E-mail: " + provider.ProviderEmail;
            if (!string.IsNullOrEmpty(provider.ProviderFax)) contantDetails += "<br />Fax: " + provider.ProviderFax;
            if (!string.IsNullOrEmpty(provider.ProviderURL)) contantDetails += "<br />Ιστοσελίδα: " + provider.ProviderURL;

            return contantDetails;
        }

        protected string GetLegalPersonDetails(InternshipProvider provider)
        {
            if (provider == null) return string.Empty;
            string identification;
            switch (provider.LegalPersonIdentificationType)
            {
                case enIdentificationType.Passport:
                    identification = string.Format("{0}, {1}",
                        provider.LegalPersonIdentificationType.GetLabel(),
                        provider.LegalPersonIdentificationNumber);
                    break;
                case enIdentificationType.ID:
                    identification = string.Format("{0}, {1}, {2}, {3}",
                        provider.LegalPersonIdentificationType.GetLabel(),
                        provider.LegalPersonIdentificationNumber,
                        provider.LegalPersonIdentificationIssuer,
                        provider.LegalPersonIdentificationIssueDate.Value.ToShortDateString());
                    break;
                default:
                    identification = string.Empty;
                    break;
            }


            return string.Format("{0}<br />{1}<br />{2}<br />{3}",
                provider.LegalPersonName,
                provider.LegalPersonPhone,
                provider.LegalPersonEmail,
                identification);
        }

        protected string GetResponsiblePersonDetails(InternshipProvider provider)
        {
            if (provider == null) return string.Empty;
            return string.Format("{0}<br />{1}, {2}<br />{3}", provider.ContactName, provider.ContactPhone, provider.ContactMobilePhone, provider.ContactEmail);
        }

        protected string GetAlternateResponsiblePersonDetails(InternshipProvider provider)
        {
            if (provider == null) return string.Empty;
            if (string.IsNullOrEmpty(provider.AlternateContactName))
                return string.Empty;
            return string.Format("{0}<br />{1}, {2}<br />{3}", provider.AlternateContactName, provider.AlternateContactPhone, provider.AlternateContactMobilePhone, provider.AlternateContactEmail);
        }

        protected string GetLegalPersonIdentification(InternshipProvider provider)
        {
            if (provider == null) return string.Empty;
            string identification;
            switch (provider.LegalPersonIdentificationType)
            {
                case enIdentificationType.Passport:
                    identification = string.Format("{0}, {1}",
                        provider.LegalPersonIdentificationType.GetLabel(),
                        provider.LegalPersonIdentificationNumber);
                    break;
                case enIdentificationType.ID:
                    identification = string.Format("{0}, {1}, {2}, {3}",
                        provider.LegalPersonIdentificationType.GetLabel(),
                        provider.LegalPersonIdentificationNumber,
                        provider.LegalPersonIdentificationIssuer,
                        provider.LegalPersonIdentificationIssueDate.Value.ToShortDateString());
                    break;
                default:
                    identification = string.Empty;
                    break;
            }
            return identification;
        }

        protected string GetVerificationDate(InternshipProvider provider)
        {
            if (provider == null || !provider.VerificationDate.HasValue)
                return string.Empty;
            return provider.VerificationDate.Value.ToString("dd/MM/yyyy");

        }

        protected void gveIntershipProviders_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var provider = gvProvidersExport.GetRow(e.VisibleIndex) as InternshipProvider;

                if (provider != null)
                {
                    switch (e.Column.Name)
                    {
                        case "IsMasterAccount":
                            e.TextValue = e.Text = provider.IsMasterAccount ? "Κεντρικός Φορέας" : "Παράρτημα";
                            break;
                        case "ProviderType":
                            e.TextValue = e.Text = provider.ProviderType.GetLabel();
                            break;
                        case "PrimaryActivity":
                            e.TextValue = e.Text = provider.GetPrimaryActivity().Replace("<br />", "\n");
                            break;
                        case "VerificationStatus":
                            e.TextValue = e.Text = provider.GetVerificationStatus().Replace("<br />", "\n");
                            break;
                        case "VerificationDate":
                            e.TextValue = e.Text = provider.GetVerificationDate();
                            break;
                        case "ProviderPrefecture":
                            e.TextValue = e.Text = provider.GetProviderPrefecture();
                            break;
                        case "ProviderCity":
                            e.TextValue = e.Text = provider.GetProviderCity();
                            break;
                        case "LegalPersonIdentification":
                            e.TextValue = e.Text = GetLegalPersonIdentification(provider);
                            break;
                        case "Country":
                            e.TextValue = e.Text = provider.GetProviderCountry();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}