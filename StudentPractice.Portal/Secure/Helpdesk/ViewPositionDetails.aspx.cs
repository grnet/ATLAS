using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewPositionDetails : BaseEntityPortalPage<InternshipPosition>
    {
        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.PhysicalObjects,
                    x => x.InternshipPositionGroup.Academics,
                    x => x.AssignedToStudent,
                    x => x.CanceledStudent,
                    x => x.PreAssignedByMasterAccount,
                    x => x.PreAssignedByMasterAccount.Academics);
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            ucPositionView.Entity = Entity;
            ucPositionView.Bind();

            if (Entity.PositionStatusInt >= (int)enPositionStatus.PreAssigned)
            {
                divOffice.Visible = true;
                ucOfficeView.Entity = Entity.PreAssignedByMasterAccount;
                ucOfficeView.Bind();

                divPreAssignment.Visible = true;
                ucPreAssignmentView.Entity = Entity;
                ucPreAssignmentView.Bind();
            }

            if (Entity.PositionStatusInt == (int)enPositionStatus.Assigned || Entity.PositionStatusInt == (int)enPositionStatus.UnderImplementation)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();

                divStudent.Visible = true;
                ucStudentView.Entity = Entity.AssignedToStudent;
                ucStudentView.Bind();
            }

            if (Entity.PositionStatusInt == (int)enPositionStatus.Completed)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();

                divStudent.Visible = true;
                ucStudentView.Entity = Entity.AssignedToStudent;
                ucStudentView.Bind();

                divCompletion.Visible = true;
                ucCompletionView.Entity = Entity;
                ucCompletionView.Bind();
            }

            if (Entity.PositionStatusInt == (int)enPositionStatus.Canceled)
            {
                divImplementation.Visible = true;
                ucImplementationView.Entity = Entity;
                ucImplementationView.Bind();

                divStudent.Visible = true;
                ucStudentView.Entity = Entity.CanceledStudent;
                ucStudentView.Bind();

                divCompletion.Visible = true;
                ucCompletionView.Entity = Entity;
                ucCompletionView.Bind();
            }

            base.OnLoad(e);
        }
    }
}
