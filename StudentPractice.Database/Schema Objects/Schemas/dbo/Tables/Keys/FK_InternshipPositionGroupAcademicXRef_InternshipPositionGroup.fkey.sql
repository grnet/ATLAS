ALTER TABLE [dbo].[InternshipPositionGroupAcademicXRef]
    ADD CONSTRAINT [FK_InternshipPositionGroupAcademicXRef_InternshipPositionGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[InternshipPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

