CREATE FUNCTION dbo.GetPositionCount (@id int)
RETURNS INT
AS
BEGIN
	declare @Return INT
    SELECT @Return = COUNT(ID) FROM [dbo].[InternshipPosition] WHERE AssignedToReporterID = @id
	RETURN(@Return)
END
GO

ALTER TABLE [dbo].[Reporter]
DROP COLUMN [PositionCount]
GO 

ALTER TABLE [dbo].[Reporter]
ADD [PositionCount] as dbo.GetPositionCount (ID)
GO