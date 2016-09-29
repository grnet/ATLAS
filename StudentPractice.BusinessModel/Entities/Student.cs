using System.Collections.Generic;

namespace StudentPractice.BusinessModel
{
    public partial class Student : IStudentInfo
    {
        public override string GetLabel()
        {
            return enReporterType.Student.GetLabel();
        }

        public enAcademicIDApplicationStatus? AcademicIDStatus
        {
            get
            {
                if (AcademicIDStatusInt.HasValue)
                    return (enAcademicIDApplicationStatus)AcademicIDStatusInt.Value;
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                    AcademicIDStatusInt = (int)value.Value;
                else
                    AcademicIDStatusInt = null;
            }
        }

        #region [ Change Tracking ]

        public List<string> ChangedProperties = new List<string>();

        protected override void OnPropertyChanged(string property)
        {
            if (AllowTrackingChanges)
            {
                ChangedProperties.Add(property);
            }
        }

        public bool AllowTrackingChanges { get; set; }

        public bool IsModified
        {
            get
            {
                return ChangedProperties.Count > 0;
            }
        }

        #endregion

        int IStudentInfo.AcademicID
        {
            get { return AcademicID.Value; }
            set { AcademicID = value; }
        }

    }
}
