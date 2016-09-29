ALTER TABLE [dbo].[BlockedPositionGroup]
    ADD CONSTRAINT [FK_BlockedPositionGroup_MasterBlock] FOREIGN KEY ([MasterBlockID]) REFERENCES [dbo].[BlockedPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

