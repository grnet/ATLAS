CREATE TABLE [dbo].[IncidentReportPost] (
    [ID]                INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IncidentReportID]  INT            NOT NULL,
    [CallType]          INT            NOT NULL,
    [ParentID]          INT            NULL,
    [PostText]          NVARCHAR (MAX) NOT NULL,
    [LastDispatchID]    INT            NULL,
    [CreatedAt]         DATETIME       NOT NULL,
    [CreatedBy]         NVARCHAR (50)  NOT NULL,
    [UpdatedAt]         DATETIME       NULL,
    [UpdatedBy]         NVARCHAR (50)  NULL,
    [CreatedAtDateOnly] DATETIME       NOT NULL
);

