namespace StudentPractice.BusinessModel
{
    public partial class IncidentReport : IUserChangeTracking
    {
        public enReportStatus ReportStatus
        {
            get
            {
                return (enReportStatus)ReportStatusInt;
            }
            set
            {
                if (ReportStatusInt != (int)value)
                    ReportStatusInt = (int)value;
            }
        }

        public enReportSubmissionType SubmissionType
        {
            get
            {
                return (enReportSubmissionType)SubmissionTypeInt;
            }
            set
            {
                if (SubmissionTypeInt != (int)value)
                    SubmissionTypeInt = (int)value;
            }
        }

        public enCallType CallType
        {
            get
            {
                return (enCallType)CallTypeInt;
            }
            set
            {
                if (CallTypeInt != (int)value)
                    CallTypeInt = (int)value;
            }
        }

        public enHandlerType HandlerType
        {
            get
            {
                return (enHandlerType)HandlerTypeInt;
            }
            set
            {
                if (HandlerTypeInt != (int)value)
                    HandlerTypeInt = (int)value;
            }
        }

        public enHandlerStatus HandlerStatus
        {
            get
            {
                return (enHandlerStatus)HandlerStatusInt;
            }
            set
            {
                if (HandlerStatusInt != (int)value)
                    HandlerStatusInt = (int)value;
            }
        }
    }
}
