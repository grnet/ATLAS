ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_CreatedAtDateOnly] DEFAULT (getdate()) FOR [CreatedAtDateOnly];

