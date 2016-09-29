ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_InternshipPosition] FOREIGN KEY ([InternshipPositionID]) REFERENCES [dbo].[InternshipPosition] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

