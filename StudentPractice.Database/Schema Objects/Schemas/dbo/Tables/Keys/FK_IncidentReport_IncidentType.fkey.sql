ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [FK_IncidentReport_IncidentType] FOREIGN KEY ([IncidentTypeID]) REFERENCES [dbo].[IncidentType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

