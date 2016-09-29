CREATE TABLE [dbo].[QueueEntry]
(
	[ID] INT NOT NULL IDENTITY, 
    [NoOfRetries] INT NOT NULL, 
    [MaxNoOfRetries] INT NULL, 
    [QueueEntryType] INT NOT NULL DEFAULT 0, 
    [QueueDataID] INT NOT NULL,
    [LastAttemptDate] DATETIME NOT NULL, 
    [QueueData] XML NULL, 
    CONSTRAINT [PK_QueueEntry] PRIMARY KEY ([ID])
)
