ALTER TABLE [dbo].[InternshipPositionGroupPhysicalObjectXRef]
    ADD CONSTRAINT [PK_InternshipPositionGroupPhysicalObjectXRef] PRIMARY KEY CLUSTERED ([GroupID] ASC, [PhysicalObjectID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

