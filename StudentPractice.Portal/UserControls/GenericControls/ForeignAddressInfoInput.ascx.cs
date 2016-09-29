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
    public partial class ForeignAddressInfoInput : BaseEntityUserControl<InternshipProvider>
    {
        #region [ Control Inits ]

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem(Resources.ProviderInput.SelectCountry, ""));
            ddlCountry.Items.AddRange(CacheManager.Countries.GetItems()
                .Where(x => x.ID != StudentPracticeConstants.GreeceCountryID && x.ID != StudentPracticeConstants.CyprusCountryID)
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());
        }

        #endregion

        #region [ Databind Methods ]

        public override InternshipProvider Fill(InternshipProvider entity)
        {
            entity.CountryID = ddlCountry.GetSelectedInteger();
            entity.Address = txtAddress.GetText();
            entity.ZipCode = txtZipCode.GetText();
            entity.CityText = txtCityText.GetText();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            ddlCountry.SelectedValue = Entity.CountryID.ToString();
            txtZipCode.Text = Entity.ZipCode;
            txtAddress.Text = Entity.Address;
            txtCityText.Text = Entity.CityText;
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
            txtCityText.Attributes["onkeyup"] = "Imis.Lib.ToUpper(this)";

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
            }

            base.OnPreRender(e);
        }

        #endregion

        #region [ UI Region ]

        private void UpdateUIUser()
        {
            txtCityText.Enabled =
            rfvCityText.Enabled = true;
        }

        /// <summary>
        /// O χρήστης είναι Helpdesk και πρέπει να ενημερωθεί το UI κατάλληλα.
        /// </summary>
        private void UpdateUIHelpdesk()
        {
            //Τα θέτουμε όλα ReadOnly
            SetReadOnly(true);

            //Όλα non-editable εκτός από
            txtCityText.Enabled =
            rfvCityText.Enabled = false;
        }

        private void SetReadOnly(bool isReadOnly)
        {
            bool isEnabled = !isReadOnly;
            foreach (WebControl c in Controls.OfType<WebControl>())
                c.Enabled = isEnabled;
        }

        /// <summary>
        /// Χρησιμοποιείται μόνο όταν θέλουμε να θέσουμε όλα τα πεδία στη φόρμα ReadOnly ανεξάρτητα από το ποιος τα βλέπει και για ποιο λόγο
        /// </summary>
        public bool? ReadOnly
        {
            get;
            set;
        }

        #endregion
    }
}