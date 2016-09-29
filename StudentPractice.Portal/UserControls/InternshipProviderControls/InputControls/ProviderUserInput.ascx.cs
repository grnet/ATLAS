using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;
using System.Xml.Linq;

namespace StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls
{
    public partial class ProviderUserInput : BaseEntityUserControl<InternshipProvider>
    {
        public bool AllowGreece { get; set; }
        public bool AllowCyprus { get; set; }

        #region [ Page Inits ]

        protected override void OnPreRender(EventArgs e)
        {
            int countryOrigin = 0;

            if (!Page.IsPostBack)
            {
                txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
                txtAlternateContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            }

            int country;
            if (Entity != null)
            {
                country = Entity.CountryID.Value;
                if (!EditMode)
                {
                    if (ReadOnly.HasValue)
                        SetReadOnly(ReadOnly.Value);
                    else
                        SetReadOnly(true);
                }
            }
            else
            {
                country = Context.LoadProviderUserSelectedCountry().GetValueOrDefault();
            }

            if (country == StudentPracticeConstants.GreeceCountryID)
                countryOrigin = enCountryOrigin.Greece.GetValue();
            else if (country == StudentPracticeConstants.CyprusCountryID)
                countryOrigin = enCountryOrigin.Cyprus.GetValue();
            else
                countryOrigin = enCountryOrigin.Foreign.GetValue();

            if (countryOrigin == enCountryOrigin.Cyprus.GetValue() || countryOrigin == enCountryOrigin.Foreign.GetValue())
            {
                txtAFM.Attributes.Add("title", FixResource(Resources.ProviderInput.AFMTooltip));
                txtProviderPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ProviderPhoneTooltip));
                txtProviderFax.Attributes.Add("title", FixResource(Resources.ProviderInput.ProviderFaxTooltip));
                txtContactPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ContactPhoneTooltip));
                txtContactMobilePhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ContactMobilePhoneTooltip));
                txtAlternateContactPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.AlternateContactPhoneTooltip));
                txtAlternateContactMobilePhone.Attributes.Add("title", FixResource(Resources.ProviderInput.AlternateContactMobilePhoneTooltip));

                cvAFM.Visible = false;
                trDOY.Visible = false;

                txtProviderPhone.MaxLength = 50;
                revProviderPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revProviderPhone.ErrorMessage = FixResource(Resources.ProviderInput.AlternateProviderPhoneInvalid);
                imgRevProviderPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.AlternateProviderPhoneInvalid));
                txtProviderFax.MaxLength = 50;
                revProviderFax.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";

                txtContactPhone.MaxLength = 50;
                revContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                txtContactMobilePhone.MaxLength = 50;
                revContactMobilePhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";

                txtAlternateContactPhone.MaxLength = 50;
                revAlternateContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                txtAlternateContactMobilePhone.MaxLength = 50;
                revAlternateContactMobilePhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
            }
            else
            {
                txtAFM.Attributes.Add("title", Resources.ProviderInput.AFMTooltip);
                txtProviderPhone.Attributes.Add("title", Resources.ProviderInput.ProviderPhoneTooltip);
                txtProviderFax.Attributes.Add("title", Resources.ProviderInput.ProviderFaxTooltip);
                txtContactPhone.Attributes.Add("title", Resources.ProviderInput.ContactPhoneTooltip);
                txtContactMobilePhone.Attributes.Add("title", Resources.ProviderInput.ContactMobilePhoneTooltip);
                txtAlternateContactPhone.Attributes.Add("title", Resources.ProviderInput.AlternateContactPhoneTooltip);
                txtAlternateContactMobilePhone.Attributes.Add("title", Resources.ProviderInput.AlternateContactMobilePhoneTooltip);
                
                cvAFM.Visible = true;
                trDOY.Visible = true;

                txtProviderPhone.MaxLength = 10;
                revProviderPhone.ValidationExpression = "^(2[0-9]{9})|(800[0-9]{7})|(801[0-9]{7})|([0-9]{5})|([0-9]{4})$";
                revProviderPhone.ErrorMessage = FixResource(Resources.ProviderInput.ProviderPhoneInvalid);
                imgRevProviderPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ProviderPhoneInvalid));
                txtProviderFax.MaxLength = 10;
                revProviderFax.ValidationExpression = "^2[0-9]{9}$";

                txtContactPhone.MaxLength = 10;
                revContactPhone.ValidationExpression = "^2[0-9]{9}$";
                txtContactMobilePhone.MaxLength = 10;
                revContactMobilePhone.ValidationExpression = "^69[0-9]{8}$";

                txtAlternateContactPhone.MaxLength = 10;
                revAlternateContactPhone.ValidationExpression = "^2[0-9]{9}$";
                txtAlternateContactMobilePhone.MaxLength = 10;
                revAlternateContactMobilePhone.ValidationExpression = "^69[0-9]{8}$";
            }
            ucAddressInfoInput.ReadOnly = ReadOnly;

            base.OnPreRender(e);
        }

        #endregion

        #region [ Databind Methods ]

        public override InternshipProvider Fill(InternshipProvider entity)
        {
            //Στοιχεία Φορέα Υποδοχής Πρακτικής Άσκησης
            entity.ProviderTypeInt = ddlProviderType.GetSelectedInteger();
            entity.PrimaryActivityID = ddlPrimaryActivity.GetSelectedInteger();
            entity.Name = txtName.GetText();
            entity.TradeName = txtTradeName.GetText();
            entity.AFM = txtAFM.GetText();
            entity.ProviderPhone = txtProviderPhone.GetText();
            entity.ProviderFax = txtProviderFax.GetText();
            entity.ProviderEmail = txtProviderEmail.GetText();
            entity.ProviderURL = txtProviderURL.GetText();
            entity.NumberOfEmployees = int.Parse(txtProviderNumberOfEmployees.Text);

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            int countryOrigin = 0;

            if (!entity.IsNew)
            {
                if (entity.CountryID.Value == StudentPracticeConstants.GreeceCountryID)
                    countryOrigin = enCountryOrigin.Greece.GetValue();
                else if (entity.CountryID.Value == StudentPracticeConstants.CyprusCountryID)
                    countryOrigin = enCountryOrigin.Cyprus.GetValue();
                else if (entity.CountryID.HasValue)
                    countryOrigin = enCountryOrigin.Foreign.GetValue();
            }
            else
                countryOrigin = Context.LoadProviderUserSelectedCountry().GetValueOrDefault();

            if (countryOrigin == enCountryOrigin.Greece.GetValue())
                entity.DOY = ddlDOY.GetSelectedString();

            ucAddressInfoInput.Fill(entity);

            //Στοιχεία Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            entity.ContactName = txtContactName.GetText();
            entity.ContactPhone = txtContactPhone.GetText();
            entity.ContactMobilePhone = txtContactMobilePhone.GetText();
            entity.ContactEmail = txtContactEmail.GetText();

            //Στοιχεία Αναπληρωτή Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            entity.AlternateContactName = txtAlternateContactName.GetText();
            entity.AlternateContactPhone = txtAlternateContactPhone.GetText();
            entity.AlternateContactMobilePhone = txtAlternateContactMobilePhone.GetText();
            entity.AlternateContactEmail = txtAlternateContactEmail.GetText();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Λογαριασμού
            txtUsername.Text = Entity.UserName;
            txtEmail.Text = Entity.Email;

            //Στοιχεία Φορέα Υποδοχής Γραφείου Πρακτικής Άσκησης
            ddlProviderType.SelectedValue = Entity.ProviderTypeInt.ToString();
            ddlPrimaryActivity.SelectedValue = Entity.PrimaryActivityID.ToString();
            txtName.Text = Entity.Name;
            txtTradeName.Text = Entity.TradeName;
            txtAFM.Text = Entity.AFM;
            txtProviderPhone.Text = Entity.ProviderPhone;
            txtProviderFax.Text = Entity.ProviderFax;
            txtProviderEmail.Text = Entity.ProviderEmail;
            txtProviderURL.Text = Entity.ProviderURL;
            txtProviderNumberOfEmployees.Text = Entity.NumberOfEmployees.ToString();

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            int countryOrigin = 0;

            if (Entity != null)
            {
                if (Entity.CountryID.Value == StudentPracticeConstants.GreeceCountryID)
                    countryOrigin = enCountryOrigin.Greece.GetValue();
                else if (Entity.CountryID.Value == StudentPracticeConstants.CyprusCountryID)
                    countryOrigin = enCountryOrigin.Cyprus.GetValue();
                else if (Entity.CountryID.HasValue)
                    countryOrigin = enCountryOrigin.Foreign.GetValue();
            }
            else
                int.TryParse(Request.QueryString["c"], out countryOrigin);

            if (countryOrigin == enCountryOrigin.Greece.GetValue() && !string.IsNullOrEmpty(Entity.DOY))
                ddlDOY.Items.FindByValue(Entity.DOY).Selected = true;

            ucAddressInfoInput.Entity = Entity;
            ucAddressInfoInput.Bind();


            //Στοιχεία Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            txtContactName.Text = Entity.ContactName;
            txtContactPhone.Text = Entity.ContactPhone;
            txtContactMobilePhone.Text = Entity.ContactMobilePhone;
            txtContactEmail.Text = Entity.ContactEmail;

            //Στοιχεία Αναπληρωτή Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            txtAlternateContactName.Text = Entity.AlternateContactName;
            txtAlternateContactPhone.Text = Entity.AlternateContactPhone;
            txtAlternateContactMobilePhone.Text = Entity.AlternateContactMobilePhone;
            txtAlternateContactEmail.Text = Entity.AlternateContactEmail;
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlProviderType_Init(object sender, EventArgs e)
        {
            ddlProviderType.Items.Add(new ListItem(Resources.ProviderInput.SelectProviderType, ""));

            foreach (enProviderType item in Enum.GetValues(typeof(enProviderType)))
            {
                ddlProviderType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
            }
        }

        protected void ddlPrimaryActivity_Init(object sender, EventArgs e)
        {
            ddlPrimaryActivity.Items.Add(new ListItem(Resources.ProviderInput.SelectPrimaryActivity, ""));
            ddlPrimaryActivity.Items.AddRange(CacheManager.PrimaryActivities.GetItems()
                .OrderBy(x => x.Name)
                .Select(x => new ListItem(x.Name, x.ID.ToString()))
                .ToArray());
        }

        protected void ddlDOY_Init(object sender, EventArgs e)
        {
            ddlDOY.Items.Add(new ListItem(Resources.ProviderInput.SelectDOY, ""));

            foreach (XElement elem in DOY.DOYsXml.Descendants("DOY"))
            {
                ddlDOY.Items.Add(new ListItem(elem.Value, elem.Value));
            }
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvName.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        protected void cvAFM_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            if (!string.IsNullOrEmpty(e.Value))
                e.IsValid = BusinessHelper.CheckAFM(e.Value);
        }

        protected void cvProviderNumberOfEmployees_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            int numberOfEmployees;
            if (int.TryParse(txtProviderNumberOfEmployees.Text.ToNull(), out numberOfEmployees) && numberOfEmployees > 0)
            {
                e.IsValid = true;
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

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
        }

        #endregion

        #region [ UI Region ]

        public bool EditMode
        {
            get
            {
                return !txtUsername.Enabled;
            }
            set
            {
                txtUsername.Enabled =
                trPassword1.Visible =
                trPassword2.Visible =
                trEmailConfirmation.Visible = !value;
            }
        }

        private void SetReadOnly(bool isReadOnly)
        {
            bool isEnabled = !isReadOnly;
            foreach (WebControl c in Controls.OfType<WebControl>())
                c.Enabled = isEnabled;

            ddlDOY.Enabled = isEnabled;

            trPassword1.Visible =
            trPassword2.Visible =
            trEmailConfirmation.Visible = !isReadOnly;
        }

        /// <summary>
        /// Χρησιμοποιείται μόνο όταν θέλουμε να θέσουμε όλα τα πεδία στη φόρμα ReadOnly ανεξάρτητα από το ποιος τα βλέπει και για ποιο λόγο
        /// </summary>
        public bool? ReadOnly { get; set; }

        #endregion

        #region [ Helper Methods ]

        private string FixResource(string resource)
        {
            return resource.IndexOf('<') > 0
                ? resource.Substring(0, resource.IndexOf('<'))
                : resource;
        }

        #endregion

        public string Username { get { return txtUsername.Text.Trim(); } }
        public string Password { get { return txtPassword1.Text.Trim(); } }
        public string Email { get { return txtEmail.Text.Trim(); } }
    }
}