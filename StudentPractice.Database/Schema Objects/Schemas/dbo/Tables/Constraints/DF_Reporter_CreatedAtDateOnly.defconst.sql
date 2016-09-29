ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_CreatedAtDateOnly] DEFAULT (getdate()) FOR [CreatedAtDateOnly];

