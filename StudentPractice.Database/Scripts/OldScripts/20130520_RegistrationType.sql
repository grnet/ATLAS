ALTER TABLE dbo.Reporter ADD
	RegistrationType int NOT NULL CONSTRAINT DF_Reporter_RegistrationType DEFAULT 0
	
UPDATE Reporter
SET RegistrationType = 1
WHERE DeclarationType = 1

UPDATE Reporter
SET RegistrationType = 2
WHERE DeclarationType = 1
AND ReporterType = 5

UPDATE Reporter
SET RegistrationType = 3
WHERE DeclarationType = 1
AND ReporterType = 5
AND AcademicIDNumber IS NOT NULL