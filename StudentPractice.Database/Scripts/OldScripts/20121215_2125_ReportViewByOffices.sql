USE [StudentPractice]
GO

/****** Object:  View [dbo].[vStatisticsByOffice]    Script Date: 15/12/2012 9:26:30 μμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE View [dbo].[vStatisticsByOffice] AS

SELECT a.ID, a.Institution, 

[dbo].[GetAcademicNamesByReporterID](ID) AS Academic,

a.OfficeType,

CASE
	WHEN t2.PreAssignedPositions IS NULL THEN 0
	ELSE t2.PreAssignedPositions
END AS PreAssignedPositions,

CASE
	WHEN t3.AssignedPositions IS NULL THEN 0
	ELSE t3.AssignedPositions
END AS AssignedPositions,

CASE
	WHEN t4.UnderImplementationPositions IS NULL THEN 0
	ELSE t4.UnderImplementationPositions
END AS UnderImplementationPositions,

CASE
	WHEN t5.CompletedPositions IS NULL THEN 0
	ELSE t5.CompletedPositions
END AS CompletedPositions,

CASE
	WHEN t6.CanceledPositions IS NULL THEN 0
	ELSE t6.CanceledPositions
END AS CanceledPositions

FROM
(
	SELECT re.ID , re.OfficeType , s.Name as Institution FROM Reporter re INNER JOIN Institution s ON re.InstitutionID = s.ID
	WHERE ReporterType = 4 AND IsMasterAccount = 1
) AS a

LEFT JOIN
(
	SELECT PreAssignedByMasterAccountID, COUNT(*) AS PreAssignedPositions
	FROM InternshipPosition 
	WHERE PositionStatus = 2
	GROUP BY PreAssignedByMasterAccountID
) AS t2 ON a.ID = t2.PreAssignedByMasterAccountID


LEFT JOIN
(
	SELECT InternshipPosition.PreAssignedByMasterAccountID, COUNT(*) AS AssignedPositions
	FROM InternshipPosition 
	WHERE PositionStatus = 3
	GROUP BY PreAssignedByMasterAccountID
) AS t3 ON a.ID = t3.PreAssignedByMasterAccountID

LEFT JOIN
(
	SELECT InternshipPosition.PreAssignedByMasterAccountID, COUNT(*) AS UnderImplementationPositions
	FROM InternshipPosition 
	WHERE PositionStatus = 4
	GROUP BY PreAssignedByMasterAccountID
) AS t4 ON a.ID = t4.PreAssignedByMasterAccountID


LEFT JOIN
(
	SELECT InternshipPosition.PreAssignedByMasterAccountID, COUNT(*) AS CompletedPositions
	FROM InternshipPosition 
	WHERE PositionStatus = 5
	GROUP BY PreAssignedByMasterAccountID
) AS t5 ON a.ID = t5.PreAssignedByMasterAccountID

LEFT JOIN
(
	SELECT InternshipPosition.PreAssignedByMasterAccountID, COUNT(*) AS CanceledPositions
	FROM InternshipPosition 
	WHERE PositionStatus = 6
	GROUP BY PreAssignedByMasterAccountID
) AS t6 ON a.ID = t6.PreAssignedByMasterAccountID


GO

