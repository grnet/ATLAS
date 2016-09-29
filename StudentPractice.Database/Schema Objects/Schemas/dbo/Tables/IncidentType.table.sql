CREATE TABLE [dbo].[IncidentType] (
    [ID]          INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [SubSystemID] INT            NOT NULL, 
    [NameInLatin] NVARCHAR(100) NULL
);

