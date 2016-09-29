using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class Default : BaseEntityPortalPage<InternshipProvider>
    {
        protected override void Fill()
        {
            var providerRepository = new InternshipProviderRepository(UnitOfWork);

            Entity = providerRepository.FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            if (!Entity.IsMasterAccount)
                providerRepository.Load(Entity.MasterAccountID.Value).SaveMasterCountryIDToCurrentContext();
            else
                Entity.SaveMasterCountryIDToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            if (!Entity.IsMasterAccount)
                liProviderUSers.Visible = false;

            btnPrintCertification.Visible = Entity.IsEmailVerified && Entity.VerificationStatus == enVerificationStatus.NotVerified;

            var atlasQsExists = new SubmittedQuestionnaireRepository(UnitOfWork).Exists(Entity.ID, enQuestionnaireType.ProviderForAtlas);
            var hasCompletedPositions = new InternshipProviderRepository(UnitOfWork).HasCompletedPositions(Entity.ID);
            int? masterCountryID = null;
            if (!Entity.IsMasterAccount)
                masterCountryID = Context.LoadMasterCountryID() ?? new InternshipProviderRepository(UnitOfWork).Load(Entity.MasterAccountID.Value).CountryID;

            if (hasCompletedPositions && !atlasQsExists)
                ClientScript.RegisterStartupScript(GetType(), "showEvaluation", "showEvaluationPopup();", true);
        }

        private bool IsForeign(int? countryID)
        {
            if (!countryID.HasValue)
                return false;

            return (countryID.Value != StudentPracticeConstants.GreeceCountryID && countryID.Value != StudentPracticeConstants.CyprusCountryID);
        }
    }
}