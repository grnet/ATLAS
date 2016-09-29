-- Changes in Reporter
ALTER TABLE Reporter ADD [Language] INT NULL
GO

-- Changes in Academic
ALTER TABLE Academic ADD InstitutionInLatin NVARCHAR(200) NULL
GO
ALTER TABLE Academic ADD SchoolInLatin NVARCHAR(200) NULL
GO
ALTER TABLE Academic ADD DepartmentInLatin NVARCHAR(200) NULL
GO
UPDATE Academic SET InstitutionInLatin = Institution
GO
UPDATE Academic SET SchoolInLatin = School
GO
UPDATE Academic SET DepartmentInLatin = Department
GO

-- Changes in Country
ALTER TABLE Country ADD NameInLatin NVARCHAR(50) NULL
GO
UPDATE Country SET NameInLatin = Name
GO

-- Changes in PhysicalObject
ALTER TABLE PhysicalObject ADD NameInLatin NVARCHAR(100) NULL
GO
UPDATE PhysicalObject SET NameInLatin = Name
GO

-- Changes in PrimaryActivity
ALTER TABLE PrimaryActivity ADD NameInLatin NVARCHAR(50) NULL
GO
UPDATE PrimaryActivity SET NameInLatin = Name
GO

-- Changes in IncidentType
ALTER TABLE IncidentType ADD NameInLatin NVARCHAR(100) NULL
GO
UPDATE IncidentType SET NameInLatin = Name
GO