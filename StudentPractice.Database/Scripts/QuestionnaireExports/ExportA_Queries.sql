--OfficeForAtlas
SELECT ReporterID,[42],[44],'"' + REPLACE([45], '
', '') + '"', [72] FROM
(
    SELECT sq.ReporterID as ReporterID, qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
    FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
    WHERE q.QuestionnaireType IN (5) 
) d
PIVOT(
	MAX(Answer)
	FOR QuestionID IN ([42],[44],[45],[72])
) piv

--ProviderForAtlas
SELECT ReporterID,[60],[61],[63],[64],'"' + REPLACE([65], '
', '') + '"' FROM 
(
	SELECT sq.ReporterID as ReporterID, qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
	WHERE q.QuestionnaireType IN (8) 
) d
PIVOT(
	MAX(Answer)
    FOR QuestionID IN ([60],[61],[63],[64],[65])
) piv

--StudentForAtlas
SELECT ReporterID,[28],[29],[30],'"' + REPLACE([31], '
', '') + '"' FROM 
(
	SELECT sq.ReporterID as ReporterID, qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
		INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
	WHERE q.QuestionnaireType IN (2) 
) d
PIVOT(
	MAX(Answer)
	FOR QuestionID IN ([28],[29],[30],[31])
) piv