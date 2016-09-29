CREATE TABLE [dbo].[InternshipPosition] (
    [ID]                           INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]                      INT            NOT NULL,
    [PositionStatus]               INT            NOT NULL,
    [PreAssignedByReporterID]      INT            NULL,
    [PreAssignedByMasterAccountID] INT            NULL,
    [PreAssignedForAcademicID]     INT            NULL,
    [PreAssignedAt]                DATETIME       NULL,
    [DaysLeftForAssignment]        INT            NULL,
    [AssignedToReporterID]         INT            NULL,
    [AssignedAt]                   DATETIME       NULL,
    [ImplementationStartDate]      DATETIME       NULL,
    [ImplementationEndDate]        DATETIME       NULL,
    [CompletedAt]                  DATETIME       NULL,
    [CompletionComments]           NVARCHAR (MAX) NULL,
    [CancellationReason]           INT            NOT NULL,
    [CanceledReporterID]           INT            NULL,
    [CreatedAt]                    DATETIME       NOT NULL,
    [CreatedAtDateOnly]            DATETIME       NOT NULL,
    [CreatedBy]                    NVARCHAR (256) NOT NULL,
    [UpdatedAt]                    DATETIME       NULL,
    [UpdatedBy]                    NVARCHAR (256) NULL
);

