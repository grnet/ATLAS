CREATE TABLE [dbo].[InternshipPositionGroupLog] (
    [ID]                INT           IDENTITY (1, 1) NOT NULL,
    [GroupID]           INT           NOT NULL,
    [OldStatus]         INT           NOT NULL,
    [NewStatus]         INT           NOT NULL,
    [CreatedAt]         DATETIME      NOT NULL,
    [CreatedAtDateOnly] DATETIME      NOT NULL,
    [CreatedBy]         NVARCHAR (50) NOT NULL,
    [UpdatedBy]         NVARCHAR (50) NULL,
    [UpdatedAt]         DATETIME      NULL,
    [UpdatedAtDateOnly] DATETIME      NULL
);

