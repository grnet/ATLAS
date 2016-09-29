ALTER TABLE [dbo].[Reporter]
    ADD CONSTRAINT [FK_Reporter_PrimaryActivity] FOREIGN KEY ([PrimaryActivityID]) REFERENCES [dbo].[PrimaryActivity] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

