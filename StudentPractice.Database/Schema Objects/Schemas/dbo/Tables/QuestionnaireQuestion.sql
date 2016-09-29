CREATE TABLE [dbo].[QuestionnaireQuestion]
(
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [QuestionnaireID] [int] NOT NULL,
    [QuestionType] [int] NOT NULL,
    [QuestionNumber] [int] NOT NULL,
    [Title] [nvarchar](500) NOT NULL,
    [PossibleAnswers] [xml] NULL, 

    CONSTRAINT [PK_QuestionnaireQuestion] PRIMARY KEY ([ID]), 
    CONSTRAINT [FK_QuestionnaireQuestion_Questionnaire] FOREIGN KEY ([QuestionnaireID]) REFERENCES [Questionnaire]([ID])
)
