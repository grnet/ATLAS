using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;
using System.Xml.Linq;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class AddressInfoInput : BaseEntityUserControl<InternshipProvider>
    {
        #region [ Properties ]

        /// <summary>
        /// Χρησιμοποιείται μόνο όταν θέλουμε να θέσουμε όλα τα πεδία στη φόρμα ReadOnly ανεξάρτητα από το ποιος τα βλέπει και για ποιο λόγο
        /// </summary>
        public bool? ReadOnly { get; set; }

        #endregion

        #region [ Control Inits ]

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            UpdateCountryDropdown();
            UpdateCountryUI();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCountryUI();
        }

        protected void ddlPrefecture_Init(object sender, EventArgs e)
        {
            InitializePrefectureDropDown();
        }

        #endregion

        #region [ Databind Methods ]

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        public override InternshipProvider Fill(InternshipProvider entity)
        {
            if (entity == null)
            {
                entity = new InternshipProvider();
                entity.UsernameFromLDAP = Guid.NewGuid().ToString();
            }

            if (ddlCity.Enabled)
            {
                entity.Address = txtAddress.Text.ToNull();
                entity.ZipCode = txtZipCode.Text.ToNull();
                entity.CityID = ddlCity.GetSelectedInteger();
                entity.PrefectureID = ddlPrefecture.GetSelectedInteger();
                entity.CityText = txtCityText.GetText();
                entity.CountryID = ddlCountry.GetSelectedInteger().Value;
            }
            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            ddlCountry.Enabled = false;
            txtZipCode.Text = Entity.ZipCode;
            txtAddress.Text = Entity.Address;

            ddlCountry.SelectedValue = Entity.CountryID.ToString();
            InitializePrefectureDropDown();
            UpdateCountryUI();

            if (Entity.CountryID != StudentPracticeConstants.GreeceCountryID
                && Entity.CountryID != StudentPracticeConstants.CyprusCountryID)
            {
                txtCityText.Text = Entity.CityText;
            }
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvAddress.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        #endregion

        #region [ Overrides ]

        protected override void OnPreRender(EventArgs e)
        {
            txtAddress.Attributes["onkeyup"] = "Imis.Lib.ToUpper(this)";
            txtZipCode.Attributes["onkeyup"] = "Imis.Lib.ToUpper(this)";

            bool isHelpdesk = Roles.IsUserInRole(BusinessModel.RoleNames.Helpdesk) || Roles.IsUserInRole(BusinessModel.RoleNames.SuperHelpdesk);
            if (Entity != null)
            {
                if (ReadOnly.HasValue)
                    SetReadOnly(ReadOnly.Value);
                else
                {
                    if (Entity.VerificationStatus == enVerificationStatus.NotVerified)
                        SetReadOnly(false);
                    
                    else if (Entity.VerificationStatus == enVerificationStatus.Verified)
                    {
                        if (!isHelpdesk)
                            UpdateUIUser();
                        else                        
                            UpdateUIHelpdesk();
                    }
                    else
                        SetReadOnly(true);
                }

                if (Page.IsPostBack)
                {
                    if (!ddlCity.Enabled && Entity != null && Entity.CityReference.EntityKey != null && Entity.PrefectureReference.EntityKey != null)
                    {

                        int prefectureID = (int)Entity.PrefectureReference.GetKey();
                        int cityID = (int)Entity.CityReference.GetKey();
                        if (prefectureID > 0)
                        {
                            ddlCity.Items.Clear();
                            ddlCity.Items.Add(new ListItem("-- επιλέξτε πόλη --", "-1"));

                            foreach (var item in CacheManager.Prefectures.Get(prefectureID).Cities)
                            {
                                ddlCity.Items.Add(new ListItem(item.Name, item.ID.ToString()));
                            }

                            ddlCity.SelectedValue = cityID.ToString();
                            cddCity.SelectedValue = cityID.ToString();
                        }
                    }
                }
            }

            base.OnPreRender(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }
            base.Render(writer);
        }

        #endregion

        #region [ UI Region ]

        private void UpdateUIUser()
        {
            ddlPrefecture.Enabled =
             rfvPrefecture.Enabled =
             ddlCity.Enabled =
             rfvCity.Enabled =
             cddCity.Enabled = true;
        }

        /// <summary>
        /// O χρήστης είναι Helpdesk και πρέπει να ενημερωθεί το UI κατάλληλα.
        /// </summary>
        private void UpdateUIHelpdesk()
        {
            //Τα θέτουμε όλα ReadOnly
            SetReadOnly(true);

            //Όλα non-editable εκτός από            
            ddlCountry.Enabled =
            ddlPrefecture.Enabled =
            rfvPrefecture.Enabled =
            ddlCity.Enabled =
            rfvCity.Enabled =
            cddCity.Enabled = false;
        }

        private void SetReadOnly(bool isReadOnly)
        {
            foreach (WebControl control in Controls.OfType<WebControl>())
                control.Enabled = !isReadOnly;
        }
        #endregion

        private void UpdateCountryUI()
        {
            Extensions.SaveProviderUserSelectedCountry(ddlCountry.GetSelectedInteger().Value);
            if (ddlCountry.GetSelectedInteger().Value == StudentPracticeConstants.GreeceCountryID)
            {
                InitializePrefectureDropDown();
                mvCountry.SetActiveView(vGreekCity);

                revZipCode.Visible =
                rfvPrefecture.Enabled =
                rfvCity.Enabled = true;

                rfvCityText.Enabled = false;

                litPrefecture.Text = Resources.PositionGroupInput.PrefectureGr;
                litCity.Text = Resources.PositionGroupInput.Kali_CityGr;
            }
            else if (ddlCountry.GetSelectedInteger().Value == StudentPracticeConstants.CyprusCountryID)
            {
                InitializePrefectureDropDown();
                mvCountry.SetActiveView(vGreekCity);

                rfvPrefecture.Enabled =
                rfvCity.Enabled = true;

                revZipCode.Visible =
                rfvCityText.Enabled = false;

                litPrefecture.Text = Resources.PositionGroupInput.PrefectureCy;
                litCity.Text = Resources.PositionGroupInput.Kali_CityCy;
            }
            else
            {
                revZipCode.Visible =
                rfvPrefecture.Enabled =
                rfvCity.Enabled = false;

                
                rfvCityText.Enabled = true;
                mvCountry.SetActiveView(vForeignCity);
            }
        }

        private void UpdateCountryDropdown()
        {
            var items = new List<Country>();
            items.Add(CacheManager.Countries.Get(StudentPracticeConstants.GreeceCountryID));

            if (Config.AllowCyprusRegistration)
                items.Add(CacheManager.Countries.Get(StudentPracticeConstants.CyprusCountryID));

            if (Config.AllowForeignRegistration)
                items.AddRange(CacheManager.Countries.GetItems().Where(x => x.ID != StudentPracticeConstants.GreeceCountryID && x.ID != StudentPracticeConstants.CyprusCountryID));

            ddlCountry.Items.AddRange(items
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());
            ddlCountry.SelectedValue = Context.LoadProvider().CountryID.ToString();
        }

        private void InitializePrefectureDropDown()
        {
            ddlPrefecture.Items.Clear();

            int countryID = ddlCountry.GetSelectedInteger().Value;

            if (countryID == StudentPracticeConstants.GreeceCountryID)
            {
                ddlPrefecture.Items.Add(new ListItem(Resources.PositionGroupInput.PrefectureGrPrompt, ""));
                rfvPrefecture.ErrorMessage = Resources.PositionGroupInput.PrefectureGrRequired;
                rfvCity.ErrorMessage = Resources.PositionGroupInput.Kali_CityGrRequired;
                cddCity.PromptText = Resources.PositionGroupInput.Kali_CityGrPrompt;
            }
            else if (countryID == StudentPracticeConstants.CyprusCountryID)
            {
                ddlPrefecture.Items.Add(new ListItem(Resources.PositionGroupInput.PrefectureCyPrompt, ""));
                rfvPrefecture.ErrorMessage = Resources.PositionGroupInput.PrefectureCyRequired;
                rfvCity.ErrorMessage = Resources.PositionGroupInput.Kali_CityCyRequired;
                cddCity.PromptText = Resources.PositionGroupInput.Kali_CityCyPrompt;
            }

            foreach (var item in CacheManager.Prefectures.GetItems().Where(x => x.CountryID == countryID).OrderBy(x => x.Name))
            {
                ddlPrefecture.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }

            if (Entity != null && Entity.PrefectureID.HasValue)
            {
                ddlPrefecture.SelectedValue = Entity.PrefectureID.Value.ToString();
                cddCity.SelectedValue = Entity.CityID.GetValueOrDefault().ToString();
            }
        }
    }
}