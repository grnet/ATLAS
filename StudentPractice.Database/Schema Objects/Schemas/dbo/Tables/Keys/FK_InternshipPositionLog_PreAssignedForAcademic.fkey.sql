ALTER TABLE [dbo].[InternshipPositionLog]
    ADD CONSTRAINT [FK_InternshipPositionLog_PreAssignedForAcademic] FOREIGN KEY ([PreAssignedForAcademicID]) REFERENCES [dbo].[Academic] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

