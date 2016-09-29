CREATE TABLE [dbo].[Country] (
    [ID]             INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [GlobalRegionID] INT           NOT NULL, 
    [NameInLatin] NVARCHAR(50) NULL
);

