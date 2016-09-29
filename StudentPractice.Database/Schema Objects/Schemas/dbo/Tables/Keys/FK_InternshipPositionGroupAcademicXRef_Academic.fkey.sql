ALTER TABLE [dbo].[InternshipPositionGroupAcademicXRef]
    ADD CONSTRAINT [FK_InternshipPositionGroupAcademicXRef_Academic] FOREIGN KEY ([AcademicID]) REFERENCES [dbo].[Academic] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

