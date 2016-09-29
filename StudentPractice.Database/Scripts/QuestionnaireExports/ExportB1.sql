DECLARE @cols AS NVARCHAR(MAX)
DECLARE @query  AS NVARCHAR(MAX)
DECLARE @questionnaireType int

SET @questionnaireType = 6

SELECT @cols = STUFF((SELECT ',' + QUOTENAME(qq.ID)
                    FROM QuestionnaireQuestion qq
      INNER JOIN Questionnaire q ON q.ID = qq.QuestionnaireID
     WHERE q.QuestionnaireType IN (@questionnaireType)
                    GROUP BY qq.ID
                    ORDER BY qq.ID
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SET @query = N'SELECT ReporterID, PositionID, GroupID, Institution, Department, StudentID, OfficeID,' + @cols + N' from 
             (
    SELECT sq.ReporterID as ReporterID, sq.PositionID as PositionID, ip.GroupID as GroupID, 
		a.Institution as Institution, a.Department as Department, r.ID as StudentID, o.ID as OfficeID,
		qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
    FROM SubmittedQuestionnaire sq
     INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
     INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
     INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
	 INNER JOIN InternshipPosition ip ON ip.ID = sq.PositionID
	 INNER JOIN Reporter o ON o.ID = ip.PreAssignedByMasterAccountID
	 INNER JOIN Reporter r ON r.ID = ip.AssignedToReporterID
	 INNER JOIN Academic a ON a.ID = r.AcademicID
    WHERE q.QuestionnaireType IN (' + CAST(@questionnaireType as nvarchar(10)) +') 
            ) d
   PIVOT(
    MAX(Answer)
    FOR QuestionID IN (' + @cols + ')
   ) piv'

SELECT @query

EXEC(@query)