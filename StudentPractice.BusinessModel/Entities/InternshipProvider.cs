namespace StudentPractice.BusinessModel
{
    public partial class InternshipProvider
    {
        public enProviderType ProviderType
        {
            get { return (enProviderType)ProviderTypeInt; }
            set
            {
                if (ProviderTypeInt != (int)value)
                    ProviderTypeInt = (int)value;
            }
        }

        public enIdentificationType LegalPersonIdentificationType
        {
            get
            {
                if (LegalPersonIdentificationTypeInt.HasValue)
                {
                    return (enIdentificationType)LegalPersonIdentificationTypeInt;                    
                }
                else
                {
                    return enIdentificationType.None;
                }
            }
            set
            {
                if (LegalPersonIdentificationTypeInt != (int)value)
                    LegalPersonIdentificationTypeInt = (int)value;
            }
        }

        public enVerificationStatus VerificationStatus
        {
            get { return (enVerificationStatus)VerificationStatusInt; }
            set
            {
                if (VerificationStatusInt != (int)value)
                    VerificationStatusInt = (int)value;
            }
        }

        public override string GetLabel()
        {
            return enReporterType.InternshipProvider.GetLabel();
        }
    }
}
