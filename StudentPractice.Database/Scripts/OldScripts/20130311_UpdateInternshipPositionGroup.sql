/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup ADD
	PositionGroupStatus int NULL
GO
ALTER TABLE dbo.InternshipPositionGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


UPDATE InternshipPositionGroup
SET PositionGroupStatus = 
	(
		CASE 
			WHEN IsDeleted = 1 THEN 2
			WHEN IsPublished = 0 AND IsCanceled = 0 THEN 0
			WHEN IsPublished = 1 AND IsCanceled = 0 THEN 1
			WHEN IsCanceled = 1 THEN 3
			ELSE 0
		END
	)
    
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT FK_InternshipPositionGroup_Reporter
GO
ALTER TABLE dbo.Reporter SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT FK_InternshipPositionGroup_Country
GO
ALTER TABLE dbo.Country SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT FK_InternshipPositionGroup_Kali_Prefectures
GO
ALTER TABLE dbo.Kali_Prefectures SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT FK_InternshipPositionGroup_Kali_Cities
GO
ALTER TABLE dbo.Kali_Cities SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT DF_InternshipPositionGroup_IsPublished
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT DF_InternshipPositionGroup_IsRemoved
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT DF_InternshipPositionGroup_IsDeleted
GO
ALTER TABLE dbo.InternshipPositionGroup
	DROP CONSTRAINT DF_InternshipPositionGroup_NoTimeLimit
GO
CREATE TABLE dbo.Tmp_InternshipPositionGroup
	(
	ID int NOT NULL IDENTITY (1, 1) NOT FOR REPLICATION,
	ReporterID int NOT NULL,
	TotalPositions int NOT NULL,
	AvailablePositions int NOT NULL,
	PreAssignedPositions int NOT NULL,
	IsVisibleToAllAcademics bit NULL,
	Title nvarchar(500) NOT NULL,
	Description nvarchar(MAX) NULL,
	Duration int NOT NULL,
	CityID int NOT NULL,
	PrefectureID int NOT NULL,
	CountryID int NOT NULL,
	NoTimeLimit bit NOT NULL,
	StartDate datetime NULL,
	EndDate datetime NULL,
	PositionType int NOT NULL,
	ContactPhone nvarchar(10) NOT NULL,
	FirstPublishedAt datetime NULL,
	LastPublishedAt datetime NULL,
	CreatedAt datetime NOT NULL,
	CreatedAtDateOnly datetime NOT NULL,
	CreatedBy nvarchar(256) NOT NULL,
	UpdatedAt datetime NULL,
	UpdatedBy nvarchar(256) NULL,
	Supervisor nvarchar(500) NULL,
	PositionGroupStatus int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_InternshipPositionGroup SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_InternshipPositionGroup ADD CONSTRAINT
	DF_InternshipPositionGroup_NoTimeLimit DEFAULT ((1)) FOR NoTimeLimit
GO
SET IDENTITY_INSERT dbo.Tmp_InternshipPositionGroup ON
GO
IF EXISTS(SELECT * FROM dbo.InternshipPositionGroup)
	 EXEC('INSERT INTO dbo.Tmp_InternshipPositionGroup (ID, ReporterID, TotalPositions, AvailablePositions, PreAssignedPositions, IsVisibleToAllAcademics, Title, Description, Duration, CityID, PrefectureID, CountryID, NoTimeLimit, StartDate, EndDate, PositionType, ContactPhone, FirstPublishedAt, LastPublishedAt, CreatedAt, CreatedAtDateOnly, CreatedBy, UpdatedAt, UpdatedBy, Supervisor, PositionGroupStatus)
		SELECT ID, ReporterID, TotalPositions, AvailablePositions, PreAssignedPositions, IsVisibleToAllAcademics, Title, Description, Duration, CityID, PrefectureID, CountryID, NoTimeLimit, StartDate, EndDate, PositionType, ContactPhone, FirstPublishedAt, LastPublishedAt, CreatedAt, CreatedAtDateOnly, CreatedBy, UpdatedAt, UpdatedBy, Supervisor, PositionGroupStatus FROM dbo.InternshipPositionGroup WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_InternshipPositionGroup OFF
GO
ALTER TABLE dbo.BlockedPositionGroup
	DROP CONSTRAINT FK_BlockedPositionGroup_InternshipPositionGroup
GO
ALTER TABLE dbo.InternshipPosition
	DROP CONSTRAINT FK_InternshipPosition_InternshipPositionGroup
GO
ALTER TABLE dbo.InternshipPositionGroupAcademicXRef
	DROP CONSTRAINT FK_InternshipPositionGroupAcademicXRef_InternshipPositionGroup
GO
ALTER TABLE dbo.InternshipPositionGroupPhysicalObjectXRef
	DROP CONSTRAINT FK_InternshipPositionGroupPhysicalObjectXRef_InternshipPositionGroup
GO
ALTER TABLE dbo.InternshipPositionGroupLog
	DROP CONSTRAINT FK_InternshipPositionGroupLog_InternshipPositionGroup
GO
DROP TABLE dbo.InternshipPositionGroup
GO
EXECUTE sp_rename N'dbo.Tmp_InternshipPositionGroup', N'InternshipPositionGroup', 'OBJECT' 
GO
ALTER TABLE dbo.InternshipPositionGroup ADD CONSTRAINT
	PK_InternshipPositionGroup PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_ReporterID ON dbo.InternshipPositionGroup
	(
	ReporterID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_CityID ON dbo.InternshipPositionGroup
	(
	CityID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_PrefectureID ON dbo.InternshipPositionGroup
	(
	PrefectureID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_CountryID ON dbo.InternshipPositionGroup
	(
	CountryID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_FirstPublishedAt ON dbo.InternshipPositionGroup
	(
	FirstPublishedAt
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_CreatedAtDateOnly ON dbo.InternshipPositionGroup
	(
	CreatedAtDateOnly
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_InternshipPositionGroup_IsVisibleToAllAcademics ON dbo.InternshipPositionGroup
	(
	IsVisibleToAllAcademics
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.InternshipPositionGroup ADD CONSTRAINT
	FK_InternshipPositionGroup_Kali_Cities FOREIGN KEY
	(
	CityID
	) REFERENCES dbo.Kali_Cities
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroup ADD CONSTRAINT
	FK_InternshipPositionGroup_Kali_Prefectures FOREIGN KEY
	(
	PrefectureID
	) REFERENCES dbo.Kali_Prefectures
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroup ADD CONSTRAINT
	FK_InternshipPositionGroup_Country FOREIGN KEY
	(
	CountryID
	) REFERENCES dbo.Country
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroup ADD CONSTRAINT
	FK_InternshipPositionGroup_Reporter FOREIGN KEY
	(
	ReporterID
	) REFERENCES dbo.Reporter
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroupLog ADD CONSTRAINT
	FK_InternshipPositionGroupLog_InternshipPositionGroup FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.InternshipPositionGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroupLog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroupPhysicalObjectXRef ADD CONSTRAINT
	FK_InternshipPositionGroupPhysicalObjectXRef_InternshipPositionGroup FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.InternshipPositionGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroupPhysicalObjectXRef SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPositionGroupAcademicXRef ADD CONSTRAINT
	FK_InternshipPositionGroupAcademicXRef_InternshipPositionGroup FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.InternshipPositionGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPositionGroupAcademicXRef SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.InternshipPosition ADD CONSTRAINT
	FK_InternshipPosition_InternshipPositionGroup FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.InternshipPositionGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.InternshipPosition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.BlockedPositionGroup ADD CONSTRAINT
	FK_BlockedPositionGroup_InternshipPositionGroup FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.InternshipPositionGroup
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.BlockedPositionGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

    
USE [StudentPractice]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InternshipPositionGroupLog_InternshipPositionGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[InternshipPositionGroupLog]'))
ALTER TABLE [dbo].[InternshipPositionGroupLog] DROP CONSTRAINT [FK_InternshipPositionGroupLog_InternshipPositionGroup]
GO

USE [StudentPractice]
GO

/****** Object:  Table [dbo].[InternshipPositionGroupLog]    Script Date: 03/08/2013 15:37:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InternshipPositionGroupLog]') AND type in (N'U'))
DROP TABLE [dbo].[InternshipPositionGroupLog]
GO

USE [StudentPractice]
GO

/****** Object:  Table [dbo].[InternshipPositionGroupLog]    Script Date: 03/08/2013 15:37:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InternshipPositionGroupLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NOT NULL,
	[OldStatus] [int] NOT NULL,
	[NewStatus] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedAtDateOnly] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedAtDateOnly] [datetime] NULL,
 CONSTRAINT [PK_InternshipPositionGroupLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[InternshipPositionGroupLog]  WITH CHECK ADD  CONSTRAINT [FK_InternshipPositionGroupLog_InternshipPositionGroup] FOREIGN KEY([GroupID])
REFERENCES [dbo].[InternshipPositionGroup] ([ID])
GO

ALTER TABLE [dbo].[InternshipPositionGroupLog] CHECK CONSTRAINT [FK_InternshipPositionGroupLog_InternshipPositionGroup]
GO


