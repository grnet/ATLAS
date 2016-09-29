CREATE TABLE [dbo].[Questionnaire]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireType] [int] NOT NULL,
	[Title] [nvarchar](50) NULL, 
    CONSTRAINT [PK_Questionnaire] PRIMARY KEY ([ID])
)
