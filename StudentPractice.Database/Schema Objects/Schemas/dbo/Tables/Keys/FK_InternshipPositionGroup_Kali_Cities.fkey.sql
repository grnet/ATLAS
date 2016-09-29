ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [FK_InternshipPositionGroup_Kali_Cities] FOREIGN KEY ([CityID]) REFERENCES [dbo].[Kali_Cities] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

