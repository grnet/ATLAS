using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxGridView;
using System.Drawing;
using StudentPractice.Portal.DataSources;
using StudentPractice.Portal.UserControls.Exporters;
using System.IO;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class InternshipPositionGroups : BaseEntityPortalPage
    {
        #region [ Control Inits ]

        protected void ddlPositionCreationType_Init(object sender, EventArgs e)
        {
            ddlPositionCreationType.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionCreationType.Items.Add(new ListItem(enPositionCreationType.FromOffice.GetLabel(), ((int)enPositionCreationType.FromOffice).ToString()));
            ddlPositionCreationType.Items.Add(new ListItem(enPositionCreationType.FromProvider.GetLabel(), ((int)enPositionCreationType.FromProvider).ToString()));
        }

        protected void ddlPhysicalObject_Init(object sender, EventArgs e)
        {
            ddlPhysicalObject.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.PhysicalObjects.GetItems())
            {
                ddlPhysicalObject.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.GreeceCountryName, StudentPracticeConstants.GreeceCountryID.ToString()));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.CyprusCountryName, StudentPracticeConstants.CyprusCountryID.ToString()));
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPositionGroupStatus_Init(object sender, EventArgs e)
        {
            ddlPositionGroupStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Deleted.GetLabel(), ((int)enPositionGroupStatus.Deleted).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.UnPublished.GetLabel(), ((int)enPositionGroupStatus.UnPublished).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Published.GetLabel(), ((int)enPositionGroupStatus.Published).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Revoked.GetLabel(), ((int)enPositionGroupStatus.Revoked).ToString()));
        }

        #endregion

        #region [ Page Methods ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtGroupId.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtProviderID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, string.Empty);
            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }

            Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, string.Empty);
            IList<Prefecture> prefectures = CacheManager.Prefectures.GetItems();
            foreach (Prefecture prefecture in prefectures)
            {
                Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, prefecture.ID.ToString());
            }

            Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, string.Empty);
            IList<Academic> academics = CacheManager.Academics.GetItems();
            foreach (Academic academic in academics)
            {
                Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, academic.ID.ToString());
            }

            base.Render(writer);
        }

        #endregion

        #region [ Button Methods]

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPositionGroup>();
            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            ParseFilters(criteria);

            var positions = new PositionGroups().FindInternshipPositionGroupReport(criteria, 0, int.MaxValue, "ID");

            gvPositionGroupsExport.DataSource = positions;
            gveIntershipPositionGroups.FileName = string.Format("IntershipPositionGroups_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipPositionGroups.WriteXlsxToResponse(true);
        }

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            var filePath = string.Format("{0}\\InternshipPositionGroups.xls", Config.ReportFilesDirectory);
            GenerateFile(filePath);

            //var fileCreated = true;

            //if (!File.Exists(filePath)
            //    || File.GetLastWriteTime(filePath).AddHours(24) < DateTime.Now)
            //{
            //    try
            //    {
            //        filePath = CreateInternshipPositionGroupsFile();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.LogError<InternshipPositionGroups>(ex, this, string.Format("Error while generating the excel file for internshipPositionGroups"));
            //        fileCreated = false;
            //    }
            //}

            //if (fileCreated)
            //{
            //    GenerateFile(filePath);
            //}
            //else
            //{
            //    Notify("Υπήρξε κάποιο πρόβλημα με την παραγωγή του αρχείου. Παρακαλώ δοκιμάστε ξανά ή επικοινωνήστε με το Γραφείο Υποστήριξης Χρηστών.");
            //}
        }

        public static string CreateInternshipPositionGroupsFile()
        {
            var iRep = new InternshipPositionGroupRepository();
            var iExporter = new InternshipPositionGroupsExporter();

            var filePath = string.Format("{0}\\InternshipPositionGroups.xls", Config.ReportFilesDirectory);
            iRep.GetInternshipPositionGroupsAsReader((reader) =>
            {
                iExporter.ExportToFile(reader, filePath);
            });

            return filePath;
        }

        private void GenerateFile(string filePath)
        {
            var fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);

            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlPathEncode(fileName));
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString());
            Response.TransmitFile(filePath);
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositionGroups.PageIndex = 0;
            gvPositionGroups.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositionGroups.DataBind();
        }

        #endregion

        #region [ Grid Methods ]

        private void ParseFilters(Criteria<InternshipPositionGroup> criteria)
        {
            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ProviderID, providerID);
            }

            int groupID;
            if (int.TryParse(txtGroupId.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, groupID);
            }

            int positionGroupStatus = -1;
            if (int.TryParse(ddlPositionGroupStatus.SelectedItem.Value, out positionGroupStatus) && positionGroupStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatusInt, positionGroupStatus);
            }

            int positionCreationType = -1;
            if (int.TryParse(ddlPositionCreationType.SelectedItem.Value, out positionCreationType) && positionCreationType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionCreationTypeInt, positionCreationType);
            }

            int submissionDate;
            if (int.TryParse(ddlSubmissionDate.SelectedItem.Value, out submissionDate) && submissionDate > 0)
            {
                switch (submissionDate)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.AddDays(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.AddDays(-7), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 3:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.AddMonths(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 4:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.AddMonths(-3), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    default:
                        break;
                }
            }

            int physicalObjectID;
            if (int.TryParse(ddlPhysicalObject.SelectedItem.Value, out physicalObjectID) && physicalObjectID > 0)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("(it.PhysicalObjects) OVERLAPS (SELECT VALUE it1 FROM PhysicalObjectSet as it1 WHERE it1.ID = {0} )", physicalObjectID));
            }

            int countryID;
            if (int.TryParse(ddlCountry.SelectedItem.Value, out countryID) && countryID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CountryID, countryID);
            }

            int prefectureID;
            if (int.TryParse(ddlPrefecture.SelectedItem.Value, out prefectureID) && prefectureID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PrefectureID, prefectureID);
            }

            int cityID;
            if (int.TryParse(ddlCity.SelectedItem.Value, out cityID) && cityID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CityID, cityID);
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID > 0)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
                orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.InstitutionID = {0})", institutionID));
                criteria.Expression = criteria.Expression.And(orExpression);
            }

            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
                orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.ID = {0})", academicID));
                criteria.Expression = criteria.Expression.And(orExpression);
            }

            var startDate = deCreatedAtFrom.GetDate();
            if (startDate != null)
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, startDate.Value, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);

            var endDate = deCreatedAtTo.GetDate();
            if (endDate != null)
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, endDate.Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
        }

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Provider)
                .Include(x => x.PhysicalObjects)
                .Include(x => x.Academics);

            //criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            ParseFilters(criteria);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvPositionGroups_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            InternshipPositionGroup group = (InternshipPositionGroup)gvPositionGroups.GetRow(e.VisibleIndex);

            switch (group.PositionGroupStatus)
            {
                case enPositionGroupStatus.Published:
                    e.Row.BackColor = Color.LightGreen;
                    break;
                case enPositionGroupStatus.UnPublished:
                    e.Row.BackColor = Color.Gray;
                    break;
                case enPositionGroupStatus.Revoked:
                    e.Row.BackColor = Color.Tomato;
                    break;
                default:
                    break;
            }
        }

        protected void gveIntershipPositionGroups_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var group = gvPositionGroupsExport.GetRow(e.VisibleIndex) as InternshipPositionGroup;

                if (group != null)
                {
                    switch (e.Column.Name)
                    {
                        case "CreatedAt":
                            e.TextValue = e.Text = group.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                            break;
                        case "FirstPublishedAt":
                            e.TextValue = e.Text = GetFirstPublishedAt(group).Replace("<br />", "\n");
                            break;
                        case "Country":
                            if (group.CountryID == StudentPracticeConstants.GreeceCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.GreeceCountryName;
                            else if (group.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.CyprusCountryName;
                            else
                                e.TextValue = e.Text = "Άλλη";
                            break;
                        case "Prefecture":
                            e.TextValue = e.Text = group.PrefectureID.HasValue ? CacheManager.Prefectures.Get(group.PrefectureID.Value).Name : string.Empty;
                            break;
                        case "City":
                            e.TextValue = e.Text = group.CityID.HasValue ? CacheManager.Cities.Get(group.CityID.Value).Name : string.Empty;
                            break;
                        case "TimeAvailable":
                            e.TextValue = e.Text = GetTimeAvailable(group);
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = group.PositionType.GetLabel();
                            break;
                        case "PositionGroupStatus":
                            e.TextValue = e.Text = GetPositionGroupStatus(group);
                            break;
                        case "PhysicalObjects":
                            e.TextValue = e.Text = group.GetPhysicalObjectDetails().Replace("<br />", ";");
                            break;
                        case "AcademicsID":
                            e.TextValue = e.Text = GetAcademicsID(group).Replace("<br />", ";");
                            break;
                        case "Institutions":
                            e.TextValue = e.Text = GetInstitutions(group).Replace("<br />", ";");
                            break;
                        case "Departments":
                            e.TextValue = e.Text = GetDepartments(group).Replace("<br />", ";");
                            break;
                        case "PositionCreationType":
                            e.TextValue = e.Text = group.PositionCreationType.GetLabel();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region [ Helper Methods ]

        protected string GetTimeConstrain(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.NoTimeLimit)
                return "Κανένας";
            else
                return string.Format("Από: {0}<br />Έως: {1}", ip.StartDate, ip.EndDate);
        }

        protected string GetTimeAvailable(InternshipPositionGroup ip)
        {
            if (ip == null || ip.NoTimeLimit)
                return string.Empty;
            else
            {
                var startDate = ip.StartDate.HasValue ? ip.StartDate.Value.ToString("dd/MM/yyyy") : "N/A";
                var endDate = ip.EndDate.HasValue ? ip.EndDate.Value.ToString("dd/MM/yyyy") : "N/A";
                return string.Format("{0} - {1}", startDate, endDate);
            }
        }

        protected string GetContanctDetails(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            else
            {
                string details = string.Empty;
                details += string.IsNullOrEmpty(ip.Supervisor) ? string.Empty : string.Format("Ον/μο Επόπτη: {0}<br />", ip.Supervisor);
                details += string.IsNullOrEmpty(ip.SupervisorEmail) ? string.Empty : string.Format("E-mail Επόπτη: {0}<br/>", ip.SupervisorEmail);
                details += string.IsNullOrEmpty(ip.ContactPhone) ? string.Empty : string.Format("Τηλέφωνο Θέσης: {0}<br />", ip.ContactPhone);
                return details;
            }
        }

        protected string GetFirstPublishedAt(InternshipPositionGroup ip)
        {
            if (ip == null || ip.PositionGroupStatus < enPositionGroupStatus.Published)
                return string.Empty;
            else if (ip.PositionCreationType == enPositionCreationType.FromOffice)
                return ip.CreatedAt.ToString("dd/MM/yyyy");
            else
                return ip.FirstPublishedAt == null ? string.Empty : ip.FirstPublishedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetPositionGroupStatus(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            else
                return ip.PositionGroupStatus.GetLabel();
        }

        protected string GetAcademicsID(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.HasValue && ip.IsVisibleToAllAcademics.Value)
                return string.Empty;
            else
                return string.Join(";", ip.Academics.Select(x => x.ID));
        }

        protected string GetInstitutions(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.HasValue && ip.IsVisibleToAllAcademics.Value)
                return "Όλα";
            else
                return string.Join(";", ip.Academics.Select(x => x.Institution).Distinct());
        }

        protected string GetDepartments(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.HasValue && ip.IsVisibleToAllAcademics.Value)
                return "Όλα";
            else
                return string.Join(";", ip.Academics.Select(x => string.Format("{0} ({1})", x.Department, x.Institution)).Distinct());
        }

        #endregion
    }
}