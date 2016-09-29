ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [DF_InternshipPosition_PositionStatus] DEFAULT ((0)) FOR [PositionStatus];

