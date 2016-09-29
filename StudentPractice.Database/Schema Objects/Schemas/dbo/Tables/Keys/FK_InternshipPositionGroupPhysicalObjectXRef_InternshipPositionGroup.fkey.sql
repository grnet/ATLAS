ALTER TABLE [dbo].[InternshipPositionGroupPhysicalObjectXRef]
    ADD CONSTRAINT [FK_InternshipPositionGroupPhysicalObjectXRef_InternshipPositionGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[InternshipPositionGroup] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

