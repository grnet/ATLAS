ALTER TABLE [dbo].[InternshipPositionGroupPhysicalObjectXRef]
    ADD CONSTRAINT [FK_InternshipPositionGroupPhysicalObjectXRef_PhysicalObject] FOREIGN KEY ([PhysicalObjectID]) REFERENCES [dbo].[PhysicalObject] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

