ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_MustChangePassword] DEFAULT ((0)) FOR [MustChangePassword];

