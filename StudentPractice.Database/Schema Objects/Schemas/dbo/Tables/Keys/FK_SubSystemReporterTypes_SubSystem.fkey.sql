ALTER TABLE [dbo].[SubSystemReporterType]
    ADD CONSTRAINT [FK_SubSystemReporterTypes_SubSystem] FOREIGN KEY ([SubSystemID]) REFERENCES [dbo].[SubSystem] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

