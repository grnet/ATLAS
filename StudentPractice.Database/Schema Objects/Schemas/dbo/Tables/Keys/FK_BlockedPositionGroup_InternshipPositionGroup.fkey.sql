ALTER TABLE [dbo].[BlockedPositionGroup]
    ADD CONSTRAINT [FK_BlockedPositionGroup_InternshipPositionGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[InternshipPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

