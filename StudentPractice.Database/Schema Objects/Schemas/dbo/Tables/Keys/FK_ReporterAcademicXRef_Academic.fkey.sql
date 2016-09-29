ALTER TABLE [dbo].[ReporterAcademicXRef]
    ADD CONSTRAINT [FK_ReporterAcademicXRef_Academic] FOREIGN KEY ([AcademicID]) REFERENCES [dbo].[Academic] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

