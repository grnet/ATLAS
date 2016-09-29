using System;
using System.Web.Security;
using System.Web.UI;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Linq;
using System.Web;
using StudentPractice.Mails;
using System.Web.Services;
using Imis.Domain;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class OfficeDetails : BaseEntityPortalPage<InternshipOffice>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        #endregion

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            ddlDepartment.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in Entity.Academics)
            {
                ddlDepartment.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            lblUserName.Text = Entity.UserName;
            txtEmail.Text = Entity.Email;

            if (Entity.IsEmailVerified)
            {
                btnSendEmailVerificationCode.Visible = false;
            }

            if (!Page.IsPostBack)
                ViewState["EntityID"] = Entity.ID;

            if (!Entity.IsMasterAccount)
            {
                dxTabs.TabPages.FindByName("tabOfficeAcademics").ClientVisible = false;
                dxTabs.TabPages.FindByName("tabOffice").ClientVisible = false;
                return;
            }

            if (!Entity.CanViewAllAcademics.HasValue || Entity.CanViewAllAcademics.Value)
            {
                mvAcademics.SetActiveView(vCanViewAllAcademics);
            }
            else
            {
                mvAcademics.SetActiveView(vCanViewCertainAcademics);

                gvAcademics.DataSource = Entity.Academics;
                gvAcademics.DataBind();
            }

            if (Entity.VerificationStatus == enVerificationStatus.Verified)
            {
                phRestrictAcademics.Visible = false;

                btnAddAllAcademics.Visible = false;
                btnAddAcademics.Visible = false;
                gvAcademics.Columns[4].Visible = false;

                lblRepresentingAcademics.Text = "Το Γραφείο Πρακτικής Άσκησης που δημιουργήσατε εκπροσωπεί τα παρακάτω Τμήματα:";
            }
            else
            {
                lblRepresentingAcademics.Text = "Επιλέξτε το Τμήμα ή τα Τμήματα τα οποία εκπροσωπεί το Γραφείο Πρακτικής που δημιουργήσατε.";
                dxTabs.TabPages.FindByName("tabPositionRules").ClientVisible = false;
            }

            divAccountVerified.Visible = Entity.VerificationStatus == enVerificationStatus.Verified;


            ucOfficeInput.Entity = Entity;

            if (!Page.IsPostBack)
            {
                ucOfficeInput.Bind();
            }

            if (Entity.VerificationStatus == enVerificationStatus.CannotBeVerified)
            {
                btnUpdateOffice.Visible = false;
            }

            base.OnLoad(e);
        }

        protected void gvAcademics_CustomDataCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var academicID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                var academic = new AcademicRepository(UnitOfWork).Load(academicID);

                Entity.Academics.Remove(academic);

                if (Entity.Academics.Count == 0)
                {
                    Entity.CanViewAllAcademics = true;
                    Entity.OfficeType = enOfficeType.Institutional;
                    if (Entity.InstitutionID.HasValue)
                    {
                        var acads = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                        foreach (var item in acads)
                            Entity.Academics.Add(item);
                    }
                }
                else if (Entity.Academics.Count == 1)
                {
                    Entity.CanViewAllAcademics = false;
                    Entity.OfficeType = enOfficeType.Departmental;
                }
                else
                {
                    Entity.CanViewAllAcademics = false;
                    Entity.OfficeType = enOfficeType.MultipleDepartmental;
                }

                UnitOfWork.Commit();
            }

            if (Entity.CanViewAllAcademics.Value)
            {
                e.Result = ResolveClientUrl("~/Secure/InternshipOffices/OfficeDetails.aspx");
            }
            else
            {
                e.Result = "DATABIND";
            }
        }

        protected void gvAcademics_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvAcademics.DataSource = Entity.Academics;
            gvAcademics.DataBind();
        }

        #region [ Button Handlers ]

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvAcademics.DataSource = Entity.Academics;
            gvAcademics.DataBind();
        }

        protected void btnAddAllAcademics_Click(object sender, EventArgs e)
        {
            var academics = Entity.Academics.ToList();

            for (int i = academics.Count - 1; i >= 0; i--)
            {
                Entity.Academics.Remove(academics[i]);
            }

            Entity.CanViewAllAcademics = true;
            Entity.OfficeType = enOfficeType.Institutional;
            if (Entity.InstitutionID.HasValue)
            {
                var acads = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                foreach (var item in acads)
                    Entity.Academics.Add(item);
            }
            UnitOfWork.Commit();

            mvAcademics.SetActiveView(vCanViewAllAcademics);
        }

        protected void btnUpdateOffice_Click(object sender, EventArgs e)
        {
            if (Entity.ID != (int)ViewState["EntityID"])
                Response.Redirect("~/Default.aspx");

            if (!Page.IsValid)
                return;

            int instID = Entity.InstitutionID.HasValue ? Entity.InstitutionID.Value : -1;
            ucOfficeInput.Fill(Entity);
            if (Entity.InstitutionID != instID)
            {
                Entity.Academics.Clear();
                var academics = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                foreach (var item in academics)
                    Entity.Academics.Add(item);
            }

            UnitOfWork.Commit();

            ucOfficeInput.Bind();
            fm.Text = "Η ενημέρωση των Στοιχείων του Γραφείου πραγματοποιήθηκε επιτυχώς";
        }

        protected void btnSendEmailVerificationCode_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Uri baseURI;
            if (Config.IsSSL)
            {
                baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }
            else
            {
                baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + Entity.EmailVerificationCode);

            var email = MailSender.SendEmailVerification(Entity.ID, Entity.Email, Entity.ContactName, uri);
            UnitOfWork.MarkAsNew(email);
            UnitOfWork.Commit();
        }

        #endregion

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlDepartment.SelectedValue))
            {
                this.ucPositionRules.MemoValue = (Entity.Academics.First(x => x.ID == Convert.ToInt32(this.ddlDepartment.SelectedValue))).PositionRules;
            }
            else
            {
                this.ucPositionRules.MemoValue = "";
            }
        }

        protected void btnUpdatePositionRules_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlDepartment.SelectedValue))
            {
                (Entity.Academics.First(x => x.ID == Convert.ToInt32(this.ddlDepartment.SelectedValue))).PositionRules = this.ucPositionRules.MemoValue;
                UnitOfWork.Commit();
                fm.Text = "Η ενημέρωση της περιγραφής της πρακτικής άσκησης του Τμήματος πραγματοποιήθηκε επιτυχώς";
            }
            else
            {
                fm.Text = "Πρέπει να επιλέξετε κάποιο τμήμα πριν προχωρήσετε σε αλλαγές.";
            }
        }

    }
}
