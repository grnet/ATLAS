CREATE TABLE [dbo].[SmsLog] (
    [ID]             INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Type]           SMALLINT       NOT NULL,
    [SendID]         NVARCHAR (200) NOT NULL,
    [ReporterID]     INT            NOT NULL,
    [ReporterNumber] NVARCHAR (12)  NOT NULL,
    [Msg]            NVARCHAR (200) NOT NULL,
    [FieldValues]    NVARCHAR (500) NULL,
    [Status]         INT            NOT NULL,
    [SentAt]         DATETIME       NOT NULL
);

