using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls
{
    public partial class PositionGroupView : BaseEntityUserControl<InternshipPositionGroup>
    {
        public List<Academic> UserAssociatedAcademics { get; set; }
        public bool HideAcademics
        {
            set
            {
                tbAcademics.Visible = !value;
            }
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Γενικά Στοιχεία Θέσης Πρακτικής Άσκησης
            lblTitle.Text = Entity.Title;
            if (Roles.IsUserInRole(RoleNames.MasterOffice) || Roles.IsUserInRole(RoleNames.OfficeUser))
            {
                gvAcademics.Columns["PositionRules"].Visible = false;
            }

            if (Roles.IsUserInRole(RoleNames.MasterOffice) || Roles.IsUserInRole(RoleNames.OfficeUser) || Roles.IsUserInRole(RoleNames.Student))
            {
                trTotalPositions.Visible = false;
                trPreAssignedPositions.Visible = false;
            }
            else
            {
                lblTotalPositions.Text = Entity.TotalPositions.ToString();
                lblPreAssignedPositions.Text = Entity.PreAssignedPositions.ToString();
            }

            txtDescription.Text = Entity.Description;
            lblDuration.Text = Entity.Duration == 0 ? string.Empty : Entity.Duration.ToString();

            if (Entity.CountryID == StudentPracticeConstants.GreeceCountryID)
            {
                litPrefecture.Text = Resources.PositionGroupInput.PrefectureGr;
                litCity.Text = Resources.PositionGroupInput.Kali_CityGr;
                lblCountry.Text = StudentPracticeConstants.GreeceCountryName;
            }
            else if (Entity.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                litPrefecture.Text = Resources.PositionGroupInput.PrefectureCy;
                litCity.Text = Resources.PositionGroupInput.Kali_CityCy;
                lblCountry.Text = StudentPracticeConstants.CyprusCountryName;
            }
            else
            {
                lblCountry.Text = CacheManager.Countries.Get(Entity.CountryID).Name;
            }

            trPrefecture.Visible = Entity.CountryID == StudentPracticeConstants.GreeceCountryID || Entity.CountryID == StudentPracticeConstants.CyprusCountryID;

            if (Entity.CountryID == StudentPracticeConstants.GreeceCountryID || Entity.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                lblPrefecture.Text = Entity.PrefectureID.HasValue
                    ? CacheManager.Prefectures.Get(Entity.PrefectureID.GetValueOrDefault()).Name
                    : string.Empty;
                lblCity.Text = Entity.CityID.HasValue
                    ? CacheManager.Cities.Get(Entity.CityID.GetValueOrDefault()).Name
                    : string.Empty;
            }
            else
            {
                lblCity.Text = Entity.CityText;
            }

            if (Entity.NoTimeLimit)
            {
                trStartDate.Visible = false;
                trEndDate.Visible = false;

                lblNoTimeLimit.Text = Resources.PositionGroupInput.TimeLimitWithout;
            }
            else
            {
                trStartDate.Visible = true;
                trEndDate.Visible = true;

                lblNoTimeLimit.Text = Resources.PositionGroupInput.TimeLimitWith;
                lblStartDate.Text = Entity.StartDate.HasValue ? Entity.StartDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
                lblEndDate.Text = Entity.EndDate.HasValue ? Entity.EndDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
            }

            lblPositionType.Text = Entity.PositionTypeInt == 0 ? string.Empty : Entity.PositionType.GetLabel();
            if (!string.IsNullOrEmpty(Entity.Supervisor))
            {
                trSupervisor.Visible = true;
                lblSupervisor.Text = Entity.Supervisor;
            }

            if (!string.IsNullOrEmpty(Entity.SupervisorEmail))
            {
                trSupervisorEmail.Visible = true;
                lblSupervisorEmail.Text = Entity.SupervisorEmail;
            }
            lblContactPhone.Text = Entity.ContactPhone;

            //Γνωστικό Αντικείμενο Θέσης
            gvPhysicalObjects.DataSource = Entity.PhysicalObjects;
            gvPhysicalObjects.DataBind();

            if (Entity.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                mvAcademics.SetActiveView(vVisibleToAllAcademics);
            }
            else
            {
                mvAcademics.SetActiveView(vVisibleToCertainAcademics);

                if (UserAssociatedAcademics != null)
                {
                    var visibleAcademics = new List<Academic>();

                    foreach (var academic in Entity.Academics)
                    {
                        if (UserAssociatedAcademics.Select(x => x.ID).Contains(academic.ID))
                        {
                            visibleAcademics.Add(academic);
                        }
                    }

                    gvAcademics.DataSource = visibleAcademics;
                }
                else
                {
                    gvAcademics.DataSource = Entity.Academics;
                }

                gvAcademics.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetAcademicRulesLink(Academic academic)
        {
            if (academic == null || string.IsNullOrEmpty(academic.PositionRules))
                return string.Empty;

            return string.Format("<a href='javascript:void(0);' class='btn-academicRules' data-aid='{0}'><img src='/_img/iconInformation.png' alt='{1}' /></a>", academic.ID, Resources.Grid.GridCaption_Rules);
        }
    }
}