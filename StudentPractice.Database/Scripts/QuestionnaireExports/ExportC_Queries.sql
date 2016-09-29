--OfficeForProvider
SELECT ReporterID, 
CASE
	WHEN OfficeType = 1 THEN 'Ιδρυματικό'
	WHEN OfficeType = 2 THEN 'Τμηματικό'
	WHEN OfficeType = 3 THEN 'Πολλαπλά Τμηματικό'	
END AS OfficeType,
ProviderID, 
CASE
	WHEN Category = 1 THEN 'MasterAccount'
	WHEN Category = 0 THEN 'UserAccount'	
END AS Category,
CASE
	WHEN ProviderType = 1 THEN 'Ιδιωτικός Φορέας'
	WHEN ProviderType = 2 THEN 'Δημόσιος Φορέας'
	WHEN ProviderType = 3 THEN 'Μη Κυβερνητική Οργάνωση'
	WHEN ProviderType = 4 THEN 'Άλλο'
END AS ProviderType,
PrimaryActivity, NumberOfEmployees,[36],[37],[38],[40],'"' + REPLACE([41], '
', '') + '"' FROM 
(
	SELECT sq.ReporterID as ReporterID, o.OfficeType as OfficeType, sq.EntityID as ProviderID,
		   p.IsMasterAccount as Category, p.ProviderType as ProviderType, pa.Name as PrimaryActivity, p.ProviderNumberOfEmployees as NumberOfEmployees,
		   qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
		INNER JOIN Reporter o ON o.ID = sq.ReporterID
		INNER JOIN Reporter p ON p.ID = sq.EntityID
		INNER JOIN PrimaryActivity pa ON pa.ID = p.PrimaryActivityID
	WHERE q.QuestionnaireType IN (4) 
) d
PIVOT(
	MAX(Answer)
    FOR QuestionID IN ([36],[37],[38],[40],[41])
) piv

--StudentForProvider
SELECT ReporterID, ProviderID, 
CASE
	WHEN Category = 1 THEN 'MasterAccount'
	WHEN Category = 0 THEN 'UserAccount'	
END AS Category, 
CASE
	WHEN ProviderType = 1 THEN 'Ιδιωτικός Φορέας'
	WHEN ProviderType = 2 THEN 'Δημόσιος Φορέας'
	WHEN ProviderType = 3 THEN 'Μη Κυβερνητική Οργάνωση'
	WHEN ProviderType = 4 THEN 'Άλλο'
END AS ProviderType,
PrimaryActivity, NumberOfEmployees, PositionID,[17],[18],[19],[20],[21],[22],[23],[24],[25],'"' + REPLACE([27], '
', '') + '"' FROM 
(
	SELECT sq.ReporterID as ReporterID, sq.EntityID as ProviderID, sq.PositionID AS PositionID,
		   p.IsMasterAccount as Category, p.ProviderType as ProviderType, pa.Name as PrimaryActivity, p.ProviderNumberOfEmployees as NumberOfEmployees,
		   qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
		INNER JOIN Reporter o ON o.ID = sq.ReporterID
		INNER JOIN Reporter p ON p.ID = sq.EntityID
		INNER JOIN PrimaryActivity pa ON pa.ID = p.PrimaryActivityID
WHERE q.QuestionnaireType IN (1) 
) d
PIVOT(
	MAX(Answer)
	FOR QuestionID IN ([17],[18],[19],[20],[21],[22],[23],[24],[25],[27])
) piv