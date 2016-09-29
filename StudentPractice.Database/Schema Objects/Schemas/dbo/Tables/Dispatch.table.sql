CREATE TABLE [dbo].[Dispatch] (
    [ID]                   INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IncidentReportPostID] INT            NOT NULL,
    [DispatchType]         INT            NOT NULL,
    [DispatchText]         NVARCHAR (MAX) NOT NULL,
    [DispatchSentAt]       DATETIME       NOT NULL,
    [DispatchSentBy]       NVARCHAR (50)  NOT NULL
);

