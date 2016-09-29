
namespace StudentPractice.BusinessModel
{
    public partial class InternshipPositionGroupLog
    {
        public enPositionGroupStatus OldStatus
        {
            get { return (enPositionGroupStatus)OldStatusInt; }
            set
            {
                if (OldStatusInt != (int)value)
                    OldStatusInt = (int)value;
            }
        }

        public enPositionGroupStatus NewStatus
        {
            get { return (enPositionGroupStatus)NewStatusInt; }
            set
            {
                if (NewStatusInt != (int)value)
                    NewStatusInt = (int)value;
            }
        }
    }
}
