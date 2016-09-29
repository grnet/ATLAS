using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Imis.Domain;
using System.Threading;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class PositionAcademics : BaseEntityPortalPage<InternshipProvider>
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
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.Academics);

                if (CurrentGroup == null)
                    Response.Redirect("InternshipPositions.aspx");
            }
            else
            {
                Response.Redirect("InternshipPositions.aspx");
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || !Config.InternshipPositionCreationAllowed)
            {
                Response.Redirect("Default.aspx");
            }

            if (CurrentGroup != null && CurrentGroup.PositionGroupStatus == enPositionGroupStatus.Published && CurrentGroup.AvailablePositions != CurrentGroup.TotalPositions)
            {
                Response.Redirect(string.Format("EditPreAssignedPosition.aspx?gID={0}", CurrentGroup.ID));
            }

            if (CurrentGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                btnRestrictAcademics.Attributes["onclick"] = string.Format("popUp.show('SelectAcademics.aspx?gID={0}','{1}',cmdRefresh, 800, 610);", CurrentGroup.ID, Resources.PositionPages.PositionAcademics_AddAcademics);
                mvAcademics.SetActiveView(vVisibleToAllAcademics);
            }
            else
            {
                btnAddAcademics.Attributes["onclick"] = string.Format("popUp.show('SelectAcademics.aspx?gID={0}','{1}',cmdRefresh, 800, 610);", CurrentGroup.ID, Resources.PositionPages.PositionAcademics_AddAcademics);
                mvAcademics.SetActiveView(vVisibleToCertainAcademics);

                gvAcademics.DataSource = CurrentGroup.Academics;
                gvAcademics.DataBind();
            }

            btnPositionDetails.HRef = string.Format("PositionDetails.aspx?gID={0}", CurrentGroup.ID);
            btnPositionPhysicalObject.HRef = string.Format("PositionPhysicalObject.aspx?gID={0}", CurrentGroup.ID);
            btnAddAllAcademics.OnClientClick = string.Format("return confirm('{0}');", Resources.PositionPages.PositionAcademics_ConfirmVisibleToAll);
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvAcademics.DataSource = CurrentGroup.Academics;
            gvAcademics.DataBind();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("PositionPhysicalObject.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void btnTermsAccepted_Click(object sender, EventArgs e)
        {
            AddAllAcademics();
            btnRestrictAcademics.Attributes["onclick"] = string.Format("popUp.show('SelectAcademics.aspx?gID={0}','{1}',cmdRefresh, 800, 610);", CurrentGroup.ID, Resources.PositionPages.PositionAcademics_AddAcademics);
            Response.Redirect(string.Format("PositionPreview.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void btnAddAllAcademics_Click(object sender, EventArgs e)
        {
            AddAllAcademics();
            Response.Redirect(string.Format("PositionPreview.aspx?gID={0}", CurrentGroup.ID));
        }

        private void AddAllAcademics()
        {
            var academics = CurrentGroup.Academics.ToList();

            for (int i = academics.Count - 1; i >= 0; i--)
            {
                CurrentGroup.Academics.Remove(academics[i]);
            }

            CurrentGroup.IsVisibleToAllAcademics = true;
            CurrentGroup.UpdatedAt = DateTime.Now;
            CurrentGroup.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

            InternshipPositionGroupLog log = new InternshipPositionGroupLog();
            log.InternshipPositionGroup = CurrentGroup;
            log.OldStatus = CurrentGroup.PositionGroupStatus;
            log.NewStatus = CurrentGroup.PositionGroupStatus;
            log.CreatedAt = DateTime.Now;
            log.CreatedAtDateOnly = DateTime.Now.Date;
            log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
            log.UpdatedAt = DateTime.Now;
            log.UpdatedAtDateOnly = DateTime.Now.Date;
            log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

            UnitOfWork.Commit();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (mvAcademics.GetActiveView() == vVisibleToCertainAcademics)
            {
                if (!CurrentGroup.IsVisibleToAllAcademics.HasValue || CurrentGroup.Academics.Count == 0)
                {
                    lblErrors.Visible = true;
                    lblErrors.Text = Resources.PositionPages.PositionAcademics_Error;
                    return;
                }
            }

            Response.Redirect(string.Format("PositionPreview.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void gvAcademics_CustomDataCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var academicID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                var academic = new AcademicRepository(UnitOfWork).Load(academicID);

                CurrentGroup.Academics.Remove(academic);
                CurrentGroup.UpdatedAt = DateTime.Now;
                CurrentGroup.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

                if (CurrentGroup.Academics.Count == 0)
                {
                    CurrentGroup.IsVisibleToAllAcademics = false;
                }

                InternshipPositionGroupLog log = new InternshipPositionGroupLog();
                log.InternshipPositionGroup = CurrentGroup;
                log.OldStatus = CurrentGroup.PositionGroupStatus;
                log.NewStatus = CurrentGroup.PositionGroupStatus;
                log.CreatedAt = DateTime.Now;
                log.CreatedAtDateOnly = DateTime.Now.Date;
                log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                log.UpdatedAt = DateTime.Now;
                log.UpdatedAtDateOnly = DateTime.Now.Date;
                log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

                UnitOfWork.Commit();
            }

            if (CurrentGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                e.Result = ResolveClientUrl(string.Format("~/Secure/InternshipProviders/PositionAcademics.aspx?gID={0}", CurrentGroup.ID));
            }
            else
            {
                e.Result = "DATABIND";
            }
        }

        protected void gvAcademics_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            gvAcademics.DataSource = CurrentGroup.Academics;
            gvAcademics.DataBind();
        }

        protected string GetAcademicRulesLink(Academic academic)
        {
            if (academic == null || string.IsNullOrEmpty(academic.PositionRules))
                return string.Empty;

            return string.Format("<a href='javascript:void(0);' class='btn-academicRules' data-aid='{0}'><img src='/_img/iconInformation.png' alt='{1}' /></a>", academic.ID, Resources.PositionPages.PositionAcademics_OfficeRules);
        }

    }
}
