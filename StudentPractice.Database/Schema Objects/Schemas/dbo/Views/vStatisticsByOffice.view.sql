CREATE View [dbo].[vStatisticsByOffice] AS
	SELECT r.ID AS OfficeID,
		   CASE
				WHEN r.OfficeType = 1 THEN 'Ιδρυματικό'
				WHEN r.OfficeType = 2 THEN 'Τμηματικό'
				WHEN r.OfficeType = 3 THEN 'Πολυ-Τμηματικό'
		   END AS OfficeType,
		   r.Institution, [dbo].[GetAcademicNamesByReporterID](ID) AS Academics,
		   CASE
				WHEN t1.PreAssignedPositions IS NULL THEN 0
				ELSE t1.PreAssignedPositions
		   END AS PreAssignedPositions,
		   CASE
				WHEN t2.AssignedPositions IS NULL THEN 0
				ELSE t2.AssignedPositions
		   END AS AssignedPositions,
		   CASE
				WHEN t3.UnderImplementationPositions IS NULL THEN 0
				ELSE t3.UnderImplementationPositions
		   END AS UnderImplementationPositions,
		   CASE
				WHEN t4.CompletedPositions IS NULL THEN 0
				ELSE t4.CompletedPositions
		   END AS CompletedPositions,
		   CASE
				WHEN t5.CanceledPositions IS NULL THEN 0
				ELSE t5.CanceledPositions
		   END AS CanceledPositions,
		   CASE
				WHEN t6.CompletedFromOfficePositions IS NULL THEN 0
				ELSE t6.CompletedFromOfficePositions
		   END AS CompletedFromOfficePositions
	FROM
	(
		SELECT r.ID, r.OfficeType, i.Name as Institution 
		FROM Reporter r
			INNER JOIN Institution i ON i.ID = r.InstitutionID
		WHERE r.ReporterType = 4
		AND r.IsMasterAccount = 1
		AND r.VerificationStatus = 1		
	) AS r
	LEFT JOIN
	(
		SELECT PreAssignedByMasterAccountID, COUNT(*) AS PreAssignedPositions
		FROM InternshipPosition 
		WHERE PositionStatus = 2
		GROUP BY PreAssignedByMasterAccountID
	) AS t1 ON r.ID = t1.PreAssignedByMasterAccountID
	LEFT JOIN
	(
		SELECT PreAssignedByMasterAccountID, COUNT(*) AS AssignedPositions
		FROM InternshipPosition 
		WHERE PositionStatus = 3
		GROUP BY PreAssignedByMasterAccountID
	) AS t2 ON r.ID = t2.PreAssignedByMasterAccountID
	LEFT JOIN
	(
		SELECT PreAssignedByMasterAccountID, COUNT(*) AS UnderImplementationPositions
		FROM InternshipPosition 
		WHERE PositionStatus = 4
		GROUP BY PreAssignedByMasterAccountID
	) AS t3 ON r.ID = t3.PreAssignedByMasterAccountID
	LEFT JOIN
	(
		SELECT PreAssignedByMasterAccountID, COUNT(*) AS CompletedPositions
		FROM InternshipPosition 
		WHERE PositionStatus = 5
		GROUP BY PreAssignedByMasterAccountID
	) AS t4 ON r.ID = t4.PreAssignedByMasterAccountID
	LEFT JOIN
	(
		SELECT PreAssignedByMasterAccountID, COUNT(*) AS CanceledPositions
		FROM InternshipPosition 
		WHERE PositionStatus = 6
		GROUP BY PreAssignedByMasterAccountID
	) AS t5 ON r.ID = t5.PreAssignedByMasterAccountID
	LEFT JOIN
	(
		SELECT p.PreAssignedByMasterAccountID, COUNT(*) AS CompletedFromOfficePositions
		FROM InternshipPosition p
			INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
		WHERE p.PositionStatus = 5 AND pg.PositionCreationType = 1
		GROUP BY p.PreAssignedByMasterAccountID
	) AS t6 ON r.ID = t6.PreAssignedByMasterAccountID


