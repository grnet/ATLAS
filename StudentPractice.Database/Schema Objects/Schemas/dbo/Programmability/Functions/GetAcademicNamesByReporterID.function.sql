CREATE FUNCTION [dbo].[GetAcademicNamesByReporterID]
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
