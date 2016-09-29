ALTER TABLE [dbo].[InternshipPosition]
    ADD CONSTRAINT [FK_InternshipPosition_PreAssignedForAcademic] FOREIGN KEY ([PreAssignedForAcademicID]) REFERENCES [dbo].[Academic] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

