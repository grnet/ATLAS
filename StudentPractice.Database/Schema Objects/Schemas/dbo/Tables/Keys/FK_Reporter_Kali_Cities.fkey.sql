ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [FK_Reporter_Kali_Cities] FOREIGN KEY ([CityID]) REFERENCES [dbo].[Kali_Cities] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

