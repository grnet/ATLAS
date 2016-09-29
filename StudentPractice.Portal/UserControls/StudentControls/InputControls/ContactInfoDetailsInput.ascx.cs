using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Drawing;

namespace StudentPractice.Portal.UserControls.StudentControls.InputControls
{
    public partial class ContactInfoDetailsInput : BaseEntityUserControl<Student>
    {
        #region [ Databind Methods ]

        public override Student Fill(Student entity)
        {
            if (entity == null)
            {
                entity = new Student();
                entity.UsernameFromLDAP = Guid.NewGuid().ToString();
            }

            //Στοιχεία Έπικοινωνίας 
            if (entity.Email != txtEmail.Text.ToNull())
                entity.ContactEmail = txtEmail.Text.ToNull();

            if (entity.ContactMobilePhone != txtMobilePhone.Text.ToNull())
                entity.ContactMobilePhone = txtMobilePhone.Text.ToNull();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Φοιτητή
            txtEmail.Text = Entity.ContactEmail;
            txtMobilePhone.Text = Entity.ContactMobilePhone;
        }

        #endregion

        #region [ Validation ]

        public string ValidationGroup
        {
            get { return rfvEmail.ValidationGroup; }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        #endregion
    }
}