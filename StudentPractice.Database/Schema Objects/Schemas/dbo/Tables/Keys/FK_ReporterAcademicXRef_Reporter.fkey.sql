ALTER TABLE [dbo].[ReporterAcademicXRef]
    ADD CONSTRAINT [FK_ReporterAcademicXRef_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

