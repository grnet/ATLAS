ALTER TABLE [dbo].[InternshipPositionGroup]
    ADD CONSTRAINT [FK_InternshipPositionGroup_Kali_Prefectures] FOREIGN KEY ([PrefectureID]) REFERENCES [dbo].[Kali_Prefectures] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

