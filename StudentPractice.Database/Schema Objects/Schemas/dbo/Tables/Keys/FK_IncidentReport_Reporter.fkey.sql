ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [FK_IncidentReport_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

