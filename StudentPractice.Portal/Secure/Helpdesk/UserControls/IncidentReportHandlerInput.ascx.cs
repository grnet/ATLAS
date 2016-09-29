using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Xml.Linq;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using Imis.Domain;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportHandlerInput : BaseUserControl<BaseEntityPortalPage<IncidentReport>>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlHandlerType_Init(object sender, EventArgs e)
        {
            ddlHandlerType.Items.Add(new ListItem("-- επιλέξτε κατηγορία --", ""));

            foreach (enHandlerType item in Enum.GetValues(typeof(enHandlerType)))
            {
                ddlHandlerType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }


        protected void ddlHandlerStatus_Init(object sender, EventArgs e)
        {
            ddlHandlerStatus.Items.Add(new ListItem("-- επιλέξτε κατάσταση επικοινωνίας --", ""));

            foreach (enHandlerStatus item in Enum.GetValues(typeof(enHandlerStatus)))
            {
                ddlHandlerStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        public void FillIncidentReportHandler(IncidentReport ir)
        {
            ir.HandlerType = (enHandlerType)Convert.ToInt32(ddlHandlerType.SelectedValue);

            if (ir.HandlerType == enHandlerType.Helpdesk)
            {
                ir.HandlerStatus = enHandlerStatus.NotHandledBySupervisor;
            }
            else if (ir.HandlerType == enHandlerType.Supervisor)
            {
                ir.HandlerStatus = (enHandlerStatus)Convert.ToInt32(ddlHandlerStatus.SelectedValue);

                if (ir.HandlerStatus == enHandlerStatus.NotHandledBySupervisor)
                {
                    ir.HandlerStatus = enHandlerStatus.Pending;
                }
            }
        }

        public void SetIncidentReportHandler(IncidentReport ir)
        {
            ddlHandlerType.SelectedValue = ((int)ir.HandlerType).ToString();
            ddlHandlerStatus.SelectedValue = ((int)ir.HandlerStatus).ToString();
        }
    }
}