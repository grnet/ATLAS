--ProviderForOffice
SELECT ReporterID, OfficeID, Academics, InstitutionName, 
CASE
	WHEN OfficeType = 1 THEN 'Ιδρυματικό'
	WHEN OfficeType = 2 THEN 'Τμηματικό'
	WHEN OfficeType = 3 THEN 'Πολλαπλά Τμηματικό'	
END AS OfficeType,
[54],[55],[57],'"' + REPLACE([59], '
', '') + '"',[66] FROM 
(
	SELECT sq.ReporterID as ReporterID, o.ID as OfficeID, o.OfficeType as OfficeType, i.Name as InstitutionName, dbo.GetAcademicNamesByReporterID(o.ID) as Academics,
		   qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
		INNER JOIN Reporter o ON o.ID = sq.EntityID
		INNER JOIN Institution i ON i.ID = o.InstitutionID
	WHERE q.QuestionnaireType IN (7) 
) d
PIVOT(
	MAX(Answer)
	FOR QuestionID IN ([54],[55],[57],[59],[66])
) piv

--StudentForOffice
SELECT ReporterID, OfficeID, Academics, InstitutionName, 
CASE
	WHEN OfficeType = 1 THEN 'Ιδρυματικό'
	WHEN OfficeType = 2 THEN 'Τμηματικό'
	WHEN OfficeType = 3 THEN 'Πολλαπλά Τμηματικό'	
END AS OfficeType,PositionID,[14],[15],'"' + REPLACE([16], '
', '') + '"',[69],[70],[71] FROM
(
    SELECT sq.ReporterID as ReporterID, o.ID as OfficeID, o.OfficeType as OfficeType, i.Name as InstitutionName, 
		   dbo.GetAcademicNamesByReporterID(o.ID) as Academics, sq.PositionID as PositionID,
		   qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
    FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
		INNER JOIN Reporter o ON o.ID = sq.EntityID
		INNER JOIN Institution i ON i.ID = o.InstitutionID
	WHERE q.QuestionnaireType IN (0) 
) d
PIVOT(
	MAX(Answer)
	FOR QuestionID IN ([14],[15],[16],[69],[70],[71])
) piv