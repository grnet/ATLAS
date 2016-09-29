using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.DataSources
{
    public class IncidentReportPosts : BaseDataSource<IncidentReportPost>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<IncidentReportPost> FindByIncidentReportID(int incidentReportID)
        {
            var incidentReportPosts = new IncidentReportPostRepository().FindByIncidentReportID(incidentReportID);

            return incidentReportPosts;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<IncidentReportPost> FindDispatchedByIncidentReportID(int incidentReportID)
        {
            var incidentReportPosts = new IncidentReportPostRepository().FindDispatchedByIncidentReportID(incidentReportID);

            return incidentReportPosts;
        }
    }
}