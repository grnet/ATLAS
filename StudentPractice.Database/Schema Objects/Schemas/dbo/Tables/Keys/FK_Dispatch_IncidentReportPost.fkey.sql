ALTER TABLE [dbo].[Dispatch]
    ADD CONSTRAINT [FK_Dispatch_IncidentReportPost] FOREIGN KEY ([IncidentReportPostID]) REFERENCES [dbo].[IncidentReportPost] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

