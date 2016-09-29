namespace StudentPractice.BusinessModel
{
    public partial class Dispatch
    {
        public enDispatchType DispatchType
        {
            get { return (enDispatchType)DispatchTypeInt; }
            set
            {
                if (DispatchTypeInt != (int)value)
                    DispatchTypeInt = (int)value;
            }
        }
    }
}
