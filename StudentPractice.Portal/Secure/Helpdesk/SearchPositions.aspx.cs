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

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchPositions : BaseEntityPortalPage<object>
    {
        #region [ Control Inits ]

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

        protected void ddlCreationType_Init(object sender, EventArgs e)
        {
            ddlCreationType.Items.Add(new ListItem("-- αδιάφορο --", "-1"));
            ddlCreationType.Items.Add(new ListItem("Από ΦΥΠΑ", enPositionCreationType.FromProvider.GetValue().ToString()));
            ddlCreationType.Items.Add(new ListItem("Από ΓΠΑ", enPositionCreationType.FromOffice.GetValue().ToString()));
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
            int studentID;
            if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
            {
                txtStudentID.Text = studentID.ToString();
            }
            gvPositions.DataBind();
        }

        #endregion

        #region [ Button Methods ]

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        #endregion

        #region [ Grid Methods ]

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.InternshipPositionGroup.Academics);

            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, providerID);
            }

            int studentID;
            if (int.TryParse(txtStudentID.Text.ToNull(), out studentID) && studentID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudentID, studentID);
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

            if (!string.IsNullOrEmpty(txtStudentNumber.Text.ToNull()))
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.StudentNumber, txtStudentNumber.Text);

            int creationType;
            if (int.TryParse(ddlCreationType.SelectedItem.Value, out creationType) && creationType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt, creationType);
            }

            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
                orExpression = orExpression.Where(x => x.InternshipPositionGroup.IsVisibleToAllAcademics, true);
                orExpression = orExpression.Or(string.Format("(it.InternshipPositionGroup.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.ID = {0})", academicID));
                criteria.Expression = criteria.Expression.And(orExpression);
            }

            int fundingType;
            if (int.TryParse(ddlFundingType.SelectedItem.Value, out fundingType) && fundingType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.FundingTypeInt, fundingType);
            }

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

        protected void gvPositions_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);
        }

        #endregion

        #region [ Helpder Methods ]

        protected string GetPublishDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;
            if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                return string.Format("Πρώτη: {0:dd/MM/yyyy}<br/>Τελευταία: {1:dd/MM/yyyy}", position.InternshipPositionGroup.CreatedAt, position.InternshipPositionGroup.CreatedAt);
            return string.Format("Πρώτη: {0:dd/MM/yyyy}<br/>Τελευταία: {1:dd/MM/yyyy}", position.InternshipPositionGroup.FirstPublishedAt, position.InternshipPositionGroup.LastPublishedAt);
        }

        protected string GetProviderDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (!string.IsNullOrEmpty(position.InternshipPositionGroup.Provider.TradeName))
            {
                providerDetails = string.Format("{0} <br/>{1} <br/>{2}", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.TradeName, position.InternshipPositionGroup.Provider.AFM);
            }
            else
            {
                providerDetails = string.Format("{0} <br/>{1}", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.AFM);
            }

            return providerDetails;
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
                return string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}<br/>{2}",
                    ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy"),
                    ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy"),
                    ip.FundingType.GetLabel());
            }
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
