ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [DF_InternshipPositionGroup_PositionCreationType] DEFAULT ((0)) FOR [PositionCreationType];

