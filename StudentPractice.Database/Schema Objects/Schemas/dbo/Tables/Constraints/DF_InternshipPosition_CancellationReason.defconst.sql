ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [DF_InternshipPosition_CancellationReason] DEFAULT ((0)) FOR [CancellationReason];

