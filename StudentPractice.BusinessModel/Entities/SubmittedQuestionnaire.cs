using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public partial class SubmittedQuestionnaire
    {
        public enQuestionnaireEntityType EntityType
        {
            get { return (enQuestionnaireEntityType)EntityTypeInt; }
            set
            {
                int intValue = (int)value;
                if (intValue != EntityTypeInt)
                    EntityTypeInt = intValue;
            }
        }
    }
}
