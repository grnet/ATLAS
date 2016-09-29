CREATE VIEW [dbo].[vStatisticsByProvider]
AS
SELECT r.ID AS ProviderID, r.ProviderName, r.ProviderTradeName, r.ProviderAFM, r.ProviderDOY, 
	   CASE 
			WHEN t1.CreatedPositions IS NULL THEN 0 		
			ELSE t1.CreatedPositions 
	   END AS CreatedPositions,
	   CASE 
			WHEN t2.PublishedPositions IS NULL THEN 0 		
			ELSE t2.PublishedPositions 
	   END AS PublishedPositions, 
	   CASE 
			WHEN t3.PreAssignedPositions IS NULL THEN 0 		
			ELSE t3.PreAssignedPositions 
	   END AS PreAssignedPositions, 
	   CASE 
			WHEN t4.AssignedPositions IS NULL THEN 0 
			ELSE t4.AssignedPositions 
	   END AS AssignedPositions, 
	   CASE 
			WHEN t5.UnderImplementationPositions IS NULL THEN 0 
			ELSE t5.UnderImplementationPositions 
	   END AS UnderImplementationPositions, 
	   CASE 
			WHEN t6.CompletedPositions IS NULL THEN 0 
			ELSE t6.CompletedPositions 
	   END AS CompletedPositions, 
	   CASE 
			WHEN t7.CanceledPositions IS NULL THEN 0 
			ELSE t7.CanceledPositions 
	   END AS CanceledPositions,
	   CASE 
			WHEN t8.CompletedFromOfficePositions IS NULL THEN 0 
			ELSE t8.CompletedFromOfficePositions 
	   END AS CompletedFromOfficePositions
FROM Reporter r
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS CreatedPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus >= 0
        GROUP BY pg.ReporterID
	) AS t1 ON r.ID = t1.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS PublishedPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus >= 1
        GROUP BY pg.ReporterID
	) AS t2 ON r.ID = t2.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS PreAssignedPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus >= 2
        GROUP BY pg.ReporterID
	) AS t3 ON r.ID = t3.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS AssignedPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus >= 3
        GROUP BY pg.ReporterID
	) AS t4 ON r.ID = t4.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS UnderImplementationPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus >= 4
        GROUP BY pg.ReporterID
	) AS t5 ON r.ID = t5.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS CompletedPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus = 5
        GROUP BY pg.ReporterID
	) AS t6 ON r.ID = t6.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS CanceledPositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus = 6
        GROUP BY pg.ReporterID
	) AS t7 ON r.ID = t7.ReporterID
	LEFT JOIN
	(
		SELECT pg.ReporterID, COUNT(p.ID) AS CompletedFromOfficePositions
        FROM InternshipPositionGroup pg 
			INNER JOIN InternshipPosition p ON pg.ID = p.GroupID
        WHERE p.PositionStatus = 5 AND pg.PositionCreationType = 1
        GROUP BY pg.ReporterID
	) AS t8 ON r.ID = t8.ReporterID
WHERE r.ReporterType = 3
AND r.VerificationStatus = 1