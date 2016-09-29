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
using System.Web.Script.Serialization;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class SearchPositions : BaseEntityPortalPage<InternshipOffice>
    {
        List<InternshipProvider> _providers;
        List<Country> _coutries;

        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();

            if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
            {
                return;
            }

            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.UsePaging = false;

            criteria.Include(x => x.Provider);

            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            criteria.Expression = criteria.Expression.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);

            if (Entity.IsMasterAccount)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", Entity.ID));
            }
            else
            {
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", Entity.MasterAccountID));
            }

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;

            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);

            if (Entity.CanViewAllAcademics.GetValueOrDefault())
            {
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value).Select(x => x.ID))));
            }
            else
            {
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", Entity.Academics.Select(x => x.ID))));
            }

            criteria.Expression = criteria.Expression.And(orExpression);

            int positionCount;
            var positions = new InternshipPositionGroupRepository(UnitOfWork).FindWithCriteria(criteria, out positionCount);
            _providers = positions.Select(x => x.Provider).Distinct().OrderBy(x => x.Name).ToList();
            _coutries = positions.Select(x => CacheManager.Countries.Get(x.CountryID)).Distinct().OrderBy(x => x.Name).ToList();


        }

        #endregion

        #region [ Control Inits ]

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            foreach (var item in _coutries)
            {
                if (item.ID == StudentPracticeConstants.GreeceCountryID || item.ID == StudentPracticeConstants.CyprusCountryID)
                    ddlCountry.Items.Insert(1, new ListItem(item.Name, item.ID.ToString()));
                else
                    ddlCountry.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPhysicalObject_Init(object sender, EventArgs e)
        {
            ddlPhysicalObject.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.PhysicalObjects.GetItems().OrderBy(x => x.NameInGreek))
            {
                ddlPhysicalObject.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            ddlDepartment.Items.Add(new ListItem("-- αδιάφορο --", ""));

            List<Academic> academics;

            if (Entity.CanViewAllAcademics.GetValueOrDefault())
            {
                academics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value).OrderBy(x => x.Department).ToList();
            }
            else
            {
                academics = Entity.Academics.OrderBy(x => x.Department).ToList();
            }

            foreach (var item in academics)
            {
                ddlDepartment.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        #endregion

        #region [ Page Methods ]

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "providers", string.Format("var _providers = {0};", new JavaScriptSerializer().Serialize(_providers.Select(x => x.Name).ToList())), true);


            Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, string.Empty);

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
            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
            {
                Response.Redirect("OfficeDetails.aspx");
            }

            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τις θέσεις πρακτικής άσκησης, γιατί δεν έχετε ενεργοποιήσει το e-mail σας.";
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τις θέσεις πρακτικής άσκησης, γιατί δεν έχει πιστοποιηθεί ο λογαριασμός σας.<br/>Παρακαλούμε εκτυπώστε τη Βεβαίωση Συμμετοχής και αποστείλτε τη με ΦΑΞ στο Γραφείο Αρωγής για να πιστοποιηθεί.";
            }
            else
            {
                if (!Config.PreAssignmentAllowed)
                {
                    mvAccount.SetActiveView(vAccountNotVerified);
                    lblVerificationError.Text = Config.PreAssignmentMessage;
                }
                else
                {
                    mvAccount.SetActiveView(vAccountVerified);
                }
            }

            if (!Page.IsPostBack)
            {
                txtGroupID.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtTitle.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
            }

            gvPositionGroups.DataBind();
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPositionGroup>();
            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            ParseFilters(criteria);

            var positions = new PositionGroups().FindInternshipPositionGroupReport(criteria, 0, int.MaxValue, "ID");

            gvPositionGroupsExport.DataSource = positions;
            gveIntershipPositionGroups.FileName = string.Format("IntershipPositionGroups_GPA_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveIntershipPositionGroups.WriteXlsxToResponse(true);
        }

        #endregion

        #region [ Grid Methods ]

        protected void ParseFilters(Criteria<InternshipPositionGroup> criteria)
        {
            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            criteria.Expression = criteria.Expression.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);

            if (Entity.IsMasterAccount)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", Entity.ID));
            }
            else
            {
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", Entity.MasterAccountID));
            }

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;

            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);

            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID = {0})", academicID));
            }
            else
            {
                if (Entity.CanViewAllAcademics.GetValueOrDefault())
                {
                    orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value).Select(x => x.ID))));
                }
                else
                {
                    orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", Entity.Academics.Select(x => x.ID))));
                }
            }

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

            if (!string.IsNullOrEmpty(txtProvider.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Provider.Name, txtProvider.Text, Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Provider.AFM, txtProviderAFM.Text);
            }

            int firstPublishedAt;
            if (int.TryParse(ddlFirstPublishedAt.SelectedItem.Value, out firstPublishedAt) && firstPublishedAt > 0)
            {
                switch (firstPublishedAt)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.Date.AddDays(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.Date.AddDays(-7), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                    case 3:
                        criteria.Expression = criteria.Expression.Where(x => x.FirstPublishedAt, DateTime.Now.Date.AddDays(-31), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                }
            }

            if (!chbxIsVisibleToAllAcademics.Checked)
            {
                criteria.Expression = criteria.Expression.Where(x => x.IsVisibleToAllAcademics, false);
            }
        }

        protected void odsPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();

            criteria.Include(x => x.Provider)
                .Include(x => x.City)
                .Include(x => x.PhysicalObjects);

            criteria.Sort.OrderBy(x => x.IsVisibleToAllAcademics);
            ParseFilters(criteria);

            e.InputParameters["criteria"] = criteria;
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
                        case "PhysicalObjects":
                            e.TextValue = e.Text = group.GetPhysicalObjectDetails().Replace("<br />", ";");
                            break;
                        case "Country":
                            if (group.CountryID == StudentPracticeConstants.GreeceCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.GreeceCountryName;
                            else if (group.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.CyprusCountryName;
                            else
                                e.TextValue = e.Text = "Άλλη";
                            break;
                        case "ProviderType":
                            e.TextValue = e.Text = group.Provider.ProviderType.GetLabel();
                            break;
                        case "Prefecture":
                            e.TextValue = e.Text = group.PrefectureID.HasValue ? CacheManager.Prefectures.Get(group.PrefectureID.Value).Name : string.Empty;
                            break;
                        case "City":
                            e.TextValue = e.Text = group.CityID.HasValue ? CacheManager.Cities.Get(group.CityID.Value).Name : string.Empty;
                            break;
                        case "FirstPublishedAt":
                            e.TextValue = e.Text = GetFirstPublishedAt(group).Replace("<br />", "\n");
                            break;
                        case "TimeAvailable":
                            e.TextValue = e.Text = GetTimeAvailable(group);
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = group.PositionType.GetLabel();
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

        #region [ Helper Methods ]
        
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
                return ip.FirstPublishedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetDepartments(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.HasValue && ip.IsVisibleToAllAcademics.Value)
                return "Όλα";
            else
                return string.Join(";", ip.Academics.Where(x => Entity.Academics.Select(y => y.ID).Contains(x.ID)).Select(x => string.Format("{0} ({1})", x.Department, x.Institution)).Distinct());
        }
        #endregion

    }
}
