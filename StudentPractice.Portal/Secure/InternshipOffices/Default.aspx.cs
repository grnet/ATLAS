using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class Default : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            if (Entity.IsMasterAccount)
            {
                mvDefault.SetActiveView(vMasterOffice);

                if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
                {
                    Response.Redirect("OfficeDetails.aspx");
                }

                btnPrintCertification.Visible = Entity.IsEmailVerified && Entity.VerificationStatus == enVerificationStatus.NotVerified;
            }
            else
            {
                mvDefault.SetActiveView(vOfficeUser);
            }

            phVideo.Visible = phVideoUser.Visible = Entity.VerificationStatus == enVerificationStatus.Verified;

            var atlasQsExists = new SubmittedQuestionnaireRepository(UnitOfWork).Exists(Entity.ID, enQuestionnaireType.OfficeForAtlas);
            var hasCompletedPositions = new InternshipOfficeRepository(UnitOfWork).HasCompletedPositions(Entity);
            if (hasCompletedPositions && !atlasQsExists)
                ClientScript.RegisterStartupScript(GetType(), "showEvaluation", "showEvaluationPopup();", true);
        }
    }
}