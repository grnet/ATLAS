ALTER TABLE [dbo].[Academic]
    ADD CONSTRAINT [FK_Academic_Institution] FOREIGN KEY ([InstitutionID]) REFERENCES [dbo].[Institution] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

