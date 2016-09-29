using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls
{
    public partial class PositionGroupInput : BaseEntityUserControl<InternshipPositionGroup>
    {
        public int CountryID { get; set; }
        public bool FinishedPositionMode { get; set; }

        #region [ Control Inits ]

        protected void ddlPositionType_Init(object sender, EventArgs e)
        {
            ddlPositionType.Items.Add(new ListItem(Resources.PositionGroupInput.PositionType_ddl, ""));

            foreach (enPositionType item in Enum.GetValues(typeof(enPositionType)))
            {
                ddlPositionType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem(CacheManager.Countries.Get(StudentPracticeConstants.GreeceCountryID).Name, StudentPracticeConstants.GreeceCountryID.ToString()));
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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            CacheManager.Cities.GetItems().ForEach(x => Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, x.ID.ToString()));
            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtSupervisor.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";

            if (Page.IsPostBack)
            {
                int noTimeLimit;
                if (int.TryParse(ddlNoTimeLimit.SelectedValue, out noTimeLimit) && noTimeLimit == 1)
                {
                    rfvStartDate.Enabled = false;
                    rfvEndDate.Enabled = false;
                }
            }


            if (Entity == null && !Page.IsPostBack)
            {
                UpdateCountryDropdown();
                ddlCountry.SelectedValue = CountryID.ToString();
                UpdateCountryUI();
            }
            if (FinishedPositionMode)
            {
                example.Visible =
                rowPositionCount.Visible =
                rowNoTimeLimit.Visible =
                rfvPositionCount.Enabled =
                rvPositionCount.Enabled = false;
            }
        }

        #region [ Databind Methods ]

        public override InternshipPositionGroup Fill(InternshipPositionGroup entity)
        {
            if (entity == null)
                entity = new InternshipPositionGroup();

            //Γενικά Στοιχεία Θέσης Πρακτικής Άσκησης
            entity.Title = txtTitle.Text.ToNull();
            entity.Description = txtDescription.Text.ToNull();
            entity.Duration = txtDuration.GetInteger().Value;
            entity.CityID = ddlCity.GetSelectedInteger();
            entity.PrefectureID = ddlPrefecture.GetSelectedInteger();
            entity.CityText = txtCityText.GetText();
            entity.CountryID = ddlCountry.GetSelectedInteger().Value;

            if (FinishedPositionMode)
            {
                entity.NoTimeLimit = false;
                DateTime startDate;
                if (DateTime.TryParse(txtStartDate.Text, out startDate) && entity.StartDate != startDate)
                {
                    entity.StartDate = startDate;
                }

                DateTime endDate;
                if (DateTime.TryParse(txtEndDate.Text, out endDate) && entity.EndDate != endDate)
                {
                    entity.EndDate = endDate;
                }
            }

            else
            {
                int noTimeLimit;
                if (int.TryParse(ddlNoTimeLimit.SelectedValue, out noTimeLimit) && (noTimeLimit == 0 || noTimeLimit == 1))
                {
                    if (noTimeLimit == 0)
                    {
                        if (entity.NoTimeLimit)
                            entity.NoTimeLimit = false;

                        DateTime startDate;
                        if (DateTime.TryParse(txtStartDate.Text, out startDate) && entity.StartDate != startDate)
                            entity.StartDate = startDate;

                        DateTime endDate;
                        if (DateTime.TryParse(txtEndDate.Text, out endDate) && entity.EndDate != endDate)
                            entity.EndDate = endDate;
                    }
                    else
                    {
                        if (!entity.NoTimeLimit)
                            entity.NoTimeLimit = true;

                        entity.StartDate = null;
                        entity.EndDate = null;
                    }
                }
            }

            entity.PositionTypeInt = ddlPositionType.GetSelectedInteger().Value;
            entity.Supervisor = txtSupervisor.GetText();
            entity.SupervisorEmail = txtSupervisorEmail.GetText();
            entity.ContactPhone = txtContactPhone.GetText();
            entity.TotalPositions = txtPositionCount.GetInteger().Value;

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
            txtPositionCount.Text = Entity.TotalPositions.ToString();
            txtDescription.Text = Entity.Description;
            txtDuration.Text = Entity.Duration.ToString();

            UpdateCountryDropdown();
            ddlCountry.SelectedValue = Entity.CountryID.ToString();
            InitializePrefectureDropDown();
            UpdateCountryUI();

            if (Entity.CountryID != StudentPracticeConstants.GreeceCountryID
                && Entity.CountryID != StudentPracticeConstants.CyprusCountryID)
            {
                txtCityText.Text = Entity.CityText;
            }

            if (Entity.PositionCreationType == enPositionCreationType.FromOffice)
            {
                trStartDate.Visible = true;
                trEndDate.Visible = true;

                ddlNoTimeLimit.SelectedValue = "0";
                txtStartDate.Text = Entity.StartDate.HasValue ? Entity.StartDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
                txtEndDate.Text = Entity.EndDate.HasValue ? Entity.EndDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
            }

            else if (Entity.NoTimeLimit)
            {
                ddlNoTimeLimit.SelectedValue = "1";
            }

            else
            {
                trStartDate.Visible = true;
                trEndDate.Visible = true;

                ddlNoTimeLimit.SelectedValue = "0";
                txtStartDate.Text = Entity.StartDate.HasValue ? Entity.StartDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
                txtEndDate.Text = Entity.EndDate.HasValue ? Entity.EndDate.Value.ToString(Resources.GlobalProvider.DateFormat) : string.Empty;
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

        protected void rvPositionCount_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            int positionCount;
            if (int.TryParse(txtPositionCount.Text.ToNull(), out positionCount) && positionCount > 0 && positionCount <= 99)
                e.IsValid = true;
        }

        protected void rvDuration_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            int duration;
            if (int.TryParse(txtDuration.Text.ToNull(), out duration) && duration > 0 && duration <= 99)
                e.IsValid = true;
        }

        protected void cvMinStartDate_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            DateTime startDate;
            if (!FinishedPositionMode && DateTime.TryParse(txtStartDate.Text.ToNull(), out startDate) && startDate < DateTime.Now)
                e.IsValid = false;
        }

        protected void cvMinEndDate_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            int noTimeLimit;
            if (int.TryParse(ddlNoTimeLimit.SelectedValue, out noTimeLimit) && noTimeLimit == 0)
            {
                DateTime startDate;
                DateTime endDate;
                if (DateTime.TryParse(txtStartDate.Text.ToNull(), out startDate) && DateTime.TryParse(txtEndDate.Text.ToNull(), out endDate) && endDate <= startDate)
                    e.IsValid = false;
            }
        }

        protected void cvMinDuration_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            int noTimeLimit;
            if (int.TryParse(ddlNoTimeLimit.SelectedValue, out noTimeLimit) && noTimeLimit == 0)
            {
                int duration;
                if (int.TryParse(txtDuration.Text.ToNull(), out duration) && duration > 0)
                {
                    int implementationDuration;
                    DateTime startDate;
                    DateTime endDate;

                    if (DateTime.TryParse(txtStartDate.Text.ToNull(), out startDate) && DateTime.TryParse(txtEndDate.Text.ToNull(), out endDate) && endDate > startDate)
                    {
                        implementationDuration = (int)endDate.Subtract(startDate).TotalDays;
                        if (implementationDuration < 7 * duration)
                            e.IsValid = false;
                    }
                }
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
                revContactPhone.Enabled = true;
                txtContactPhone.MaxLength = 15;
                txtContactPhone.Attributes.Add("title", "Contact phone");
            }
        }

        private void UpdateCountryDropdown()
        {
            if (CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.CyprusCountryName, StudentPracticeConstants.CyprusCountryID.ToString()));
            }

            else if (CountryID != StudentPracticeConstants.CyprusCountryID && CountryID != StudentPracticeConstants.GreeceCountryID)
            {
                ddlCountry.Items.AddRange(CacheManager.Countries.GetItems()
                .Where(x => x.ID != StudentPracticeConstants.GreeceCountryID)
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());
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

            ddlPrefecture.Items.AddRange(CacheManager.Prefectures.GetItems()
                .Where(x => x.CountryID == countryID)
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());

            if (Entity != null && Entity.PrefectureID.HasValue)
            {
                ddlPrefecture.SelectedValue = Entity.PrefectureID.Value.ToString();
                cddCity.SelectedValue = Entity.CityID.GetValueOrDefault().ToString();
            }
        }

        #endregion

    }
}