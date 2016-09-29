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
    public partial class GreekAddressInfoInput : BaseEntityUserControl<InternshipProvider>
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
            ddlCountry.Items.Add(new ListItem(StudentPracticeConstants.GreeceCountryName, StudentPracticeConstants.GreeceCountryID.ToString()));
        }

        protected void ddlPrefecture_Init(object sender, EventArgs e)
        {
            ddlPrefecture.Items.Add(new ListItem("-- επιλέξτε περιφερειακή ενότητα --", ""));

            foreach (var item in CacheManager.Prefectures.GetItems().Where(x => x.CountryID == StudentPracticeConstants.GreeceCountryID).OrderBy(x => x.Name))
            {
                ddlPrefecture.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        #endregion

        #region [ Databind Methods ]

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
                entity.CountryID = StudentPracticeConstants.GreeceCountryID;
            }

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            txtZipCode.Text = Entity.ZipCode;
            txtAddress.Text = Entity.Address;

            if (Entity.PrefectureID.HasValue)
            {
                var pref = CacheManager.Prefectures.Get(Entity.PrefectureID.Value);
                ddlPrefecture.SelectedValue = pref.ID.ToString();

                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-- επιλέξτε νομό --", "-1"));

                foreach (var item in CacheManager.Cities.GetItems().Where(x => x.PrefectureID == pref.ID))
                {
                    ddlCity.Items.Add(new ListItem(item.Name, item.ID.ToString()));
                }

                if (Entity.CityID.HasValue)
                {
                    var city = CacheManager.Cities.Get(Entity.CityID.Value);
                    ddlCity.SelectedValue = city.ID.ToString();
                    cddCity.SelectedValue = city.ID.ToString();
                }
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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }
            base.Render(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            txtAddress.Attributes["onkeyup"] = "Imis.Lib.ToElUpper(this)";

            bool isHelpdesk = Roles.IsUserInRole(BusinessModel.RoleNames.Helpdesk) || Roles.IsUserInRole(BusinessModel.RoleNames.SuperHelpdesk);
            if (Entity != null)
            {
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
    }
}