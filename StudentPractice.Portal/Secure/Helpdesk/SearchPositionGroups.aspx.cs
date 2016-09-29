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
    public partial class SearchPositionGroups : BaseEntityPortalPage<object>
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

        protected void ddlPositionGroupStatus_Init(object sender, EventArgs e)
        {
            ddlPositionGroupStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.UnPublished.GetLabel(), ((int)enPositionGroupStatus.UnPublished).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Published.GetLabel(), ((int)enPositionGroupStatus.Published).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Revoked.GetLabel(), ((int)enPositionGroupStatus.Revoked).ToString()));
            ddlPositionGroupStatus.Items.Add(new ListItem(enPositionGroupStatus.Deleted.GetLabel(), ((int)enPositionGroupStatus.Deleted).ToString()));
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.GreeceCountryName, StudentPracticeConstants.GreeceCountryID.ToString()));
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.CyprusCountryName, StudentPracticeConstants.CyprusCountryID.ToString()));
        }

        #endregion

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

                int providerID;
                if (int.TryParse(Request.QueryString["pID"], out providerID) && providerID > 0)
                {
                    txtProviderID.Text = providerID.ToString();
                }
            }

            gvPositionGroups.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositionGroups.PageIndex = 0;
            gvPositionGroups.DataBind();
        }

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Provider).Include(x => x.PhysicalObjects);

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

            int creationType;
            if (int.TryParse(ddlCreationType.SelectedItem.Value, out creationType) && creationType > 0)
            {
                switch (creationType)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.PositionCreationType, enPositionCreationType.FromProvider);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.PositionCreationType, enPositionCreationType.FromOffice);
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
                if (group.PositionCreationType == enPositionCreationType.FromOffice)
                {
                    if (group.PositionGroupStatus == enPositionGroupStatus.Deleted)
                        e.Row.BackColor = Color.BlueViolet;
                    else if (group.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                        e.Row.BackColor = Color.Gray;
                    else
                        e.Row.BackColor = Color.LightBlue;

                }
            }
        }

        protected void gvPositionGroups_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);

            if (action == "undeletegroup")
            {
                var group = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID);
                InternshipPositionGroupTriggerParams triggerParams = new InternshipPositionGroupTriggerParams();
                triggerParams.ExecutionDate = DateTime.Now;
                triggerParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggerParams.UnitOfWork = UnitOfWork;
                var stateMachine = new InternshipPositionGroupStateMachine(group);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.RollbackDelete))
                    stateMachine.RollbackDelete(triggerParams);

                UnitOfWork.Commit();
                gvPositionGroups.DataBind();
            }
        }

        protected string GetPublishDetails(InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;
            if (group.PositionCreationType == enPositionCreationType.FromOffice)
                return string.Format("Πρώτη: {0:dd/MM/yyyy}<br/>Τελευταία: {1:dd/MM/yyyy}", group.CreatedAt, group.CreatedAt);
            return string.Format("Πρώτη: {0:dd/MM/yyyy}<br/>Τελευταία: {1:dd/MM/yyyy}", group.FirstPublishedAt, group.LastPublishedAt);
        }

        protected string GetProviderDetails(InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (!string.IsNullOrEmpty(group.Provider.TradeName))
            {
                providerDetails = string.Format("{0} <br/>{1} <br/>{2}", group.Provider.Name, group.Provider.TradeName, group.Provider.AFM);
            }
            else
            {
                providerDetails = string.Format("{0} <br/>{1}", group.Provider.Name, group.Provider.AFM);
            }

            return providerDetails;
        }

        protected bool CanUnDeleteGroup(InternshipPositionGroup group)
        {
            if (group == null)
                return false;

            return group.PositionGroupStatus == enPositionGroupStatus.Deleted && group.PositionCreationType != enPositionCreationType.FromOffice;
        }
    }
}
