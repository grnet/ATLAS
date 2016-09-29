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
using StudentPractice.Utils;
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class InternshipPositions : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        List<Country> _countries;

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            var cIDs = new InternshipPositionGroupRepository(UnitOfWork).GetCountryIDsOfProviderPositions(Entity.ID);

            _countries = cIDs
                .Select(x => CacheManager.Countries.Get(x))
                .OrderBy(x => x.Name)
                .ToList();
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlPhysicalObject_Init(object sender, EventArgs e)
        {
            ddlPhysicalObject.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));

            foreach (var item in CacheManager.PhysicalObjects.GetItems().OrderBy(x => x.Name))
            {
                ddlPhysicalObject.Items.Add(new ListItem(item.Name, item.ID.ToString()));
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

        protected void ddlPositionGroupStatus_Init(object sender, EventArgs e)
        {
            ddlPositionGroupStatus.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.UnPublished.GetLabel(), ((int)enPositionGroupStatus.UnPublished).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Published.GetLabel(), ((int)enPositionGroupStatus.Published).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Revoked.GetLabel(), ((int)enPositionGroupStatus.Revoked).ToString()));
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));

            foreach (var item in CacheManager.Institutions.GetItems().OrderBy(x => x.Name))
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlCreationType_Init(object sender, EventArgs e)
        {
            ddlCreationType.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));
            ddlCreationType.Items.Add(new ListItem(Resources.GlobalProvider.DropDownCreationTypeFYPA, enPositionCreationType.FromProvider.GetValue().ToString()));
            ddlCreationType.Items.Add(new ListItem(Resources.GlobalProvider.DropDownCreationTypeGPA, enPositionCreationType.FromOffice.GetValue().ToString()));
        }

        #endregion

        #region [ Page Methods ]

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, string.Empty);

            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }

            IList<Prefecture> prefectures = CacheManager.Prefectures.GetItems();
            foreach (Prefecture prefecture in prefectures)
            {
                Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, prefecture.ID.ToString());
            }

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
                lblVerificationError.Text = Resources.InternshipPositions.EmailNotVerified;
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.InternshipPositions.AccountNotVerified;
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }

            gvPositionGroupsExport.Columns.FindByName("User").Visible = Entity.IsMasterAccount;
            btnExportUsers.Visible = Entity.IsMasterAccount;

            gvPositionGroups.DataBind();
        }

        #endregion

        #region [ Button Methods ]

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPositionGroup>();
            ParseFilters(criteria, true);

            var positions = new PositionGroups().FindInternshipPositionGroupReport(criteria, 0, int.MaxValue, "ID");

            gvPositionGroupsExport.DataSource = positions;
            gveIntershipPositionGroups.FileName = string.Format("IntershipPositionGroups_FYPA_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipPositionGroups.WriteXlsxToResponse(true);
        }

        protected void btnExportUsers_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPositionGroup>();
            ParseFilters(criteria, false);

            var positions = new PositionGroups().FindInternshipPositionGroupReport(criteria, 0, int.MaxValue, "ID");

            gvPositionGroupsExport.DataSource = positions;
            gveIntershipPositionGroups.FileName = string.Format("IntershipPositionGroupsByUsers_FYPA_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipPositionGroups.WriteXlsxToResponse(true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositionGroups.PageIndex = 0;
            gvPositionGroups.DataBind();
        }

        #endregion

        #region [ Grid Methods ]

        private void ParseFilters(Criteria<InternshipPositionGroup> criteria, bool forMasterAccount)
        {
            if (forMasterAccount)
                criteria.Expression = criteria.Expression.Where(x => x.ProviderID, Entity.ID);
            else
                criteria.Expression = criteria.Expression.Where(x => x.Provider.MasterAccountID, Entity.ID);

            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression.Where(x => x.PositionCreationType, enPositionCreationType.FromOffice).And(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, groupID);
            }

            int physicalObjectID;
            if (int.TryParse(ddlPhysicalObject.SelectedItem.Value, out physicalObjectID) && physicalObjectID > 0)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("(it.PhysicalObjects) OVERLAPS (SELECT VALUE it1 FROM PhysicalObjectSet as it1 WHERE it1.ID = {0} )", physicalObjectID));
            }

            int positionGroupStatus;
            if (int.TryParse(ddlPositionGroupStatus.SelectedItem.Value, out positionGroupStatus) && positionGroupStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, (enPositionGroupStatus)positionGroupStatus);
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

            if (!string.IsNullOrEmpty(txtCity.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CityText, txtCity.Text);
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

            int creationType;
            if (int.TryParse(ddlCreationType.SelectedItem.Value, out creationType) && creationType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionCreationTypeInt, creationType);
            }

            if (!chbxShowRevokedPositions.Checked)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Revoked, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            }
        }

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Positions)
                .Include(x => x.PhysicalObjects)
                .Include(x => x.Academics)
                .Include(x => x.LogEntries);


            ParseFilters(criteria, true);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvPositionGroups_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            InternshipPositionGroup group = (InternshipPositionGroup)gvPositionGroups.GetRow(e.VisibleIndex);

            if (group != null)
            {
                switch (group.PositionGroupStatus)
                {
                    case enPositionGroupStatus.UnPublished:
                        e.Row.BackColor = Color.Gray;
                        break;
                    case enPositionGroupStatus.Published:
                        e.Row.BackColor = Color.LightGreen;
                        if (group.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.LightBlue;
                        break;
                    case enPositionGroupStatus.Revoked:
                        e.Row.BackColor = Color.Tomato;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvPositionGroups_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            try
            {
                var parameters = e.Parameters.Split(':');
                var action = parameters[0].ToLower();
                var groupID = int.Parse(parameters[1]);
                var group = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects, x => x.Academics, x => x.Positions);

                switch (action)
                {
                    case "clone":
                        var newGroup = BusinessHelper.ClonePositionGroup(group);
                        var position = new InternshipPosition();
                        position.InternshipPositionGroup = newGroup;

                        UnitOfWork.MarkAsNew(newGroup);
                        UnitOfWork.Commit();
                        e.Result = ResolveClientUrl(string.Format("~/Secure/InternshipProviders/PositionDetails.aspx?gID={0}&shownote=true", newGroup.ID));

                        break;
                    case "edit":
                        if (group.PhysicalObjects.Count == 0)
                        {
                            e.Result = ResolveClientUrl(string.Format("~/Secure/InternshipProviders/PositionPhysicalObject.aspx?gID={0}", groupID));
                        }
                        else
                        {
                            e.Result = ResolveClientUrl(string.Format("~/Secure/InternshipProviders/PositionAcademics.aspx?gID={0}", groupID));
                        }

                        break;
                }
            }
            catch (Exception)
            {
                e.Result = "ERROR";
            }
        }

        protected void gvPositionGroups_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);

            var group = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID,
                x => x.PhysicalObjects,
                x => x.Academics,
                x => x.Positions,
                x => x.LogEntries);

            InternshipPositionGroupTriggerParams triggerParams = new InternshipPositionGroupTriggerParams();
            triggerParams.ExecutionDate = DateTime.Now;
            triggerParams.Username = Thread.CurrentPrincipal.Identity.Name;
            triggerParams.UnitOfWork = UnitOfWork;

            if (action == "cancel")
            {
                enPositionGroupStatus oldStatus = group.PositionGroupStatus;
                try
                {
                    triggerParams.CancellationReason = enCancellationReason.FromProvider;
                    triggerParams.Positions = new InternshipPositionRepository(UnitOfWork).FindUnPreAssignedInternshipPositions(group.ID, x => x.InternshipPositionGroup, x => x.PreAssignedForAcademic);
                    var stateMachine = new InternshipPositionGroupStateMachine(group);
                    if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Revoke))
                        stateMachine.Revoke(triggerParams);

                    UnitOfWork.Commit();
                    gvPositionGroups.DataBind();
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex, this, ex.Message);
                    group.PositionGroupStatus = oldStatus;
                    UnitOfWork.Commit();
                    gvPositionGroups.DataBind();
                }
            }
            else
            {
                if (new InternshipPositionRepository(UnitOfWork).ExistingPreAssignedInternshipPositions(groupID))
                {
                    throw new InvalidOperationException(Resources.InternshipPositions.ExistingPreAssignedInternshipPositions);
                }
                else
                {
                    var stateMachine = new InternshipPositionGroupStateMachine(group);
                    switch (action)
                    {
                        case "publish":
                            if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Publish))
                                stateMachine.Publish(triggerParams);

                            UnitOfWork.Commit();
                            gvPositionGroups.DataBind();
                            break;
                        case "unpublish":
                            if (stateMachine.CanFire(enInternshipPositionGroupTriggers.UnPublish))
                                stateMachine.UnPublish(triggerParams);

                            UnitOfWork.Commit();
                            gvPositionGroups.DataBind();
                            break;
                        case "delete":
                            triggerParams.Positions = new InternshipPositionRepository(UnitOfWork).FindUnPreAssignedInternshipPositions(group.ID, x => x.InternshipPositionGroup, x => x.PreAssignedForAcademic);
                            if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Delete))
                                stateMachine.Delete(triggerParams);

                            UnitOfWork.Commit();
                            gvPositionGroups.DataBind();
                            break;
                        default:
                            break;
                    }
                }
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
                        case "User":
                            e.TextValue = e.Text = group.Provider.IsMasterAccount ? Resources.ProviderLiterals.MasterAccount : string.Format(Resources.ProviderLiterals.UserAccount, group.Provider.UserName);
                            break;
                        case "Provider":
                            e.TextValue = e.Text = string.Format("{0} ({1})", group.Provider.Name, group.Provider.TradeName);
                            break;
                        case "CreatedAt":
                            e.TextValue = e.Text = group.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                            break;
                        case "FirstPublishedAt":
                            e.TextValue = e.Text = GetFirstPublishedAt(group).Replace("<br />", "\n");
                            break;
                        case "Country":
                            e.TextValue = e.Text = CacheManager.Countries.Get(group.CountryID).Name;
                            break;
                        case "Prefecture":
                            if (group.CountryID == StudentPracticeConstants.GreeceCountryID || group.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = CacheManager.Prefectures.Get(group.PrefectureID.Value).Name;
                            else
                                e.TextValue = e.Text = string.Empty;
                            break;
                        case "City":
                            if (group.CountryID == StudentPracticeConstants.GreeceCountryID || group.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = CacheManager.Cities.Get(group.CityID.Value).Name;
                            else
                                e.TextValue = e.Text = group.CityText;
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
                        case "Institutions":
                            e.TextValue = e.Text = GetInstitutions(group);
                            break;
                        case "Departments":
                            e.TextValue = e.Text = GetDepartments(group);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region [ Helper Methods Get ]

        protected string GetPreAssignedPositions(InternshipPositionGroup group)
        {
            if (group == null)
                return String.Empty;

            if (group.PreAssignedPositions > 0)
                return string.Format("<a href='SelectedPositions.aspx?gID={0}'>{1}</a>", group.ID, group.PreAssignedPositions);
            else
                return group.PreAssignedPositions.ToString();
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
                details += string.IsNullOrEmpty(ip.Supervisor) ? string.Empty : string.Format("{0}: {1}<br />", Resources.InternshipPositions.Supervisor, ip.Supervisor);
                details += string.IsNullOrEmpty(ip.SupervisorEmail) ? string.Empty : string.Format("{0}: {1}<br/>", Resources.InternshipPositions.SupervisorEmail, ip.SupervisorEmail);
                details += string.IsNullOrEmpty(ip.ContactPhone) ? string.Empty : string.Format("{0}: {1}<br />", Resources.InternshipPositions.ContactPhone, ip.ContactPhone);
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
                return ip.FirstPublishedAt.Value.ToString("dd/MM/yyyy");
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
                return Resources.GlobalProvider.AllInstitutions;
            else
                return string.Join(";", ip.Academics.Select(x => x.Institution).Distinct());
        }

        protected string GetDepartments(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.HasValue && ip.IsVisibleToAllAcademics.Value)
                return Resources.GlobalProvider.AllAcademics;
            else
                return string.Join(";", ip.Academics.Select(x => string.Format("{0} ({1})", x.Department, x.Institution)).Distinct());
        }

        #endregion

        #region [ Helper Methods Need ]

        protected bool NeedsEdit(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return group.PhysicalObjects.Count == 0 || !group.IsVisibleToAllAcademics.HasValue;
        }

        #endregion

        #region [ Helper Methods Can ]

        protected bool CanPublishGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.Publish);
        }

        protected bool CanUnPublishGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.UnPublish) && group.PositionCreationType != enPositionCreationType.FromOffice;
        }

        protected bool CanCancelGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.Revoke) && group.PositionCreationType != enPositionCreationType.FromOffice;
        }

        protected bool CanEditGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return group.PositionGroupStatus != enPositionGroupStatus.Revoked && group.PositionCreationType != enPositionCreationType.FromOffice;
        }

        protected bool CanDeleteGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.Delete) && group.PositionCreationType != enPositionCreationType.FromOffice;
        }

        protected bool CanCloneGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return group.PositionCreationType != enPositionCreationType.FromOffice;
        }

        #endregion
    }
}
