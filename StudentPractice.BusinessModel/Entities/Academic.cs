using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public partial class Academic
    {
        public string Institution
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? InstitutionInLatin : InstitutionInGreek;
            }

            set
            {
                if (LanguageService.GetUserLanguage() == enLanguage.English)
                    InstitutionInLatin = value;
                else
                    InstitutionInGreek = value;
            }
        }

        public string School
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? SchoolInLatin : SchoolInGreek;
            }
            
            set
            {
                if (LanguageService.GetUserLanguage() == enLanguage.English)
                    SchoolInLatin = value;
                else
                    SchoolInGreek = value;
            }
        }

        public string Department
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? DepartmentInLatin : DepartmentInGreek;
            }

            set
            {
                if (LanguageService.GetUserLanguage() == enLanguage.English)
                    DepartmentInLatin = value;
                else
                    DepartmentInGreek = value;
            }
        }
    }
}
