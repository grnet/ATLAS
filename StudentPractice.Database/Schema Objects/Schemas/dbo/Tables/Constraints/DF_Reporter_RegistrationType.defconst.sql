ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_RegistrationType] DEFAULT ((0)) FOR [RegistrationType];

