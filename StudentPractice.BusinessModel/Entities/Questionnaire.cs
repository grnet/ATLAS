using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public partial class Questionnaire
    {
        public enQuestionnaireType QuestionnaireType
        {
            get { return (enQuestionnaireType)QuestionnaireTypeInt; }
            set
            {
                int intValue = (int)value;
                if (QuestionnaireTypeInt != intValue)
                    QuestionnaireTypeInt = intValue;
            }
        }

        public string Title
        {
            get
            {
                if (LanguageService.GetUserLanguage() == enLanguage.Greek)
                    return TitleGR;
                else
                    return TitleEN;
            }
        }
    }
}
