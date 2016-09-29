namespace StudentPractice.BusinessModel
{
    public partial class BlockedPositionGroup : IUserChangeTracking
    {
        public enBlockingReason BlockingReason {
            get { return (enBlockingReason)BlockingReasonInt; }
            set {
                if (BlockingReasonInt != (int)value)
                    BlockingReasonInt = (int)value;
            }
        }
    }
}
