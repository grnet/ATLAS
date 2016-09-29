using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal
{
    namespace CacheManagerExtensions
    {
        public static class PrivateExtensions
        {
            public static City GetCity(this int id)
            {
                return CacheManager.Cities.Get(id);
            }

            public static Prefecture GetPrefecture(this int id)
            {
                return CacheManager.Prefectures.Get(id);
            }

            public static Region GetRegion(this int id)
            {
                return CacheManager.Regions.Get(id);
            }

            public static Country GetCountry(this int id)
            {
                return CacheManager.Countries.Get(id);
            }

            public static GlobalRegion GetGlobalRegion(this int id)
            {
                return CacheManager.GlobalRegions.Get(id);
            }

            public static Academic GetAcademic(this int id)
            {
                return CacheManager.Academics.Get(id);
            }

            public static Institution GetInstitution(this int id)
            {
                return CacheManager.Institutions.Get(id);
            }

            public static PhysicalObject GetPhysicalObject(this int id)
            {
                return CacheManager.PhysicalObjects.Get(id);
            }

            public static PrimaryActivity GetPrimaryActivity(this int id)
            {
                return CacheManager.PrimaryActivities.Get(id);
            }
        }
    }

    public class CacheManager
    {
        static CacheManager()
        {
            SubSystems.GetItems();
            IncidentTypes.GetItems();
            SubSystemReporterTypes.GetItems();
            ReporterIncidentTypes.GetItems();
            Cities.GetItems();
            Prefectures.GetItems();
            Regions.GetItems();
            Countries.GetItems();
            GlobalRegions.GetItems();
            Academics.GetItems();
            Institutions.GetItems();
            PhysicalObjects.GetItems();
            PrimaryActivities.GetItems();
            Questionnaires.GetItems();
            QuestionnaireQuestions.GetItems();
        }

        public static StudentPracticeCacheManager<SubSystem> SubSystems
        {
            get { return StudentPracticeCacheManager<SubSystem>.Current; }
        }

        public static StudentPracticeCacheManager<IncidentType> IncidentTypes
        {
            get { return StudentPracticeCacheManager<IncidentType>.Current; }
        }

        public static StudentPracticeCacheManager<SubSystemReporterType> SubSystemReporterTypes
        {
            get { return StudentPracticeCacheManager<SubSystemReporterType>.Current; }
        }

        public static StudentPracticeCacheManager<ReporterIncidentType> ReporterIncidentTypes
        {
            get { return StudentPracticeCacheManager<ReporterIncidentType>.Current; }
        }

        public static StudentPracticeCacheManager<City> Cities
        {
            get { return StudentPracticeCacheManager<City>.Current; }
        }

        public static StudentPracticeCacheManager<Prefecture> Prefectures
        {
            get { return StudentPracticeCacheManager<Prefecture>.Current; }
        }

        public static StudentPracticeCacheManager<Region> Regions
        {
            get { return StudentPracticeCacheManager<Region>.Current; }
        }

        public static StudentPracticeCacheManager<Country> Countries
        {
            get { return StudentPracticeCacheManager<Country>.Current; }
        }

        public static StudentPracticeCacheManager<GlobalRegion> GlobalRegions
        {
            get { return StudentPracticeCacheManager<GlobalRegion>.Current; }
        }

        public static StudentPracticeCacheManager<Academic> Academics
        {
            get { return StudentPracticeCacheManager<Academic>.Current; }
        }

        public static StudentPracticeCacheManager<Institution> Institutions
        {
            get { return StudentPracticeCacheManager<Institution>.Current; }
        }

        public static StudentPracticeCacheManager<PhysicalObject> PhysicalObjects
        {
            get { return StudentPracticeCacheManager<PhysicalObject>.Current; }
        }

        public static StudentPracticeCacheManager<PrimaryActivity> PrimaryActivities
        {
            get { return StudentPracticeCacheManager<PrimaryActivity>.Current; }
        }

        public static StudentPracticeCacheManager<Questionnaire> Questionnaires
        {
            get { return StudentPracticeCacheManager<Questionnaire>.Current; }
        }

        public static StudentPracticeCacheManager<QuestionnaireQuestion> QuestionnaireQuestions
        {
            get { return StudentPracticeCacheManager<QuestionnaireQuestion>.Current; }
        }

        #region [ Ordered Lists ]

        public static List<Institution> GetOrderedInstitutions()
        {
            return StudentPracticeCacheManager<Institution>.Current
                    .GetItems()
                    .OrderBy(x => x.Name)
                    .ToList();
        }

        public static List<Academic> GetOrderedAcademics(int institutionID)
        {
            return StudentPracticeCacheManager<Academic>.Current
                        .GetItems()
                        .Where(x => x.InstitutionID == institutionID)
                        .OrderBy(x => x.Department)
                        .ToList();
        }

        public static List<Academic> GetActiveAcademics(int institutionID)
        {
            return StudentPracticeCacheManager<Academic>.Current
                        .GetItems()
                        .Where(x => x.InstitutionID == institutionID)
                        .Where(x => x.IsActive)
                        .OrderBy(x => x.Department)
                        .ToList();
        }

        public static List<Prefecture> GetOrderedPrefectures()
        {
            return StudentPracticeCacheManager<Prefecture>.Current
                    .GetItems()
                    .Where(x => x.ID != 0)
                    .OrderBy(x => x.Name)
                    .ToList();
        }

        public static List<City> GetOrderedCities(int prefectureID)
        {
            return StudentPracticeCacheManager<City>.Current
                        .GetItems()
                        .Where(x => x.PrefectureID == prefectureID)
                        .OrderBy(x => x.Name)
                        .ToList();
        }

        #endregion

        public static void Refresh()
        {
            SubSystems.Refresh();
            IncidentTypes.Refresh();
            SubSystemReporterTypes.Refresh();
            ReporterIncidentTypes.Refresh();
            Cities.Refresh();
            Prefectures.Refresh();
            Regions.Refresh();
            Countries.Refresh();
            GlobalRegions.Refresh();
            Academics.Refresh();
            Institutions.Refresh();
            PhysicalObjects.Refresh();
            PrimaryActivities.Refresh();
            Questionnaires.Refresh();
            QuestionnaireQuestions.Refresh();
        }
    }
}
