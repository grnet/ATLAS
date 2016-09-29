ALTER TABLE [dbo].[MassMessage]
    ADD CONSTRAINT [DF_MassMessage_SentAt] DEFAULT (getdate()) FOR [SentAt];

