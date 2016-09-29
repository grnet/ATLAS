CREATE TABLE [dbo].[Academic] (
    [ID]                             INT            NOT NULL,
    [InstitutionID]                  INT            NOT NULL,
    [Institution]                    NVARCHAR (100) NULL,
    [School]                         NVARCHAR (100) NULL,
    [Department]                     NVARCHAR (100) NULL,
    [Address]                        NVARCHAR (200) NULL,
    [ZipCode]                        NVARCHAR (5)   NULL,
    [CityID]                         INT            NULL,
    [PrefectureID]                   INT            NULL,
    [Semesters]                      INT            NULL,
    [MaxAllowedPreAssignedPositions] INT            NOT NULL,
    [PreAssignedPositions]           INT            NOT NULL, 
    [PositionRules] NVARCHAR(MAX) NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [InstitutionInLatin] NVARCHAR(200) NULL, 
    [SchoolInLatin] NVARCHAR(200) NULL, 
    [DepartmentInLatin] NVARCHAR(200) NULL
);

