﻿--OfficeForStudent
SELECT ReporterID, PositionID, GroupID, Institution, Department, StudentID, ProviderID,[32],[33],[34],'"' + REPLACE([35], '
', '') + '"' FROM 
(
	SELECT sq.ReporterID as ReporterID, sq.PositionID as PositionID, ip.GroupID as GroupID, 
	   	   a.Institution as Institution, a.Department as Department, r.ID as StudentID, p.ID as ProviderID,
		   qa.QuestionID, ISNULL(qa.AnswerString, qa.AnswerInt) as Answer
	FROM SubmittedQuestionnaire sq
	    INNER JOIN QuestionnaireAnswer qa ON qa.SubmittedQuestionnaireID = sq.ID
		INNER JOIN Questionnaire q ON q.ID = sq.QuestionnaireID
		INNER JOIN QuestionnaireQuestion qq ON qq.ID = qa.QuestionID
		INNER JOIN InternshipPosition ip ON ip.ID = sq.PositionID
		INNER JOIN InternshipPositionGroup ipg ON ipg.ID = ip.GroupID
		INNER JOIN Reporter p ON p.ID = ipg.ReporterID
		INNER JOIN Reporter r ON r.ID = ip.AssignedToReporterID
		INNER JOIN Academic a ON a.ID = r.AcademicID
	WHERE q.QuestionnaireType IN (3) 
) d
PIVOT(
	MAX(Answer)
    FOR QuestionID IN ([32],[33],[34],[35])
) piv

--ProviderForStudent
SELECT ReporterID, PositionID, GroupID, Institution, Department, StudentID, OfficeID,[46],[47],[48],[49],[51],[52],'"' + REPLACE([53], '
', '') + '"' FROM 
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
    WHERE q.QuestionnaireType IN (6) 
            ) d
   PIVOT(
    MAX(Answer)
    FOR QuestionID IN ([46],[47],[48],[49],[51],[52],[53])
   ) piv