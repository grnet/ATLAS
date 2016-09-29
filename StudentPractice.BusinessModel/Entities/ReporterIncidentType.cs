namespace StudentPractice.BusinessModel
{
    public partial class ReporterIncidentType
    {
        public enReporterType ReporterType
        {
            get { return (enReporterType)ReporterTypeInt; }
            set
            {
                if (ReporterTypeInt != (int)value)
                    ReporterTypeInt = (int)value;
            }
        }
    }
}
