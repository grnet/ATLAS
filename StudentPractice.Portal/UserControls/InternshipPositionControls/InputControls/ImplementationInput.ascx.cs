using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;
using System.Web;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls
{
    public partial class ImplementationInput : BaseEntityUserControl<InternshipPosition>
    {
        #region [ Properties ]

        public int AcademicID { get; set; }

        #endregion

        #region [ Control Inits ]

        //ToFix: FundingType
        //protected void ddlFundingType_Init(object sender, EventArgs e)
        //{
        //    foreach (enFundingType item in Enum.GetValues(typeof(enFundingType)))
        //    {
        //        ddlFundingType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
        //    }
        //}

        #endregion

        #region [ Databind Methods ]

        protected void ddlPreAssignAcademics_Init(object sender, EventArgs e)
        {
            var office = HttpContext.Current.LoadOffice();
            var student = HttpContext.Current.LoadStudent();


            if (student.PreviousAcademicID.HasValue)
            {
                ddlPreAssignAcademics.Items.Add(new ListItem()
                {
                    Value = student.PreviousAcademicID.Value.ToString(),
                    Text = CacheManager.Academics.Get(student.PreviousAcademicID.Value).Department
                });
            }

            if (student.AcademicID.HasValue && office.Academics.Any(x => x.ID == student.AcademicID.Value))
            {
                ddlPreAssignAcademics.Items.Add(new ListItem()
                {
                    Value = student.AcademicID.Value.ToString(),
                    Text = CacheManager.Academics.Get(student.AcademicID.Value).Department
                });
            }

        }

        public override InternshipPosition Fill(InternshipPosition entity)
        {
            if (entity == null)
                entity = new InternshipPosition();

            var student = entity.AssignedToStudent;

            //Στοιχεία Φοιτητή
            if (chbxIsNameLatin.Checked)
            {
                student.IsNameLatin = true;

                if (student.GreekFirstName != txtLatinFirstName.Text.ToNull())
                    student.GreekFirstName = txtLatinFirstName.Text.ToNull();

                if (student.GreekLastName != txtLatinLastName.Text.ToNull())
                    student.GreekLastName = txtLatinLastName.Text.ToNull();
            }
            else
            {
                student.IsNameLatin = false;

                if (student.GreekFirstName != txtGreekFirstName.Text.ToNull())
                    student.GreekFirstName = txtGreekFirstName.Text.ToNull();

                if (student.GreekLastName != txtGreekLastName.Text.ToNull())
                    student.GreekLastName = txtGreekLastName.Text.ToNull();
            }

            if (student.LatinFirstName != txtLatinFirstName.Text.ToNull())
                student.LatinFirstName = txtLatinFirstName.Text.ToNull();

            if (student.LatinLastName != txtLatinLastName.Text.ToNull())
                student.LatinLastName = txtLatinLastName.Text.ToNull();

            //Στοιχεία Εκτέλεσης Πρακτικής Άσκησης

            DateTime startDate;
            if (DateTime.TryParse(txtStartDate.Text, out startDate) && entity.ImplementationStartDate != startDate)
            {
                entity.ImplementationStartDate = startDate;
            }

            DateTime endDate;
            if (DateTime.TryParse(txtEndDate.Text, out endDate) && entity.ImplementationEndDate != endDate)
            {
                entity.ImplementationEndDate = endDate;
            }

            //ToFix: FundingType
            entity.FundingType = enFundingType.ESPA;
            //entity.FundingTypeInt = ddlFundingType.GetSelectedInteger().Value;

            if (entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                entity.PreAssignedForAcademicID = ddlPreAssignAcademics.GetInteger().Value;
                entity.InternshipPositionGroup.StartDate = entity.ImplementationStartDate;
                entity.InternshipPositionGroup.EndDate = entity.ImplementationEndDate;

                TimeSpan ts = entity.ImplementationEndDate.Value.Subtract(entity.ImplementationStartDate.Value);
                entity.InternshipPositionGroup.Duration = ts.Days / 7;

                entity.CompletionComments = txtCompletionComments.Text;
                //entity.PositionStatus = enPositionStatus.Completed;
            }

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
            {
                return;
            }

            var student = Entity.AssignedToStudent;

            //Στοιχεία Φοιτητή
            chbxIsNameLatin.Checked = student.IsNameLatin.GetValueOrDefault();
            txtGreekFirstName.Text = student.GreekFirstName;
            txtGreekLastName.Text = student.GreekLastName;
            txtLatinFirstName.Text = student.LatinFirstName;
            txtLatinLastName.Text = student.LatinLastName;

            //Στοιχεία Εκτέλεσης Πρακτικής Άσκησης
            txtStartDate.Text = Entity.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
            txtEndDate.Text = Entity.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
            //ToFix: FundingType
            //ddlFundingType.SelectedValue = Entity.FundingTypeInt.ToString();

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                rowPreAssignedAcademic.Visible = true;
                rowCompletionComments.Visible = true;
                txtCompletionComments.Text = Entity.CompletionComments;
                ltrStartDate.Text = "Ημ/νία έναρξης:";
                ltrEndDate.Text = "Ημ/νία λήξης:";

                if (Entity.PreAssignedForAcademicID.HasValue)
                    ddlPreAssignAcademics.SelectedValue = Entity.PreAssignedForAcademicID.Value.ToString();
                if (AcademicID != 0)
                    ddlPreAssignAcademics.SelectedValue = AcademicID.ToString();
            }
        }

        public void FillReadOnlyFields(Student s)
        {
            lblFullNameFromLDAP.Text = s.OriginalFirstName + " " + s.OriginalLastName;

            if (!string.IsNullOrEmpty(s.GreekFirstName))
            {
                phFieldChecking.Visible = true;
            }

            chbxIsNameLatin.Checked = s.IsNameLatin.GetValueOrDefault();
            txtGreekFirstName.Text = s.GreekFirstName;
            txtGreekLastName.Text = s.GreekLastName;
            txtLatinFirstName.Text = s.LatinFirstName;
            txtLatinLastName.Text = s.LatinLastName;

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                rowPreAssignedAcademic.Visible = true;
                rowCompletionComments.Visible = true;
                txtCompletionComments.Text = Entity.CompletionComments;
                ltrStartDate.Text = "Ημ/νία έναρξης:";
                ltrEndDate.Text = "Ημ/νία λήξης:";

                rfvStartDate.ErrorMessage = "Το πεδίο 'Hμ/νία έναρξης' είναι υποχρεωτικό";
                rfvEndDate.ErrorMessage = "Το πεδίο 'Hμ/νία λήξης' είναι υποχρεωτικό";

                cvMinEndDate.ErrorMessage = "Η 'Hμ/νία λήξης' πρέπει να είναι μεταγενέστερη της ημ/νίας έναρξης";
                cvMinEndDate.Text = "Πρέπει να εισάγετε ημ/νία μεταγενέστερη της ημ/νίας έναρξης";

                if (Entity.PreAssignedForAcademicID.HasValue)
                    ddlPreAssignAcademics.SelectedValue = Entity.PreAssignedForAcademicID.Value.ToString();
                if (AcademicID != 0)
                    ddlPreAssignAcademics.SelectedValue = AcademicID.ToString();
            }

        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvStartDate.ValidationGroup; }
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

        protected void cvMinEndDate_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            DateTime startDate;
            DateTime endDate;
            if (DateTime.TryParse(txtStartDate.Text.ToNull(), out startDate) &&
                DateTime.TryParse(txtEndDate.Text.ToNull(), out endDate) &&
                endDate < startDate)
            {
                e.IsValid = false;
            }

        }

        protected void cvMaxEndDate_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;

            DateTime endDate;
            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice
                && DateTime.TryParse(txtEndDate.Text.ToNull(), out endDate) && endDate >= DateTime.Now.Date)
            {
                e.IsValid = false;
            }
        }

        #endregion

        #region [ Overrides ]

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtGreekFirstName.Attributes["onkeyup"] = "Imis.Lib.ToElUpperForNames(this)";
                txtGreekLastName.Attributes["onkeyup"] = "Imis.Lib.ToElUpperForNames(this)";
                txtLatinFirstName.Attributes["onkeyup"] = "Imis.Lib.ToEnUpperForNames(this)";
                txtLatinLastName.Attributes["onkeyup"] = "Imis.Lib.ToEnUpperForNames(this)";
            }

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