﻿CREATE TABLE [dbo].[IncidentReport] (
    [ID]                   INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SubSystemID]          INT            NOT NULL,
    [SubmissionType]       INT            NOT NULL,
    [CallType]             INT            NOT NULL,
    [HandlerType]          INT            NOT NULL,
    [HandlerStatus]        INT            NOT NULL,
    [IncidentTypeID]       INT            NOT NULL,
    [ReporterID]           INT            NOT NULL,
    [ReportStatus]         INT            NOT NULL,
    [ReporterName]         NVARCHAR (100) NULL,
    [ReporterPhone]        NVARCHAR (100) NULL,
    [ReporterEmail]        NVARCHAR (100) NULL,
    [ReportText]           NVARCHAR (MAX) NULL,
    [LastPostID]           INT            NULL,
    [LastDispatchedPostID] INT            NULL,
    [IsLocked]             BIT            NOT NULL,
    [LastLockAt]           DATETIME       NULL,
    [LastLockBy]           NVARCHAR (256) NULL,
    [CreatedAt]            DATETIME       NOT NULL,
    [CreatedBy]            NVARCHAR (50)  NOT NULL,
    [UpdatedAt]            DATETIME       NULL,
    [UpdatedBy]            NVARCHAR (50)  NULL,
    [CreatedAtDateOnly]    DATETIME       NOT NULL
);

