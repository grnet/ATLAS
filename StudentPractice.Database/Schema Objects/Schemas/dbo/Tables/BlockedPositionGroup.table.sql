CREATE TABLE [dbo].[BlockedPositionGroup] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [MasterBlockID]     INT            NULL,
    [GroupID]           INT            NOT NULL,
    [MasterAccountID]   INT            NOT NULL,
    [BlockingReason]    INT            NOT NULL,
    [DaysLeft]          INT            NOT NULL,
    [CreatedAt]         DATETIME       NOT NULL,
    [CreatedAtDateOnly] DATETIME       NOT NULL,
    [CreatedBy]         NVARCHAR (256) NOT NULL,
    [UpdatedAt]         DATETIME       NULL,
    [UpdatedBy]         NVARCHAR (256) NULL
);

