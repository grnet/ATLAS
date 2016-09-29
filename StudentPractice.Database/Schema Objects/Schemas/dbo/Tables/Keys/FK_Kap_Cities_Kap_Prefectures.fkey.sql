ALTER TABLE [dbo].[Kali_Cities]
    ADD CONSTRAINT [FK_Kap_Cities_Kap_Prefectures] FOREIGN KEY ([PrefectureID]) REFERENCES [dbo].[Kali_Prefectures] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

