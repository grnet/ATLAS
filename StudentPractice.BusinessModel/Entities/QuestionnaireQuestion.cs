using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StudentPractice.BusinessModel
{
    public partial class QuestionnaireQuestion
    {
        public enQuestionType QuestionType
        {
            get { return (enQuestionType)QuestionTypeInt; }
            set
            {
                int intValue = (int)value;
                if (QuestionTypeInt != intValue)
                    QuestionTypeInt = intValue;
            }
        }

        public PossibleAnswers GetPossibleAnswers()
        {
            string answersXml = LanguageService.GetUserLanguage() == enLanguage.Greek ? PossibleAnswersGRXml : PossibleAnswersENXml;

            XElement parentEl = XElement.Parse(answersXml);

            return new PossibleAnswers(parentEl.Descendants("Answer")
                .Select(x => new PossibleAnswer()
                    {
                        Text = x.Attribute("Text").Value,
                        Value = int.Parse(x.Attribute("Value").Value)
                    }));
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
