ALTER TABLE [dbo].[IncidentReportPost]
    ADD CONSTRAINT [DF_IncidentReportPost_CreatedAt] DEFAULT (getdate()) FOR [CreatedAt];

