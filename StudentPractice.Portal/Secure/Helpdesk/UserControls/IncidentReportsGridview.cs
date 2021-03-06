﻿
//Auto-Generated by djsolid 
using DevExpress.Web.ASPxGridView;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
	public enum enIncidentReportsGridviewColumns
	{
		CreatedAt = 1,
		Reporter_ReporterType = 2,
		ReporterName = 4,
		SpecialDetailsOfReporter = 8,
		ReportStatus = 16,
		ReportText = 32,
		LastPost_PostText = 64,
		HandlerType = 128,
		Commands = 256
	}
	
	public partial class IncidentReportsGridview
	{
		
		[Browsable(false)]
        public List<enIncidentReportsGridviewColumns> HiddenColumns { get; set; }
		
		[Browsable(false)]
        public List<enIncidentReportsGridviewColumns> DefaultColumns { get; set; }
		
		protected bool IsHiddenColumn(GridViewColumn column)
        {
            if (HiddenColumns == null || HiddenColumns.Count == 0)
                return false;

            string columnName = column.Name.Replace('.', '_');
            enIncidentReportsGridviewColumns columnEnum = (enIncidentReportsGridviewColumns)Enum.Parse(typeof(enIncidentReportsGridviewColumns), columnName);
            return HiddenColumns.Exists(x => x == columnEnum);
        }
		
		protected bool IsDefaultColumn(GridViewColumn column)
        {
            if (DefaultColumns == null || DefaultColumns.Count == 0)
                return false;

            string columnName = column.Name.Replace('.', '_');
            if (columnName.StartsWith("CUSTOM_"))
                return true;
            enIncidentReportsGridviewColumns columnEnum = (enIncidentReportsGridviewColumns)Enum.Parse(typeof(enIncidentReportsGridviewColumns), columnName);
            return DefaultColumns.Exists(x => x == columnEnum);
        }
		
		public void HideHiddenColumns()
        {
            if (HiddenColumns != null && HiddenColumns.Count > 0)
            {
                foreach (enIncidentReportsGridviewColumns item in HiddenColumns)
                {
                    var columnName = item.ToString().Replace('_', '.');
                    gvIncidentReports.Columns[columnName].Visible = gvIncidentReports.Columns[columnName].ShowInCustomizationForm = false;
                }
            }
        }
		
		protected void SetUItoDefault()
        {
            var listColumns = gvIncidentReports.Columns
                .OfType<GridViewColumn>();

            foreach (var column in listColumns.Where(c => IsDefaultColumn(c)).ToList())
            {
                column.Visible = true;
            }
            foreach (var column in listColumns.Where(c => !IsDefaultColumn(c)).ToList())
            {
                column.Visible = false;
            }
        }
	}
}