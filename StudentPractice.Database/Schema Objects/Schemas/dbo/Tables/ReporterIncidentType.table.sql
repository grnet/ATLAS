CREATE TABLE [dbo].[ReporterIncidentType] (
    [ID]             INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IncidentTypeID] INT NOT NULL,
    [ReporterType]   INT NOT NULL
);

