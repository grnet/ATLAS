ALTER TABLE Reporter ADD PositionCount INT NULL
GO

UPDATE r
SET PositionCount = (
	SELECT COUNT(ip.ID) 
	FROM InternshipPosition ip 
	WHERE ip.AssignedToReporterID = r.ID
		--AND (ip.PositionStatus = 3 OR ip.PositionStatus = 4 OR ip.PositionStatus = 5 OR (ip.PositionStatus = 6 AND ip.CancellationReason = 1))
)
FROM Reporter r
WHERE r.ReporterType = 5
GO

UPDATE Reporter 
SET IsAssignedToPosition = 0 
WHERE ID IN 
(
	SELECT DISTINCT AssignedToReporterID 
	FROM InternshipPosition
	WHERE PositionStatus = 5
)
GO

UPDATE Reporter 
SET IsAssignedToPosition = 1 
WHERE ID IN 
(
	SELECT DISTINCT AssignedToReporterID 
	FROM InternshipPosition
	WHERE PositionStatus = 3 OR PositionStatus = 4
)
GO