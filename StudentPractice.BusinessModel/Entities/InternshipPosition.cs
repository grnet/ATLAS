namespace StudentPractice.BusinessModel
{
    public partial class InternshipPosition : IUserChangeTracking
    {
        public enPositionStatus PositionStatus
        {
            get { return (enPositionStatus)PositionStatusInt; }
            set
            {
                if (PositionStatusInt != (int)value)
                    PositionStatusInt = (int)value;
            }
        }

        public enCancellationReason CancellationReason {
            get { return (enCancellationReason)CancellationReasonInt; }
            set {
                if (CancellationReasonInt != (int)value)
                    CancellationReasonInt = (int)value;
            }
        }

        public enFundingType FundingType
        {
            get { return (enFundingType)FundingTypeInt; }
            set
            {
                if (FundingTypeInt != (int)value)
                    FundingTypeInt = (int)value;
            }
        }
    }
}
