CREATE TABLE [dbo].[Kali_Cities] (
    [ID]           INT            NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [PrefectureID] INT            NOT NULL, 
    [CountryID] INT NOT NULL,
	CONSTRAINT [FK_Kali_Cities_Country] FOREIGN KEY ([CountryID]) REFERENCES [Country]([ID])
);

