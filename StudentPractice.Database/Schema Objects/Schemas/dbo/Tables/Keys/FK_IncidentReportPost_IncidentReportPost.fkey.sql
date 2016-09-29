ALTER TABLE [dbo].[IncidentReportPost]
    ADD CONSTRAINT [FK_IncidentReportPost_IncidentReportPost] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[IncidentReportPost] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

