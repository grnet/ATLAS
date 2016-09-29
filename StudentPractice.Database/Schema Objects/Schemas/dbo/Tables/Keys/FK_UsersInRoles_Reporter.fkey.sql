ALTER TABLE [dbo].[UsersInRoles]
    ADD CONSTRAINT [FK_UsersInRoles_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

