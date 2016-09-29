ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [DF_InternshipPositionLog_CancellationReason] DEFAULT ((0)) FOR [CancellationReason];

