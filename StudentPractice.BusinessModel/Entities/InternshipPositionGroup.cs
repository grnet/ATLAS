namespace StudentPractice.BusinessModel
{
    public partial class InternshipPositionGroup : IUserChangeTracking
    {
        public enPositionType PositionType
        {
            get { return (enPositionType)PositionTypeInt; }
            set
            {
                if (PositionTypeInt != (int)value)
                    PositionTypeInt = (int)value;
            }
        }

        public enPositionGroupStatus PositionGroupStatus
        {
            get { return (enPositionGroupStatus)PositionGroupStatusInt; }
            set
            {
                if (PositionGroupStatusInt != (int)value)
                    PositionGroupStatusInt = (int)value;
            }
        }

        public enPositionCreationType PositionCreationType
        {
            get { return (enPositionCreationType)PositionCreationTypeInt; }
            set
            {
                if (PositionCreationTypeInt != (int)value)
                    PositionCreationTypeInt = (int)value;
            }
        }
    }
}
