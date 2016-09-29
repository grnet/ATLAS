ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

