USE [StudentPractice]
GO

/****** Object:  UserDefinedFunction [dbo].[GetAcademicNamesByReporterID]    Script Date: 12/15/2012 19:07:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[GetAcademicNamesByReporterID]
(@reporterID INT)
RETURNS NVARCHAR(MAX)
AS
BEGIN

	DECLARE @result NVARCHAR(MAX)
	SELECT  
		@result = CASE
		WHEN @result IS NULL
		THEN a.Department
		ELSE @result + ';' + a.Department
	END
	FROM ReporterAcademicXRef ra
	INNER JOIN Academic a ON ra.AcademicID = a.ID
	WHERE ra.ReporterID = @reporterID
	
	RETURN @result
END

GO




USE [StudentPractice]
GO

/****** Object:  StoredProcedure [dbo].[sp_UpdateStatisticsByDay]    Script Date: 12/15/2012 19:04:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[sp_UpdateStatisticsByDay]
AS 
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @StartDate DATETIME
        DECLARE @EndDate DATETIME

        SET @StartDate = '2012-12-01'
        SET @EndDate = DATEADD(dd, -1, DATEDIFF(dd, 0, GETDATE()))


--CLEAR StatisticsByDay Table
		TRUNCATE TABLE StatisticsByDay

--INSERT All Student Statistics into Temp Table
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
        INSERT  INTO StatisticsByDay
                EXEC sp_StatisticsByDay @StartDate, @EndDate


	END

GO


USE [StudentPractice]
GO

/****** Object:  View [dbo].[vStatisticsByProvider]    Script Date: 12/15/2012 19:05:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
WHERE     (r.ReporterType = 3)
ORDER BY r.ID

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "r"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 297
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t1"
            Begin Extent = 
               Top = 292
               Left = 171
               Bottom = 381
               Right = 361
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t2"
            Begin Extent = 
               Top = 267
               Left = 415
               Bottom = 356
               Right = 589
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t3"
            Begin Extent = 
               Top = 243
               Left = 634
               Bottom = 332
               Right = 868
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t4"
            Begin Extent = 
               Top = 101
               Left = 591
               Bottom = 190
               Right = 773
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "t5"
            Begin Extent = 
               Top = 33
               Left = 825
               Bottom = 122
               Right = 1000
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1815
         Alias = 2550
         Table = 11' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vStatisticsByProvider'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'70
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vStatisticsByProvider'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vStatisticsByProvider'
GO



