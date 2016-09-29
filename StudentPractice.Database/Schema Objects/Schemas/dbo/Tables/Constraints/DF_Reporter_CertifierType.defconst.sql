ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_CertifierType] DEFAULT ((0)) FOR [CertifierType];

