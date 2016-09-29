ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [FK_Reporter_Kali_Prefectures] FOREIGN KEY ([PrefectureID]) REFERENCES [dbo].[Kali_Prefectures] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

