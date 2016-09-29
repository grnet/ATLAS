ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_IsApproved] DEFAULT ((0)) FOR [IsApproved];

