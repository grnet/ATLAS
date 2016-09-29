ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [FK_Reporter_MasterProvider] FOREIGN KEY ([MasterAccountID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

