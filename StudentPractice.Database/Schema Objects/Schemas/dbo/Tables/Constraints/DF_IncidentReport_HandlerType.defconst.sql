ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_HandlerType] DEFAULT ((0)) FOR [HandlerType];

