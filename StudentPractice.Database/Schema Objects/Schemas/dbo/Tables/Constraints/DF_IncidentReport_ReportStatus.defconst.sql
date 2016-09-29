ALTER TABLE [dbo].[IncidentReport]
    ADD CONSTRAINT [DF_IncidentReport_ReportStatus] DEFAULT ((0)) FOR [ReportStatus];

