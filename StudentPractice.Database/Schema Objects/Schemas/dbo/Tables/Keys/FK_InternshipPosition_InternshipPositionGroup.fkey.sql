ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [FK_InternshipPosition_InternshipPositionGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[InternshipPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

