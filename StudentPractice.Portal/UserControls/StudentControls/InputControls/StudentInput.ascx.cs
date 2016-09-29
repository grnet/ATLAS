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
    public partial class StudentInput : BaseEntityUserControl<Student>
    {
        #region [ Databind Methods ]

        public override Student Fill(Student entity)
        {
            //Στοιχεία Φοιτητή
            if (chbxIsNameLatin.Checked)
            {
                entity.IsNameLatin = true;

                if (entity.GreekFirstName != txtLatinFirstName.Text.ToNull())
                    entity.GreekFirstName = txtLatinFirstName.Text.ToNull();

                if (entity.GreekLastName != txtLatinLastName.Text.ToNull())
                    entity.GreekLastName = txtLatinLastName.Text.ToNull();
            }
            else
            {
                entity.IsNameLatin = false;

                if (entity.GreekFirstName != txtGreekFirstName.Text.ToNull())
                    entity.GreekFirstName = txtGreekFirstName.Text.ToNull();

                if (entity.GreekLastName != txtGreekLastName.Text.ToNull())
                    entity.GreekLastName = txtGreekLastName.Text.ToNull();
            }

            if (entity.LatinFirstName != txtLatinFirstName.Text.ToNull())
                entity.LatinFirstName = txtLatinFirstName.Text.ToNull();

            if (entity.LatinLastName != txtLatinLastName.Text.ToNull())
                entity.LatinLastName = txtLatinLastName.Text.ToNull();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
            {
                return;
            }

            //Στοιχεία Φοιτητή
            chbxIsNameLatin.Checked = Entity.IsNameLatin.GetValueOrDefault();
            txtGreekFirstName.Text = Entity.GreekFirstName;
            txtGreekLastName.Text = Entity.GreekLastName;
            txtLatinFirstName.Text = Entity.LatinFirstName;
            txtLatinLastName.Text = Entity.LatinLastName;
        }

        public void FillReadOnlyFields(Student s)
        {
            lblFullNameFromLDAP.Text = s.OriginalFirstName + " " + s.OriginalLastName;
            chbxIsNameLatin.Checked = s.IsNameLatin.GetValueOrDefault();
            txtGreekFirstName.Text = s.GreekFirstName;
            txtGreekLastName.Text = s.GreekLastName;
            txtLatinFirstName.Text = s.LatinFirstName;
            txtLatinLastName.Text = s.LatinLastName;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvLatinFirstName.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        protected void cvGreekName_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (!chbxIsNameLatin.Checked)
            {
                e.IsValid = !string.IsNullOrEmpty(e.Value);
            }
            else
            {
                e.IsValid = true;
            }
        }

        #endregion

        #region [ Overrides ]

        protected override void OnPreRender(EventArgs e)
        {
            txtGreekFirstName.Attributes["onkeyup"] = "Imis.Lib.ToElUpperForNames(this)";
            txtGreekLastName.Attributes["onkeyup"] = "Imis.Lib.ToElUpperForNames(this)";
            txtLatinFirstName.Attributes["onkeyup"] = "Imis.Lib.ToEnUpperForNames(this)";
            txtLatinLastName.Attributes["onkeyup"] = "Imis.Lib.ToEnUpperForNames(this)";

            base.OnPreRender(e);
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
    }
}