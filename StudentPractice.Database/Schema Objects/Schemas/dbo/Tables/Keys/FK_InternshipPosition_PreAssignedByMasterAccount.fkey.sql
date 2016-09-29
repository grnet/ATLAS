ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [FK_InternshipPosition_PreAssignedByMasterAccount] FOREIGN KEY ([PreAssignedByMasterAccountID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

