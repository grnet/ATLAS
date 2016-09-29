ALTER TABLE [dbo].[IncidentType]
    ADD CONSTRAINT [FK_IncidentType_SubSystem] FOREIGN KEY ([SubSystemID]) REFERENCES [dbo].[SubSystem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

