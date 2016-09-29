using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Common
{
    public partial class Questionnaire : BaseEntityPortalPage<Reporter>
    {
        #region [ QueryString Parsing ]

        protected enQuestionnaireType QuestionnaireType
        {
            get { return (enQuestionnaireType)Enum.Parse(typeof(enQuestionnaireType), Request.QueryString["qType"]); }
        }

        protected int? EntityID
        {
            get
            {
                int eID = 0;
                if (int.TryParse(Request.QueryString["entityID"], out eID))
                    return eID;
                else
                    return null;
            }
        }

        protected int? PositionID
        {
            get
            {
                int pID = 0;
                if (int.TryParse(Request.QueryString["positionID"], out pID))
                    return pID;
                else
                    return null;
            }
        }

        #endregion

        protected SubmittedQuestionnaire Existing { get; set; }

        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(User.Identity.Name);

            var repo = new SubmittedQuestionnaireRepository(UnitOfWork);
            switch (QuestionnaireType)
            {
                case enQuestionnaireType.StudentForAtlas:
                case enQuestionnaireType.OfficeForAtlas:
                case enQuestionnaireType.ProviderForAtlas:
                    Existing = repo.FindOne(Entity.ID, QuestionnaireType);
                    break;

                case enQuestionnaireType.OfficeForProvider:
                case enQuestionnaireType.ProviderForOffice:
                    Existing = repo.FindOne(Entity.ID, QuestionnaireType, EntityID.GetValueOrDefault());
                    break;

                case enQuestionnaireType.StudentForOffice:
                case enQuestionnaireType.StudentForProvider:
                case enQuestionnaireType.OfficeForStudent:
                case enQuestionnaireType.ProviderForStudent:
                    Existing = repo.FindOne(Entity.ID, QuestionnaireType, EntityID.GetValueOrDefault(), PositionID.GetValueOrDefault());
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnSubmit.Visible = Existing == null;
            }
        }

        protected void ucQuestionnaireInput_ControlInit(object sender, StudentPractice.Portal.UserControls.QuestionnaireControls.InputControls.QuestionnaireInputInitEventArgs e)
        {
            e.QuestionnaireType = QuestionnaireType;

            if (Existing != null)
            {
                e.SubmittedQuestionnaire = Existing;
                e.ReadOnly = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var submitted = ucQuestionnaireInput.ExtractAnswers();

            submitted.ReporterID = Entity.ID;
            submitted.SubmittedAt = DateTime.Now;
            submitted.AcademicYear = 2014;
            submitted.EntityID = EntityID;
            submitted.PositionID = PositionID;

            switch (QuestionnaireType)
            {
                case enQuestionnaireType.OfficeForAtlas:
                case enQuestionnaireType.ProviderForAtlas:
                case enQuestionnaireType.StudentForAtlas:
                    submitted.EntityType = enQuestionnaireEntityType.Atlas;
                    break;
                case enQuestionnaireType.OfficeForStudent:
                case enQuestionnaireType.ProviderForStudent:
                    submitted.EntityType = enQuestionnaireEntityType.Student;
                    break;
                case enQuestionnaireType.ProviderForOffice:
                case enQuestionnaireType.StudentForOffice:
                    submitted.EntityType = enQuestionnaireEntityType.Office;
                    break;
                case enQuestionnaireType.OfficeForProvider:
                case enQuestionnaireType.StudentForProvider:
                    submitted.EntityType = enQuestionnaireEntityType.Provider;
                    break;
            }

            UnitOfWork.MarkAsNew(submitted);

            var answers = submitted.QuestionnaireAnswers.ToList();
            var nullableAnswerExists = false;

            foreach (var answer in answers)
            {
                if (answer.AnswerInt.HasValue && answer.AnswerInt == 0)
                {
                    nullableAnswerExists = true;
                }
            }

            string message = string.Empty;
            if (nullableAnswerExists)
            {
                if (QuestionnaireType == enQuestionnaireType.ProviderForAtlas || QuestionnaireType == enQuestionnaireType.StudentForAtlas || QuestionnaireType == enQuestionnaireType.OfficeForAtlas)
                {
                    message = Resources.Evaluation.EvalErrorWithRefresh;
                }
                else
                {
                    message = Resources.Evaluation.EvalError;
                }
            }
            else
            {
                UnitOfWork.Commit();
                
                if (QuestionnaireType == enQuestionnaireType.ProviderForAtlas || QuestionnaireType == enQuestionnaireType.StudentForAtlas || QuestionnaireType == enQuestionnaireType.OfficeForAtlas)
                {
                    message = Resources.Evaluation.EvalComplete;
                }
                else
                {
                    message = Resources.Evaluation.EvalSaved;
                }
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", string.Format("window.parent.popUp.hideWithMessage('{0}');", message), true);
        }
    }
}