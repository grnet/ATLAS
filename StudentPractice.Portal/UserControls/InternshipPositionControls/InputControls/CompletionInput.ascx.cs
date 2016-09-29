using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Imis.Web.Controls;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls
{
    public partial class CompletionInput : BaseEntityUserControl<InternshipPosition>
    {
        public int CompletionVerdict
        {
            get { return int.Parse(rbtlCompletionVerdict.SelectedValue); }
        }

        #region [ Control Inits ]

        protected void rbtlCompletionVerdict_Init(object sender, EventArgs e)
        {
            rbtlCompletionVerdict.Items.Add(new ListItem("Η πρακτική ολοκληρώθηκε επιτυχώς", ((int)enPositionStatus.Completed).ToString("D")));
            rbtlCompletionVerdict.Items.Add(new ListItem("Η πρακτική ΔΕΝ ολοκληρώθηκε", ((int)enPositionStatus.Canceled).ToString("D")));
        }

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

        public override InternshipPosition Fill(InternshipPosition entity)
        {
            if (entity == null)
                entity = new InternshipPosition();

            //Στοιχεία Ολοκλήρωσης
            int completionVerdict;
            if (int.TryParse(rbtlCompletionVerdict.SelectedItem.Value, out completionVerdict) && completionVerdict > 0)
            {
                if (entity.CompletionComments != txtCompletionComments.Text.ToNull())
                    entity.CompletionComments = txtCompletionComments.Text.ToNull();

                if (completionVerdict == (int)enPositionStatus.Completed)
                {
                    DateTime implementationStartDate;
                    if (DateTime.TryParse(txtImplementationStartDate.Text, out implementationStartDate))
                    {
                        if (entity.ImplementationStartDate != implementationStartDate)
                            entity.ImplementationStartDate = implementationStartDate;
                    }

                    DateTime implementationEndDate;
                    if (DateTime.TryParse(txtImplementationEndDate.Text, out implementationEndDate))
                    {
                        if (entity.ImplementationEndDate != implementationEndDate)
                            entity.ImplementationEndDate = implementationEndDate;
                    }
                }

                //ToFix: FundingType
                entity.FundingType = enFundingType.ESPA;
                //entity.FundingTypeInt = ddlFundingType.GetSelectedInteger().Value;
            }

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Ολοκλήρωσης
            if (Entity.PositionStatus == enPositionStatus.Canceled)
            {
                rbtlCompletionVerdict.SelectedValue = ((int)enPositionStatus.Canceled).ToString("D");
            }
            else
            {
                rbtlCompletionVerdict.SelectedValue = ((int)enPositionStatus.Completed).ToString("D");
                txtImplementationStartDate.Text = Entity.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
                txtImplementationEndDate.Text = Entity.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
                //ToFix: FundingType
                //ddlFundingType.SelectedValue = Entity.FundingTypeInt.ToString();
            }

            txtCompletionComments.Text = Entity.CompletionComments;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rbtlCompletionVerdict.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        #endregion
    }
}