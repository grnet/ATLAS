﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;
using System.Xml.Linq;

namespace StudentPractice.Portal.UserControls.InternshipProviderControls.ViewControls
{
    public partial class ProviderInput : BaseEntityUserControl<InternshipProvider>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Φορέα Υποδοχής Γραφείου Πρακτικής Άσκησης
            lblProviderType.Text = Entity.ProviderType.GetLabel();
            lblPrimaryActivity.Text = Entity.PrimaryActivityID.HasValue ? CacheManager.PrimaryActivities.Get(Entity.PrimaryActivityID.Value).Name : string.Empty;
            lblName.Text = Entity.Name;
            lblTradeName.Text = Entity.TradeName;
            lblAFM.Text = Entity.AFM;
            lblDOY.Text = Entity.DOY;
            lblProviderPhone.Text = Entity.ProviderPhone;
            lblProviderFax.Text = Entity.ProviderFax;
            lblProviderEmail.Text = Entity.ProviderEmail;
            hlkProviderURL.Text = hlkProviderURL.NavigateUrl = Entity.ProviderURL;
            lblProviderNumberOfEmployees.Text = Entity.NumberOfEmployees.ToString();

            //Στοιχεία Διεύθυνσης Φορέα Υποδοχής Πρακτικής Άσκησης
            lblAddress.Text = Entity.Address;
            lblZipCode.Text = Entity.ZipCode;

            if (Entity != null)
            {
                if (Entity.CountryID.Value == StudentPracticeConstants.GreeceCountryID)
                {
                    lblCountry.Text = StudentPracticeConstants.GreeceCountryName;
                    ltrPrefecture.Text = "Περιφερειακή Ενότητα";
                    ltrCity.Text = "Καλλικρατικός Δήμος";
                    lblPrefecture.Text = Entity.PrefectureID.HasValue ? CacheManager.Prefectures.Get(Entity.PrefectureID.Value).Name : string.Empty;
                    lblCity.Text = Entity.CityID.HasValue ? CacheManager.Cities.Get(Entity.CityID.Value).Name : string.Empty;
                }
                else if (Entity.CountryID.Value == StudentPracticeConstants.CyprusCountryID)
                {
                    lblCountry.Text = StudentPracticeConstants.CyprusCountryName;
                    ltrPrefecture.Text = "Επαρχία";
                    ltrCity.Text = "Δήμος";
                    lblPrefecture.Text = Entity.PrefectureID.HasValue ? CacheManager.Prefectures.Get(Entity.PrefectureID.Value).Name : string.Empty;
                    lblCity.Text = Entity.CityID.HasValue ? CacheManager.Cities.Get(Entity.CityID.Value).Name : string.Empty;
                }
                else
                {
                    lblCountry.Text = CacheManager.Countries.Get(Entity.CountryID.GetValueOrDefault()).Name;
                    ltrCity.Text = "Πόλη";
                    lblCity.Text = Entity.CityText;
                    trPrefecture.Visible = false;
                }
            }


            // Στοιχεία Νομίμου Εκπροσώπου Φορέα Υποδοχής Πρακτικής Άσκησης
            lblLegalPersonName.Text = Entity.LegalPersonName;
            lblLegalPersonPhone.Text = Entity.LegalPersonPhone;
            lblLegalPersonEmail.Text = Entity.LegalPersonEmail;

            if (Entity.LegalPersonIdentificationType == enIdentificationType.ID)
            {
                lblIdType.Text = enIdentificationType.ID.GetLabel();
                ltrIdNumber.Text = "Αριθμός Ταυτότητας:";
                lblIdNumber.Text = Entity.LegalPersonIdentificationNumber;
                lblIdIssuer.Text = Entity.LegalPersonIdentificationIssuer;
                lblIdIssueDate.Text = Entity.LegalPersonIdentificationIssueDate.Value.ToString("dd/MM/yyyy");
            }
            else if (Entity.LegalPersonIdentificationType == enIdentificationType.Passport)
            {
                lblIdType.Text = enIdentificationType.Passport.GetLabel();
                ltrIdNumber.Text = "Αριθμός Διαβατηρίου:";
                lblIdNumber.Text = Entity.LegalPersonIdentificationNumber;
                idIssuer.Visible = false;
                idIssueDate.Visible = false;
            }
            else
            {
                idType.Visible = false;
                idNumber.Visible = false;
                idIssuer.Visible = false;
                idIssueDate.Visible = false;
            }

            //Στοιχεία Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            lblContactName.Text = Entity.ContactName;
            lblContactPhone.Text = Entity.ContactPhone;
            lblContactMobilePhone.Text = Entity.ContactMobilePhone;
            lblContactEmail.Text = Entity.ContactEmail;

            //Στοιχεία Αναπληρωτή Υπευθύνου Φορέα Υποδοχής Πρακτικής Άσκησης
            lblAlternateContactName.Text = Entity.AlternateContactName;
            lblAlternateContactPhone.Text = Entity.AlternateContactPhone;
            lblAlternateContactMobilePhone.Text = Entity.AlternateContactMobilePhone;
            lblAlternateContactEmail.Text = Entity.AlternateContactEmail;

        }
    }
}