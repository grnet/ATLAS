CREATE View [dbo].[vOfficeCounters] AS
	SELECT r.ID AS OfficeID, 
		   CASE
				WHEN r.OfficeType = 1 THEN 'Ιδρυματικό'
				WHEN r.OfficeType = 2 THEN 'Τμηματικό'
				WHEN r.OfficeType = 3 THEN 'Πολυ-Τμηματικό'
		   END AS OfficeType,
		   r.Institution, [dbo].[GetAcademicNamesByReporterID](ID) AS Academics,
		   CASE
				WHEN TotalPreAssignedPositions IS NULL THEN 0
				ELSE TotalPreAssignedPositions
		   END AS TotalPreAssignedPositions,
		   CASE
				WHEN TotalAssignedPositions IS NULL THEN 0
				ELSE TotalAssignedPositions
		   END AS TotalAssignedPositions
	FROM
	( 
		SELECT r.ID, r.OfficeType, i.Name AS Institution
		FROM Reporter r
			INNER JOIN Institution i ON i.ID = r.InstitutionID
		WHERE r.ReporterType = 4
		AND r.IsMasterAccount = 1
		AND r.VerificationStatus = 1
	) r
	LEFT JOIN
	(
		SELECT ipl.PreAssignedByMasterAccountID, COUNT(DISTINCT ipl.InternshipPositionID) AS TotalPreAssignedPositions
		FROM InternshipPositionLog ipl
		WHERE ipl.OldStatus = 1 
		AND ipl.NewStatus = 2
		GROUP BY ipl.PreAssignedByMasterAccountID
	) t1 ON r.id = t1.PreAssignedByMasterAccountID 
	LEFT JOIN
	(
		SELECT ipl.AssignedByMasterAccountID, COUNT(DISTINCT ipl.InternshipPositionID) AS TotalAssignedPositions
		FROM InternshipPositionLog ipl	
		WHERE ipl.OldStatus = 2 
		AND (ipl.NewStatus = 3 OR ipl.NewStatus = 4)
		GROUP BY ipl.AssignedByMasterAccountID
	) t2 ON r.id = t2.AssignedByMasterAccountID


