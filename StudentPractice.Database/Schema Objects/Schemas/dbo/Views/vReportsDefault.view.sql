﻿CREATE View [dbo].[vReportsDefault] AS
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