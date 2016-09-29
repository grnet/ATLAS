ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [FK_InternshipPosition_PreAssignedBy] FOREIGN KEY ([PreAssignedByReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

