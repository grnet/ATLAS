ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_IsEmailVerified] DEFAULT ((0)) FOR [IsEmailVerified];

