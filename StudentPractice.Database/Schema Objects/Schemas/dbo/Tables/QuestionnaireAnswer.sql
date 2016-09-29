CREATE TABLE [dbo].[QuestionnaireAnswer]
(
    [ID] [int] NOT NULL,
    [SubmittedQuestionnaireID] [int] NOT NULL,
    [QuestionID] [int] NOT NULL,
    [AnswerInt] [int] NULL,
    [AnswerString] [nvarchar](max) NULL, 
    CONSTRAINT [PK_QuestionnaireAnswer] PRIMARY KEY ([ID]), 
    CONSTRAINT [FK_QuestionnaireAnswer_SubmittedQuestionnaire] FOREIGN KEY ([SubmittedQuestionnaireID]) REFERENCES [SubmittedQuestionnaire]([ID]), 
    CONSTRAINT [FK_QuestionnaireAnswer_QuestionnaireQuestion] FOREIGN KEY ([QuestionID]) REFERENCES [QuestionnaireQuestion]([ID])
)
