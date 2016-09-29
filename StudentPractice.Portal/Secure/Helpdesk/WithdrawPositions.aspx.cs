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

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class WithdrawPositions : BaseEntityPortalPage<object>
    {
        protected bool IsSuperHelpdesk { get; set; }

        protected override void Fill()
        {
            IsSuperHelpdesk = System.Web.Security.Roles.IsUserInRole(RoleNames.SuperHelpdesk);
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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, string.Empty);
            IList<Prefecture> prefectures = CacheManager.Prefectures.GetItems();
            foreach (Prefecture prefecture in prefectures)
            {
                Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, prefecture.ID.ToString());
            }

            Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, string.Empty);
            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }
            base.Render(writer);
        }

        protected void ddlPositionGroupStatus_Init(object sender, EventArgs e)
        {
            ddlPositionGroupStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.UnPublished.GetLabel(), ((int)enPositionGroupStatus.UnPublished).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Published.GetLabel(), ((int)enPositionGroupStatus.Published).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Revoked.GetLabel(), ((int)enPositionGroupStatus.Revoked).ToString()));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtProviderID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtProviderAFM.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtGroupID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
            }

            gvPositionGroups.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositionGroups.PageIndex = 0;
            gvPositionGroups.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositionGroups.DataBind();
        }

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Provider)
                .Include(x => x.PhysicalObjects)
                .Include(x => x.Academics)
                .Include(x => x.Positions)
                .Include(x => x.LogEntries);

            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.PositionCreationType, enPositionCreationType.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ProviderID, providerID);
            }

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, groupID);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Provider.AFM, txtProviderAFM.Text.ToNull());
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

            int positionGroupStatus;
            if (int.TryParse(ddlPositionGroupStatus.SelectedItem.Value, out positionGroupStatus) && positionGroupStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, (enPositionGroupStatus)positionGroupStatus);
            }

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
                        break;
                    case enPositionGroupStatus.Revoked:
                        e.Row.BackColor = Color.Tomato;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void gvPositionGroups_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!IsSuperHelpdesk)
                return;

            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);

            var positions = new InternshipPositionRepository(UnitOfWork).FindByGroupID(groupID,
                x => x.InternshipPositionGroup,
                x => x.InternshipPositionGroup.PhysicalObjects,
                x => x.InternshipPositionGroup.Academics,
                x => x.InternshipPositionGroup.LogEntries,
                x => x.AssignedToStudent);

            if (positions.Count == 0)
                return;
            var group = positions[0].InternshipPositionGroup;

            InternshipPositionGroupTriggerParams triggerParams = new InternshipPositionGroupTriggerParams();
            triggerParams.ExecutionDate = DateTime.Now;
            triggerParams.Username = Thread.CurrentPrincipal.Identity.Name;
            triggerParams.UnitOfWork = UnitOfWork;

            if (action == "unpublish")
            {
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.UnPublish))
                    stateMachine.UnPublish(triggerParams);
            }
            else if (action == "publish")
            {
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Publish))
                    stateMachine.Publish(triggerParams);
            }
            else if (action == "revoke")
            {
                triggerParams.CancellationReason = enCancellationReason.FromHelpdesk;
                triggerParams.Positions = positions;
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Revoke))
                    stateMachine.Revoke(triggerParams);
            }
            else if (action == "rollback")
            {
                triggerParams.Positions = positions;
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.RollbackRevoke))
                    stateMachine.RollbackRevoke(triggerParams);
            }
            else if (action == "rollbackpublish")
            {
                triggerParams.Positions = positions;
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.RollbackRevokeNPublish))
                    stateMachine.RollbackRevokeNPublish(triggerParams);
            }

            UnitOfWork.Commit();
            gvPositionGroups.DataBind();
        }

        protected string GetProviderDetails(InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            string providerDetails = String.Empty;

            providerDetails = string.Format("{0}<br/>{1}<br/>{2}<br/>{3}", group.Provider.Name, group.Provider.ContactName, group.Provider.ContactPhone, group.Provider.ContactEmail);

            return providerDetails;

        }

        protected string GetGroupStatus(InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            return group.PositionGroupStatus.GetLabel();
        }

        protected string GetPositionsStatus(InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            var positions = group.Positions;
            if (positions.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            int unPublishedPositions = positions.Count(x => x.PositionStatus == enPositionStatus.UnPublished);
            if (unPublishedPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.UnPublished.GetLabel(), unPublishedPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.UnPublished.GetLabel(), unPublishedPositions));

            }
            int availablePositions = positions.Count(x => x.PositionStatus == enPositionStatus.Available);
            if (availablePositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.Available.GetLabel(), availablePositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.Available.GetLabel(), availablePositions));
            }
            int preAssignedPositions = positions.Count(x => x.PositionStatus == enPositionStatus.PreAssigned);
            if (preAssignedPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.PreAssigned.GetLabel(), preAssignedPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.PreAssigned.GetLabel(), preAssignedPositions));
            }
            int assignedPositions = positions.Count(x => x.PositionStatus == enPositionStatus.Assigned);
            if (assignedPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.Assigned.GetLabel(), assignedPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.Assigned.GetLabel(), assignedPositions));
            }
            int underImplementationPositions = positions.Count(x => x.PositionStatus == enPositionStatus.UnderImplementation);
            if (underImplementationPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.UnderImplementation.GetLabel(), underImplementationPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.UnderImplementation.GetLabel(), underImplementationPositions));
            }
            int completedPositions = positions.Count(x => x.PositionStatus == enPositionStatus.Completed);
            if (completedPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.Completed.GetLabel(), completedPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.Completed.GetLabel(), completedPositions));
            }
            int canceledPositions = positions.Count(x => x.PositionStatus == enPositionStatus.Canceled);
            if (canceledPositions != 0)
            {
                if (builder.Length == 0)
                    builder.Append(string.Format("{0}: ({1})", enPositionStatus.Canceled.GetLabel(), canceledPositions));
                else
                    builder.Append(string.Format("<br />{0}: ({1})", enPositionStatus.Canceled.GetLabel(), canceledPositions));
            }

            return builder.ToString();
        }

        protected bool CanUnPublish(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            bool cond = new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.UnPublish);
            return cond && IsSuperHelpdesk;
        }

        protected bool CanRevoke(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            bool cond = new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.Revoke);
            return cond && IsSuperHelpdesk;
        }

        protected bool CanPublish(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            bool cond = new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.Publish);
            return cond && IsSuperHelpdesk;
        }

        protected bool CanRollback(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            bool cond = new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.RollbackRevoke);
            return cond && IsSuperHelpdesk;
        }

        protected bool CanRollbackNPublish(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            bool cond = new InternshipPositionGroupStateMachine(group).CanFire(enInternshipPositionGroupTriggers.RollbackRevokeNPublish);
            return cond && IsSuperHelpdesk;
        }
    }
}