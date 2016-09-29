ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_HandlerStatus] DEFAULT ((0)) FOR [HandlerStatus];

