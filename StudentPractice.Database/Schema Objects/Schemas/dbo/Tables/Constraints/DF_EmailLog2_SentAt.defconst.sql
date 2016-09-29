ALTER TABLE [dbo].[EmailLog]
    ADD CONSTRAINT [DF_EmailLog2_SentAt] DEFAULT (getdate()) FOR [SentAt];

