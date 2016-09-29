ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [FK_IncidentReport_IncidentReportPost] FOREIGN KEY ([LastPostID]) REFERENCES [dbo].[IncidentReportPost] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

