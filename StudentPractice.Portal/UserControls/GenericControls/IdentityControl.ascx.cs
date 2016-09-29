using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class IdentityControl : System.Web.UI.UserControl, IScriptControl
    {
        #region [ Control Inits ]

        protected void rblIdType_Init(object sender, EventArgs e)
        {
            foreach (enIdentificationType item in Enum.GetValues(typeof(enIdentificationType)))
            {
                if (item == enIdentificationType.None)
                    continue;
                rblIdType.Items.Add(new ListItem(item.GetLabel(), item.GetValue().ToString()));
            }
            rblIdType.Items[0].Selected = true;
        }

        #endregion

        #region [ Data Binding ]

        public IdentificationDetails Fill()
        {
            if (ReadOnly)
                return null;

            IdentificationDetails entity = new IdentificationDetails()
            {
                IdType = (enIdentificationType)int.Parse(rblIdType.SelectedItem.Value),
                IdNumber = txtIdNumber.Text.ToNull()
            };

            entity.IdIssueDate = entity.IdType == enIdentificationType.ID 
                ? DateTime.Parse(txtIdIssueDate.Text)
                : (DateTime?)null;

            entity.IdIssuer = entity.IdType == enIdentificationType.ID 
                ? txtIdIssuer.Text.ToNull()
                : null ;
            return entity;
        }

        public void Bind(IdentificationDetails entity)
        {
            if (entity == null)
                return;

            txtIdNumber.Text = entity.IdNumber;
            txtIdIssuer.Text = entity.IdIssuer;
            rblIdType.SelectedValue = entity.IdType.ToString("D");
            if (entity.IdIssueDate.HasValue)
                txtIdIssueDate.Text = entity.IdIssueDate.Value.ToString(Resources.GlobalProvider.DateFormat);
        }

        #endregion

        #region [ Control Methods ]

        protected override void OnPreRender(EventArgs e)
        {
            if (!ReadOnly)
            {
                cvIssuer.ClientValidationFunction = string.Format("$find('{0}').validateIssuer", tbIdentificationInfo.ClientID);
                cvNumber.ClientValidationFunction = string.Format("$find('{0}').validateNumber", tbIdentificationInfo.ClientID);
                cuvIssueDate.ClientValidationFunction = string.Format("$find('{0}').validateIssueDate", tbIdentificationInfo.ClientID);
            }
            if (ScriptManager.GetCurrent(Page) == null)
                throw new NullReferenceException("A ScriptManager control must exist on the Page");
            else
            {
                ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
            }

            if (string.IsNullOrEmpty(ValidationGroup))
            {
                foreach (var v in Controls.OfType<BaseValidator>())
                    v.ValidationGroup = tbIdentificationInfo.ClientID + "_vgIdDetails";
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);
            base.Render(writer);
        }

        #endregion

        protected void cvMaxDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime issueDate;
            var idType = (enIdentificationType)int.Parse(rblIdType.SelectedItem.Value);
            if (idType == enIdentificationType.ID)
            {
                if (DateTime.TryParse(txtIdIssueDate.Text, out issueDate))
                {
                    args.IsValid = DateTime.Now.Date > issueDate.Date;
                }
                else
                    args.IsValid = false;
            }
        }

        public string ValidationGroup
        {
            get { return cvIssuer.ValidationGroup; }
            set
            {
                foreach (var v in this.RecursiveOfType<BaseValidator>())
                {
                    v.ValidationGroup = value;
                }
            }
        }

        public bool ReadOnly
        {
            private get { return !txtIdNumber.Enabled; }
            set
            {
                txtIdIssueDate.Enabled =
                txtIdNumber.Enabled =
                rblIdType.Enabled =
                txtIdIssuer.Enabled =
                lnkSelectDate.Visible =
                cvIssueDate.Enabled =
                cvMaxDate.Enabled =
                ceSelectDate.Enabled = !value;
            }
        }

        public string ClientInstanceName { get { return tbIdentificationInfo.ClientID; } }

        public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor scd = new ScriptControlDescriptor("StudentPractice.Portal.UserControls.GenericControls.IdentityControl", tbIdentificationInfo.ClientID);
            scd.AddElementProperty("rblist", rblIdType.ClientID);
            scd.AddProperty("txtIdIssuer", txtIdIssuer.ClientID);
            scd.AddProperty("txtIdIssueDate", txtIdIssueDate.ClientID);
            scd.AddProperty("txtIdNumber", txtIdNumber.ClientID);
            scd.AddProperty("lblIdNumber", lblIdNumber.ClientID);
            if (rblIdType.SelectedItem != null)
                scd.AddProperty("idType", rblIdType.SelectedItem.Value);

            yield return scd;
        }

        public IEnumerable<ScriptReference> GetScriptReferences()
        {
            yield return new ScriptReference("~/UserControls/GenericControls/IdentityControl.js");
        }


    }
}