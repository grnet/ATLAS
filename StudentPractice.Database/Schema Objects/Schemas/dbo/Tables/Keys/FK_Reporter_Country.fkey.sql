ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [FK_Reporter_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

