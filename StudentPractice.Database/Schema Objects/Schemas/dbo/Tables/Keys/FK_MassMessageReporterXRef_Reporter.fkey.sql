ALTER TABLE [dbo].[MassMessageReporterXRef]
    ADD CONSTRAINT [FK_MassMessageReporterXRef_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

