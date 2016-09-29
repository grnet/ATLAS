ALTER TABLE Reporter ALTER COLUMN ProviderAFM NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN ProviderPhone NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN ProviderFax NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN ZipCode NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN LegalPersonPhone NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN ContactPhone NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN ContactMobilePhone NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN AlternateContactPhone NVARCHAR(50)
ALTER TABLE Reporter ALTER COLUMN AlternateContactMobilePhone NVARCHAR(50)

ALTER TABLE Academic ADD PositionRules nvarchar(MAX) NULL

ALTER TABLE Reporter ADD CityText NVARCHAR(200)
ALTER TABLE InternshipPositionGroup ALTER COLUMN ContactPhone NVARCHAR(50)
ALTER TABLE InternshipPositionGroup ADD CityText NVARCHAR(200)
ALTER TABLE InternshipPositionGroup ADD ContactPhone NVARCHAR(50)

ALTER View [dbo].[vOfficeCounters] AS
	SELECT r.ID AS OfficeID, 
		   CASE
				WHEN r.OfficeType = 1 THEN 'Ιδρυματικό'
				WHEN r.OfficeType = 2 THEN 'Τμηματικό'
				WHEN r.OfficeType = 3 THEN 'Πολυ-Τμηματικό'
		   END AS OfficeType,
		   r.Institution, [dbo].[GetAcademicNamesByReporterID](ID) AS Academics,
		   CASE
				WHEN TotalPreAssignedPositions IS NULL THEN 0
				ELSE TotalPreAssignedPositions
		   END AS TotalPreAssignedPositions,
		   CASE
				WHEN TotalAssignedPositions IS NULL THEN 0
				ELSE TotalAssignedPositions
		   END AS TotalAssignedPositions
	FROM
	( 
		SELECT r.ID, r.OfficeType, i.Name AS Institution
		FROM Reporter r
			INNER JOIN Institution i ON i.ID = r.InstitutionID
		WHERE r.ReporterType = 4
		AND r.IsMasterAccount = 1
		AND r.VerificationStatus = 1
	) r
	LEFT JOIN
	(
		SELECT ipl.PreAssignedByMasterAccountID, COUNT(DISTINCT ipl.InternshipPositionID) AS TotalPreAssignedPositions
		FROM InternshipPositionLog ipl
		WHERE ipl.OldStatus = 1 
		AND ipl.NewStatus = 2
		GROUP BY ipl.PreAssignedByMasterAccountID
	) t1 ON r.id = t1.PreAssignedByMasterAccountID 
	LEFT JOIN
	(
		SELECT ipl.AssignedByMasterAccountID, COUNT(DISTINCT ipl.InternshipPositionID) AS TotalAssignedPositions
		FROM InternshipPositionLog ipl	
		WHERE ipl.OldStatus = 2 
		AND (ipl.NewStatus = 3 OR ipl.NewStatus = 4)
		GROUP BY ipl.AssignedByMasterAccountID
	) t2 ON r.id = t2.AssignedByMasterAccountID
GO


ALTER View [dbo].[vReportsDefault] AS
SELECT	
		TotalInternshipProviders,
		TotalInternshipProviders_Verified,
		TotalInternshipOffices,
		TotalInternshipOffices_Verified,
		TotalStudents,
		TotalInternshipPositions,
		TotalUnPublishedInternshipPositions,
		TotalAvailableInternshipPositions,
		TotalPreAssignedInternshipPositions,
		TotalAssignedInternshipPositions,
		TotalUnderImplementationInternshipPositions,
		TotalCompletedInternshipPositions,
		TotalCanceledInternshipPositions,
		TotalCanceledInternshipPositionsFromProvider,
		TotalDeletedInternshipPositions,
		TotalFinishedInternshipPositions
FROM
		--Count the total registered internship providers
		(
			SELECT COUNT(r.ID) AS TotalInternshipProviders
			FROM Reporter r
			WHERE r.ReporterType = 3
			AND r.DeclarationType = 1
		) t1,
		--Count the total registered verified internship providers
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
		--Count the total registered verified internship offices
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
			WHERE pg.PositionGroupStatus <> 2 AND NOT (pg.PositionCreationType = 1 AND p.PositionStatus = 6)
		) t6,
		--Count the total created unpublished internship positions
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
		--Count the total created preassigned internship positions
		(
			SELECT COUNT(p.ID) AS TotalPreAssignedInternshipPositions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 2
		) t9,
		--Count the total created assigned internship positions
		(
			SELECT COUNT(p.ID) AS TotalAssignedInternshipPositions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 3
		) t10,
		--Count the total created under implementation internship positions
		(
			SELECT COUNT(p.ID) AS TotalUnderImplementationInternshipPositions
			FROM InternshipPosition p
			WHERE p.PositionStatus = 4
		) t11,
		--Count the total created completed internship positions
		(
			SELECT COUNT(p.ID) AS TotalCompletedInternshipPositions
			FROM InternshipPosition p
			INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
					WHERE pg.PositionCreationType = 0
			AND p.PositionStatus = 5
		) t12,
		--Count the total created canceled internship positions
		(
			SELECT COUNT(p.ID) AS TotalCanceledInternshipPositions
			FROM InternshipPosition p 
				INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
					WHERE pg.PositionCreationType = 0
			AND p.PositionStatus = 6
			AND p.CancellationReason = 1
		) t13,
		--Count the total created canceled internship positions
		(
			SELECT COUNT(p.ID) AS TotalCanceledInternshipPositionsFromProvider
			FROM InternshipPosition p
				INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
					WHERE pg.PositionCreationType = 0
			AND p.PositionStatus = 6
			AND p.CancellationReason != 1 
			AND p.CancellationReason != 0
		) t14,
		(
			SELECT COUNT(p.ID) AS TotalDeletedInternshipPositions
			FROM InternshipPosition p
				INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
					WHERE pg.PositionCreationType = 1
			AND p.PositionStatus = 6
		) t15,
		(
			SELECT COUNT(p.ID) AS TotalFinishedInternshipPositions
			FROM InternshipPosition p
				INNER JOIN InternshipPositionGroup pg ON p.GroupID = pg.ID
					WHERE pg.PositionCreationType = 1
			AND p.PositionStatus = 5
		) t16
GO

