
ALTER TABLE dbo.Reporter ADD
	PreviousAcademicID int NULL,
	PreviousStudentNumber nvarchar(50) NULL,
    AcademicIDStatus INT NULL, 
    AcademicIDSubmissionDate DATETIME NULL
GO
ALTER TABLE dbo.Reporter ADD CONSTRAINT
	FK_Reporter_Academic FOREIGN KEY
	(
	PreviousAcademicID
	) REFERENCES dbo.Academic
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

ALTER TABLE dbo.Academic ADD IsActive BIT NOT NULL DEFAULT 1
ALTER TABLE dbo.Reporter ADD IsActive BIT NOT NULL DEFAULT 1