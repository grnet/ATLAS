USE [StudentPractice]
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















USE [StudentPractice]
GO

CREATE View [dbo].[vReportsDefault] AS
SELECT	
		TotalInternshipProviders,
		TotalInternshipProviders_Verified,
		TotalInternshipOffices,
		TotalInternshipOffices_Verified,
		TotalStudents,
		TotalInternshipPostions,
		TotalUnPublishedInternshipPostions,
		TotalAvailableInternshipPostions,
		TotalPreAssignedInternshipPostions,
		TotalAssignedInternshipPostions,
		TotalUnderImplementationInternshipPostions,
		TotalCompletedInternshipPostions,
		TotalCanceledInternshipPostions	
FROM
		--Count the total registered internship providers
		(
			SELECT COUNT(r.ID) AS TotalInternshipProviders
			FROM Reporter r
			WHERE r.ReporterType = 3			
		) t1,
		--Count the total registered verified internship providers
		(
			SELECT COUNT(r.ID) AS TotalInternshipProviders_Verified
			FROM Reporter r
			WHERE r.ReporterType = 3
			AND r.VerificationStatus = 1
		) t2,


		--Count the total registered internship offices
		(
			SELECT COUNT(r.ID) AS TotalInternshipOffices 
			FROM Reporter r
			WHERE r.ReporterType = 4			
		) t3,
		--Count the total registered verified internship offices
		(
			SELECT COUNT(r.ID) AS TotalInternshipOffices_Verified
			FROM Reporter r
			WHERE r.ReporterType = 4
			AND r.VerificationStatus = 1
		) t4,


		--Count the total registered students
		(
			SELECT COUNT(r.ID) AS TotalStudents
			FROM Reporter r
			WHERE r.ReporterType = 5
		) t5,


		--Count the total created internship positions
		(
			SELECT COUNT(p.ID) AS TotalInternshipPostions
			FROM InternshipPosition p
		) t6,
		--Count the total created unpublished internship positions
		(
			SELECT COUNT(p.ID) AS TotalUnPublishedInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 0
		) t7,
		--Count the total created available internship positions
		(
			SELECT COUNT(p.ID) AS TotalAvailableInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 1
		) t8,
		--Count the total created preassigned internship positions
		(
			SELECT COUNT(p.ID) AS TotalPreAssignedInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 2
		) t9,
		--Count the total created assigned internship positions
		(
			SELECT COUNT(p.ID) AS TotalAssignedInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 3
		) t10,
		--Count the total created under implementation internship positions
		(
			SELECT COUNT(p.ID) AS TotalUnderImplementationInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 4
		) t11,
		--Count the total created completed internship positions
		(
			SELECT COUNT(p.ID) AS TotalCompletedInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 5
		) t12,
		--Count the total created canceled internship positions
		(
			SELECT COUNT(p.ID) AS TotalCanceledInternshipPostions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 6
		) t13



GO











USE [StudentPractice]
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
	WHERE ReporterType = 4 AND IsMasterAccount = 1 AND VerificationStatus = 1
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











USE [StudentPractice]
GO

CREATE VIEW [dbo].[vStatisticsByProvider]
AS
SELECT     TOP (100) PERCENT r.ID, r.ProviderName, r.ProviderTradeName, r.ProviderAFM, r.ProviderDOY, CASE WHEN PreAssignedPositions IS NULL 
                      THEN 0 ELSE PreAssignedPositions END AS PreAssignedPositions, CASE WHEN AssignedPositions IS NULL 
                      THEN 0 ELSE AssignedPositions END AS AssignedPositions, CASE WHEN UnderImplementationPositions IS NULL 
                      THEN 0 ELSE UnderImplementationPositions END AS UnderImplementationPositions, CASE WHEN CompletedPositions IS NULL 
                      THEN 0 ELSE CompletedPositions END AS CompletedPositions, CASE WHEN CanceledPositions IS NULL 
                      THEN 0 ELSE CanceledPositions END AS CanceledPositions
FROM         dbo.Reporter AS r LEFT OUTER JOIN
                          (SELECT     pg.ReporterID AS ProviderID, COUNT(p.ID) AS PreAssignedPositions
                            FROM          dbo.InternshipPositionGroup AS pg INNER JOIN
                                                   dbo.InternshipPosition AS p ON pg.ID = p.GroupID
                            WHERE      (p.PositionStatus > 1)
                            GROUP BY pg.ReporterID) AS t1 ON r.ID = t1.ProviderID LEFT OUTER JOIN
                          (SELECT     pg.ReporterID AS ProviderID, COUNT(p.ID) AS AssignedPositions
                            FROM          dbo.InternshipPositionGroup AS pg INNER JOIN
                                                   dbo.InternshipPosition AS p ON pg.ID = p.GroupID
                            WHERE      (p.PositionStatus > 2)
                            GROUP BY pg.ReporterID) AS t2 ON r.ID = t2.ProviderID LEFT OUTER JOIN
                          (SELECT     pg.ReporterID AS ProviderID, COUNT(p.ID) AS UnderImplementationPositions
                            FROM          dbo.InternshipPositionGroup AS pg INNER JOIN
                                                   dbo.InternshipPosition AS p ON pg.ID = p.GroupID
                            WHERE      (p.PositionStatus > 3)
                            GROUP BY pg.ReporterID) AS t3 ON r.ID = t3.ProviderID LEFT OUTER JOIN
                          (SELECT     pg.ReporterID AS ProviderID, COUNT(p.ID) AS CompletedPositions
                            FROM          dbo.InternshipPositionGroup AS pg INNER JOIN
                                                   dbo.InternshipPosition AS p ON pg.ID = p.GroupID
                            WHERE      (p.PositionStatus = 5)
                            GROUP BY pg.ReporterID) AS t4 ON r.ID = t4.ProviderID LEFT OUTER JOIN
                          (SELECT     pg.ReporterID AS ProviderID, COUNT(p.ID) AS CanceledPositions
                            FROM          dbo.InternshipPositionGroup AS pg INNER JOIN
                                                   dbo.InternshipPosition AS p ON pg.ID = p.GroupID
                            WHERE      (p.PositionStatus = 6)
                            GROUP BY pg.ReporterID) AS t5 ON r.ID = t5.ProviderID
WHERE     (r.ReporterType = 3) AND (r.VerificationStatus = 1)
ORDER BY r.ID
GO












USE [StudentPractice]
GO

ALTER PROCEDURE [dbo].[sp_StatisticsByDay]
	@StartDate DATETIME,
	@EndDate DATETIME = NULL
AS

DECLARE @Days TABLE
(
	DateAt DATETIME NOT NULL
);

--Fill the @Days table with the days since the start date
DECLARE @CurDateDay DATETIME
SET @CurDateDay = DATEADD(dd,0, DATEDIFF(dd,0,@StartDate))

DECLARE @EndDateDay DATETIME
SET @EndDateDay =
	CASE 
		WHEN (@EndDate IS NOT NULL) THEN @EndDate
		ELSE GETDATE()
	END

WHILE (@CurDateDay <= DateAdd(dd,0, DATEDIFF(dd,0,@EndDateDay)))
BEGIN
	INSERT INTO @Days
	VALUES (@CurDateDay)

	SET @CurDateDay = DATEADD(DAY, 1, @CurDateDay)
END

--Now do the joins with all the statistics we want 

SELECT DateAt, 
CASE
	WHEN CreatedPositions IS NULL THEN 0
	ELSE CreatedPositions
END AS CreatedPositions,
CASE
	WHEN PublishedPositions IS NULL THEN 0
	ELSE PublishedPositions
END AS PublishedPositions,
CASE
	WHEN PreAssignedPositions IS NULL THEN 0
	ELSE PreAssignedPositions
END AS PreAssignedPositions,
CASE
	WHEN AssignedPositions IS NULL THEN 0
	ELSE AssignedPositions
END AS AssignedPositions,
CASE
	WHEN UnderImplementationPositions IS NULL THEN 0
	ELSE UnderImplementationPositions
END AS UnderImplementationPositions,
CASE
	WHEN CompletedPositions IS NULL THEN 0
	ELSE CompletedPositions
END AS CompletedPositions,
CASE
	WHEN CanceledPositions IS NULL THEN 0
	ELSE CanceledPositions
END AS CanceledPositions
FROM @Days d LEFT JOIN
(
	SELECT p.CreatedAtDateOnly AS CreatedPositionDate, COUNT(p.ID) AS CreatedPositions
	FROM InternshipPosition p	
	GROUP BY CreatedAtDateOnly
) t1 ON d.DateAt = t1.CreatedPositionDate LEFT JOIN
(
	SELECT p.CreatedAtDateOnly AS PublishedPositionDate, COUNT(p.ID) AS PublishedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 0 	
	GROUP BY CreatedAtDateOnly
) t2 ON d.DateAt = t2.PublishedPositionDate LEFT JOIN
(
	SELECT p.PreAssignedAt AS PreAssignedPositionDate, COUNT(p.ID) AS PreAssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 1 	
	GROUP BY PreAssignedAt
) t3 ON d.DateAt = t3.PreAssignedPositionDate LEFT JOIN
(
	SELECT p.AssignedAt AS AssignedPositionDate, COUNT(p.ID) AS AssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 2
	GROUP BY AssignedAt
) t4 ON d.DateAt = t4.AssignedPositionDate LEFT JOIN
(
	SELECT p.ImplementationStartDate AS UnderImplementationPositionDate, COUNT(p.ID) AS UnderImplementationPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 3
	GROUP BY ImplementationStartDate
) t5 ON d.DateAt = t5.UnderImplementationPositionDate LEFT JOIN
(
	SELECT p.CompletedAt AS CompletedPositionDate, COUNT(p.ID) AS CompletedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus = 5
	GROUP BY CompletedAt
) t6 ON d.DateAt = t6.CompletedPositionDate LEFT JOIN
(
	SELECT p.CompletedAt AS CanceledPositionDate, COUNT(p.ID) AS CanceledPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus = 6
	GROUP BY CompletedAt
) t7 ON d.DateAt = t7.CanceledPositionDate
ORDER BY DateAt ASC


GO















