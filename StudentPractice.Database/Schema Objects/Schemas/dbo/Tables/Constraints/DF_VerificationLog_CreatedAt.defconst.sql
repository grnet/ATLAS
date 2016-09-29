ALTER TABLE [dbo].[VerificationLog]
    ADD CONSTRAINT [DF_VerificationLog_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

