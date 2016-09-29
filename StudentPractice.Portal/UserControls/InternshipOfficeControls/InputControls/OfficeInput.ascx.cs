using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;

namespace StudentPractice.Portal.UserControls.InternshipOfficeControls.InputControls
{
    public partial class OfficeInput : BaseEntityUserControl<InternshipOffice>
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            CacheManager.Cities.GetItems().ForEach(x => Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, x.ID.ToString()));
            base.Render(writer);
        }

        #region [ Databind Methods ]

        public override InternshipOffice Fill(InternshipOffice entity)
        {
            if (entity == null)
            {
                entity = new InternshipOffice();
                entity.UsernameFromLDAP = Guid.NewGuid().ToString();
            }

            //Στοιχεία Υπεύθυνου Γραφείου Πρακτικής Άσκησης
            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedValue, out institutionID) && institutionID > 0)
            {
                if (entity.InstitutionID != institutionID)
                    entity.InstitutionID = institutionID;
            }

            int officeType;
            if (int.TryParse(rbtlOfficeType.SelectedValue, out officeType) && officeType > 0)
            {
                if (officeType == 1)
                {
                    entity.CanViewAllAcademics = true;
                }
                else if (officeType == 2)
                {
                    entity.CanViewAllAcademics = false;
                }
            }

            if (entity.ContactName != txtContactName.Text.ToNull())
                entity.ContactName = txtContactName.Text.ToNull();

            if (entity.ContactPhone != txtContactPhone.Text.ToNull())
                entity.ContactPhone = txtContactPhone.Text.ToNull();

            if (entity.ContactMobilePhone != txtContactMobilePhone.Text.ToNull())
                entity.ContactMobilePhone = txtContactMobilePhone.Text.ToNull();

            if (entity.ContactEmail != txtContactEmail.Text.ToNull())
                entity.ContactEmail = txtContactEmail.Text.ToNull();

            //Πιστοποιούσα Αρχή
            entity.CertifierType = (enCertifierType)int.Parse(rbtlCertifierType.SelectedItem.Value);
            if (entity.CertifierName != txtCertifierName.Text.ToNull())
                entity.CertifierName = txtCertifierName.Text.ToNull();

            //Στοιχεία Διεύθυνσης Γραφείου Πρακτικής Άσκησης
            if (entity.Address != txtAddress.Text.ToNull())
                entity.Address = txtAddress.Text.ToNull();

            if (entity.ZipCode != txtZipCode.Text.ToNull())
                entity.ZipCode = txtZipCode.Text.ToNull();

            int cityID;
            if (int.TryParse(ddlCity.SelectedValue, out cityID) && cityID > 0)
            {
                if (entity.CityID != cityID)
                    entity.CityID = cityID;
            }

            int prefectureID;
            if (int.TryParse(ddlPrefecture.SelectedValue, out prefectureID) && prefectureID > 0)
            {
                if (entity.PrefectureID != prefectureID)
                    entity.PrefectureID = prefectureID;
            }

            //Στοιχεία Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης
            if (entity.AlternateContactName != txtAlternateContactName.Text.ToNull())
                entity.AlternateContactName = txtAlternateContactName.Text.ToNull();

            if (entity.AlternateContactPhone != txtAlternateContactPhone.Text.ToNull())
                entity.AlternateContactPhone = txtAlternateContactPhone.Text.ToNull();

            if (entity.AlternateContactMobilePhone != txtAlternateContactMobilePhone.Text.ToNull())
                entity.AlternateContactMobilePhone = txtAlternateContactMobilePhone.Text.ToNull();

            if (entity.AlternateContactEmail != txtAlternateContactEmail.Text.ToNull())
                entity.AlternateContactEmail = txtAlternateContactEmail.Text.ToNull();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Υπευθύνου Γραφείου Πρακτικής Άσκησης
            ddlInstitution.SelectedValue = Entity.InstitutionID.ToString();
            if (Entity.CanViewAllAcademics.GetValueOrDefault())
            {
                rbtlCertifierType.SelectedValue = "1";
            }
            else
            {
                rbtlCertifierType.SelectedValue = "2";
            }

            txtContactName.Text = Entity.ContactName;
            txtContactPhone.Text = Entity.ContactPhone;
            txtContactMobilePhone.Text = Entity.ContactMobilePhone;
            txtContactEmail.Text = Entity.ContactEmail;

            //Πιστοποιούσα Αρχή
            var rblItem = rbtlCertifierType.Items.FindByValue(Entity.CertifierType.ToString("D"));
            if (rblItem != null)
                rbtlCertifierType.SelectedValue = rblItem.Value;
            txtCertifierName.Text = Entity.CertifierName;

            //Στοιχεία Διεύθυνσης Γραφείου Πρακτικής Άσκησης
            txtZipCode.Text = Entity.ZipCode;
            txtAddress.Text = Entity.Address;

            var pref = CacheManager.Prefectures.Get(Entity.PrefectureID.Value);
            ddlPrefecture.SelectedValue = pref.ID.ToString();

            ddlCity.Items.Clear();
            ddlCity.Items.Add(new ListItem("-- επιλέξτε νομό --", "-1"));

            foreach (var item in pref.Cities)
            {
                ddlCity.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }

            var city = CacheManager.Cities.Get(Entity.CityID.Value);
            ddlCity.SelectedValue = city.ID.ToString();
            cddCity.SelectedValue = city.ID.ToString();

            //Στοιχεία Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης 
            txtAlternateContactName.Text = Entity.AlternateContactName;
            txtAlternateContactPhone.Text = Entity.AlternateContactPhone;
            txtAlternateContactMobilePhone.Text = Entity.AlternateContactMobilePhone;
            txtAlternateContactEmail.Text = Entity.AlternateContactEmail;
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- επιλέξτε ίδρυμα --", ""));
            ddlInstitution.Items.AddRange(CacheManager.Institutions.GetItems()
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray()); 
        }

        protected void rbtlOfficeType_Init(object sender, EventArgs e)
        {
            rbtlOfficeType.Items.Add(new ListItem("Ολόκληρο το Ίδρυμα", "1"));
            rbtlOfficeType.Items.Add(new ListItem("Συγκεκριμένα Τμήματα (θα κληθείτε να τα επιλέξετε σε επόμενο βήμα)", "2"));
        }

        protected void rbtlCertifierType_Init(object sender, EventArgs e)
        {
            foreach (enCertifierType item in Enum.GetValues(typeof(enCertifierType)))
            {
                if (item == enCertifierType.None)
                    continue;
                rbtlCertifierType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
            }
        }

        protected void ddlPrefecture_Init(object sender, EventArgs e)
        {
            ddlPrefecture.Items.Add(new ListItem("-- επιλέξτε περιφερειακή ενότητα --", ""));
            ddlPrefecture.Items.AddRange(CacheManager.Prefectures.GetItems()
                .Where(x => x.CountryID == StudentPracticeConstants.GreeceCountryID)
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());            
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvContactEmail.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        protected void cvAlternativeGroup_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAlternateContactName.Text) && string.IsNullOrEmpty(txtAlternateContactPhone.Text) && string.IsNullOrEmpty(txtAlternateContactMobilePhone.Text) && string.IsNullOrEmpty(txtAlternateContactEmail.Text))
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = !string.IsNullOrEmpty(e.Value);
            }
        }

        #endregion

        #region [ Overrides ]

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
                txtCertifierName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
                txtAddress.Attributes["onkeyup"] = "Imis.Lib.ToElUpper(this)";
                txtAlternateContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            }

            bool isHelpdesk = Roles.IsUserInRole(BusinessModel.RoleNames.Helpdesk) || Roles.IsUserInRole(BusinessModel.RoleNames.SuperHelpdesk);
            if (Entity != null)
            {
                trDepartmentInfo.Visible = false;
                trOfficeType.Visible = false;

                if (ReadOnly.HasValue)
                    SetReadOnly(ReadOnly.Value);
                else
                {
                    if (Entity.VerificationStatus == enVerificationStatus.NotVerified)
                    {
                        SetReadOnly(false);
                    }
                    else if (Entity.VerificationStatus == enVerificationStatus.Verified)
                    {
                        if (!isHelpdesk)
                        {
                            UpdateUIUser();
                        }
                        else
                        {
                            UpdateUIHelpdesk();
                        }
                    }
                    else
                    {
                        SetReadOnly(true);
                    }
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

        #endregion

        #region [ UI Region ]

        private void UpdateUIUser()
        {
            ddlInstitution.Enabled =
            rbtlOfficeType.Enabled =
            txtContactName.Enabled =
            trCertifierInfo.Visible =
            rbtlCertifierType.Enabled =
            txtCertifierName.Enabled = false;
        }

        /// <summary>
        /// O χρήστης είναι Helpdesk και πρέπει να ενημερωθεί το UI κατάλληλα.
        /// </summary>
        private void UpdateUIHelpdesk()
        {
            //Τα θέτουμε όλα ReadOnly
            SetReadOnly(true);

            //Όλα non-editable εκτός από
            //ddlInstitution.Enabled =
            //rfvInstitution.Enabled =
            rbtlOfficeType.Enabled =
            rfvOfficeType.Enabled =
            txtContactName.Enabled =
            rfvContactName.Enabled =
            rbtlCertifierType.Enabled =
            rfvCertifierType.Enabled =
            txtCertifierName.Enabled =
            rfvCertifierName.Enabled = true;

            trCertifierInfo.Visible = false;
        }

        private void SetReadOnly(bool isReadOnly)
        {
            bool isEnabled = !isReadOnly;
            foreach (WebControl c in Controls.OfType<WebControl>())
                c.Enabled = isEnabled;
            cddCity.Enabled = isEnabled;
            trCertifierInfo.Visible = isEnabled;
        }

        /// <summary>
        /// Χρησιμοποιείται μόνο όταν θέλουμε να θέσουμε όλα τα πεδία στη φόρμα ReadOnly ανεξάρτητα από το ποιος τα βλέπει και για ποιο λόγο
        /// </summary>
        public bool? ReadOnly { get; set; }

        #endregion
    }
}