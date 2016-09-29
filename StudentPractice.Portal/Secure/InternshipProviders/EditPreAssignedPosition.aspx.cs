using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Imis.Domain;
using StudentPractice.BusinessModel.Flow;
using System.Threading;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class EditPreAssignedPosition : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        private InternshipPositionGroup CurrentGroup;

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects, x => x.Academics, x => x.Positions);

                if (CurrentGroup == null)
                    Response.Redirect("InternshipPositions.aspx");
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtSupervisor.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";

            if (!Entity.IsEmailVerified || !Config.InternshipPositionCreationAllowed)
            {
                Response.Redirect("Default.aspx");
            }

            if (CurrentGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                mvAcademics.SetActiveView(vVisibleToAllAcademics);
            }
            else
            {
                btnAddAcademics.Attributes["onclick"] = string.Format("popUp.show('AddAcademics.aspx?gID={0}','Προσθήκη Σχολών/Τμημάτων',cmdRefresh);", CurrentGroup.ID);
                mvAcademics.SetActiveView(vVisibleToCertainAcademics);

                gvAcademics.DataSource = CurrentGroup.Academics;
                gvAcademics.DataBind();
            }

            if (!Page.IsPostBack)
            {
                txtSupervisor.Text = CurrentGroup.Supervisor;
                txtSupervisorEmail.Text = CurrentGroup.SupervisorEmail;
                txtContactPhone.Text = CurrentGroup.ContactPhone;
            }

            ucPositionGroupView.Entity = CurrentGroup;
            ucPositionGroupView.Bind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvAcademics.DataSource = CurrentGroup.Academics;
            gvAcademics.DataBind();
        }

        protected void btnAddAllAcademics_Click(object sender, EventArgs e)
        {
            var academics = CurrentGroup.Academics.ToList();

            for (int i = academics.Count - 1; i >= 0; i--)
            {
                CurrentGroup.Academics.Remove(academics[i]);
            }

            CurrentGroup.IsVisibleToAllAcademics = true;
            UnitOfWork.Commit();

            Response.Redirect(string.Format("EditPreAssignedPosition.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void btnCancelUnPreAssignedPositions_Click(object sender, EventArgs e)
        {
            enPositionGroupStatus oldStatus = CurrentGroup.PositionGroupStatus;
            try
            {
                InternshipPositionGroupTriggerParams triggerParams = new InternshipPositionGroupTriggerParams();
                triggerParams.ExecutionDate = DateTime.Now;
                triggerParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggerParams.UnitOfWork = UnitOfWork;
                triggerParams.CancellationReason = enCancellationReason.FromProvider;
                triggerParams.Positions = new InternshipPositionRepository(UnitOfWork).FindUnPreAssignedInternshipPositions(CurrentGroup.ID, x => x.InternshipPositionGroup, x => x.PreAssignedForAcademic);
                var stateMachine = new InternshipPositionGroupStateMachine(CurrentGroup);
                if (stateMachine.CanFire(enInternshipPositionGroupTriggers.Revoke))
                    stateMachine.Revoke(triggerParams);

                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, this, ex.Message);
                CurrentGroup.PositionGroupStatus = oldStatus;
                UnitOfWork.Commit();
            }

            Response.Redirect("InternshipPositions.aspx");
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("InternshipPositions.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (CurrentGroup.Supervisor != txtSupervisor.Text.ToNull())
                CurrentGroup.Supervisor = txtSupervisor.Text.ToNull();

            if (CurrentGroup.SupervisorEmail != txtSupervisorEmail.Text.ToNull())
                CurrentGroup.SupervisorEmail = txtSupervisorEmail.Text.ToNull();

            if (CurrentGroup.ContactPhone != txtContactPhone.Text.ToNull())
                CurrentGroup.ContactPhone = txtContactPhone.Text.ToNull();

            UnitOfWork.Commit();
            Response.Redirect("InternshipPositions.aspx");
        }

        protected string GetAcademicRulesLink(Academic academic)
        {
            if (academic == null || string.IsNullOrEmpty(academic.PositionRules))
                return string.Empty;

            return string.Format("<a href='javascript:void(0);' class='btn-academicRules' data-aid='{0}'><img src='/_img/iconInformation.png' alt='Περιγραφή Πρακτικής Άσκησης Τμημάτων' /></a>", academic.ID);
        }
    }
}
