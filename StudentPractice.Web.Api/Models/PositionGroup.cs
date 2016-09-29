using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class PositionGroupRequest
    {
        public int? GroupID { get; set; }
        public int? NumberOfPositions { get; set; }
        public int? AcademicID { get; set; }
    }

    public class PositionGroupResult
    {
        public int ID { get; set; }
        public int ProviderID { get; set; }

        public int AvailablePositions { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public string Country { get; set; }
        public int CountryID { get; set; }
        public string Prefecture { get; set; }
        public int? PrefectureID { get; set; }
        public string City { get; set; }
        public int? CityID { get; set; }

        public string TimeLimit { get; set; }
        public bool HasTimeLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }

        public string PositionType { get; set; }
        public List<string> PhysicalObjects { get; set; }

        public List<AcademicResult> Academics { get; set; }
        public bool? IsAvailableToAllAcademics { get; set; }

        public string Supervisor { get; set; }
        public string SupervisorEmail { get; set; }
        public string ContactPhone { get; set; }
    }

    public static class PositionGroup
    {
        public static PositionGroupResult ToPositionGroupResult(this InternshipPositionGroup group)
        {
            return new PositionGroupResult()
            {
                ID = group.ID,
                ProviderID = group.ProviderID,

                AvailablePositions = group.AvailablePositions,
                Title = group.Title,
                Description = group.Description,
                Duration = group.Duration,

                Country = StudentPracticeCacheManager<Country>.Current.Get(group.CountryID).Name,
                Prefecture = group.PrefectureID.HasValue ? StudentPracticeCacheManager<Prefecture>.Current.Get(group.PrefectureID.Value).Name : string.Empty,
                City = group.CityID.HasValue ? StudentPracticeCacheManager<City>.Current.Get(group.CityID.Value).Name : group.CityText,
                CountryID = group.CountryID,
                PrefectureID = group.PrefectureID,
                CityID = group.CityID,

                TimeLimit = group.NoTimeLimit ? "Χωρίς χρονικό περιορισμό" : "Με χρονικό περιορισμό",
                HasTimeLimit = !group.NoTimeLimit,
                StartDate = group.StartDate,
                StartDateString = group.StartDate.HasValue ? group.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                EndDate = group.EndDate,
                EndDateString = group.EndDate.HasValue ? group.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty,

                PositionType = group.PositionType.GetLabel(),
                PhysicalObjects = group.PhysicalObjects.Any() ? group.PhysicalObjects.Select(x => x.Name).ToList() : null,

                Academics = group.Academics.Any() ? group.Academics.Select(x => x.ToServiceLookup()).ToList() : null,
                IsAvailableToAllAcademics = group.IsVisibleToAllAcademics,

                Supervisor = group.Supervisor,
                SupervisorEmail = group.SupervisorEmail,
                ContactPhone = group.ContactPhone
            };
        }
    }
}
