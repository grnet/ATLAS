ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_IsLocked] DEFAULT ((0)) FOR [IsLocked];

