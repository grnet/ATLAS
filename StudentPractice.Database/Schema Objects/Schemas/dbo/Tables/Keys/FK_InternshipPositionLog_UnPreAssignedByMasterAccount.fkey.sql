ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_UnPreAssignedByMasterAccount] FOREIGN KEY ([UnPreAssignedByMasterAccountID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

