CREATE TABLE [dbo].[EmailLog] (
    [ID]           INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Type]         INT            NOT NULL,
    [ReporterID]   INT            NOT NULL,
    [SentAt]       DATETIME       NOT NULL,
    [EmailAddress] NVARCHAR (256) NOT NULL,
    [Subject]      NVARCHAR (MAX) NOT NULL,
    [Body]         NVARCHAR (MAX) NOT NULL
);

