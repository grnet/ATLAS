using System;
namespace StudentPractice.BusinessModel
{
    public partial class Reporter : IUserChangeTracking
    {
        protected override void OnPropertyChanged(string property)
        {
            if (property.Equals("ID") && string.IsNullOrWhiteSpace(UsernameFromLDAP))
                UsernameFromLDAP = ID.ToString();
        }

        public enReporterType ReporterType
        {
            get
            {
                if (this is InternshipProvider)
                    return enReporterType.InternshipProvider;
                else if (this is InternshipOffice)
                    return enReporterType.InternshipOffice;
                else if (this is Student)
                    return enReporterType.Student;
                else if (this is FacultyMember)
                    return enReporterType.FacultyMember;
                else if (this is Other)
                    return enReporterType.Other;

                return enReporterType.Other;
            }
        }

        public enReporterDeclarationType DeclarationType
        {
            get { return (enReporterDeclarationType)DeclarationTypeInt; }
            set
            {
                if (DeclarationTypeInt != (int)value)
                    DeclarationTypeInt = (int)value;
            }
        }

        public enRegistrationType RegistrationType
        {
            get { return (enRegistrationType)RegistrationTypeInt; }
            set
            {
                if (RegistrationTypeInt != (int)value)
                    RegistrationTypeInt = (int)value;
            }
        }

        public enLanguage? Language
        {
            get { return (enLanguage?)LanguageInt; }
            set { LanguageInt = (int?)value; }
        }

        public virtual string GetLabel()
        {
            return string.Empty;
        }
    }
}
