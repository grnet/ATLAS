namespace StudentPractice.BusinessModel
{
    public partial class VerificationLog
    {
        public enVerificationStatus OldVerificationStatus
        {
            get { return (enVerificationStatus)OldVerificationStatusInt; }
            set
            {
                if (OldVerificationStatusInt != (int)value)
                    OldVerificationStatusInt = (int)value;
            }
        }

        public enVerificationStatus NewVerificationStatus
        {
            get { return (enVerificationStatus)NewVerificationStatusInt; }
            set
            {
                if (NewVerificationStatusInt != (int)value)
                    NewVerificationStatusInt = (int)value;
            }
        }
    }
}
