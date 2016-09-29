namespace StudentPractice.BusinessModel
{
    public partial class IncidentReportPost : IUserChangeTracking
    {
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
    }
}
