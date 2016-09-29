using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class GPAPositionRequest
    {
        public int? ProviderID { get; set; }
        public int? StudentID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int? PositionTypeInt { get; set; }
        public int? Duration { get; set; }
        public string ContactPhone { get; set; }
        public string Supervisor { get; set; }
        public string SupervisorEmail { get; set; }

        public int? CountryID { get; set; }
        public int? PrefectureID { get; set; }
        public int? CityID { get; set; }
        public List<int> PhysicalObjectsID { get; set; }

        public DateTime? ImplementationStartDate { get; set; }
        public DateTime? ImplementationEndDate { get; set; }

        public string ImplementationStartDateString { get; set; }
        public string ImplementationEndDateString { get; set; }
        public string ImplementationStartDateStringFormat { get; set; }
        public string ImplementationEndDateStringFormat { get; set; }

        public string CompletionComments { get; set; }
    }

    public class GPAPosition
    {
        public int? ID { get; set; }
        public int? ProviderID { get; set; }
        public int? StudentID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PositionType { get; set; }
        public int? PositionTypeInt { get; set; }
        public int? Duration { get; set; }
        public string ContactPhone { get; set; }
        public string Supervisor { get; set; }
        public string SupervisorEmail { get; set; }

        public string Country { get; set; }
        public int? CountryID { get; set; }

        public string Prefecture { get; set; }
        public int? PrefectureID { get; set; }

        public string City { get; set; }
        public int? CityID { get; set; }

        public List<string> PhysicalObjects { get; set; }
        public List<int> PhysicalObjectsID { get; set; }

        public DateTime? ImplementationStartDate { get; set; }
        public DateTime? ImplementationEndDate { get; set; }

        public string ImplementationStartDateString { get; set; }
        public string ImplementationEndDateString { get; set; }
        public string ImplementationStartDateStringFormat { get; set; }
        public string ImplementationEndDateStringFormat { get; set; }

        public string CompletionComments { get; set; }
    }

    public static class GPAPositionExt
    {
        public static GPAPosition ToGPAPosition(this InternshipPosition position)
        {
            return new GPAPosition()
            {
                ID = position.ID,

                ProviderID = position.InternshipPositionGroup.ProviderID,
                StudentID = position.AssignedToStudentID,

                Title = position.InternshipPositionGroup.Title,
                Description = position.InternshipPositionGroup.Description,
                PositionType = position.InternshipPositionGroup.PositionType.GetLabel(),
                PositionTypeInt = position.InternshipPositionGroup.PositionTypeInt,
                Duration = position.InternshipPositionGroup.Duration,
                ContactPhone = position.InternshipPositionGroup.ContactPhone,
                Supervisor = position.InternshipPositionGroup.Supervisor,
                SupervisorEmail = position.InternshipPositionGroup.SupervisorEmail,

                Country = StudentPracticeCacheManager<Country>.Current.Get(position.InternshipPositionGroup.CountryID).Name,
                CountryID = position.InternshipPositionGroup.CountryID,

                Prefecture = position.InternshipPositionGroup.PrefectureID.HasValue ?
                    StudentPracticeCacheManager<Prefecture>.Current.Get(position.InternshipPositionGroup.PrefectureID.Value).Name : string.Empty,
                PrefectureID = position.InternshipPositionGroup.PrefectureID,

                City = position.InternshipPositionGroup.CityID.HasValue ?
                    StudentPracticeCacheManager<City>.Current.Get(position.InternshipPositionGroup.CityID.Value).Name : position.InternshipPositionGroup.CityText,
                CityID = position.InternshipPositionGroup.CityID,

                PhysicalObjectsID = position.InternshipPositionGroup.PhysicalObjects.Any() ? position.InternshipPositionGroup.PhysicalObjects.Select(x => x.ID).ToList() : null,
                PhysicalObjects = position.InternshipPositionGroup.PhysicalObjects.Any() ? position.InternshipPositionGroup.PhysicalObjects.Select(x => x.Name).ToList() : null,


                ImplementationStartDate = position.InternshipPositionGroup.StartDate,
                ImplementationStartDateString = position.InternshipPositionGroup.StartDate.HasValue ? position.InternshipPositionGroup.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                ImplementationStartDateStringFormat = "dd/MM/yyyy",
                ImplementationEndDate = position.InternshipPositionGroup.EndDate,
                ImplementationEndDateString = position.InternshipPositionGroup.EndDate.HasValue ? position.InternshipPositionGroup.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                ImplementationEndDateStringFormat = "dd/MM/yyyy",
                CompletionComments = position.CompletionComments
            };
        }
    }

}
