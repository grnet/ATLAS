ALTER TABLE [dbo].[Kali_Prefectures]
    ADD CONSTRAINT [FK_Kap_Prefectures_Kap_Regions] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Kali_Regions] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

