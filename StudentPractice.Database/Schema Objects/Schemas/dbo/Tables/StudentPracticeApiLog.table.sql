CREATE TABLE [dbo].[StudentPracticeApiLog] (
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [ServiceCaller]       INT           NOT NULL,
    [ServiceCalledAt]     DATETIME      NOT NULL,
    [ServiceMethodCalled] NVARCHAR (50) NOT NULL,
    [ServiceCallerID]     INT           NULL,
    [ErrorCode]           NVARCHAR (50) NULL,
    [Success]             BIT           NULL,
    [IP]                  NVARCHAR (50) NULL,
    [Request]             XML           NULL
);

