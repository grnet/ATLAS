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
    public enum enCountryOrigin
    {
        Greece = 0,
        Cyprus = 1,
        Foreign = 2
    }

    public partial class ProviderInput : BaseEntityUserControl<InternshipProvider>
    {
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

        #region [ Page Inits ]

        protected override void OnPreRender(EventArgs e)
        {
            int countryOrigin = 0;
            bool isHelpdesk = Roles.IsUserInRole(BusinessModel.RoleNames.Helpdesk) || Roles.IsUserInRole(BusinessModel.RoleNames.SuperHelpdesk);

            if (!Page.IsPostBack)
            {
                txtLegalPersonName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
                txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
                txtAlternateContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            }

            if (Entity != null)
            {

                if (Entity.CountryID.Value == StudentPracticeConstants.GreeceCountryID)
                    countryOrigin = enCountryOrigin.Greece.GetValue();

                else if (Entity.CountryID.Value == StudentPracticeConstants.CyprusCountryID)
                    countryOrigin = enCountryOrigin.Cyprus.GetValue();

                else
                    countryOrigin = enCountryOrigin.Foreign.GetValue();

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
            }
            else
            {
                int.TryParse(Request.QueryString["c"], out countryOrigin);
            }

            if ((countryOrigin == enCountryOrigin.Cyprus.GetValue() && Config.AllowCyprusRegistration) ||
                (countryOrigin == enCountryOrigin.Foreign.GetValue() && Config.AllowForeignRegistration))
            {
                if (!Page.IsPostBack)
                {
                    txtAFM.Attributes.Add("title", FixResource(Resources.ProviderInput.AFMTooltip));
                    txtProviderPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ProviderPhoneTooltip));
                    txtProviderFax.Attributes.Add("title", FixResource(Resources.ProviderInput.ProviderFaxTooltip));
                    txtLegalPersonPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.LegalPersonPhoneTooltip));
                    txtContactPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ContactPhoneTooltip));
                    txtContactMobilePhone.Attributes.Add("title", FixResource(Resources.ProviderInput.ContactMobilePhoneTooltip));
                    txtAlternateContactPhone.Attributes.Add("title", FixResource(Resources.ProviderInput.AlternateContactPhoneTooltip));
                    txtAlternateContactMobilePhone.Attributes.Add("title", FixResource(Resources.ProviderInput.AlternateContactMobilePhoneTooltip));
                }

                cvAFM.Visible = false;
                txtAFM.MaxLength = 0;
                trDOY.Visible = false;

                txtProviderPhone.MaxLength = 50;
                revProviderPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revProviderPhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revProviderPhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                txtProviderFax.MaxLength = 50;
                revProviderFax.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revProviderFax.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revProviderFaxTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                txtLegalPersonPhone.MaxLength = 50;
                revLegalPersonPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revLegalPersonPhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revLegalPersonPhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);


                txtContactPhone.MaxLength = 50;
                revContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revContactPhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revContactPhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                txtContactMobilePhone.MaxLength = 50;
                revContactMobilePhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revContactMobilePhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revContactMobilePhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                txtAlternateContactPhone.MaxLength = 50;
                revAlternateContactPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revAlternateContactPhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revAlternateContactPhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                txtAlternateContactMobilePhone.MaxLength = 50;
                revAlternateContactMobilePhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                revAlternateContactMobilePhone.ErrorMessage = Resources.ProviderInput.AlternatePhoneValidationMessage;
                revAlternateContactMobilePhoneTip.Attributes.Add("title", Resources.ProviderInput.AlternatePhoneValidationMessage);

                if (countryOrigin == enCountryOrigin.Cyprus.GetValue())
                {
                    mvAddress.SetActiveView(vCyprusAddress);
                }
                else if (countryOrigin == enCountryOrigin.Foreign.GetValue())
                {
                    mvAddress.SetActiveView(vForeignAddress);
                }
            }
            else
            {
                mvAddress.SetActiveView(vGreekAddress);
            }

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
            {
                int.TryParse(Request.QueryString["c"], out countryOrigin);
            }


            if (countryOrigin == enCountryOrigin.Foreign.GetValue() && Config.AllowForeignRegistration)
            {
                ucForeignAddressInfoInput.Fill(entity);
            }
            else if (countryOrigin == enCountryOrigin.Cyprus.GetValue() && Config.AllowCyprusRegistration)
            {
                ucCyprusAddressInfoInput.Fill(entity);
            }
            else
            {
                entity.DOY = ddlDOY.GetSelectedString();
                ucGreekAddressInfoInput.Fill(entity);
            }

            //Στοιχεία Νομίμου Εκπροσώπου Φορέα Υποδοχής Πρακτικής Άσκησης
            entity.LegalPersonName = txtLegalPersonName.GetText();
            entity.LegalPersonPhone = txtLegalPersonPhone.GetText();
            entity.LegalPersonEmail = txtLegalPersonEmail.GetText();

            var legalPersonID = idLegalPerson.Fill();
            if (legalPersonID != null)
            {
                entity.LegalPersonIdentificationType = legalPersonID.IdType;
                entity.LegalPersonIdentificationNumber = legalPersonID.IdNumber;
                entity.LegalPersonIdentificationIssuer = legalPersonID.IdIssuer;
                entity.LegalPersonIdentificationIssueDate = legalPersonID.IdIssueDate;
            }

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
            {
                int.TryParse(Request.QueryString["c"], out countryOrigin);
            }

            if (countryOrigin == enCountryOrigin.Greece.GetValue())
            {
                ddlDOY.Items.FindByValue(Entity.DOY).Selected = true;
                ucGreekAddressInfoInput.Entity = Entity;
                ucGreekAddressInfoInput.Bind();
            }
            else if (countryOrigin == enCountryOrigin.Cyprus.GetValue() && Config.AllowCyprusRegistration)
            {
                ucCyprusAddressInfoInput.Entity = Entity;
                ucCyprusAddressInfoInput.Bind();
            }
            else if (countryOrigin == enCountryOrigin.Foreign.GetValue() && Config.AllowForeignRegistration)
            {
                ucForeignAddressInfoInput.Entity = Entity;
                ucForeignAddressInfoInput.Bind();
            }


            // Στοιχεία Νομίμου Εκπροσώπου Φορέα Υποδοχής Πρακτικής Άσκησης
            txtLegalPersonName.Text = Entity.LegalPersonName;
            txtLegalPersonPhone.Text = Entity.LegalPersonPhone;
            txtLegalPersonEmail.Text = Entity.LegalPersonEmail;
            IdentificationDetails legalPersonID = new IdentificationDetails();
            legalPersonID.IdNumber = Entity.LegalPersonIdentificationNumber;
            legalPersonID.IdType = Entity.LegalPersonIdentificationType;
            if (Entity.LegalPersonIdentificationType == enIdentificationType.ID)
            {
                legalPersonID.IdIssuer = Entity.LegalPersonIdentificationIssuer;
                legalPersonID.IdIssueDate = Entity.LegalPersonIdentificationIssueDate;
            }
            idLegalPerson.Bind(legalPersonID);

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

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvContactEmail.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;

                txtAFM.ValidationGroup = value;
                idLegalPerson.ValidationGroup = value;
            }
        }

        protected void cvAFM_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = BusinessHelper.CheckAFM(e.Value);
        }

        protected void cvProviderNumberOfEmployees_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;
            int numberOfEmployees;
            if (int.TryParse(txtProviderNumberOfEmployees.Text.ToNull(), out numberOfEmployees) && numberOfEmployees > 0)
                e.IsValid = true;
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

        #region [ UI Region ]

        private void UpdateUIUser()
        {
            ddlProviderType.Enabled =
            ddlPrimaryActivity.Enabled =
            rfvProviderType.Enabled =
            txtName.Enabled =
            rfvName.Enabled =
            txtTradeName.Enabled =
            txtAFM.Enabled =
            rfvAFM.Enabled =
            ddlDOY.Enabled =
            rfvDOY.Enabled =
            txtProviderNumberOfEmployees.Enabled =
            rfvProviderNumberOfEmployees.Enabled =
            revProviderNumberOfEmployees.Enabled =
            cvProviderNumberOfEmployees.Enabled =
            txtLegalPersonName.Enabled =
            rfvLegalPersonName.Enabled =
            txtContactName.Enabled =
            rfvContactName.Enabled = false;

            idLegalPerson.ReadOnly = true;
        }

        /// <summary>
        /// O χρήστης είναι Helpdesk και πρέπει να ενημερωθεί το UI κατάλληλα.
        /// </summary>
        private void UpdateUIHelpdesk()
        {
            //Τα θέτουμε όλα ReadOnly
            SetReadOnly(true);

            //Όλα non-editable εκτός από
            ddlProviderType.Enabled =
            rfvProviderType.Enabled =
            ddlPrimaryActivity.Enabled =
            rfvPrimaryActivity.Enabled =
            txtName.Enabled =
            rfvName.Enabled =
            txtTradeName.Enabled =
            txtAFM.Enabled =
            rfvAFM.Enabled =
            cvAFM.Enabled =
            ddlDOY.Enabled =
            rfvDOY.Enabled =
            txtProviderNumberOfEmployees.Enabled =
            rfvProviderNumberOfEmployees.Enabled =
            revProviderNumberOfEmployees.Enabled =
            cvProviderNumberOfEmployees.Enabled =
            txtLegalPersonName.Enabled =
            rfvLegalPersonName.Enabled =
            txtContactName.Enabled =
            rfvContactName.Enabled = true;

            idLegalPerson.ReadOnly = false;
            btnAlternateContact.Visible = false;
        }

        private void SetReadOnly(bool isReadOnly)
        {
            foreach (WebControl c in Controls.OfType<WebControl>())
            {
                c.Enabled = !isReadOnly;
            }

            idLegalPerson.ReadOnly = isReadOnly;
            btnAlternateContact.Visible = !isReadOnly;
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
    }
}