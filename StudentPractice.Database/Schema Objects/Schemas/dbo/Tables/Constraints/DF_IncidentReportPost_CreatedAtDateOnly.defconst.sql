ALTER TABLE [dbo].[IncidentReportPost]
    ADD CONSTRAINT [DF_IncidentReportPost_CreatedAtDateOnly] DEFAULT (getdate()) FOR [CreatedAtDateOnly];

