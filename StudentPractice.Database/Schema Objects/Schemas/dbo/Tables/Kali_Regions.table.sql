CREATE TABLE [dbo].[Kali_Regions] (
    [ID]   INT            NOT NULL,
    [Name] NVARCHAR (100) NOT NULL, 
    [CountryID] INT NOT NULL,
	CONSTRAINT [FK_Kali_Regions_Country] FOREIGN KEY ([CountryID]) REFERENCES [Country]([ID])
);

