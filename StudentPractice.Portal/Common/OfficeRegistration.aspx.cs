using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Common
{
    public partial class OfficeRegistration : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOffice();
            Entity.UsernameFromLDAP = Guid.NewGuid().ToString();
        }

        protected override void OnInit(EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Config.OfficeRegistrationAllowed)
                mvRegistration.SetActiveView(vNotAllowed);

            base.OnLoad(e);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            ucOfficeInput.Fill(Entity);

            string username = null;

            try
            {
                username = ucRegisterUserInput.CreateUser();
                if (string.IsNullOrEmpty(username))
                    throw new MembershipCreateUserException("CreateUser returned empty username");
            }
            catch (MembershipCreateUserException)
            {
                return;
            }

            if (Entity.CanViewAllAcademics.Value)
            {
                Entity.OfficeType = enOfficeType.Institutional;
            }
            else
            {
                Entity.OfficeType = enOfficeType.None;
            }

            Entity.DeclarationType = enReporterDeclarationType.FromRegistration;
            Entity.RegistrationType = enRegistrationType.Membership;
            Entity.IsActive = true;
            Entity.IsMasterAccount = true;
            Entity.VerificationStatus = enVerificationStatus.NotVerified;
            Entity.UserName = Entity.CreatedBy = ucRegisterUserInput.Username;
            Entity.Email = ucRegisterUserInput.Email;

            Entity.IsEmailVerified = false;
            Entity.EmailVerificationCode = Guid.NewGuid().ToString();


            try
            {
                if (Entity.OfficeType == enOfficeType.Institutional && Entity.InstitutionID.HasValue)
                {
                    var academics = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                    foreach (var item in academics)
                        Entity.Academics.Add(item);
                }
                UnitOfWork.MarkAsNew(Entity);
                UnitOfWork.Commit();

                var provider = Roles.Provider as StudentPracticeRoleProvider;
                provider.AddUsersToRoles(new[] { Entity.UserName }, new[] { RoleNames.MasterOffice });
            }
            catch (Exception)
            {
                Membership.DeleteUser(username);
                throw;
            }

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

            AuthenticationService.LoginReporter(Entity);

            Response.Redirect("~/Secure/InternshipOffices/Default.aspx");
        }
    }
}
