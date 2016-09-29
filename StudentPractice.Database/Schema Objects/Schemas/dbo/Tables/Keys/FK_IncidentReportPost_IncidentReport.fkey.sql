ALTER TABLE [dbo].[IncidentReportPost]
    ADD CONSTRAINT [FK_IncidentReportPost_IncidentReport] FOREIGN KEY ([IncidentReportID]) REFERENCES [dbo].[IncidentReport] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

