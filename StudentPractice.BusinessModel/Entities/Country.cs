using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public partial class Country
    {
        public string Name
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? NameInLatin : NameInGreek;
            }
        }
    }
}
