using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Security.Principal;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Net;
using System.Web;
using System.Web.Security;
using Imis.Domain;
using StudentPractice.Utils;
using log4net;
using StudentPractice.Mails;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class ProviderGroupPairList
    {
        public List<ProviderGroupPairResult> Pairs { get; set; }
        public int NumberOfItems { get; set; }
       
    }

    public class ProviderGroupPairResult
    {
        public int ProviderID { get; set; }
        public int PositionGroupID { get; set; }

        public DateTime? PositionGroupLastUpdate { get; set; }
        public string PositionGroupLastUpdateString { get; set; }

        public DateTime? ProviderLastUpdate { get; set; }
        public string ProviderLastUpdateString { get; set; }
    }

    public static class ProviderGroupPair
    {
        public static ProviderGroupPairResult ToProviderGroupPairResult(this InternshipPositionGroup group)
        {
            return new ProviderGroupPairResult()
            {
                PositionGroupID = group.ID,
                ProviderID = group.ProviderID,

                PositionGroupLastUpdate = group.UpdatedAt,
                PositionGroupLastUpdateString = group.UpdatedAt.HasValue ? group.UpdatedAt.Value.ToString() : string.Empty,

                ProviderLastUpdate = group.Provider.UpdatedAt,
                ProviderLastUpdateString = group.Provider.UpdatedAt.HasValue ? group.Provider.UpdatedAt.Value.ToString() : string.Empty,
            };
        }
    }
}
