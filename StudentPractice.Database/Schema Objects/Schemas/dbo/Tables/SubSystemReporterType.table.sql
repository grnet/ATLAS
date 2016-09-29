CREATE TABLE [dbo].[SubSystemReporterType] (
    [ID]           INT IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SubSystemID]  INT NOT NULL,
    [ReporterType] INT NOT NULL
);

