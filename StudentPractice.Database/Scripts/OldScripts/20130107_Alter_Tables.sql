ALTER TABLE dbo.InternshipPosition ADD
	CancellationReason int NOT NULL CONSTRAINT DF_InternshipPosition_CancellationReason DEFAULT 0

ALTER TABLE dbo.InternshipPositionLog ADD
	CancellationReason int NOT NULL CONSTRAINT DF_InternshipPositionLog_CancellationReason DEFAULT 0

