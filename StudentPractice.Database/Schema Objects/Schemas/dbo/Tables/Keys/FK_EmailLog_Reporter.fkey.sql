ALTER TABLE [dbo].[EmailLog]
    ADD CONSTRAINT [FK_EmailLog_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

