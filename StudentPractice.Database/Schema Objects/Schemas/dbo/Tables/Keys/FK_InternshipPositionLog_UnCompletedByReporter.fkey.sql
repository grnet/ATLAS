ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_UnCompletedByReporter] FOREIGN KEY ([UnCompletedByReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

