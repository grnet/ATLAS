CREATE TABLE [dbo].[MassMessage] (
    [ID]           INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReporterType] INT            NOT NULL,
    [DispatchType] INT            NOT NULL,
    [SentAt]       DATETIME       NOT NULL,
    [MessageText]  NVARCHAR (MAX) NOT NULL
);

