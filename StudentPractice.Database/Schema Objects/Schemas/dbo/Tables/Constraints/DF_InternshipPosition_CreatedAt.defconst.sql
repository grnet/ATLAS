ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [DF_InternshipPosition_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

