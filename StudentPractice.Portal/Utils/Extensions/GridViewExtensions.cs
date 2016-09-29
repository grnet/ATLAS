using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal
{
    public static class GridViewExtensions
    {
        #region [ Reporters ]

        public static string GetAccountDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            return string.Format("{0}<br/>{1}", reporter.UserName, reporter.Email);
        }

        public static string GetContactDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            return string.Format("{0}<br/>{1}<br/>{2}", reporter.ContactName, reporter.ContactPhone, reporter.ContactMobilePhone);
        }

        public static string GetCertificationDetails(this Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            if (reporter.CertificationNumber.HasValue)
                return string.Format("{0} / {1:dd-MM-yyyy}", reporter.CertificationNumber.Value, reporter.CertificationDate);
            else
                return string.Empty;
        }

        #endregion

        #region [ Positions - PositionGroups ]

        public static string GetFirstPublishedAt(this InternshipPositionGroup group)
        {
            if (group == null)
                return String.Empty;

            return String.Format("{0:dd/MM/yyyy}", group.FirstPublishedAt);
        }

        public static string GetLastPublishedAt(this InternshipPositionGroup group)
        {
            if (group == null)
                return String.Empty;

            return String.Format("{0:dd/MM/yyyy}", group.LastPublishedAt);
        }

        public static string GetAcademics(this InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            if (group.IsVisibleToAllAcademics.GetValueOrDefault())
                return "Όλα";
            else
                return string.Join("<br />", group.Academics.Select(x => string.Format("({0}) - {1} - {2}", x.ID, x.Institution, x.Department)));
        }

        public static string GetAcademics(this InternshipPosition ip)
        {
            if (ip == null || ip.InternshipPositionGroup == null)
                return string.Empty;

            return GetAcademics(ip.InternshipPositionGroup);
        }

        public static string GetPhysicalObjectDetails(this InternshipPositionGroup group, bool commaSeparated = false)
        {
            if (group == null)
                return string.Empty;

            return string.Join(commaSeparated ? ";" : "<br />", group.PhysicalObjects.Select(x => x.Name));
        }

        public static string GetPhysicalObjectDetails(this InternshipPosition position, bool commaSeparated = false)
        {
            if (position == null || position.InternshipPositionGroup == null)
                return String.Empty;

            return GetPhysicalObjectDetails(position.InternshipPositionGroup, commaSeparated);
        }

        public static string GetAddressDetails(this InternshipPositionGroup group)
        {
            if (group == null)
                return String.Empty;

            if (group.CountryID == StudentPracticeConstants.GreeceCountryID)
            {
                var city = group.CityID.HasValue ? CacheManager.Cities.Get(group.CityID.Value) : null;
                var prefecture = group.PrefectureID.HasValue ? CacheManager.Prefectures.Get(group.PrefectureID.Value) : null;
                if (city != null && prefecture != null)
                    return string.Format("{0}<br />{1}<br />{2}", StudentPracticeConstants.GreeceCountryName, city.Name, prefecture.Name);
                else
                    return string.Empty;
            }
            else if (group.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                var city = group.CityID.HasValue ? CacheManager.Cities.Get(group.CityID.Value) : null;
                var prefecture = group.PrefectureID.HasValue ? CacheManager.Prefectures.Get(group.PrefectureID.Value) : null;
                if (city != null && prefecture != null)
                    return string.Format("{0}<br />{1}<br />{2}", StudentPracticeConstants.CyprusCountryName, city.Name, prefecture.Name);
                else
                    return string.Empty;
            }
            else
            {
                var country = CacheManager.Countries.Get(group.CountryID);
                return string.Format("{0}<br />{1}", country.Name, group.CityText);
            }
        }

        public static string GetAddressDetails(this InternshipPosition position)
        {
            if (position == null || position.InternshipPositionGroup == null)
                return String.Empty;

            return GetAddressDetails(position.InternshipPositionGroup);
        }

        public static string GetDaysLeftForAssignment(this InternshipPosition position)
        {
            if (position == null ||
               (position.InternshipPositionGroup != null && position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice))
                return String.Empty;

            return position.DaysLeftForAssignment.ToString();
        }

        public static string GetTotalPositions(this InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            return group.TotalPositions.ToString();
        }

        public static string GetImplementationDetails(this InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            //ToFix: FundingType
            if (position.ImplementationStartDate.HasValue && position.ImplementationEndDate.HasValue && position.FundingTypeInt.HasValue)
            {
                return string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}",
                    position.ImplementationStartDate,
                    position.ImplementationEndDate);
                //return string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}<br/>{2}",                    
                //    position.ImplementationStartDate,                    
                //    position.ImplementationEndDate,                    
                //    position.FundingType.GetLabel());
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetPositionName(this InternshipPositionGroup position)
        {
            if (position == null)
                return string.Empty;

            return string.Format("{0}<br />{1}", position.Title, position.Description);
        }

        public static string GetPositionName(this InternshipPosition position)
        {
            if (position == null || position.InternshipPositionGroup == null)
                return string.Empty;

            return GetPositionName(position.InternshipPositionGroup);
        }

        public static string GetProviderDetails(this InternshipPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            return string.Format("<b>{0}<br/>{1}</b><br/>{2}<br/>Tηλ.: {3}<br/>ΑΦΜ: {4}",
                group.Provider.Name,
                group.Provider.ContactName,
                group.Provider.ContactEmail,
                group.Provider.ContactPhone,
                group.Provider.AFM);
        }

        #endregion

        #region [ Office ]

        public static string GetOfficeAcademics(this InternshipOffice office, int entityAcademicsCount, bool commaSeparated = false)
        {
            if (office == null)
                return string.Empty;

            if (office.Academics.Count() == entityAcademicsCount)
                return "Όλα";
            else
                return string.Join(commaSeparated ? ";" : "<br />", office.Academics.OrderBy(x => x.Department).Select(x => x.Department));
        }

        public static string GetOfficeInstitution(this InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            return office.InstitutionID.HasValue ? CacheManager.Institutions.Get(office.InstitutionID.Value).Name : string.Empty;
        }        

        public static string GetVerificationStatus(this InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            switch (office.VerificationStatus)
            {
                case enVerificationStatus.NotVerified:
                    return "Όχι";
                case enVerificationStatus.Verified:
                    return "Ναι";
                case enVerificationStatus.CannotBeVerified:
                    return "Απορρίφθηκε";
                default:
                    return string.Empty;
            }
        }

        public static string GetVerificationDate(this InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            return office.VerificationDate.HasValue ? office.VerificationDate.Value.ToString("dd/MM/yyyy") : string.Empty;
        }

        #endregion

        #region [ Providers ]

        public static string GetPrimaryActivity(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return provider.PrimaryActivityID.HasValue ? CacheManager.PrimaryActivities.Get(provider.PrimaryActivityID.Value).Name : string.Empty;
        }

        public static string GetAddressDetails(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            if (provider.CountryID == StudentPracticeConstants.GreeceCountryID)
            {
                var city = provider.CityID.HasValue ? CacheManager.Cities.Get(provider.CityID.Value) : null;
                var prefecture = provider.PrefectureID.HasValue ? CacheManager.Prefectures.Get(provider.PrefectureID.Value) : null;
                if (city != null && prefecture != null)
                    return string.Format("{0}<br />{1}", city.Name, prefecture.Name);
                else
                    return string.Empty;
            }
            else if (provider.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                var city = provider.CityID.HasValue ? CacheManager.Cities.Get(provider.CityID.Value) : null;
                var prefecture = provider.PrefectureID.HasValue ? CacheManager.Prefectures.Get(provider.PrefectureID.Value) : null;
                if (city != null && prefecture != null)
                    return string.Format("{0}<br />{1}<br />{2}", StudentPracticeConstants.CyprusCountryName, city.Name, prefecture.Name);
                else
                    return string.Empty;
            }
            else
            {
                if (provider.CountryID.HasValue)
                {
                    var country = CacheManager.Countries.Get(provider.CountryID.Value);
                    return string.Format("{0}<br />{1}", country.Name, provider.CityText);
                }
                else
                    return string.Empty;
            }
        }

        public static string GetProviderCountry(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            if (provider.CountryID == StudentPracticeConstants.GreeceCountryID)
                return StudentPracticeConstants.GreeceCountryName;
            else if (provider.CountryID == StudentPracticeConstants.CyprusCountryID)
                return StudentPracticeConstants.CyprusCountryName;
            else
                return provider.CountryID.HasValue ? CacheManager.Countries.Get(provider.CountryID.Value).Name : string.Empty;
        }

        public static string GetProviderPrefecture(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return provider.PrefectureID.HasValue ? CacheManager.Prefectures.Get(provider.PrefectureID.Value).Name : provider.CityText;
        }

        public static string GetProviderCity(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return provider.CityID.HasValue ? CacheManager.Cities.Get(provider.CityID.Value).Name : string.Empty;
        }

        public static string GetVerificationStatus(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            switch (provider.VerificationStatus)
            {
                case enVerificationStatus.NotVerified:
                    return "Όχι";
                case enVerificationStatus.Verified:
                    return "Ναι";
                case enVerificationStatus.CannotBeVerified:
                    return "Δεν μπορεί να πιστοποιηθεί";
                default:
                    return string.Empty;
            }
        }

        public static string GetVerificationDate(this InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return provider.VerificationDate.HasValue ? provider.VerificationDate.Value.ToString("dd/MM/yyyy") : string.Empty;
        }

        #endregion
    }
}