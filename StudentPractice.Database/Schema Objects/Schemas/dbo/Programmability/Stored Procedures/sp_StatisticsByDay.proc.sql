CREATE PROCEDURE [dbo].[sp_StatisticsByDay]
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
END AS CanceledPositions,
CASE
	WHEN RevokedPositions IS NULL THEN 0
	ELSE RevokedPositions
END AS RevokedPositions,
CASE
	WHEN DeletedPositions IS NULL THEN 0
	ELSE DeletedPositions
END AS DeletedPositions
FROM @Days d LEFT JOIN
(
	SELECT p.CreatedAtDateOnly AS CreatedPositionDate, COUNT(p.ID) AS CreatedPositions
	FROM InternshipPosition p
		INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
	WHERE pg.PositionCreationType = 0
	GROUP BY p.CreatedAtDateOnly
) t1 ON d.DateAt = t1.CreatedPositionDate 
LEFT JOIN
(
	SELECT p.CreatedAtDateOnly AS PublishedPositionDate, COUNT(p.ID) AS PublishedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 0 	
	GROUP BY CreatedAtDateOnly
) t2 ON d.DateAt = t2.PublishedPositionDate 
LEFT JOIN
(
	SELECT p.PreAssignedAt AS PreAssignedPositionDate, COUNT(p.ID) AS PreAssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 1 	
	GROUP BY PreAssignedAt
) t3 ON d.DateAt = t3.PreAssignedPositionDate 
LEFT JOIN
(
	SELECT p.AssignedAt AS AssignedPositionDate, COUNT(p.ID) AS AssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 2
	GROUP BY AssignedAt
) t4 ON d.DateAt = t4.AssignedPositionDate 
LEFT JOIN
(
	SELECT p.ImplementationStartDate AS UnderImplementationPositionDate, COUNT(p.ID) AS UnderImplementationPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 3
	GROUP BY ImplementationStartDate
) t5 ON d.DateAt = t5.UnderImplementationPositionDate 
LEFT JOIN
(
	SELECT p.CompletedAt AS CompletedPositionDate, COUNT(p.ID) AS CompletedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus = 5
	GROUP BY CompletedAt
) t6 ON d.DateAt = t6.CompletedPositionDate 
LEFT JOIN
(
	SELECT p.CompletedAt AS CanceledPositionDate, COUNT(p.ID) AS CanceledPositions
	FROM InternshipPosition p
		INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
	WHERE pg.PositionCreationType = 0
	AND p.PositionStatus = 6
	AND p.CancellationReason = 1
	GROUP BY CompletedAt
) t7 ON d.DateAt = t7.CanceledPositionDate
LEFT JOIN
(
	SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, p.UpdatedAt)) AS RevokedPositionDate, COUNT(p.ID) AS RevokedPositions
	FROM InternshipPosition p
		INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
	WHERE pg.PositionCreationType = 0
	AND p.PositionStatus = 6
	AND p.CancellationReason > 1
	GROUP BY  DATEADD(dd, 0, DATEDIFF(dd, 0, p.UpdatedAt)) 
) t8 ON d.DateAt = t8.RevokedPositionDate
LEFT JOIN
(
	SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, pg.UpdatedAt)) AS DeletedGroupDate, COUNT(p.ID) AS DeletedPositions
	FROM InternshipPosition p
		INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
	WHERE pg.PositionCreationType = 0
	AND pg.PositionGroupStatus = 2	
	GROUP BY DATEADD(dd, 0, DATEDIFF(dd, 0, pg.UpdatedAt))
) t9 ON d.DateAt = t9.DeletedGroupDate
ORDER BY DateAt ASC