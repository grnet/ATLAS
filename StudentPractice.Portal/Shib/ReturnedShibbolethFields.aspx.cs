using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.Shib
{
    public partial class ReturnedShibbolethFields : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShibDetails shibDetails = ShibDetails.Create(Request.ServerVariables);

                lblFullName.Text = shibDetails.FullName;
                lblFirstName.Text = shibDetails.FirstName;
                lblLastName.Text = shibDetails.LastName;
                lblHomeOrganization.Text = shibDetails.HomeOrganization;
                lblPersonalUniqueCode.Text = shibDetails.PersonalUniqueCode;
                lblUserName.Text = shibDetails.Username;

                int academicID;
                if (int.TryParse(shibDetails.AcademicID, out academicID))
                {
                    if (!string.IsNullOrWhiteSpace(shibDetails.FullName) &&
                        !string.IsNullOrWhiteSpace(shibDetails.FirstName) &&
                        !string.IsNullOrWhiteSpace(shibDetails.LastName) &&
                        !string.IsNullOrWhiteSpace(shibDetails.Username) &&
                        !string.IsNullOrWhiteSpace(shibDetails.StudentCode) &&
                        CacheManager.Academics.Get(academicID) != null)
                    {
                        lblMessage.ForeColor = Color.Green;
                        lblMessage.Text = "Ο ΜΗΧΑΝΙΣΜΟΣ ΤΟΥ SHIBBOLETH ΕΙΝΑΙ ΣΩΣΤΑ ΡΥΘΜΙΣΜΕΝΟΣ";
                    }
                }
            }

            base.OnLoad(e);
        }
    }
}