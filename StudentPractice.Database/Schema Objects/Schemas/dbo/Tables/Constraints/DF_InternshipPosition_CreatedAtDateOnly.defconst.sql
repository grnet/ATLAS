ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [DF_InternshipPosition_CreatedAtDateOnly] DEFAULT (getdate()) FOR [CreatedAtDateOnly];

