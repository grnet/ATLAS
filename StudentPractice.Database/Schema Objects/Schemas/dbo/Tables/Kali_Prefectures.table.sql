CREATE TABLE [dbo].[Kali_Prefectures] (
    [ID]       INT            NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
    [RegionID] INT            NOT NULL, 
    [CountryID] INT NOT NULL,
	CONSTRAINT [FK_Kali_Prefectures_Country] FOREIGN KEY ([CountryID]) REFERENCES [Country]([ID])
);

