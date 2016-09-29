
CREATE PROCEDURE [dbo].[sp_UpdateStatisticsByDay]
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
			  [DeletedPositions] [int] NOT NULL 
            )
        ON  [PRIMARY]


--CLEAR StatisticsByDay Table
		TRUNCATE TABLE StatisticsByDay

--INSERT All Student Statistics into Temp Table
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
        INSERT  INTO StatisticsByDay
                EXEC sp_StatisticsByDay @StartDate, @EndDate


	END




