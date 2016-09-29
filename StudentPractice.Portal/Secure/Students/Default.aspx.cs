using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using StudentPractice.BusinessModel;
using System.Web.Profile;

using StudentPractice.Portal.Controls;
using System.Collections.Generic;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class Default : BaseEntityPortalPage<Student>
    {
        protected override void Fill()
        {
            Entity = new StudentRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsContactInfoCompleted)
            {
                Response.Redirect("~/Secure/Students/ContactInfoDetails.aspx");
            }

            var atlasQsExists = new SubmittedQuestionnaireRepository(UnitOfWork).Exists(Entity.ID, enQuestionnaireType.StudentForAtlas);
            var hasCompletedPositions = new StudentRepository(UnitOfWork).HasCompletedPositions(Entity.ID);
            if (hasCompletedPositions && !atlasQsExists)
                ClientScript.RegisterStartupScript(GetType(), "showEvaluation", "showEvaluationPopup();", true);
        }
    }
}
