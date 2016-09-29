ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

