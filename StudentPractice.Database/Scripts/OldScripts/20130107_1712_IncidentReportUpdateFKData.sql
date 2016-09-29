DECLARE @relData TABLE (
	IncidentReportID INT NOT NULL,
	LastDispatchedPostID INT NOT NULL
);

INSERT INTO @relData
SELECT ir.ID, (
	SELECT TOP 1 irp.ID
	FROM IncidentReportPost irp
	WHERE irp.IncidentReportID = ir.ID AND irp.LastDispatchID IS NOT NULL
	ORDER BY irp.ID DESC
)
FROM IncidentReport ir
WHERE ir.LastPostID IS NOT NULL

UPDATE ir
SET ir.LastDispatchedPostID = rd.LastDispatchedPostID
FROM IncidentReport ir
INNER JOIN @relData rd ON ir.ID = rd.IncidentReportID
WHERE ir.ID = rd.IncidentReportID