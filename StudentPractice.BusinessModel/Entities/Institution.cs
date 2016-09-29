namespace StudentPractice.BusinessModel
{
    public partial class Institution
    {
        public enInstitutionType InstitutionType
        {
            get { return (enInstitutionType)InstitutionTypeInt; }
            set
            {
                if (InstitutionTypeInt != (int)value)
                    InstitutionTypeInt = (int)value;
            }
        }

        public string Name
        {
            get
            {
                return (LanguageService.GetUserLanguage() == enLanguage.English) ? NameInLatin : NameInGreek;
            }
        }
    }
}
