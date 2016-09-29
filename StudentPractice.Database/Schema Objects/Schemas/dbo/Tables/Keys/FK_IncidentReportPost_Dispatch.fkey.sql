ALTER TABLE [dbo].[IncidentReportPost]
    ADD CONSTRAINT [FK_IncidentReportPost_Dispatch] FOREIGN KEY ([LastDispatchID]) REFERENCES [dbo].[Dispatch] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

