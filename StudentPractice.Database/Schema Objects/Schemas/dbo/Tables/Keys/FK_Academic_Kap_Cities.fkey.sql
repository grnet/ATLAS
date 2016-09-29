ALTER TABLE [dbo].[Academic]
    ADD CONSTRAINT [FK_Academic_Kap_Cities] FOREIGN KEY ([CityID]) REFERENCES [dbo].[Kali_Cities] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

