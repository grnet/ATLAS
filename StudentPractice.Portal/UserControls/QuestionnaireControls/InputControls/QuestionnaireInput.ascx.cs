using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.QuestionnaireControls.InputControls
{
    public class QuestionnaireInputInitEventArgs : EventArgs
    {
        public enQuestionnaireType QuestionnaireType { get; set; }
        public SubmittedQuestionnaire SubmittedQuestionnaire { get; set; }
        public bool ReadOnly { get; set; }
    }

    public partial class QuestionnaireInput : BaseEntityUserControl<object>
    {
        public enQuestionnaireType QuestionnaireType { get; set; }

        public SubmittedQuestionnaire SubmittedQuestionnaire { get; set; }

        public event EventHandler<QuestionnaireInputInitEventArgs> ControlInit;

        protected override void OnPreRender(EventArgs e)
        {
            if (QuestionnaireType == enQuestionnaireType.ProviderForAtlas || QuestionnaireType == enQuestionnaireType.StudentForAtlas || QuestionnaireType == enQuestionnaireType.OfficeForAtlas)
                divAtlasEval.Visible = true;

            base.OnPreRender(e);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            var args = new QuestionnaireInputInitEventArgs();
            if (ControlInit != null)
            {
                ControlInit(this, args);
                QuestionnaireType = args.QuestionnaireType;
                SubmittedQuestionnaire = args.SubmittedQuestionnaire;
                ReadOnly = args.ReadOnly;
            }

            var questionnaire = CacheManager.Questionnaires.GetItems().Where(x => x.QuestionnaireType == QuestionnaireType).First();
            var questions = CacheManager.QuestionnaireQuestions.GetItems().Where(x => x.QuestionnaireID == questionnaire.ID).ToList();

            litTitle.Text = questionnaire.Title;

            rpt.DataSource = questions.OrderBy(x => x.QuestionNumber).ToList();
            rpt.DataBind();
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var input = e.Item.FindControl("ucQuestionInput") as QuestionnaireQuestionInput;
            if (input != null)
            {
                input.ReadOnly = ReadOnly;
                input.ValidationGroup = ValidationGroup;
                input.IsRequired = IsRequired;

                if (SubmittedQuestionnaire != null)
                {
                    var answer = SubmittedQuestionnaire.QuestionnaireAnswers.FirstOrDefault(x => x.QuestionID == input.QuestionID);
                    if (answer != null)
                        input.Answer = answer;
                }

                input.Bind();
            }
        }

        public SubmittedQuestionnaire ExtractAnswers()
        {
            var questionnaire = CacheManager.Questionnaires.GetItems().Where(x => x.QuestionnaireType == QuestionnaireType).First();

            var submittedQuestionnaire = new SubmittedQuestionnaire();
            submittedQuestionnaire.QuestionnaireID = questionnaire.ID;

            foreach (RepeaterItem item in rpt.Items)
            {
                var input = item.FindControl("ucQuestionInput") as QuestionnaireQuestionInput;
                var answer = input.Fill();

                submittedQuestionnaire.QuestionnaireAnswers.Add(answer);
            }

            return submittedQuestionnaire;
        }

        #region [ Properties ]

        public bool ReadOnly { get; set; }

        public bool IsRequired { get; set; }

        public string ValidationGroup { get; set; }

        #endregion

    }
}