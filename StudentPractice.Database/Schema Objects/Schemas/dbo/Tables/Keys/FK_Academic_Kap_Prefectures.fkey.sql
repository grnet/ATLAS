ALTER TABLE [dbo].[Academic]
    ADD CONSTRAINT [FK_Academic_Kap_Prefectures] FOREIGN KEY ([PrefectureID]) REFERENCES [dbo].[Kali_Prefectures] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

