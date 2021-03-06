﻿CREATE TABLE [dbo].[InternshipPositionGroup] (
    [ID]                      INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReporterID]              INT            NOT NULL,
    [TotalPositions]          INT            NOT NULL,
    [AvailablePositions]      INT            NOT NULL,
    [PreAssignedPositions]    INT            NOT NULL,
    [IsVisibleToAllAcademics] BIT            NULL,
    [Title]                   NVARCHAR (500) NOT NULL,
    [Description]             NVARCHAR (MAX) NULL,
    [Duration]                INT            NOT NULL,
    [CityID]                  INT            NULL,
    [PrefectureID]            INT            NULL,
    [CountryID]               INT            NOT NULL,
	[CityText]                NVARCHAR (200) NULL,	
    [NoTimeLimit]             BIT            NOT NULL,
    [StartDate]               DATETIME       NULL,
    [EndDate]                 DATETIME       NULL,
    [PositionType]            INT            NOT NULL,
    [ContactPhone]            NVARCHAR (50)  NOT NULL,
    [FirstPublishedAt]        DATETIME       NULL,
    [LastPublishedAt]         DATETIME       NULL,
    [CreatedAt]               DATETIME       NOT NULL,
    [CreatedAtDateOnly]       DATETIME       NOT NULL,
    [CreatedBy]               NVARCHAR (256) NOT NULL,
    [UpdatedAt]               DATETIME       NULL,
    [UpdatedBy]               NVARCHAR (256) NULL,
    [Supervisor]              NVARCHAR (500) NULL,
    [PositionGroupStatus]     INT            NOT NULL,
    [SupervisorEmail]         NVARCHAR (256) NULL,
    [PositionCreationType]    INT            NOT NULL
);

