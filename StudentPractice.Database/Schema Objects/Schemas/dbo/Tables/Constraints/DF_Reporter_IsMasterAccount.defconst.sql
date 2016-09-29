ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [DF_Reporter_IsMasterAccount] DEFAULT ((0)) FOR [IsMasterAccount];

