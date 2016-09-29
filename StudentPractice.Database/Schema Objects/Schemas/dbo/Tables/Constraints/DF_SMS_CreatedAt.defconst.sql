ALTER TABLE [dbo].[SmsLog]
    ADD CONSTRAINT [DF_SMS_CreatedAt] DEFAULT (getdate()) FOR [SentAt];

