ALTER TABLE Questionnaire ADD TitleEN NVARCHAR(50) NULL

ALTER TABLE QuestionnaireQuestion ADD TitleEN NVARCHAR(500) NULL
ALTER TABLE QuestionnaireQuestion ADD PossibleAnswersEN XML NULL

UPDATE Questionnaire SET TitleEN = Title
UPDATE QuestionnaireQuestion SET TitleEN = Title
UPDATE QuestionnaireQuestion SET PossibleAnswersEN = PossibleAnswers