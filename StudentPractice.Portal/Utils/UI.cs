using System;
using System.Web.UI;
using System.Web;
using StudentPractice.BusinessModel;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DevExpress.Web.ASPxEditors;
namespace StudentPractice.Portal
{
    public static class UI
    {
        const string PreventViewstateCaching_KEY = "fixautocomplete";
        const string PreventViewstateCaching_SCRIPT = @"

<script type='text/javascript'>
       function setAutoCompleteOff(id) {
            var elem = document.getElementById(id);
            if(elem) {
                elem.setAttribute('autocomplete', 'off');
            }           
        }
       
        setAutoCompleteOff('__VIEWSTATE');
        setAutoCompleteOff('__EVENTTARGET');
        setAutoCompleteOff('__EVENTARGUMENT');
        setAutoCompleteOff('__EVENTVALIDATION');
</script>
";

        public static void PreventViewstateCaching(Page page)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), PreventViewstateCaching_KEY))
                page.ClientScript.RegisterStartupScript(typeof(Page), PreventViewstateCaching_KEY, PreventViewstateCaching_SCRIPT);
        }

        public static void Message(string key)
        {
            Message(key, false);
        }

        public static void Message(string key, bool redirect)
        {
            if (redirect)
                HttpContext.Current.Response.Redirect("~/Common/Msg.aspx?key=" + key, true);
            else
                HttpContext.Current.Server.Transfer("~/Common/Msg.aspx?key=" + key);
        }

        public static IEnumerable<T> RecursiveOfType<T>(this Control root)
        {
            List<T> items = new List<T>();
            if (root != null)
            {
                items.AddRange(root.Controls.OfType<T>());
                foreach (Control c in root.Controls)
                    items.AddRange(c.RecursiveOfType<T>());
            }
            return items;
        }

        public static IEnumerable<T> RecursiveOfType<T>(this ControlCollection rootControls)
        {
            List<T> items = new List<T>();
            if (rootControls != null)
            {
                foreach (Control c in rootControls)
                    items.AddRange(c.RecursiveOfType<T>());
            }
            return items;
        }

        /// <summary>
        /// Searches recursively in this control to find a control with the name specified.
        /// </summary>
        /// <param name="root">The Control in which to begin searching.</param>
        /// <param name="id">The ID of the control to be found.</param>
        /// <returns>The control if it is found or null if it is not.</returns>
        public static Control FindControlRecursive(this Control root, string id)
        {
            if (root != null)
            {
                Control controlFound = root.FindControl(id);
                if (controlFound != null)
                {
                    return controlFound;
                }
                foreach (Control c in root.Controls)
                {
                    controlFound = c.FindControlRecursive(id);
                    if (controlFound != null)
                        return controlFound;
                }
            }
            return null;
        }

        public static void FillFromList<T>(this DropDownList list, IEnumerable<T> items, Func<T, ListItem> creator, string defaultText = "")
        {
            list.Items.Clear();
            if (!string.IsNullOrWhiteSpace(defaultText))
                list.Items.Add(new ListItem(defaultText, string.Empty));
            foreach (T e in items)
            {
                list.Items.Add(creator(e));
            }
        }
        public static void FillFromEnum(this DropDownList list, Type en)
        {
            list.Items.Clear();
            foreach (Enum e in Enum.GetValues(en))
            {
                string val = e.ToString("D");
                if (val != "0")
                {
                    list.Items.Add(new ListItem(e.GetLabel(), val));
                }
            }
        }

        public static void FillIncidentTypes(this ASPxComboBox ddlIncidentType, int reporterType)
        {
            var reporterIncidentTypes = CacheManager.ReporterIncidentTypes.GetItems().Where(x => x.ReporterTypeInt == reporterType);

            if (reporterIncidentTypes.Count() == 1)
            {
                var incidentType = CacheManager.IncidentTypes.Get(reporterIncidentTypes.FirstOrDefault().IncidentTypeID);
                ddlIncidentType.Items.Add(new ListEditItem(incidentType.Name, incidentType.ID));
            }
            else
            {
                ddlIncidentType.Items.Add(new ListEditItem(Resources.HelpdeskContact.Form_ReportTypePrompt, null));

                foreach (var item in reporterIncidentTypes)
                {
                    var incidentType = CacheManager.IncidentTypes.Get(item.IncidentTypeID);
                    ddlIncidentType.Items.Add(new ListEditItem(incidentType.Name, incidentType.ID));
                }
            }
        }

        public static void FillDepartments(this ASPxComboBox ddlDepartment, string instID)
        {
            int institutionID;
            if (int.TryParse(instID, out institutionID) && institutionID > 0)
            {
                var academics = CacheManager.GetOrderedAcademics(institutionID);

                ddlDepartment.Items.Clear();
                if (academics.Count() == 1)
                {
                    ddlDepartment.Items.Add(new ListEditItem(academics.FirstOrDefault().Department, academics.FirstOrDefault().ID));
                }
                else
                {
                    ddlDepartment.Items.Add(new ListEditItem(Resources.HelpdeskContact.Form_DepartmentPrompt, null));

                    foreach (Academic item in academics)
                    {
                        ddlDepartment.Items.Add(new ListEditItem(item.Department, item.ID));
                    }
                }
            }
        }

        public static void FillDepartments(this ASPxComboBox ddlDepartment, string instID, bool isActive)
        {
            int institutionID;
            if (int.TryParse(instID, out institutionID) && institutionID > 0)
            {
                var academics = isActive
                                ? CacheManager.GetActiveAcademics(institutionID)
                                : CacheManager.GetOrderedAcademics(institutionID);

                ddlDepartment.Items.Clear();
                if (academics.Count() == 1)
                {
                    ddlDepartment.Items.Add(new ListEditItem(academics.FirstOrDefault().Department, academics.FirstOrDefault().ID));
                }
                else
                {
                    ddlDepartment.Items.Add(new ListEditItem(Resources.HelpdeskContact.Form_DepartmentPrompt, null));

                    foreach (Academic item in academics)
                    {
                        ddlDepartment.Items.Add(new ListEditItem(item.Department, item.ID));
                    }
                }
            }
        }
    }
}
