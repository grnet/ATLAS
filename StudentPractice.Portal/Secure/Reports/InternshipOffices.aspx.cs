using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Drawing;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class InternshipOffices : BaseEntityPortalPage<object>
    {
        protected override void Fill()
        {
            base.Fill();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvOfficesExport.Visible = true;
            gveIntershipOffices.FileName = String.Format("IntershipOffices_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipOffices.WriteXlsxToResponse(true);
        }

        protected void btnExportUsers_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipOffice>();
            criteria.Expression = criteria.Expression.IsNotNull(x => x.MasterAccountID);

            var offices = new DataSources.Offices().FindInternshipOfficeUsersReport(criteria, 0, int.MaxValue, "MasterAccountID");

            gvOfficeUsersExport.DataSource = offices;
            gveInternshipOfficeUsers.FileName = string.Format("InternshipOfficeUsers_{0}", DateTime.Now.ToString("yyyyMMdd"));

            gveInternshipOfficeUsers.WriteXlsxToResponse();
        }

        protected void odsOffices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipOffice> criteria = new Criteria<InternshipOffice>();

            criteria.Include(x => x.Academics)
                    .Include(x => x.ChildAccounts);

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);
            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, true);

            int verificationStatus;
            if (int.TryParse(ddlVerificationStatus.SelectedItem.Value, out verificationStatus) && verificationStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.VerificationStatus, (enVerificationStatus)verificationStatus);
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

            int officeID;
            if (int.TryParse(txtOfficeID.Text, out officeID) && officeID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, officeID);
            }

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

        protected string GetOfficeAcademics(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            if (office.CanViewAllAcademics.GetValueOrDefault())
                return string.Empty;
            else
            {
                if (office.Academics.Count == 1)
                    return string.IsNullOrEmpty(office.Academics.FirstOrDefault().Department) ? string.Empty : office.Academics.FirstOrDefault().Department;
                else
                    return string.Join(";", office.Academics.Select(x => x.Department).ToArray());
            }
        }

        protected string GetVerificationDate(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            return office.VerificationDate.HasValue ? office.VerificationDate.Value.ToString("dd/MM/yyyy") : string.Empty;
        }

        protected string GetOfficeDetails(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            string officeDetails = "Email: " + office.Email;
            officeDetails += "<br />Οδός - Αριθμός: " + office.Address;
            officeDetails += "<br />Τ.Κ.: " + office.ZipCode;
            if (office.PrefectureID.HasValue) officeDetails += "<br />Νομός: " + CacheManager.Prefectures.Get(office.PrefectureID.Value).Name;
            if (office.CityID.HasValue) officeDetails += "<br />Πολη: " + CacheManager.Cities.Get(office.CityID.Value).Name;

            return officeDetails;
        }

        protected string GetOfficePrefecture(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            return office.PrefectureID.HasValue ? CacheManager.Prefectures.Get(office.PrefectureID.Value).Name : string.Empty;
        }

        protected string GetOfficeCity(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            return office.CityID.HasValue ? CacheManager.Cities.Get(office.CityID.Value).Name : string.Empty;
        }

        protected string GetOfficeAdmin(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            string officeAdmin = "Ονοματεπώνυμο: " + office.ContactName;
            officeAdmin += "<br />Τηλέφωνο (σταθερό): " + office.ContactPhone;
            officeAdmin += "<br />Τηλέφωνο (κινητό): " + office.ContactMobilePhone;
            if (!string.IsNullOrEmpty(office.Email)) officeAdmin += "<br />Email: " + office.ContactEmail;

            return officeAdmin;
        }

        protected string GetOfficeAdminAlt(InternshipOffice office)
        {
            if (office == null) return string.Empty;

            string officeAdmin = string.Empty;
            if (!string.IsNullOrEmpty(office.AlternateContactName))
                officeAdmin += "Ονοματεπώνυμο: " + office.AlternateContactName;
            if (!string.IsNullOrEmpty(office.AlternateContactPhone))
                officeAdmin += "<br />Τηλέφωνο (σταθερό): " + office.AlternateContactPhone;
            if (!string.IsNullOrEmpty(office.AlternateContactMobilePhone))
                officeAdmin += "<br />Τηλέφωνο (κινητό): " + office.AlternateContactMobilePhone;
            if (!string.IsNullOrEmpty(office.AlternateContactEmail))
                officeAdmin += "<br />Email: " + office.AlternateContactEmail;

            return officeAdmin;
        }

        protected string GetOfficeCertifier(InternshipOffice office)
        {
            if (office == null) return string.Empty;
            return string.Format("{0}: {1}", office.CertifierType.GetLabel(), office.CertifierName);
        }

        protected void gveIntershipOffices_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var office = gvOfficesExport.GetRow(e.VisibleIndex) as InternshipOffice;

                if (office != null)
                {
                    switch (e.Column.Name)
                    {
                        case "CreatedAt":
                            e.TextValue = e.Text = office.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                            break;
                        case "OfficeType":
                            e.TextValue = e.Text = office.OfficeType.GetLabel();
                            break;
                        case "OfficeInstitute":
                            e.TextValue = e.Text = office.GetOfficeInstitution();
                            break;
                        case "OfficeAcademics":
                            e.TextValue = e.Text = GetOfficeAcademics(office);
                            break;
                        case "OfficePrefecture":
                            e.TextValue = e.Text = GetOfficePrefecture(office);
                            break;
                        case "OfficeCity":
                            e.TextValue = e.Text = GetOfficeCity(office);
                            break;
                        case "OfficeCertifier":
                            e.TextValue = e.Text = GetOfficeCertifier(office).Replace("<br />", "\n");
                            break;
                        case "OfficeCertification":
                            e.TextValue = e.Text = office.GetCertificationDetails().Replace("<br />", "\n");
                            break;
                        case "VerificationStatus":
                            e.TextValue = e.Text = office.GetVerificationStatus().Replace("<br />", "\n");
                            break;
                        case "VerificationDate":
                            e.TextValue = e.Text = GetVerificationDate(office);
                            break;
                        case "OfficeUsersCount":
                            e.TextValue = e.Text = GetOfficeUsersCount(office);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void gveInternshipOfficeUsers_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var office = gvOfficeUsersExport.GetRow(e.VisibleIndex) as InternshipOffice;

                if (office != null)
                {
                    switch (e.Column.Name)
                    {
                        case "Academics":
                            e.TextValue = e.Text = office.GetOfficeAcademics(((InternshipOffice)office.MasterAccount).Academics.Count, true);
                            break;
                        case "IsApproved":
                            e.TextValue = e.Text = office.IsApproved ? "ΝΑΙ" : "ΟΧΙ";
                            break;
                        case "Institution":
                            e.TextValue = e.Text = CacheManager.Institutions.Get(office.InstitutionID.Value).Name;
                            break;
                        case "MasterOfficeType":
                            e.TextValue = e.Text = ((InternshipOffice)office.MasterAccount).OfficeType.GetLabel();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static string GetOfficeUsersCount(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            return office.ChildAccounts.Count().ToString();
        }
    }
}