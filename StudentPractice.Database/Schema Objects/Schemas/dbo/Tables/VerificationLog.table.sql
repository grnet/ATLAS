CREATE TABLE [dbo].[VerificationLog] (
    [ID]                    INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReporterID]            INT            NOT NULL,
    [OldVerificationStatus] INT            NOT NULL,
    [NewVerificationStatus] INT            NOT NULL,
    [CreatedAt]             DATETIME       NOT NULL,
    [CreatedBy]             NVARCHAR (256) NOT NULL
);

