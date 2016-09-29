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
using StudentPractice.Utils;
using System.IO;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class InternshipPositions : BaseEntityPortalPage
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

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPositionStatus_Init(object sender, EventArgs e)
        {
            ddlPositionStatus.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            foreach (enPositionStatus item in Enum.GetValues(typeof(enPositionStatus)))
            {
                ddlPositionStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
            ddlPositionStatus.Items.Add(new ListItem("Αποσυρμένη", "99"));
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.GreeceCountryName, StudentPracticeConstants.GreeceCountryID.ToString()));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.CyprusCountryName, StudentPracticeConstants.CyprusCountryID.ToString()));
        }

        protected void ddlFundingType_Init(object sender, EventArgs e)
        {
            ddlFundingType.Items.Add(new ListItem("-- αδιάφορο --", ""));
            foreach (enFundingType item in Enum.GetValues(typeof(enFundingType)))
            {
                ddlFundingType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
            }
        }

        #endregion

        #region [ Page Methods ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtGroupId.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtPositionID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
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
            var criteria = new Criteria<InternshipPosition>();
            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            ParseFilters(criteria);

            var positions = new Positions().FindInternshipPositionReport(criteria, 0, int.MaxValue, "InternshipPositionGroup.ID");

            gvPositionsExport.DataSource = positions;
            gveIntershipPositions.FileName = string.Format("IntershipPositions_{0}", DateTime.Now.ToString("yyyyMMdd"));

            gveIntershipPositions.WriteXlsxToResponse();
        }

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            var filePath = string.Format("{0}\\InternshipPositions.xls", Config.ReportFilesDirectory);
            GenerateFile(filePath);

            //var fileCreated = true;

            //try
            //{
            //    filePath = CreateInternshipPositionsFile();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.LogError<InternshipPositions>(ex, this, string.Format("Error while generating the excel file for internshipPositions"));
            //    fileCreated = false;
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

        public static string CreateInternshipPositionsFile()
        {
            var iRep = new InternshipPositionRepository();
            var iExporter = new InternshipPositionsExporter();

            var filePath = string.Format("{0}\\InternshipPositions.xls", Config.ReportFilesDirectory);
            iRep.GetInternshipPositionsAsReader((reader) =>
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
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositions.DataBind();
        }

        #endregion

        #region [ Grid Methods ]

        private void ParseFilters(Criteria<InternshipPosition> criteria)
        {
            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, providerID);
            }

            int groupID;
            if (int.TryParse(txtGroupId.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ID, groupID);
            }

            int positionID;
            if (int.TryParse(txtPositionID.Text.ToNull(), out positionID) && positionID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, positionID);
            }

            int positionCreationType = -1;
            if (int.TryParse(ddlPositionCreationType.SelectedItem.Value, out positionCreationType) && positionCreationType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt, positionCreationType);
            }

            int submissionDate;
            if (int.TryParse(ddlSubmissionDate.SelectedItem.Value, out submissionDate) && submissionDate > 0)
            {
                switch (submissionDate)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.AddDays(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.AddDays(-7), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 3:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.AddMonths(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    case 4:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.AddMonths(-3), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                    default:
                        break;
                }
            }

            int positionStatusID;
            if (int.TryParse(ddlPositionStatus.SelectedItem.Value, out positionStatusID) && positionStatusID >= 0)
            {
                if (positionStatusID == 99)
                {
                    criteria.Expression = criteria.Expression.Where(x => x.PositionStatus, enPositionStatus.Canceled);
                    criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                }
                else if (positionStatusID == (int)enPositionStatus.Canceled)
                {
                    criteria.Expression = criteria.Expression.Where(x => x.PositionStatus, enPositionStatus.Canceled);
                    criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.LessThanEquals);
                }
                else
                {
                    criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, positionStatusID);
                }
            }

            int physicalObjectID;
            if (int.TryParse(ddlPhysicalObject.SelectedItem.Value, out physicalObjectID) && physicalObjectID > 0)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("(it.InternshipPositionGroup.PhysicalObjects) OVERLAPS (SELECT VALUE it1 FROM PhysicalObjectSet as it1 WHERE it1.ID = {0} )", physicalObjectID));
            }

            int countryID;
            if (int.TryParse(ddlCountry.SelectedItem.Value, out countryID) && countryID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CountryID, countryID);
            }

            int prefectureID;
            if (int.TryParse(ddlPrefecture.SelectedItem.Value, out prefectureID) && prefectureID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PrefectureID, prefectureID);
            }

            int cityID;
            if (int.TryParse(ddlCity.SelectedItem.Value, out cityID) && cityID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CityID, cityID);
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID > 0)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
                orExpression = orExpression.Where(x => x.InternshipPositionGroup.IsVisibleToAllAcademics, true);
                orExpression = orExpression.Or(string.Format("(it.InternshipPositionGroup.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.InstitutionID = {0})", institutionID));
                criteria.Expression = criteria.Expression.And(orExpression);
            }

            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
                orExpression = orExpression.Where(x => x.InternshipPositionGroup.IsVisibleToAllAcademics, true);
                orExpression = orExpression.Or(string.Format("(it.InternshipPositionGroup.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.ID = {0})", academicID));
                criteria.Expression = criteria.Expression.And(orExpression);
            }

            var startDate = deCreatedAtFrom.GetDate();
            if (startDate != null)
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, startDate.Value, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);

            var endDate = deCreatedAtTo.GetDate();
            if (endDate != null)
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, endDate.Value.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);

            int fundingType;
            if (int.TryParse(ddlFundingType.SelectedItem.Value, out fundingType) && fundingType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.FundingTypeInt, fundingType);
            }

        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.InternshipPositionGroup.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            ParseFilters(criteria);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            InternshipPosition position = (InternshipPosition)gvPositions.GetRow(e.VisibleIndex);

            if (position != null)
            {
                switch (position.PositionStatus)
                {
                    case enPositionStatus.UnPublished:
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice
                            && position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                            e.Row.BackColor = Color.Gray;
                        break;
                    case enPositionStatus.Assigned:
                        e.Row.BackColor = Color.LightPink;
                        break;
                    case enPositionStatus.UnderImplementation:
                        e.Row.BackColor = Color.Yellow;
                        break;
                    case enPositionStatus.Completed:
                        e.Row.BackColor = Color.LightGreen;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.LightBlue;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice
                            && position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                            e.Row.BackColor = Color.Gray;
                        break;
                    case enPositionStatus.Canceled:
                        e.Row.BackColor = Color.Tomato;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.Purple;
                        break;
                }
            }
        }

        protected void gveIntershipPositions_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var position = gvPositionsExport.GetRow(e.VisibleIndex) as InternshipPosition;

                if (position != null)
                {
                    switch (e.Column.Name)
                    {
                        case "CreatedAt":
                            e.TextValue = e.Text = position.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                            break;
                        case "FirstPublishedAt":
                            e.TextValue = e.Text = GetFirstPublishedAt(position).Replace("<br />", "\n");
                            break;
                        case "Country":
                            if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.GreeceCountryName;
                            else if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.CyprusCountryName;
                            else
                                e.TextValue = e.Text = "Άλλη";
                            break;
                        case "Prefecture":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PrefectureID.HasValue ?
                                CacheManager.Prefectures.Get(position.InternshipPositionGroup.PrefectureID.Value).Name : string.Empty;
                            break;
                        case "City":
                            e.TextValue = e.Text = position.InternshipPositionGroup.CityID.HasValue ?
                                CacheManager.Cities.Get(position.InternshipPositionGroup.CityID.Value).Name : position.InternshipPositionGroup.CityText;
                            break;
                        case "TimeAvailable":
                            e.TextValue = e.Text = GetTimeAvailable(position);
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PositionType.GetLabel();
                            break;
                        case "PositionGroupStatus":
                            e.TextValue = e.Text = GetPositionGroupStatus(position);
                            break;
                        case "PositionStatus":
                            e.TextValue = e.Text = GetPositionStatus(position);
                            break;
                        case "PreAssignedAt":
                            e.TextValue = e.Text = GetPreAssignedAt(position);
                            break;
                        case "PreAssignedForAcademic.Institution":
                            e.TextValue = e.Text = (position.PreAssignedForAcademic == null ? string.Empty : position.PreAssignedForAcademic.Institution);
                            break;
                        case "PreAssignedForAcademic.Department":
                            e.TextValue = e.Text = (position.PreAssignedForAcademic == null ? string.Empty : position.PreAssignedForAcademic.Department);
                            break;
                        case "AssignedAt":
                            e.TextValue = e.Text = GetAssignedAt(position);
                            break;
                        case "AssignedToStudent.ID":
                            e.TextValue = e.Text = (position.AssignedToStudentID.HasValue ? position.AssignedToStudentID.Value.ToString() : string.Empty);
                            break;
                        case "AssignedToStudent.ContactName":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.ContactName);
                            break;
                        case "AssignedToStudent.StudentNumber":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.StudentNumber);
                            break;
                        case "AssignedToStudent.AcademicIDNumber":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.AcademicIDNumber);
                            break;
                        case "ImplementationStartDate":
                            e.TextValue = e.Text = GetImplementationStartDate(position);
                            break;
                        case "ImplementationEndDate":
                            e.TextValue = e.Text = GetImplementationEndDate(position);
                            break;
                        case "FundingType":
                            if (position.FundingTypeInt.HasValue)
                            {
                                e.TextValue = e.Text = position.FundingType.GetLabel();
                            }
                            break;
                        case "CompletedAt":
                            e.TextValue = e.Text = GetCompletedAt(position);
                            break;
                        case "PhysicalObjects":
                            e.TextValue = e.Text = position.GetPhysicalObjectDetails().Replace("<br />", ";");
                            break;
                        case "PositionCreationType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PositionCreationType.GetLabel();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region [ Helper Methods ]

        protected string GetPositionGroupStatus(InternshipPosition ip)
        {
            if (ip == null || ip.InternshipPositionGroup == null)
                return string.Empty;
            else
                return ip.InternshipPositionGroup.PositionGroupStatus.GetLabel();
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else if (ip.PositionStatus == enPositionStatus.Canceled && ip.CancellationReason > enCancellationReason.FromOffice)
                return "Αποσυρμένη";
            else
                return ip.PositionStatus.GetLabel();
        }

        protected string GetContanctDetails(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else
            {
                string details = string.Empty;
                details += string.IsNullOrEmpty(ip.InternshipPositionGroup.Supervisor) ? string.Empty : string.Format("Ον/μο Επόπτη: {0}<br />", ip.InternshipPositionGroup.Supervisor);
                details += string.IsNullOrEmpty(ip.InternshipPositionGroup.SupervisorEmail) ? string.Empty : string.Format("E-mail Επόπτη: {0}<br/>", ip.InternshipPositionGroup.SupervisorEmail);
                details += string.IsNullOrEmpty(ip.InternshipPositionGroup.ContactPhone) ? string.Empty : string.Format("Τηλέφωνο Θέσης: {0}<br />", ip.InternshipPositionGroup.ContactPhone);
                return details;
            }
        }


        protected string GetTimeConstrain(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.InternshipPositionGroup.NoTimeLimit)
                return "Κανένας";
            else
            {
                var startDate = ip.InternshipPositionGroup.StartDate.HasValue ? ip.InternshipPositionGroup.StartDate.Value.ToString("dd/MM/yyyy") : "N/A";
                var endDate = ip.InternshipPositionGroup.EndDate.HasValue ? ip.InternshipPositionGroup.EndDate.Value.ToString("dd/MM/yyyy") : "N/A";
                return string.Format("Από: {0}<br />Έως: {1}", startDate, endDate);
            }
        }

        protected string GetPreAssignInfo(InternshipPosition ip)
        {
            if (ip == null || !ip.PreAssignedAt.HasValue)
                return string.Empty;
            else
                return string.Join("<br />",
                    ip.PreAssignedAt.Value.ToString("dd/MM/yyyy HH:mm"),
                    ip.PreAssignedByOfficeID,
                    ip.PreAssignedForAcademic.Institution,
                    ip.PreAssignedForAcademic.Department);
        }

        protected string GetAssignInfo(InternshipPosition ip)
        {
            if (ip == null || !ip.AssignedAt.HasValue || ip.AssignedToStudent == null)
                return string.Empty;
            else
                return string.Join("<br />",
                    ip.AssignedAt.Value.ToString("dd/MM/yyyy"),
                    ip.AssignedToStudent.ContactName,
                    ip.AssignedToStudent.ID,
                    ip.AssignedToStudent.StudentNumber,
                    ip.AssignedToStudent.AcademicIDNumber);
        }

        protected string GetImplementationInfo(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue || !ip.FundingTypeInt.HasValue)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0:dd/MM/yyyy}<br/>έως<br/>{1:dd/MM/yyyy}<br/>{2}",
                    ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy"),
                    ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy"),
                    ip.FundingType.GetLabel());
            }
        }

        protected string GetTimeAvailable(InternshipPosition ip)
        {
            if (ip == null || ip.InternshipPositionGroup.NoTimeLimit)
                return string.Empty;
            else
            {
                var startDate = ip.InternshipPositionGroup.StartDate.HasValue ? ip.InternshipPositionGroup.StartDate.Value.ToString("dd/MM/yyyy") : "N/A";
                var endDate = ip.InternshipPositionGroup.EndDate.HasValue ? ip.InternshipPositionGroup.EndDate.Value.ToString("dd/MM/yyyy") : "N/A";
                return string.Format("{0} - {1}", startDate, endDate);
            }
        }

        protected string GetFirstPublishedAt(InternshipPosition ip)
        {
            if (ip == null || ip.PositionStatus < enPositionStatus.Available)
                return string.Empty;
            else if (ip.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                return ip.InternshipPositionGroup.CreatedAt.ToString("dd/MM/yyyy");
            else
                return ip.InternshipPositionGroup.FirstPublishedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetPreAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.PreAssignedAt.HasValue)
                return string.Empty;
            else
                return ip.PreAssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.AssignedAt.HasValue || ip.AssignedToStudent == null)
                return string.Empty;
            else
                return ip.AssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationStartDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationEndDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetCompletedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.CompletedAt.HasValue)
                return string.Empty;
            else
                return ip.CompletedAt.Value.ToString("dd/MM/yyyy");
        }

        #endregion
    }
}