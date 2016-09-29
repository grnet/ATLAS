namespace StudentPractice.BusinessModel
{
    public partial class InternshipPositionLog
    {
        public enPositionStatus OldStatus
        {
            get { return (enPositionStatus)OldStatusInt; }
            set
            {
                if (OldStatusInt != (int)value)
                    OldStatusInt = (int)value;
            }
        }

        public enPositionStatus NewStatus
        {
            get { return (enPositionStatus)NewStatusInt; }
            set
            {
                if (NewStatusInt != (int)value)
                    NewStatusInt = (int)value;
            }
        }

        public enCancellationReason CancellationReason
        {
            get { return (enCancellationReason)CancellationReasonInt; }
            set
            {
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
