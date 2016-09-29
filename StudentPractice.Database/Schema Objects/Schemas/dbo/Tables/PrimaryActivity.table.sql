CREATE TABLE [dbo].[PrimaryActivity] (
    [ID]   INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name] NVARCHAR (50) NOT NULL, 
    [NameInLatin] NVARCHAR(50) NULL
);

