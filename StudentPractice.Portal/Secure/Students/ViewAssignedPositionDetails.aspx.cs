using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class ViewAssignedPositionDetails : BaseEntityPortalPage<InternshipPosition>
    {
        public Student CurrentStudent { get; set; }

        protected override void Fill()
        {
            CurrentStudent = new StudentRepository(UnitOfWork).FindByUsername(User.Identity.Name);
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).FindOneByStudentID(positionID, CurrentStudent.ID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.PhysicalObjects,
                    x => x.InternshipPositionGroup.Academics);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucPositionView.Entity = Entity;
            ucPositionView.Bind();

            if (Entity.PositionStatus == enPositionStatus.Assigned || Entity.PositionStatus == enPositionStatus.UnderImplementation)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();
            }

            if (Entity.PositionStatus == enPositionStatus.Completed)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();

                divCompletion.Visible = true;
                ucCompletionView.Entity = Entity;
                ucCompletionView.Bind();
            }

            if (Entity.PositionStatus == enPositionStatus.Canceled)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();

                divCompletion.Visible = true;
                ucCompletionView.Entity = Entity;
                ucCompletionView.Bind();
            }
        }
    }
}