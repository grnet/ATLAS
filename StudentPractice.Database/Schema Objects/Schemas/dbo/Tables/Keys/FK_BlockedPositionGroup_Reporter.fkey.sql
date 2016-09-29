ALTER TABLE [dbo].[BlockedPositionGroup]
    ADD CONSTRAINT [FK_BlockedPositionGroup_Reporter] FOREIGN KEY ([MasterAccountID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

