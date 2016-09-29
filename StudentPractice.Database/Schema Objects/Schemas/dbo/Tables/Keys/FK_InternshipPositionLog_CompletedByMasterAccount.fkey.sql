ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_CompletedByMasterAccount] FOREIGN KEY ([CompletedByMasterAccountID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

