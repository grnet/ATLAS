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
    public partial class PositionView : BaseEntityUserControl<InternshipPosition>
    {
        public List<Academic> UserAssociatedAcademics { get; set; }
        public bool HideAcademics
        {
            get { return !tbAcademics.Visible; }
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
            if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Helpdesk) ||
                Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.SuperHelpdesk) ||
                Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.SystemAdministrator))
            {
                lblPositionID.Text = Entity.ID.ToString();
            }
            else
            {
                trPositionID.Visible = false;
            }
            lblTitle.Text = Entity.InternshipPositionGroup.Title;
            txtDescription.Text = Entity.InternshipPositionGroup.Description;
            lblDuration.Text = Entity.InternshipPositionGroup.Duration == 0 ? string.Empty : Entity.InternshipPositionGroup.Duration.ToString();

            if (Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID)
            {
                lblCountry.Text = StudentPracticeConstants.GreeceCountryName;
            }
            else if (Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                lblCountry.Text = StudentPracticeConstants.CyprusCountryName;
                ltrPrefecture.Text = "Επαρχία";
                ltrCity.Text = "Δήμος";
            }
            else
            {
                lblCountry.Text = CacheManager.Countries.Get(Entity.InternshipPositionGroup.CountryID).Name;
            }

            trPrefecture.Visible = Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID || Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID;

            if (Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID || Entity.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                lblPrefecture.Text = Entity.InternshipPositionGroup.PrefectureID.HasValue 
                    ? CacheManager.Prefectures.Get(Entity.InternshipPositionGroup.PrefectureID.GetValueOrDefault()).Name
                    : string.Empty;
                lblCity.Text = Entity.InternshipPositionGroup.CityID.HasValue 
                    ? CacheManager.Cities.Get(Entity.InternshipPositionGroup.CityID.GetValueOrDefault()).Name
                    : string.Empty;
            }
            else
            {
                lblCity.Text = Entity.InternshipPositionGroup.CityText;
            }

            if (Entity.InternshipPositionGroup.NoTimeLimit)
            {
                trStartDate.Visible = false;
                trEndDate.Visible = false;

                lblNoTimeLimit.Text = "Χωρίς χρονικό περιορισμό";
            }
            else
            {
                trStartDate.Visible = true;
                trEndDate.Visible = true;

                lblNoTimeLimit.Text = "Με χρονικό περιορισμό";
                lblStartDate.Text = Entity.InternshipPositionGroup.StartDate.HasValue ? Entity.InternshipPositionGroup.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                lblEndDate.Text = Entity.InternshipPositionGroup.EndDate.HasValue ? Entity.InternshipPositionGroup.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            }

            lblPositionType.Text = Entity.InternshipPositionGroup.PositionTypeInt == 0 ? string.Empty : Entity.InternshipPositionGroup.PositionType.GetLabel();
            if (!string.IsNullOrEmpty(Entity.InternshipPositionGroup.Supervisor))
            {
                trSupervisor.Visible = true;
                lblSupervisor.Text = Entity.InternshipPositionGroup.Supervisor;
            }

            if (!string.IsNullOrEmpty(Entity.InternshipPositionGroup.SupervisorEmail))
            {
                trSupervisorEmail.Visible = true;
                lblSupervisorEmail.Text = Entity.InternshipPositionGroup.SupervisorEmail;
            }
            lblContactPhone.Text = Entity.InternshipPositionGroup.ContactPhone;

            //Γνωστικό Αντικείμενο Θέσης
            gvPhysicalObjects.DataSource = Entity.InternshipPositionGroup.PhysicalObjects;
            gvPhysicalObjects.DataBind();

            //Σχολές/Τμήματα
            if (Entity.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                mvAcademics.SetActiveView(vVisibleToAllAcademics);
            }
            else
            {
                mvAcademics.SetActiveView(vVisibleToCertainAcademics);

                if (UserAssociatedAcademics != null)
                {
                    var visibleAcademics = new List<Academic>();

                    foreach (var academic in Entity.InternshipPositionGroup.Academics)
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
                    gvAcademics.DataSource = Entity.InternshipPositionGroup.Academics;
                }
                gvAcademics.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}