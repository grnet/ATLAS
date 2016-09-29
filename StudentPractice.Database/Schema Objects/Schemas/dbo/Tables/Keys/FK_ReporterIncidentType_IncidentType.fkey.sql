ALTER TABLE [dbo].[ReporterIncidentType]
    ADD CONSTRAINT [FK_ReporterIncidentType_IncidentType] FOREIGN KEY ([IncidentTypeID]) REFERENCES [dbo].[IncidentType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

