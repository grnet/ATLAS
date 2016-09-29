ALTER TABLE [dbo].[BlockedPositionGroup]
    ADD CONSTRAINT [DF_BlockedPositionGroup_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

