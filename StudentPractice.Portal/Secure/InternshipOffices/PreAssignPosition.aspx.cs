using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel.Flow;
using System.Threading;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class PreAssignPosition : BaseEntityPortalPage<InternshipOffice>
    {
        private InternshipPositionGroup CurrentGroup;
        private List<Academic> SelectableAcademics;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics, x => x.PreAssignedByMasterAccountPositions);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID,
                    x => x.PhysicalObjects,
                    x => x.Positions,
                    x => x.Academics);
            }
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            ddlDepartment.Items.Add(new ListItem("-- αδιάφορο --", ""));

            List<Academic> academics = Entity.Academics.Where(x => x.IsActive && x.InstitutionID == Entity.InstitutionID).OrderBy(x => x.Department).ToList();

            SelectableAcademics = new List<Academic>();

            if (!CurrentGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                foreach (var academic in academics)
                {
                    if (CurrentGroup.Academics.Any(x => x.IsActive && x.ID == academic.ID))
                    {
                        SelectableAcademics.Add(academic);
                    }
                }
            }
            else
            {
                SelectableAcademics = academics;
            }

            foreach (var item in SelectableAcademics)
            {
                ddlDepartment.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            ucPositionGroupView.Entity = CurrentGroup;
            ucPositionGroupView.UserAssociatedAcademics = Entity.Academics.ToList();
            ucPositionGroupView.Bind();

            lblAvailablePositions.Text = CurrentGroup.AvailablePositions.ToString();
            rvPositionCount.MaximumValue = CurrentGroup.AvailablePositions.ToString();

            if (SelectableAcademics.Count == 0)
            {
                divErrors.Visible = true;
                btnSubmit.Visible = false;
                lblErrors.Text = "Δεν μπορείτε να προδεσμεύσετε αυτήν την θέση γιατί δεν είναι διαθέσιμη σε κάποιο από τα ενεργά τμήματα που εξυπηρετεί το γραφείο.";
            }

            base.OnLoad(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int positionCount;
            int academicID;

            if (int.TryParse(txtPositionCount.Text.ToNull(), out positionCount) && positionCount > 0 &&
                int.TryParse(ddlDepartment.Text.ToNull(), out academicID) && academicID > 0)
            {

                if (positionCount > CurrentGroup.AvailablePositions)
                {
                    divErrors.Visible = true;
                    lblErrors.Text = "Δεν μπορείτε να προδεσμεύσετε πάνω από τον αριθμό των διαθέσιμων θέσεων";
                    return;
                }

                var academic = new AcademicRepository(UnitOfWork).Load(academicID);
                if (academic == null || !academic.IsActive)
                {
                    divErrors.Visible = true;
                    lblErrors.Text = "Δεν βρέθηκε η σχολή";
                    return;
                }

                var availablePositionsForPreAssignment = academic.MaxAllowedPreAssignedPositions - academic.PreAssignedPositions;

                if (positionCount > availablePositionsForPreAssignment)
                {
                    divErrors.Visible = true;
                    lblErrors.Text = string.Format("Μπορείτε να προδεσμεύσετε μέχρι {0} ακόμα θέσεις καθότι έχει φτάσει το μέγιστο επιτρεπτόμενο όριο προδεσμεύσεων για το Τμήμα που επιλέξατε.", availablePositionsForPreAssignment);
                    return;
                }

                var criteria = new Criteria<InternshipPosition>();
                criteria.UsePaging = false;
                criteria.Expression = criteria.Expression.Where(x => x.GroupID, CurrentGroup.ID);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, enPositionStatus.Available.GetValue());

                int positionResults;
                var positions = new InternshipPositionRepository(UnitOfWork).FindWithCriteria(criteria, out positionResults);

                if (positionResults < positionCount)
                {
                    divErrors.Visible = true;
                    lblErrors.Text = "Ζητήσατε να προδεσμεύσετε περισσότερες από τις διαθέσιμες θέσεις.";
                    return;
                }

                for (int i = 0; i < positionCount; i++)
                {
                    InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                    triggersParams.OfficeID = Entity.ID;

                    if (Entity.MasterAccountID.HasValue)
                        triggersParams.MasterAccountID = Entity.MasterAccountID.Value;
                    else
                        triggersParams.MasterAccountID = Entity.ID;

                    triggersParams.Academic = academic;
                    triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                    triggersParams.ExecutionDate = DateTime.Now;
                    triggersParams.UnitOfWork = UnitOfWork;

                    var stateMachine = new InternshipPositionStateMachine(positions[i]);
                    if (stateMachine.CanFire(enInternshipPositionTriggers.PreAssign))
                        stateMachine.PreAssign(triggersParams);
                }

                CurrentGroup.AvailablePositions -= positionCount;
                CurrentGroup.PreAssignedPositions += positionCount;
                UnitOfWork.Commit();
            }

            Session["flash"] = positionCount > 1
                ? "Οι θέσεις έχουν προδεσμευτεί. Για να δείτε τις προδεσμευμένες θέσεις μπορείτε να μεταβείτε στην καρτέλα <a style='font-weight: bold; font-size: 13px; color: Blue' href='SelectedPositions.aspx'>Επιλεγμένες Θέσεις</a>"
                : "Η θέση έχει προδεσμευτεί. Για να δείτε τις προδεσμευμένες θέσεις μπορείτε να μεταβείτε στην καρτέλα <a style='font-weight: bold; font-size: 13px; color: Blue' href='SelectedPositions.aspx'>Επιλεγμένες Θέσεις</a>";

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
