using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using System.Drawing;
using StudentPractice.Portal.UserControls;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Mails;
using Imis.Domain;
using System.Threading;
using StudentPractice.Utils;
using System.Text;
using StudentPractice.BusinessModel.Flow;
using StudentPractice.Portal.UserControls.GenericControls;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.Security;
using System.Data;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class AssignPosition : BaseEntityPortalPage<InternshipOffice>
    {
        #region [ Databind Methods ]

        private InternshipPosition CurrentPosition;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();

            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                CurrentPosition = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.PhysicalObjects,
                    x => x.InternshipPositionGroup.Academics,
                    x => x.InternshipPositionGroup.Provider,
                    x => x.AssignedToStudent);

                if (CurrentPosition == null)
                    Response.Redirect("SelectedPositions.aspx");

                CurrentPosition.SaveToCurrentContext();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ucAssignStudent.Entity = Entity;
            ucAssignStudent.UnitOfWork = UnitOfWork;
            ucAssignStudent.CurrentPosition = CurrentPosition;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
            {
                Response.Redirect("OfficeDetails.aspx");
            }

            if (!Page.IsPostBack)
                ucAssignStudent.Bind();
        }

        #endregion

        #region [ Events ]

        protected void ucAssignStudent_StudentAssignmentComplete(object sender, EventArgs e)
        {
            if (CurrentPosition.AssignedToStudentID.HasValue)
            {
                Response.Redirect(string.Format("AssignPosition.aspx?pID={0}", CurrentPosition.ID));
            }
            else
            {
                ucAssignStudent.ReBindGrid();
            }
        }

        protected void ucAssignStudent_StudentAssignmentCanceled(object sender, EventArgs e)
        {
            Response.Redirect("SelectedPositions.aspx");
        }

        #endregion
    }
}