ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_IsContactInfoCompleted] DEFAULT ((0)) FOR [IsContactInfoCompleted];

