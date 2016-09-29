using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:AFMInput runat=server></{0}:AFMInput>")]
    public class AFMInput : CompositeControl, ITextControl
    {
        private TextBox txtAFM = new TextBox();
        private RequiredFieldValidator rfvAFM = new RequiredFieldValidator();
        private CustomValidator valAFM = new CustomValidator();

        public AFMInput()
        {
            txtAFM.ID = "txtAFM";
            rfvAFM.ID = "rfvAFM";
            valAFM.ID = "valAFM";

            txtAFM.MaxLength = 10;

            rfvAFM.ControlToValidate = txtAFM.ID;
            rfvAFM.Text = "<img src=\"/_img/error.gif\" class=\"errortip\" title=\"Το πεδίο είναι υποχρεωτικό\" />";
            rfvAFM.ErrorMessage = "Το πεδίο 'Α.Φ.Μ.' είναι υποχρεωτικό";

            rfvAFM.Display = ValidatorDisplay.Dynamic;

            valAFM.ControlToValidate = txtAFM.ID;
            valAFM.Text = "<img src=\"/_img/error.gif\" class=\"errortip\" title=\"Το Α.Φ.Μ. που δώσατε δεν είναι έγκυρο\" />";
            valAFM.ErrorMessage = "Το Α.Φ.Μ. που δώσατε δεν είναι έγκυρο";
            valAFM.Display = ValidatorDisplay.Dynamic;
            valAFM.ServerValidate += new ServerValidateEventHandler(valAFM_ServerValidate);
            valAFM.ClientValidationFunction = "Imis.Lib.CheckAfm";
            Controls.Add(txtAFM);
            Controls.Add(rfvAFM);
            Controls.Add(valAFM);
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get { return txtAFM.Text; }
            set { txtAFM.Text = value; }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return txtAFM.ReadOnly; }
            set
            {
                txtAFM.ReadOnly = value;
                valAFM.Enabled = !value;
                rfvAFM.Enabled = !value;
            }
        }

        public override string CssClass
        {
            get
            {
                return txtAFM.CssClass;
            }
            set
            {
                txtAFM.CssClass = value;
            }
        }

        public override Unit Width
        {
            get
            {
                return txtAFM.Width;
            }
            set
            {
                txtAFM.Width = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                return rfvAFM.ValidationGroup;
            }
            set
            {
                rfvAFM.ValidationGroup = value;
                valAFM.ValidationGroup = value;
            }
        }

        void valAFM_ServerValidate(object source, ServerValidateEventArgs e)
        {
            e.IsValid = CheckAFM(e.Value);
        }

        /// <summary>
        /// CheckAFM: Ελέγχει αν ένα ΑΦΜ είναι σωστό
        /// </summary>
        /// <param name="cAfm">Το ΑΦΜ που θα ελέγξουμε</param>
        /// <returns>true = ΑΦΜ σωστό, false = ΑΦΜ Λάθος</returns>
        public static bool CheckAFM(string cAfm)
        {
            int nExp = 1;

            // Ελεγχος αν περιλαμβάνει μόνο γράμματα

            long nAfm;

            if (!long.TryParse(cAfm, out nAfm))
                return false;


            // Ελεγχος μήκους ΑΦΜ

            cAfm = cAfm.Trim();

            int nL = cAfm.Length;

            if (nL != 9) return false;



            //Υπολογισμός αν το ΑΦΜ είναι σωστό

            int nSum = 0;

            int xDigit = 0;

            int nT = 0;

            for (int i = nL - 2; i >= 0; i--)
            {

                xDigit = int.Parse(cAfm.Substring(i, 1));

                nT = xDigit * (int)(Math.Pow(2, nExp));

                nSum += nT;

                nExp++;

            }



            xDigit = int.Parse(cAfm.Substring(nL - 1, 1));

            nT = nSum / 11;

            int k = nT * 11;

            k = nSum - k;



            if (k == 10) k = 0;

            if (xDigit != k) return false;

            return true;
        }
    }
}
