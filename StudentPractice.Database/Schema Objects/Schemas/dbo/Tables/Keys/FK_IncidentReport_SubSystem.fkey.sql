ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [FK_IncidentReport_SubSystem] FOREIGN KEY ([SubSystemID]) REFERENCES [dbo].[SubSystem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

