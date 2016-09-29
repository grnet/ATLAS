DECLARE @cols AS NVARCHAR(MAX)
DECLARE @query  AS NVARCHAR(MAX)
DECLARE @questionnaireType int

SET @questionnaireType = 4

SELECT @cols = STUFF((SELECT ',' + QUOTENAME(qq.ID)
                    FROM QuestionnaireQuestion qq
      INNER JOIN Questionnaire q ON q.ID = qq.QuestionnaireID
     WHERE q.QuestionnaireType IN (@questionnaireType)
                    GROUP BY qq.ID
                    ORDER BY qq.ID
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SET @query = N'SELECT ReporterID, OfficeType, ProviderID, Category, ProviderType, PrimaryActivity, NumberOfEmployees,' + @cols + N' from 
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
    WHERE q.QuestionnaireType IN (' + CAST(@questionnaireType as nvarchar(10)) +') 
            ) d
   PIVOT(
    MAX(Answer)
    FOR QuestionID IN (' + @cols + ')
   ) piv'

SELECT @query

EXEC(@query)
