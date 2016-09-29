using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.Web.Api
{
    public class ChangeFundingTypeRequest
    {
        public int PositionID { get; set; }
        public string FundingType { get; set; }
    }

    public class ChangeFundingTypeResult
    {
        public int PositionID { get; set; }
        public string OldFundingType { get; set; }
        public string NewFundingType { get; set; }
    }

    public class GetFundingTypeRequest
    {
        public int PositionID { get; set; }        
    }

    public class GetFundingTypeResult
    {
        public int PositionID { get; set; }        
        public string FundingType { get; set; }
    }
}
