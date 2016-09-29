using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.Web.Api
{
    public class PreAssignPositionRequest
    {
        public int GroupID { get; set; }
        public int NumberOfPositions { get; set; }
        public int AcademicID { get; set; }
    }
}
