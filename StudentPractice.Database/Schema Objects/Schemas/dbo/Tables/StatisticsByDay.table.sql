CREATE TABLE [dbo].[StatisticsByDay] (
    [CreatedAt]                    DATETIME NOT NULL,
    [CreatedPositions]             INT      NOT NULL,
    [PublishedPositions]           INT      NOT NULL,
    [PreAssignedPositions]         INT      NOT NULL,
    [AssignedPositions]            INT      NOT NULL,
    [UnderImplementationPositions] INT      NOT NULL,
    [CompletedPositions]           INT      NOT NULL,
    [CanceledPositions]            INT      NOT NULL, 
    [RevokedPositions] INT NOT NULL, 
    [DeletedPositions] INT NOT NULL, 
    [CompletedByOfficePositions] INT NOT NULL
);

