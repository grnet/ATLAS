namespace StudentPractice.BusinessModel
{
    public partial class InternshipOffice
    {
        public enOfficeType OfficeType {
            get { return (enOfficeType)OfficeTypeInt; }
            set {
                if (OfficeTypeInt != (int)value)
                    OfficeTypeInt = (int)value;
            }
        }

        public enCertifierType CertifierType
        {
            get { return (enCertifierType)CertifierTypeInt; }
            set
            {
                if (CertifierTypeInt != (int)value)
                    CertifierTypeInt = (int)value;
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
            return enReporterType.InternshipOffice.GetLabel();
        }
    }
}
