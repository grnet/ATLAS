
INSERT INTO ReporterAcademicXRef (ReporterID, AcademicID)
SELECT r.ID, a.ID
FROM Reporter r
INNER JOIN Academic a ON a.InstitutionID = r.InstitutionID
WHERE r.ReporterType = 4 AND OfficeType = 1


USE [StudentPractice]
GO
/****** Object:  UserDefinedFunction [dbo].[GetAcademicNamesByReporterID]    Script Date: 02/22/2013 13:25:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GetAcademicNamesByReporterID]
(@reporterID INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN

	DECLARE @result NVARCHAR(MAX)
	SELECT  
		@result = CASE
		WHEN @result IS NULL
		THEN a.Department
		ELSE @result + ';' + a.Department
	END
	FROM ReporterAcademicXRef ra
	INNER JOIN Academic a ON ra.AcademicID = a.ID
	INNER JOIN Reporter r ON ra.ReporterID = r.ID
	WHERE ra.ReporterID = @reporterID AND (r.CanViewAllAcademics IS NULL OR r.CanViewAllAcademics = 0)
	
	RETURN @result
END
