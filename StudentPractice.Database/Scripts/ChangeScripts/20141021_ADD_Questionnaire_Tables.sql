CREATE TABLE [dbo].[Questionnaire](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireType] [int] NOT NULL,
	[Title] [nvarchar](50) NULL,
 CONSTRAINT [PK_Questionnaire] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[QuestionnaireQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireID] [int] NOT NULL,
	[QuestionType] [int] NOT NULL,
	[QuestionNumber] [int] NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[PossibleAnswers] [xml] NULL,
 CONSTRAINT [PK_QuestionnaireQuestion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[QuestionnaireQuestion]  WITH CHECK ADD  CONSTRAINT [FK_QuestionnaireQuestion_Questionnaire] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Questionnaire] ([ID])
GO

ALTER TABLE [dbo].[QuestionnaireQuestion] CHECK CONSTRAINT [FK_QuestionnaireQuestion_Questionnaire]
GO


CREATE TABLE [dbo].[SubmittedQuestionnaire](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReporterID] [int] NOT NULL,
	[EntityID] [int] NULL,
	[PositionID] [int] NULL,
	[EntityType] [int] NOT NULL,
	[QuestionnaireID] [int] NOT NULL,
	[AcademicYear] [int] NOT NULL,
	[SubmittedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_SubmittedQuestionnaire] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SubmittedQuestionnaire]  WITH CHECK ADD  CONSTRAINT [FK_SubmittedQuestionnaire_Questionnaire] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Questionnaire] ([ID])
GO

ALTER TABLE [dbo].[SubmittedQuestionnaire] CHECK CONSTRAINT [FK_SubmittedQuestionnaire_Questionnaire]
GO

ALTER TABLE [dbo].[SubmittedQuestionnaire]  WITH CHECK ADD  CONSTRAINT [FK_SubmittedQuestionnaire_Reporter] FOREIGN KEY([ReporterID])
REFERENCES [dbo].[Reporter] ([ID])
GO

ALTER TABLE [dbo].[SubmittedQuestionnaire] CHECK CONSTRAINT [FK_SubmittedQuestionnaire_Reporter]
GO


CREATE TABLE [dbo].[QuestionnaireAnswer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubmittedQuestionnaireID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[AnswerInt] [int] NULL,
	[AnswerString] [nvarchar](max) NULL,
 CONSTRAINT [PK_QuestionnaireAnswer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[QuestionnaireAnswer]  WITH CHECK ADD  CONSTRAINT [FK_QuestionnaireAnswer_QuestionnaireQuestion] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[QuestionnaireQuestion] ([ID])
GO

ALTER TABLE [dbo].[QuestionnaireAnswer] CHECK CONSTRAINT [FK_QuestionnaireAnswer_QuestionnaireQuestion]
GO

ALTER TABLE [dbo].[QuestionnaireAnswer]  WITH CHECK ADD  CONSTRAINT [FK_QuestionnaireAnswer_SubmittedQuestionnaire] FOREIGN KEY([SubmittedQuestionnaireID])
REFERENCES [dbo].[SubmittedQuestionnaire] ([ID])
GO

ALTER TABLE [dbo].[QuestionnaireAnswer] CHECK CONSTRAINT [FK_QuestionnaireAnswer_SubmittedQuestionnaire]
GO
