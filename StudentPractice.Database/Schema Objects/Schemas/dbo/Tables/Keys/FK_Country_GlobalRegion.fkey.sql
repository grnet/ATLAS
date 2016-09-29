ALTER TABLE [dbo].[Country]
    ADD CONSTRAINT [FK_Country_GlobalRegion] FOREIGN KEY ([GlobalRegionID]) REFERENCES [dbo].[GlobalRegion] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

