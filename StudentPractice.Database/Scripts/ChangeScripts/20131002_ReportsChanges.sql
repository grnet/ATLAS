ALTER View [dbo].[vReportsDefault] AS
SELECT	
        TotalInternshipProviders,
        TotalInternshipProviders_Verified,
        TotalInternshipOffices,
        TotalInternshipOffices_Verified,
        TotalStudents,
        TotalInternshipPositions,
        TotalFromOfficeInternshipPositions,
        TotalUnPublishedInternshipPositions,
        TotalAvailableInternshipPositions,
        TotalPreAssignedInternshipPositions,
        TotalAssignedInternshipPositions,
        TotalUnderImplementationInternshipPositions,
        TotalCompletedInternshipPositions,
        TotalCompletedFromOfficeInternshipPositions,
        TotalCanceledInternshipPositionsFromOffice,
        TotalRevokedInternshipPositions,
        TotalDeletedInternshipPositions
FROM
        --Count the total registered internship providers
        (
            SELECT COUNT(r.ID) AS TotalInternshipProviders
            FROM Reporter r
            WHERE r.ReporterType = 3
            AND r.DeclarationType = 1
        ) t1,
        --Count the total verified internship providers
        (
            SELECT COUNT(r.ID) AS TotalInternshipProviders_Verified
            FROM Reporter r
            WHERE r.ReporterType = 3
            AND r.DeclarationType = 1
            AND r.VerificationStatus = 1
        ) t2,


        --Count the total registered internship offices
        (
            SELECT COUNT(r.ID) AS TotalInternshipOffices 
            FROM Reporter r
            WHERE r.ReporterType = 4
            AND r.DeclarationType = 1
            AND r.IsMasterAccount = 1
        ) t3,
        --Count the total verified internship offices
        (
            SELECT COUNT(r.ID) AS TotalInternshipOffices_Verified
            FROM Reporter r
            WHERE r.ReporterType = 4
            AND r.DeclarationType = 1
            AND r.VerificationStatus = 1
            AND r.IsMasterAccount = 1
        ) t4,


        --Count the total registered students
        (
            SELECT COUNT(r.ID) AS TotalStudents
            FROM Reporter r
            WHERE r.ReporterType = 5
            AND r.DeclarationType = 1
        ) t5,


        --Count the total created internship positions
        (
            SELECT COUNT(p.ID) AS TotalInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
        ) t6,
        --Count the total unpublished internship positions
        (
            SELECT COUNT(p.ID) AS TotalUnPublishedInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE p.PositionStatus = 0			
            AND pg.PositionGroupStatus <> 2
            
        ) t7,
        --Count the total created available internship positions
        (
            SELECT COUNT(p.ID) AS TotalAvailableInternshipPositions
            FROM InternshipPosition p
            WHERE p.PositionStatus = 1
        ) t8,
        --Count the total preassigned internship positions
        (
            SELECT COUNT(p.ID) AS TotalPreAssignedInternshipPositions
            FROM InternshipPosition p
            WHERE p.PositionStatus = 2
        ) t9,
        --Count the total assigned internship positions
        (
            SELECT COUNT(p.ID) AS TotalAssignedInternshipPositions
            FROM InternshipPosition p
            WHERE p.PositionStatus = 3
        ) t10,
        --Count the total under implementation internship positions
        (
            SELECT COUNT(p.ID) AS TotalUnderImplementationInternshipPositions
            FROM InternshipPosition p
            WHERE p.PositionStatus = 4
        ) t11,
        --Count the total completed internship positions
        (
            SELECT COUNT(p.ID) AS TotalCompletedInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 0
            AND p.PositionStatus = 5
        ) t12,
        --Count the total canceled internship positions
        (
            SELECT COUNT(p.ID) AS TotalCanceledInternshipPositionsFromOffice
            FROM InternshipPosition p 
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 0
            AND p.PositionStatus = 6
            AND p.CancellationReason = 1
        ) t13,
        --Count the total revoked internship positions
        (
            SELECT COUNT(p.ID) AS TotalRevokedInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 0
            AND p.PositionStatus = 6
            AND p.CancellationReason > 1 
        ) t14,
        --Count the total deleted internship positions
        (
            SELECT COUNT(p.ID) AS TotalDeletedInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 0
            AND pg.PositionGroupStatus = 2			
        ) t15,
        --Count the total completed from office internship positions
        (
            SELECT COUNT(p.ID) AS TotalCompletedFromOfficeInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 1
            AND p.PositionStatus = 5
        ) t16,
        --Count the total created internship positions
        (
            SELECT COUNT(p.ID) AS TotalFromOfficeInternshipPositions
            FROM InternshipPosition p
                INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
            WHERE pg.PositionCreationType = 1
        ) t17


GO

ALTER VIEW [dbo].[vStatisticsByProvider]
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

GO

ALTER View [dbo].[vStatisticsByOffice] AS
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


GO

ALTER PROCEDURE [dbo].[sp_UpdateStatisticsByDay]
AS 
    BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @StartDate DATETIME
        DECLARE @EndDate DATETIME

        SET @StartDate = '2012-09-17'
        SET @EndDate = DATEADD(dd, -1, DATEDIFF(dd, 0, GETDATE()))

        CREATE TABLE #TempStatisticsByDay_All
            (
              [CreatedAt] [datetime] NOT NULL,
              [CreatedPositions] [int] NOT NULL,
              [PublishedPositions] [int] NOT NULL,
              [PreAssignedPosistions] [int] NOT NULL,
              [AssignedPositions] [int] NOT NULL,
              [UnderImplementationPositions] [int] NOT NULL,
              [CompletedPositions] [int] NOT NULL,
              [CanceledPositions] [int] NOT NULL,
              [RevokedPositions] [int] NOT NULL,
              [DeletedPositions] [int] NOT NULL,
              [CompletedFromOfficePositions] [int] NOT NULL
            )
        ON  [PRIMARY]


--CLEAR StatisticsByDay Table
        TRUNCATE TABLE StatisticsByDay

--INSERT All Student Statistics into Temp Table
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
        INSERT  INTO StatisticsByDay
                EXEC sp_StatisticsByDay @StartDate, @EndDate


    END


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
END AS CanceledPositions,
CASE
    WHEN RevokedPositions IS NULL THEN 0
    ELSE RevokedPositions
END AS RevokedPositions,
CASE
    WHEN DeletedPositions IS NULL THEN 0
    ELSE DeletedPositions
END AS DeletedPositions,
CASE
    WHEN CompletedFromOfficePositions IS NULL THEN 0
    ELSE CompletedFromOfficePositions
END AS CompletedFromOfficePositions
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
LEFT JOIN
(
    SELECT p.CompletedAt AS CompletedFromOfficePositionDate, COUNT(p.ID) AS CompletedFromOfficePositions
    FROM InternshipPosition p
        INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
    WHERE p.PositionStatus = 5 AND pg.PositionCreationType = 1
    GROUP BY CompletedAt
) t10 ON d.DateAt = t10.CompletedFromOfficePositionDate 
ORDER BY DateAt ASC

GO


DELETE FROM StatisticsByDay
GO

ALTER TABLE StatisticsByDay ADD [CompletedByOfficePositions] INT NOT NULL
GO

EXEC sp_UpdateStatisticsByDay
