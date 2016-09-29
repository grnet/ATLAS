using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;

namespace StudentPractice.Portal.UserControls.FinishedInternshipPositionControls.InputControls
{
    public partial class FinishedPositionGroupInput : BaseEntityUserControl<InternshipPositionGroup>
    {
        public int CountryID { get; set; }

        #region [ Control Inits ]

        protected void ddlPositionType_Init(object sender, EventArgs e)
        {
            ddlPositionType.Items.Add(new ListItem("-- επιλέξτε είδος θέσης --", ""));

            foreach (enPositionType item in Enum.GetValues(typeof(enPositionType)))
            {
                ddlPositionType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem(CacheManager.Countries.Get(StudentPracticeConstants.GreeceCountryID).Name, StudentPracticeConstants.GreeceCountryID.ToString()));
            ddlCountry.Items.Add(new ListItem(CacheManager.Countries.Get(StudentPracticeConstants.CyprusCountryID).Name, StudentPracticeConstants.CyprusCountryID.ToString()));

            ddlCountry.Items.AddRange(CacheManager.Countries.GetItems()
                .Where(x => x.ID != StudentPracticeConstants.GreeceCountryID && x.ID != StudentPracticeConstants.CyprusCountryID)
                .OrderBy(x => x.NameInGreek)
                .Select(x => new ListItem(x.NameInGreek, x.ID.ToString()))
                .ToArray());
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

        #region [ Page Methods ]

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }
            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtSupervisor.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            if (Entity != null && !Page.IsPostBack)
            {
                ddlCountry.SelectedValue = CountryID.ToString();
                UpdateCountryUI();
            }
        }

        #endregion

        #region [ Databind Methods ]

        public override InternshipPositionGroup Fill(InternshipPositionGroup entity)
        {
            if (entity == null)
                entity = new InternshipPositionGroup();

            //Γενικά Στοιχεία Θέσης Πρακτικής Άσκησης
            entity.Title = txtTitle.Text.ToNull();
            entity.Description = txtDescription.Text.ToNull();
            entity.CityID = ddlCity.GetSelectedInteger();
            entity.PrefectureID = ddlPrefecture.GetSelectedInteger();
            entity.CityText = txtCityText.GetText();
            entity.CountryID = ddlCountry.GetSelectedInteger().Value;

            entity.PositionTypeInt = ddlPositionType.GetSelectedInteger().Value;
            entity.Supervisor = txtSupervisor.GetText();
            entity.SupervisorEmail = txtSupervisorEmail.GetText();
            entity.ContactPhone = txtContactPhone.GetText();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
            {
                return;
            }

            //Γενικά Στοιχεία Θέσης Πρακτικής Άσκησης
            txtTitle.Text = Entity.Title;
            txtDescription.Text = Entity.Description;

            CountryID = Entity.CountryID;
            ddlCountry.SelectedValue = Entity.CountryID.ToString();
            InitializePrefectureDropDown();
            UpdateCountryUI();


            if (Entity.CountryID != StudentPracticeConstants.GreeceCountryID
                && Entity.CountryID != StudentPracticeConstants.CyprusCountryID)
            {
                txtCityText.Text = Entity.CityText;
            }

            ddlPositionType.SelectedValue = Entity.PositionTypeInt.ToString();
            txtSupervisor.Text = Entity.Supervisor;
            txtSupervisorEmail.Text = Entity.SupervisorEmail;
            txtContactPhone.Text = Entity.ContactPhone;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvTitle.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        #endregion

        #region [ UI Region ]

        private void SetReadOnly(bool isReadOnly)
        {
            bool isEnabled = !isReadOnly;
            foreach (WebControl c in Controls.OfType<WebControl>())
                c.Enabled = isEnabled;
        }

        /// <summary>
        /// Χρησιμοποιείται μόνο όταν θέλουμε να θέσουμε όλα τα πεδία στη φόρμα ReadOnly ανεξάρτητα από το ποιος τα βλέπει και για ποιο λόγο
        /// </summary>
        public bool? ReadOnly { get; set; }

        #endregion

        #region [ Helper Methods ]

        private void UpdateCountryUI()
        {
            if (ddlCountry.GetSelectedInteger().Value == StudentPracticeConstants.GreeceCountryID)
            {
                InitializePrefectureDropDown();
                mvCountry.SetActiveView(vGreekCity);
                revContactPhone.ValidationExpression = "^(2[0-9]{9})|(69[0-9]{8})$";
                revContactPhone.ErrorMessage = Resources.PositionGroupInput.ContactPhoneGrRegex;
                revContactPhoneTip.Attributes.Add("title", Resources.PositionGroupInput.ContactPhoneGrRegex);
                revContactPhone.Enabled = true;
                txtContactPhone.MaxLength = 10;
                txtContactPhone.Attributes.Add("title", Resources.PositionInput.ContactPhone);

                litPrefecture.Text = Resources.PositionGroupInput.PrefectureGr;
                litCity.Text = Resources.PositionGroupInput.Kali_CityGr;
            }
            else if (ddlCountry.GetSelectedInteger().Value == StudentPracticeConstants.CyprusCountryID)
            {
                InitializePrefectureDropDown();
                mvCountry.SetActiveView(vGreekCity);
                revContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revContactPhone.Text = revContactPhone.ErrorMessage = Resources.PositionGroupInput.ContactPhoneFrRegex;
                revContactPhoneTip.Attributes.Add("title", Resources.PositionGroupInput.ContactPhoneFrRegex);
                revContactPhone.Enabled = true;
                txtContactPhone.MaxLength = 15;
                txtContactPhone.Attributes.Add("title", "Το τηλέφωνο επικοινωνίας");

                litPrefecture.Text = Resources.PositionGroupInput.PrefectureCy;
                litCity.Text = Resources.PositionGroupInput.Kali_CityCy;
            }
            else
            {
                mvCountry.SetActiveView(vForeignCity);
                revContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revContactPhone.Text = revContactPhone.ErrorMessage = Resources.PositionGroupInput.ContactPhoneFrRegex;
                revContactPhoneTip.Attributes.Add("title", Resources.PositionGroupInput.ContactPhoneFrRegex);
                revContactPhone.Enabled = false;
                txtContactPhone.MaxLength = 15;
                txtContactPhone.Attributes.Add("title", "Το τηλέφωνο επικοινωνίας");
            }
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

        #endregion
    }
}