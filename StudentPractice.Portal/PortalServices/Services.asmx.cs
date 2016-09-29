using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ComponentModel;
using AjaxControlToolkit;
using StudentPractice.BusinessModel;
using System.Web.Security;
using Imis.Domain;
using System.Threading;
using StudentPractice.Mails;

namespace StudentPractice.Portal.PortalServices
{
    /// <summary>
    /// Summary description for Services
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class Services : System.Web.Services.WebService
    {
        #region [ AJAX Dropdown Services ]

        [WebMethod]
        public CascadingDropDownNameValue[] GetPrefectures()
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            foreach (Prefecture p in CacheManager.Prefectures.GetItems().OrderBy(x => x.Name).ToList())
            {
                values.Add(new CascadingDropDownNameValue(p.Name, p.ID.ToString()));
            }

            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetPrefectures(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int countryID;
            if (int.TryParse(knownCategoryValues.Split(':')[1].Replace(";", ""), out countryID) && countryID >= 0)
            {
                foreach (Prefecture p in CacheManager.Prefectures.GetItems().Where(x => x.CountryID == countryID).OrderBy(x => x.Name))
                {
                    values.Add(new CascadingDropDownNameValue(p.Name, p.ID.ToString()));
                }
            }

            return values.OrderBy(x => x.name).ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetCities(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int prefectureID;

            var splittedCategories = knownCategoryValues.Split(':');

            if (int.TryParse(splittedCategories[splittedCategories.Length - 1].Replace(";", ""), out prefectureID) && prefectureID >= 0)
            {
                foreach (City c in CacheManager.Cities.GetItems().Where(x => x.PrefectureID == prefectureID).OrderBy(x => x.Name))
                {
                    values.Add(new CascadingDropDownNameValue(c.Name, c.ID.ToString()));
                }
            }

            return values.OrderBy(x => x.name).ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetDepartments(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int departmentID;
            if (int.TryParse(knownCategoryValues.Split(':')[1].Replace(";", ""), out departmentID) && departmentID >= 0)
            {
                int institutionID = int.Parse(knownCategoryValues.Split(':')[1].Replace(";", ""));
                foreach (Academic a in CacheManager.Academics.GetItems().Where(x => x.InstitutionID == institutionID))
                {
                    values.Add(new CascadingDropDownNameValue(a.Department, a.ID.ToString()));
                }
            }

            return values.OrderBy(x => x.name).ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetIncidentTypes(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int reporterTypeID;
            if (int.TryParse(knownCategoryValues.Split(':')[1].Replace(";", ""), out reporterTypeID) && reporterTypeID >= 0)
            {
                var incidentTypeIDs = CacheManager.ReporterIncidentTypes.GetItems().Where(x => x.ReporterType == (enReporterType)reporterTypeID).Select(x => x.IncidentTypeID).ToList();
                foreach (IncidentType it in CacheManager.IncidentTypes.GetItems().Where(x => incidentTypeIDs.Contains(x.ID)))
                {
                    values.Add(new CascadingDropDownNameValue(it.Name, it.ID.ToString()));
                }
            }

            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetAcademics(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int institutionID;
            if (int.TryParse(knownCategoryValues.Split(':')[1].Replace(";", ""), out institutionID) && institutionID >= 0)
            {
                foreach (Academic it in CacheManager.Academics.GetItems().Where(x => x.InstitutionID == institutionID).OrderBy(x => x.Department))
                {
                    values.Add(new CascadingDropDownNameValue(it.Department, it.ID.ToString()));
                }
            }

            return values.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetActiveAcademics(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

            int institutionID;
            if (int.TryParse(knownCategoryValues.Split(':')[1].Replace(";", ""), out institutionID) && institutionID >= 0)
            {
                foreach (Academic it in CacheManager.Academics.GetItems().Where(x => x.InstitutionID == institutionID && x.IsActive).OrderBy(x => x.Department))
                {
                    values.Add(new CascadingDropDownNameValue(it.Department, it.ID.ToString()));
                }
            }

            return values.ToArray();
        }

        #endregion

        #region [ Helpdesk ]

        [WebMethod]
        public string ChangeIncidentReportStatus(int incidentReportID, enReportStatus newStatus)
        {
            var roles = Roles.GetRolesForUser();
            var isHelpdesk = roles.Any(x => x == RoleNames.Helpdesk || x == RoleNames.SuperHelpdesk || x == RoleNames.Supervisor);
            if (!isHelpdesk)
                return null;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var ic = new IncidentReportRepository(uow).Load(incidentReportID);
                if (ic == null)
                    return null;
                ic.ReportStatus = newStatus;
                uow.Commit();
                return newStatus.GetIcon();
            }
        }

        [WebMethod]
        public bool? ChangeEmail(string username, string newEmail)
        {
            var roles = Roles.GetRolesForUser();
            var isHelpdesk = roles.Any(x => x == RoleNames.Helpdesk || x == RoleNames.SuperHelpdesk || x == RoleNames.Supervisor);
            var isSecure = roles.Any(x => x == RoleNames.MasterOffice || x == RoleNames.OfficeUser || x == RoleNames.MasterProvider || x == RoleNames.ProviderUser || x == RoleNames.Student);
            if (!isHelpdesk && !isSecure)
                return null;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                if (!isHelpdesk || string.IsNullOrEmpty(username))
                    username = Thread.CurrentPrincipal.Identity.Name;

                var reporter = new ReporterRepository(uow).FindByUsername(username);
                if (reporter == null)
                    return null;

                reporter.ContactEmail = newEmail;
                reporter.Email = newEmail;

                if (Membership.FindUsersByEmail(newEmail).Count > 0)
                    return false;

                var user = Membership.GetUser(username);
                if (user != null)
                {
                    if (user.Email == newEmail)
                        return true;

                    user.Email = newEmail;
                    Membership.UpdateUser(user);
                }

                reporter.IsEmailVerified = false;
                reporter.EmailVerificationCode = Guid.NewGuid().ToString();
                reporter.EmailVerificationDate = null;

                Uri baseURI;
                if (Config.IsSSL)
                {
                    baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
                }
                else
                {
                    baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
                }

                Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + reporter.EmailVerificationCode);

                var email = MailSender.SendEmailVerification(reporter.ID, reporter.Email, reporter.ContactName, uri, reporter.Language.GetValueOrDefault());
                uow.MarkAsNew(email);
                uow.Commit();

                return true;
            }
        }

        [WebMethod]
        public bool? ChangeMobilePhone(string username, string newMobilePhone)
        {
            var roles = Roles.GetRolesForUser();
            var isHelpdesk = roles.Any(x => x == RoleNames.Helpdesk || x == RoleNames.SuperHelpdesk || x == RoleNames.Supervisor);
            var isSecure = roles.Any(x => x == RoleNames.MasterOffice || x == RoleNames.OfficeUser || x == RoleNames.MasterProvider || x == RoleNames.ProviderUser || x == RoleNames.Student);
            if (!isHelpdesk && !isSecure)
                return null;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                if (!isHelpdesk || string.IsNullOrEmpty(username))
                    username = Thread.CurrentPrincipal.Identity.Name;

                var reporter = new ReporterRepository(uow).FindByUsername(username);
                if (reporter == null)
                    return null;

                reporter.ContactMobilePhone = newMobilePhone;

                uow.Commit();

                return true;
            }
        }

        [WebMethod]
        public bool? UnlockUser(string username)
        {
            var roles = Roles.GetRolesForUser();
            var isHelpdesk = roles.Any(x => x == RoleNames.Helpdesk || x == RoleNames.SuperHelpdesk || x == RoleNames.Supervisor);
            if (!isHelpdesk)
                return null;

            var user = Membership.GetUser(username);
            if (user == null)
                return null;
            if (!user.IsLockedOut)
                return true;
            user.UnlockUser();
            return true;
        }

        [WebMethod]
        public bool UsernameExists(string username)
        {
            if (Membership.GetUser(username) == null)
                return false;

            return true;
        }

        [WebMethod]
        public bool EmailExists(string email)
        {
            if (string.IsNullOrEmpty(Membership.GetUserNameByEmail(email)))
                return false;

            return true;
        }

        #endregion

        #region [ Academic Position Rules ]

        [WebMethod]
        public object GetAcademicPositionRules(IEnumerable<int> ids)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                List<Academic> academics;
                if (ids == null || ids.Count() == 0)
                    academics = new AcademicRepository(uow).LoadAll().ToList();
                else
                    academics = new AcademicRepository(uow).LoadMany(ids).ToList();

                return academics.Where(x => !string.IsNullOrEmpty(x.PositionRules))
                    .Select(x => new { x.ID, x.Department, x.Institution, x.PositionRules });
            }
        }

        #endregion
    }
}