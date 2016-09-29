ALTER TABLE [dbo].[InternshipPositionGroupLog]
    ADD CONSTRAINT [FK_InternshipPositionGroupLog_InternshipPositionGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[InternshipPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

