using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public partial class PhysicalObject
    {
        public string Name
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? NameInLatin : NameInGreek;
            }

            set
            {
                if (LanguageService.GetUserLanguage() == enLanguage.English)
                    NameInLatin = value;
                else
                    NameInGreek = value;
            }
        }
    }
}
