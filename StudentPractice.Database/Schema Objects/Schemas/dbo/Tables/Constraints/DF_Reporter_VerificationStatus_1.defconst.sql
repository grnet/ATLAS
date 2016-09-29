ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_VerificationStatus_1] DEFAULT ((0)) FOR [VerificationStatus];

