USE [StudentPractice]
GO

/****** Object:  View [dbo].[vOfficeCounter]    Script Date: 21/12/2012 9:08:45 πμ ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE View [dbo].[vOfficeCounter] AS
SELECT repo.ID, repo.CreatedAt , repo.OfficeType , repo.Name AS Institute , [dbo].[GetAcademicNamesByReporterID](ID) AS Academic ,
CASE
	WHEN PreAssigned IS NULL THEN 0
	ELSE PreAssigned
END AS PreAssigned,
CASE
	WHEN Assigned IS NULL THEN 0
	ELSE Assigned
END AS Assigned
FROM
( SELECT a.ID, a.CreatedAt , a.OfficeType , s.Name FROM [dbo].[Reporter] AS a 
  INNER JOIN Institution s ON a.InstitutionID = s.ID
  WHERE a.IsMasterAccount = 1 AND a.ReporterType = 4
) repo 

LEFT JOIN
(
	SELECT l.PreAssignedByMasterAccountID, COUNT(DISTINCT l.InternshipPositionID) AS PreAssigned
	FROM InternshipPositionLog l	
	WHERE l.OldStatus = 1 AND l.NewStatus = 2 
	GROUP BY l.PreAssignedByMasterAccountID
) t1 ON repo.id = t1.PreAssignedByMasterAccountID LEFT JOIN
(
	SELECT l.AssignedByMasterAccountID, COUNT(DISTINCT l.InternshipPositionID) AS Assigned
	FROM InternshipPositionLog l	
	WHERE l.OldStatus = 2 AND l.NewStatus = 3 
	GROUP BY l.AssignedByMasterAccountID
) t2 ON repo.id = t2.AssignedByMasterAccountID





GO

