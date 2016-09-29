namespace StudentPractice.BusinessModel
{
    public enum enErrorCode
    {
        AccessServices = 1,

        UserValidation,

        ObjectNotFount,

        NoAcademinID,

        NoGroupID,

        NoPositionNumber,

        OfficeIDError,

        AvailablePreAssignLimit,

        NumberOfPositionsError,

        PreAssignPositionError,

        StudentCreationError,

        PositionIsAssigned,

        StudentIsAssigned,

        CompleteImplementationError,

        CancelImplementationError,

        RollbackPreAssignmentPenalty,

        RollbackPreAssignmentError,

        DeleteAssignmentPenalty,

        DeleteAssignmentError,

        StudentUpdateError,

        StudentUpdateNotDeclaredFromService,

        StudentUpdateErrorUsernameExists,

        AcademicIDNotFound,

        AcademicIDNotInOffice,

        AcademinIDFYPAUnavailable,

        StudentUpdateHasAcademicID,

        StudentUpdateErrorAcademicIDExists,

        StudentUpdateErrorIsAssigned,

        StudentUpdateDifferentAcademicID,

        CancelNoCancelReason,

        DateTimeEndLessThanStart,

        AssignWrongAcademic,

        PreAssignWrongAcademic,

        PreAssignNoAcademic,

        StudentNumberNotFound,

        OnlyAcademicIDOrData,

        OfficeHasNotAcademic,

        ProviderNotFound,

        StudentNotFound,

        AcademicIDExists,

        AcademicIDExistsProceedToUpdate,

        FinishedPositionNotExists,

        FinishedPositionNotInUser,

        AcIDCreationError,

        AcIDUpdateError,

        AcademicDeleted,

        AcademicNotActive,

        StudentNotActive,

        InternalServerError,

        NoFundingType
    }
}
