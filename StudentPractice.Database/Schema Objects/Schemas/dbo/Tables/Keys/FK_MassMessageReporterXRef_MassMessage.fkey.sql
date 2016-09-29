ALTER TABLE [dbo].[MassMessageReporterXRef]
    ADD CONSTRAINT [FK_MassMessageReporterXRef_MassMessage] FOREIGN KEY ([MassMessageID]) REFERENCES [dbo].[MassMessage] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

