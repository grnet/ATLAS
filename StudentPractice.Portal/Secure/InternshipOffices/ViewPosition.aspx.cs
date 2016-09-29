using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class ViewPosition : BaseEntityPortalPage<InternshipPosition>
    {
        InternshipOffice CurrentOffice;

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
                    x => x.CanceledStudent);
            }

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
        }

        protected override void OnLoad(EventArgs e)
        {
            ucPositionGroupView.Entity = Entity.InternshipPositionGroup;

            if (CurrentOffice.CanViewAllAcademics.GetValueOrDefault())
            {
                ucPositionGroupView.UserAssociatedAcademics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == CurrentOffice.InstitutionID.Value && x.IsActive).ToList();
            }
            else
            {
                ucPositionGroupView.UserAssociatedAcademics = CurrentOffice.Academics.ToList();
            }

            ucPositionGroupView.Bind();

            if (Entity.PositionStatusInt >= (int)enPositionStatus.PreAssigned)
            {
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

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                ucPositionGroupView.HideAcademics = true;
            }


            base.OnLoad(e);
        }
    }
}
