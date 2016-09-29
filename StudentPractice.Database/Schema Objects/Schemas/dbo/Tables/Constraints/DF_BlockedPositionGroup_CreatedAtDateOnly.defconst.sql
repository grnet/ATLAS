ALTER TABLE [dbo].[BlockedPositionGroup]
    ADD CONSTRAINT [DF_BlockedPositionGroup_CreatedAtDateOnly] DEFAULT (getdate()) FOR [CreatedAtDateOnly];

