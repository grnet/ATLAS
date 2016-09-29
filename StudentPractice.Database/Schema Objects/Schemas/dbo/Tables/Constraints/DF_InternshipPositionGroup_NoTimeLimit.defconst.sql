ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [DF_InternshipPositionGroup_NoTimeLimit] DEFAULT ((1)) FOR [NoTimeLimit];

