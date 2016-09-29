ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [FK_IncidentReport_LastDispatchedPost] FOREIGN KEY ([LastDispatchedPostID]) REFERENCES [dbo].[IncidentReportPost] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

