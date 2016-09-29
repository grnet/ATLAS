ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [FK_InternshipPosition_AssignedTo] FOREIGN KEY ([AssignedToReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

