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
    public partial class QuestionnaireQuestionInput : BaseEntityUserControl<object>
    {
        public int QuestionID { get; set; }

        public QuestionnaireAnswer Answer { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            reFreeText.ErrorMessage = string.Format(reFreeText.ErrorMessage, StudentPracticeConstants.QuestionnaireFreeTextMaxLength);
            reFreeText.ValidationExpression = string.Format("^[\\s\\S]{{0,{0}}}$", StudentPracticeConstants.QuestionnaireFreeTextMaxLength);
            base.OnPreRender(e);
        }

        public override void Bind()
        {
            var question = CacheManager.QuestionnaireQuestions.Get(QuestionID);

            litTitle.Text = string.Format("{0}. {1}", question.QuestionNumber, question.Title);

            if (question.QuestionType == enQuestionType.FreeText)
            {
                mv.SetActiveView(vFreeText);

                if (Answer != null)
                    txtAnswer.Text = Answer.AnswerString;
            }
            else if (question.QuestionType == enQuestionType.MultipleAnswer)
            {
                mv.SetActiveView(vMultipleAnswer);

                var answers = question.GetPossibleAnswers();
                foreach (var item in answers)
                    rblMultipleAnswer.Items.Add(new ListItem(item.Text, item.Value.ToString()));

                if (Answer != null)
                    rblMultipleAnswer.SelectedValue = Answer.AnswerInt.ToString();
            }

            if (ReadOnly)
            {
                txtAnswer.ReadOnly = true;
                rblMultipleAnswer.Enabled = false;
            }
        }

        public new QuestionnaireAnswer Fill()
        {
            var question = CacheManager.QuestionnaireQuestions.Get(QuestionID);

            var answer = new QuestionnaireAnswer();
            answer.QuestionID = QuestionID;
            if (question.QuestionType == enQuestionType.FreeText)
            {
                answer.AnswerString = txtAnswer.GetText();
            }
            else if (question.QuestionType == enQuestionType.MultipleAnswer)
            {
                answer.AnswerInt = rblMultipleAnswer.GetSelectedInteger().GetValueOrDefault();
            }

            return answer;
        }

        #region [ Properties ]

        public bool ReadOnly { get; set; }

        public bool IsRequired
        {
            get { return rfFreeText.Visible; }
            set { rfFreeText.Visible = rfMultipleAnswer.Visible = value; }
        }

        public string ValidationGroup
        {
            get { return rfFreeText.ValidationGroup; }
            set { rfFreeText.ValidationGroup = rfMultipleAnswer.ValidationGroup = reFreeText.ValidationGroup = value; }
        }

        #endregion
    }
}