CREATE TABLE [dbo].[SubmittedQuestionnaire]
(
	[ID] [int] NOT NULL,
	[ReporterID] [int] NOT NULL,
	[EntityID] [int] NULL,
	[PositionID] [int] NULL,
	[EntityType] [int] NOT NULL,
	[QuestionnaireID] [int] NOT NULL,
	[AcademicYear] [int] NOT NULL,
	[SubmittedAt] DATETIME NULL, 
    CONSTRAINT [PK_SubmittedQuestionnaire] PRIMARY KEY ([ID]), 
    CONSTRAINT [FK_SubmittedQuestionnaire_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [Reporter]([ID]),
    CONSTRAINT [FK_SubmittedQuestionnaire_Questionnaire] FOREIGN KEY ([QuestionnaireID]) REFERENCES [Questionnaire]([ID])
)
