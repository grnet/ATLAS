ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [FK_InternshipPositionGroup_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

