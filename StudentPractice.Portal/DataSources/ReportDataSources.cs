using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace StudentPractice.Portal.DataSources
{
    public class TransferChoiceData
    {
        public string Institution { get; set; }
        public string School { get; set; }
        public string Department { get; set; }
        public string TotalStudents { get; set; }
        public string TotalTransferedStudents { get; set; }
        public string LastTransferedStudentGrade { get; set; }
    }

    public class ReportDataSources
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<TransferChoiceData> GetTransferChoiceData()
        {
            return new List<TransferChoiceData>();
        }
    }
}