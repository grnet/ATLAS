ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_AssignedBy] FOREIGN KEY ([AssignedByReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

