using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel.Flow;
using System.Threading;
using System.Text;
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class SelectedPositions : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        List<Institution> _academics;
        List<Country> _countries;

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();
            criteria.UsePaging = false;
            criteria.Include(x => x.InternshipPositionGroup)
                    .Include(x => x.InternshipPositionGroup.Provider)
                    .Include(x => x.PreAssignedForAcademic);

            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.PreAssigned, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
            criteria.Expression = criteria.Expression.IsNotNull(x => x.PreAssignedByMasterAccountID);


            var expressionA = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var expressionB = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;

            expressionA = expressionA.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            expressionB = expressionB.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice).And(x => x.PositionStatus, enPositionStatus.Completed);
            expressionA = expressionA.Or(expressionB);

            criteria.Expression = criteria.Expression.And(expressionA);

            int positionCount;
            var positions = new InternshipPositionRepository(UnitOfWork).FindWithCriteria(criteria, out positionCount);

            _academics = positions
                .Where(x => x.PreAssignedForAcademicID.HasValue)
                .Select(x => CacheManager.Institutions.Get(x.PreAssignedForAcademic.InstitutionID))
                .Distinct()
                .OrderBy(x => x.Name)
                .ToList();

            _countries = positions
                .Where(x => x.InternshipPositionGroup.CountryID != 0)
                .Select(x => CacheManager.Countries.Get(x.InternshipPositionGroup.CountryID))
                .Distinct()
                .OrderBy(x => x.Name)
                .ToList();
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlPhysicalObject_Init(object sender, EventArgs e)
        {
            ddlPhysicalObject.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));

            foreach (var item in CacheManager.PhysicalObjects.GetItems())
            {
                ddlPhysicalObject.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPositionStatus_Init(object sender, EventArgs e)
        {
            ddlPositionStatus.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));

            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.PreAssigned.GetLabel(), ((int)enPositionStatus.PreAssigned).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Assigned.GetLabel(), ((int)enPositionStatus.Assigned).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.UnderImplementation.GetLabel(), ((int)enPositionStatus.UnderImplementation).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Completed.GetLabel(), ((int)enPositionStatus.Completed).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Canceled.GetLabel(), ((int)enPositionStatus.Canceled).ToString()));
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));
            foreach (var item in _academics)
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));
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
            IList<Academic> academics = CacheManager.Academics.GetItems().Where(x => x.IsActive).ToList();
            foreach (Academic academic in academics)
            {
                Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, academic.ID.ToString());
            }

            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.SelectedPositions.NotEmailVerified;
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.SelectedPositions.NotVerified;
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }

            gvPositionsExport.Columns.FindByName("User").Visible = Entity.IsMasterAccount;
            btnExportUsers.Visible = Entity.IsMasterAccount;

            if (!Page.IsPostBack)
            {
                int gID = -1;
                if (int.TryParse(Request.QueryString["gID"], out gID) && gID > 0)
                    txtGroupID.Text = gID.ToString();

                txtGroupID.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtTitle.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtFirstName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtLastName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
            }
        }

        #endregion

        #region [ Button Methods ]

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositions.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPosition>();
            ParseFilters(criteria, true);

            var positions = new Positions().FindInternshipPositionReport(criteria, 0, int.MaxValue, "InternshipPositionGroup.ID");

            gvPositionsExport.DataSource = positions;
            gveIntershipPositions.FileName = string.Format("IntershipPositions_FYPA_{0}", DateTime.Now.ToString("yyyyMMdd"));

            gveIntershipPositions.WriteXlsxToResponse();
        }

        protected void btnExportUsers_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPosition>();
            ParseFilters(criteria, false);

            var positions = new Positions().FindInternshipPositionReport(criteria, 0, int.MaxValue, "InternshipPositionGroup.ID");

            gvPositionsExport.DataSource = positions;
            gveIntershipPositions.FileName = string.Format("IntershipPositionsByUsers_FYPA_{0}", DateTime.Now.ToString("yyyyMMdd"));

            gveIntershipPositions.WriteXlsxToResponse(true);
        }

        #endregion

        #region [ Grid Methods ]

        private void ParseFilters(Criteria<InternshipPosition> criteria, bool forMasterAccount)
        {
            if (forMasterAccount)
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, Entity.ID);
            else
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.Provider.MasterAccountID, Entity.ID);

            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.PreAssigned, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
            criteria.Expression = criteria.Expression.IsNotNull(x => x.PreAssignedByMasterAccountID);

            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice)
                .And(x => x.PositionStatus, enPositionStatus.Completed)
                .And(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Published);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);

            bool specificCriteriaEntered = false;

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ID, groupID);
            }

            int positionID;
            if (int.TryParse(txtPositionID.Text.ToNull(), out positionID) && positionID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, positionID);
            }

            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                var orTitleExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;

                orTitleExpression = orTitleExpression.Where(x => x.InternshipPositionGroup.Title, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                orTitleExpression = orTitleExpression.Or(x => x.InternshipPositionGroup.Description, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteria.Expression = criteria.Expression.And(orTitleExpression);
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

            if (!string.IsNullOrEmpty(txtCity.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CityText, txtCity.Text);
            }

            int positionStatus;
            if (int.TryParse(ddlPositionStatus.SelectedItem.Value, out positionStatus) && positionStatus > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, positionStatus);

                specificCriteriaEntered = true;
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccount.InstitutionID, institutionID);
            }

            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedForAcademicID, academicID);
            }

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                specificCriteriaEntered = true;
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                specificCriteriaEntered = true;
            }

            if (!specificCriteriaEntered && chbxHideCompletedPositions.Checked)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Completed, Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.InternshipPositionGroup.Prefecture)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.PreAssignedByMasterAccount.Academics)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent);

            ParseFilters(criteria, true);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            InternshipPosition position = gvPositions.GetRow(e.VisibleIndex) as InternshipPosition;

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
                        case "User":
                            e.TextValue = e.Text = position.InternshipPositionGroup.Provider.IsMasterAccount ? Resources.ProviderLiterals.MasterAccount : string.Format(Resources.ProviderLiterals.UserAccount, position.InternshipPositionGroup.Provider.UserName);
                            break;
                        case "Provider":
                            e.TextValue = e.Text = string.Format("{0} ({1})", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.TradeName);
                            break;
                        case "Country":
                            e.TextValue = e.Text = CacheManager.Countries.Get(position.InternshipPositionGroup.CountryID).Name;
                            break;
                        case "Prefecture":
                            if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID ||
                                position.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = CacheManager.Prefectures.Get(position.InternshipPositionGroup.PrefectureID.Value).Name;
                            else
                                e.TextValue = e.Text = string.Empty;
                            break;
                        case "City":
                            if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID ||
                               position.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = CacheManager.Cities.Get(position.InternshipPositionGroup.CityID.Value).Name;
                            else
                                e.TextValue = e.Text = position.InternshipPositionGroup.CityText;
                            break;
                        case "TimeAvailable":
                            e.TextValue = e.Text = GetTimeAvailable(position);
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PositionType.GetLabel();
                            break;
                        case "PositionStatus":
                            e.TextValue = e.Text = GetPositionStatus(position);
                            break;
                        case "PreAssignedForAcademic.Institution":
                            e.TextValue = e.Text = (position.PreAssignedForAcademic == null ? string.Empty : position.PreAssignedForAcademic.Institution);
                            break;
                        case "PreAssignedForAcademic.Department":
                            e.TextValue = e.Text = (position.PreAssignedForAcademic == null ? string.Empty : position.PreAssignedForAcademic.Department);
                            break;
                        case "PreAssignedByMasterAccount.ContactName":
                            e.TextValue = e.Text = (position.PreAssignedByMasterAccount == null ? string.Empty : position.PreAssignedByMasterAccount.ContactName);
                            break;
                        case "PreAssignedByMasterAccount.ContactPhone":
                            e.TextValue = e.Text = (position.PreAssignedByMasterAccount == null ? string.Empty : position.PreAssignedByMasterAccount.ContactPhone);
                            break;
                        case "PreAssignedByMasterAccount.ContactEmail":
                            e.TextValue = e.Text = (position.PreAssignedByMasterAccount == null ? string.Empty : position.PreAssignedByMasterAccount.ContactEmail);
                            break;
                        case "AssignedToStudent.ContactName":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.ContactName);
                            break;
                        case "AssignedToStudent.ContactMobilePhone":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.ContactMobilePhone);
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
                        //ToFix: FundingType
                        //case "FundingType":
                        //    if (position.FundingTypeInt.HasValue)
                        //    {
                        //        e.TextValue = e.Text = position.FundingType.GetLabel();
                        //    }
                        //    break;
                        case "PhysicalObjects":
                            e.TextValue = e.Text = position.GetPhysicalObjectDetails().Replace("<br />", ";");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region [ Helper Methods ]

        protected string GetOfficeDetails(InternshipPosition position)
        {
            if (position == null)
                return String.Empty;

            string officeDetails = String.Empty;

            var office = position.PreAssignedByMasterAccount;

            if (office != null)
            {
                var institution = CacheManager.Institutions.Get(office.InstitutionID.Value);

                switch (office.OfficeType)
                {
                    case enOfficeType.None:
                        officeDetails = string.Format("{0}: {1}<br/>{2}: <span style='color: Red'>-</span>",
                            Resources.GlobalProvider.Institustion,
                            institution.Name,
                            Resources.GlobalProvider.Departments);
                        break;
                    case enOfficeType.Institutional:
                        if (office.CanViewAllAcademics.GetValueOrDefault())
                            officeDetails = string.Format("{0}: {1}", Resources.GlobalProvider.Institustion, institution.Name);
                        else
                            officeDetails = string.Format("{0}: {1}<br/>{2}: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={3}\",\"{4}\", null, 800, 610)'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>",
                                Resources.GlobalProvider.Institustion,
                                institution.Name,
                                Resources.GlobalProvider.Departments,
                                office.ID,
                                Resources.SelectedPositions.ShowAcademics);
                        break;
                    case enOfficeType.Departmental:
                        var academic = office.Academics.ToList()[0];

                        officeDetails = string.Format("{0}: {1}<br/>{2}: {3}",
                            Resources.GlobalProvider.Institustion,
                            institution.Name,
                            Resources.GlobalProvider.Department,
                            academic.Department);
                        break;
                    case enOfficeType.MultipleDepartmental:
                        officeDetails = string.Format("{0}: {1}<br/>{2}: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={3}\",\"{4}\", null, 800, 610)'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>",
                            Resources.GlobalProvider.Institustion,
                            institution.Name,
                            Resources.GlobalProvider.Departments,
                            office.ID,
                            Resources.SelectedPositions.ShowAcademics);
                        break;
                    default:
                        break;
                }

                officeDetails += string.Format("<br/>{0}<br/>{1}<br/>{2}", office.ContactName, office.ContactPhone, office.ContactEmail);
            }

            return officeDetails;
        }

        protected string GetStudentDetails(InternshipPosition position)
        {
            if (position == null)
                return String.Empty;

            string studentDetails = String.Empty;

            Student student = position.AssignedToStudent;

            if (student != null)
            {
                studentDetails = String.Format("{0}<br/><br/><b>{1} {2}<br/>{3}</b>",
                    CacheManager.Academics.Get(position.PreAssignedForAcademicID.Value).Department,
                    student.GreekFirstName,
                    student.GreekLastName,
                    student.ContactMobilePhone);
            }
            else
            {
                studentDetails = String.Format("{0}", CacheManager.Academics.Get(position.PreAssignedForAcademicID.Value).Department);
            }

            return studentDetails;
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else if (ip.PositionStatus == enPositionStatus.Canceled && ip.CancellationReason > enCancellationReason.FromOffice)
                return Resources.SelectedPositions.Revoked;
            else
                return ip.PositionStatus.GetLabel();
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

        #endregion
    }
}
