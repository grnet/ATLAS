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

namespace StudentPractice.Portal.Secure.Students
{
    public partial class SearchPositions : BaseEntityPortalPage<Student>
    {     
        #region [ Databind Methods ]

        List<int> _verifiedOfficeIDs;
        List<Country> _countries;

        protected override void Fill()
        {
            Entity = new StudentRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
                      
            _verifiedOfficeIDs = new InternshipOfficeRepository(UnitOfWork).GetVerifiedOfficeIDs(CacheManager.Academics.Get(Entity.AcademicID.Value).InstitutionID, Entity.AcademicID.Value);
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();
            criteria.UsePaging = false;
            criteria.Include(x => x.Provider);

            var expression1 = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            if (_verifiedOfficeIDs.Count > 0)
            {
                expression1 = expression1.Where(string.Format("EXISTS (SELECT VALUE it1 FROM InternshipPositionSet as it1 WHERE it1.GroupID = it.ID AND it1.PreAssignedForAcademicID = {0} AND it1.PositionStatusInt={1} AND it1.PreAssignedByMasterAccountID IN MULTISET ({1}))", Entity.AcademicID, (int)enPositionStatus.PreAssigned, string.Join(",", _verifiedOfficeIDs)));
            }

            var expression2 = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            expression2 = expression2.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            expression2 = expression2.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            expression2 = expression2.Where(string.Format("EXISTS (SELECT VALUE it3 FROM InternshipPositionSet as it3 WHERE it3.GroupID = it.ID AND it3.PositionStatusInt = {0})", (int)enPositionStatus.Available));

            if (_verifiedOfficeIDs.Count > 0)
            {
                expression2 = expression2.Where(string.Format("NOT EXISTS (SELECT VALUE it2 FROM BlockedPositionGroupSet as it2 WHERE it2.GroupID = it.ID AND it2.MasterAccountID IN MULTISET ({0}))", string.Join(",", _verifiedOfficeIDs)));
                criteria.Expression = expression1.Or(expression2);
            }
            else
            {
                criteria.Expression = expression2;
            }

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);
            orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.ID = {0})", Entity.AcademicID.Value));

            criteria.Expression = criteria.Expression.And(orExpression);

            int positionCount;
            var positions = new InternshipPositionGroupRepository(UnitOfWork).FindWithCriteria(criteria, out positionCount);
            _countries = positions.Select(x => CacheManager.Countries.Get(x.CountryID)).Distinct().OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region [ Control Inits ]

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
            if (!Entity.IsContactInfoCompleted)
            {
                Response.Redirect("~/Secure/Students/ContactInfoDetails.aspx");
            }

            if (!Page.IsPostBack)
            {
                txtGroupID.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtTitle.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
            }
        }

        #endregion

        #region [ Button Methods ]

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositionGroups.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositionGroups.PageIndex = 0;
            gvPositionGroups.DataBind();
        }

        #endregion

        #region [ Grid Methods ]

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Provider)
                .Include(x => x.City)
                .Include(x => x.PhysicalObjects);

            criteria.Sort.OrderBy(x => x.IsVisibleToAllAcademics);
            var expression1 = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            if (_verifiedOfficeIDs.Count > 0)
            {
                expression1 = expression1.Where(string.Format("EXISTS (SELECT VALUE it1 FROM InternshipPositionSet as it1 WHERE it1.GroupID = it.ID AND it1.PreAssignedForAcademicID = {0} AND it1.PositionStatusInt={1} AND it1.PreAssignedByMasterAccountID IN MULTISET ({1}))", Entity.AcademicID, (int)enPositionStatus.PreAssigned, string.Join(",", _verifiedOfficeIDs)));
            }

            var expression2 = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            expression2 = expression2.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            expression2 = expression2.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            expression2 = expression2.Where(string.Format("EXISTS (SELECT VALUE it3 FROM InternshipPositionSet as it3 WHERE it3.GroupID = it.ID AND it3.PositionStatusInt = {0})", (int)enPositionStatus.Available));

            if (_verifiedOfficeIDs.Count > 0)
            {
                expression2 = expression2.Where(string.Format("NOT EXISTS (SELECT VALUE it2 FROM BlockedPositionGroupSet as it2 WHERE it2.GroupID = it.ID AND it2.MasterAccountID IN MULTISET ({0}))", string.Join(",", _verifiedOfficeIDs)));
                criteria.Expression = expression1.Or(expression2);
            }
            else
            {
                criteria.Expression = expression2;
            }

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);
            orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it3 FROM AcademicSet as it3 WHERE it3.ID = {0})", Entity.AcademicID.Value));

            criteria.Expression = criteria.Expression.And(orExpression);

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, groupID);
            }

            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Title, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            int physicalObjectID;
            if (int.TryParse(ddlPhysicalObject.SelectedItem.Value, out physicalObjectID) && physicalObjectID > 0)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("(it.PhysicalObjects) OVERLAPS (SELECT VALUE it3 FROM PhysicalObjectSet as it3 WHERE it3.ID = {0} )", physicalObjectID));
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

            if (!string.IsNullOrEmpty(txtProvider.Text))
            {
                var provOrExp = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
                provOrExp = provOrExp.Where(x => x.Provider.Name, txtProvider.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                provOrExp = provOrExp.Or(x => x.Provider.TradeName, txtProvider.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                criteria.Expression = criteria.Expression.And(provOrExp);
            }

            if (!chbxIsVisibleToAllAcademics.Checked)
            {
                criteria.Expression = criteria.Expression.Where(x => x.IsVisibleToAllAcademics, false);
            }

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ Helper Methods ]

        protected string GetProviderDetails(InternshipPositionGroup group)
        {
            if (group == null)
                return String.Empty;

            string providerDetails = String.Empty;

            if (!string.IsNullOrEmpty(group.Supervisor))
            {
                providerDetails = String.Format("{0}<br/>{1}<br/>{2}", group.Provider.Name, group.Supervisor, group.ContactPhone);
            }
            else
            {
                providerDetails = String.Format("{0}<br/>{1}", group.Provider.Name, group.ContactPhone);
            }

            return providerDetails;
        }

        protected bool CanPreAssignPosition(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            return stateMachine.CanFire(enInternshipPositionTriggers.PreAssign);
        }

        #endregion
    }
}
