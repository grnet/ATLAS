using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class PositionResult
    {
        public int ID { get; set; }

        public int GroupID { get; set; }
        public PositionGroupResult GroupDetails { get; set; }

        public string PositionStatus { get; set; }
        public int? DaysLeftForAssignment { get; set; }

        public DateTime? PreAssignedAt { get; set; }
        public string PreAssignedAtString { get; set; }

        public DateTime? AssignedAt { get; set; }
        public string AssignedAtString { get; set; }

        public DateTime? ImplementationStartDate { get; set; }
        public string ImplementationStartDateString { get; set; }

        public DateTime? ImplementationEndDate { get; set; }
        public string ImplementationEndDateString { get; set; }

        public DateTime? CompletedAt { get; set; }
        public string CompletedAtString { get; set; }

        public string CompletionComments { get; set; }
        public string CancellationReason { get; set; }

        public StudentResult AssignedStudent { get; set; }
        public AcademicResult PreAssignedForAcademic { get; set; }
    }

    public static class Position
    {
        public static PositionResult ToPositionResult(this InternshipPosition position)
        {
            return new PositionResult()
            {
                ID = position.ID,
                GroupID = position.GroupID,
                GroupDetails = position.InternshipPositionGroup.ToPositionGroupResult(),
                PositionStatus = position.PositionStatus.GetLabel(),

                DaysLeftForAssignment = position.DaysLeftForAssignment,


                PreAssignedAt = position.PreAssignedAt,
                PreAssignedAtString = position.PreAssignedAt.HasValue ? position.PreAssignedAt.Value.ToString("dd/MM/yyyy") : string.Empty,

                AssignedAt = position.AssignedAt,
                AssignedAtString = position.AssignedAt.HasValue ? position.AssignedAt.Value.ToString("dd/MM/yyyy") : string.Empty,

                ImplementationStartDate = position.ImplementationStartDate,
                ImplementationStartDateString = position.ImplementationStartDate.HasValue ? position.ImplementationStartDate.Value.ToString("dd/MM/yyyy") : string.Empty,

                ImplementationEndDate = position.ImplementationEndDate,
                ImplementationEndDateString = position.ImplementationEndDate.HasValue ? position.ImplementationEndDate.Value.ToString("dd/MM/yyyy") : string.Empty,

                CompletedAt = position.CompletedAt,
                CompletedAtString = position.CompletedAt.HasValue ? position.CompletedAt.Value.ToString("dd/MM/yyyy") : string.Empty,


                CompletionComments = position.CompletionComments,
                CancellationReason = position.CancellationReason.GetLabel(),

                AssignedStudent = position.AssignedToStudent != null ? position.AssignedToStudent.ToStudentResult() : null,
                PreAssignedForAcademic = position.PreAssignedForAcademicID.HasValue ? StudentPracticeCacheManager<Academic>.Current.Get(position.PreAssignedForAcademicID.Value).ToServiceLookup() : null
            };
        }
    }
}