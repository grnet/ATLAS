using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls
{
    public partial class PhysicalObjectsInput : BaseEntityUserControl<InternshipPositionGroup>
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public override void Bind()
        {
            btnAddPhysicalObject.Attributes["onclick"] = string.Format("popUp.show('../Common/SelectPhysicalObjects.aspx?gID={0}','{1}',cmdRefresh, 800, 610);", Entity.ID, Resources.PositionPages.PositionPhysicalObject_Add);

            gvPhysicalObjects.DataSource = Entity.PhysicalObjects;
            gvPhysicalObjects.DataBind();
        }

        #region [ Events ]

        public event EventHandler Complete;

        public event EventHandler Cancel;

        private void FireCompleteEvent()
        {
            if (Complete != null)
                Complete(this, EventArgs.Empty);
        }

        private void FireCancelEvent()
        {
            if (Cancel != null)
                Cancel(this, EventArgs.Empty);
        }

        #endregion

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            lblErrors.Visible = false;

            gvPhysicalObjects.DataSource = Entity.PhysicalObjects;
            gvPhysicalObjects.DataBind();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            FireCancelEvent();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Entity.PhysicalObjects.Count == 0)
            {
                lblErrors.Visible = true;
                lblErrors.Text = Resources.PositionPages.PositionPhysicalObject_Error;
                return;
            }

            FireCompleteEvent();
        }

        protected void gvPhysicalObjects_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var physicalObjectID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                var physicalObject = new PhysicalObjectRepository(UnitOfWork).Load(physicalObjectID);

                Entity.PhysicalObjects.Remove(physicalObject);
                Entity.UpdatedAt = DateTime.Now;
                Entity.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

                InternshipPositionGroupLog log = new InternshipPositionGroupLog();
                log.InternshipPositionGroup = Entity;
                log.OldStatus = Entity.PositionGroupStatus;
                log.NewStatus = Entity.PositionGroupStatus;
                log.CreatedAt = DateTime.Now;
                log.CreatedAtDateOnly = DateTime.Now.Date;
                log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                log.UpdatedAt = DateTime.Now;
                log.UpdatedAtDateOnly = DateTime.Now.Date;
                log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

                UnitOfWork.Commit();
            }

            gvPhysicalObjects.DataSource = Entity.PhysicalObjects;
            gvPhysicalObjects.DataBind();
        }
    }
}