ALTER TABLE [dbo].[VerificationLog]
    ADD CONSTRAINT [FK_VerificationLog_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

