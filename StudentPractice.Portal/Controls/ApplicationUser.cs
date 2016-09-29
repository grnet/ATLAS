using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentPractice.BusinessModel;
using System.Web.Security;

namespace StudentPractice.Portal.Controls
{
    public class ApplicationUser
    {
        public ApplicationUser(string username)
        {
            var roles = Roles.GetRolesForUser(username);
            SubSystem = CacheManager.SubSystems.GetItems().Where(x => x.Role == roles.First()).FirstOrDefault();

            if (Roles.IsUserInRole(RoleNames.MasterOffice))
                InstitutionID = new InternshipOfficeRepository().FindInstitutionIDByUsername(username);
        }

        #region [ Public Properties ]

        public int InstitutionID { get; set; }

        public string Username { get; set; }

        public SubSystem SubSystem { get; set; }

        public List<enReporterType> ReporterTypes
        {
            get
            {
                return CacheManager.SubSystemReporterTypes.GetItems().Where(x => x.SubSystemID == SubSystem.ID).Select(x => x.ReporterType).ToList();
            }
        }

        public List<IncidentType> IncidentTypes
        {
            get
            {
                return CacheManager.IncidentTypes.GetItems().Where(x => x.SubSystemID == SubSystem.ID).ToList();
            }
        }

        #endregion
    }
}