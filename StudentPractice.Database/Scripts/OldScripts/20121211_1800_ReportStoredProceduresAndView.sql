CREATE TABLE [dbo].[StatisticsByDay](
	[CreatedAt] [datetime] NOT NULL,
	[CreatedPositions] [int] NOT NULL,
	[PublishedPositions] [int] NOT NULL,
	[PreAssignedPositions] [int] NOT NULL,
	[AssignedPositions] [int] NOT NULL,
	[UnderImplementationPositions] [int] NOT NULL,
	[CompletedPositions] [int] NOT NULL,
	[CanceledPositions] [int] NOT NULL
) ON [PRIMARY]

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

CREATE PROCEDURE [dbo].[sp_StatisticsByDay]
	@StartDate DATETIME,
	@EndDate DATETIME = NULL
AS

DECLARE @Days TABLE
(
	CreatedAt DATETIME NOT NULL
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

SELECT CreatedAt, 
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
	SELECT CreatedAtDateOnly AS CreatedPositionDate, COUNT(p.ID) AS CreatedPositions
	FROM InternshipPosition p	
	GROUP BY CreatedAtDateOnly
) t1 ON d.CreatedAt = t1.CreatedPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS PublishedPositionDate, COUNT(p.ID) AS PublishedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 0 	
	GROUP BY CreatedAtDateOnly
) t2 ON d.CreatedAt = t2.PublishedPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS PreAssignedPositionDate, COUNT(p.ID) AS PreAssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 1 	
	GROUP BY CreatedAtDateOnly
) t3 ON d.CreatedAt = t3.PreAssignedPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS AssignedPositionDate, COUNT(p.ID) AS AssignedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 2
	GROUP BY CreatedAtDateOnly
) t4 ON d.CreatedAt = t4.AssignedPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS UnderImplementationPositionDate, COUNT(p.ID) AS UnderImplementationPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus > 3
	GROUP BY CreatedAtDateOnly
) t5 ON d.CreatedAt = t5.UnderImplementationPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS CompletedPositionDate, COUNT(p.ID) AS CompletedPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus = 5
	GROUP BY CreatedAtDateOnly
) t6 ON d.CreatedAt = t6.CompletedPositionDate LEFT JOIN
(
	SELECT CreatedAtDateOnly AS CanceledPositionDate, COUNT(p.ID) AS CanceledPositions
	FROM InternshipPosition p
	WHERE p.PositionStatus = 6
	GROUP BY CreatedAtDateOnly
) t7 ON d.CreatedAt = t7.CanceledPositionDate
ORDER BY CreatedAt ASC

GO

CREATE PROCEDURE [dbo].[sp_UpdateStatisticsByDay]
AS 
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @StartDate DATETIME
        DECLARE @EndDate DATETIME

        SET @StartDate = '2012-12-01'
        SET @EndDate = DATEADD(dd, -1, DATEDIFF(dd, 0, GETDATE()))

        CREATE TABLE #TempStatisticsByDay_All
            (
              [CreatedAt] [datetime] NOT NULL ,
              [CreatedPositions] [int] NOT NULL ,
              [PublishedPositions] [int] NOT NULL ,
              [PreAssignedPosistions] [int] NOT NULL ,
              [AssignedPositions] [int] NOT NULL ,
              [UnderImplementationPositions] [int] NOT NULL ,
              [CompletedPositions] [int] NOT NULL ,
              [CanceledPositions] [int] NOT NULL 
            )
        ON  [PRIMARY]


--CLEAR StatisticsByDay Table
		TRUNCATE TABLE StatisticsByDay

--INSERT All Student Statistics into Temp Table
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
        INSERT  INTO StatisticsByDay
                EXEC sp_StatisticsByDay @StartDate, @EndDate


	END