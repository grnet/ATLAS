ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [FK_InternshipPositionGroup_Country] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Country] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

