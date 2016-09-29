using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using StudentPractice.BusinessModel;
using System.Web.Profile;
using StudentPractice.Portal.Controls;
using log4net.Repository.Hierarchy;
using System.Collections.Generic;
using Imis.Domain;
using StudentPractice.BusinessModel.Flow;
using System.Threading;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class PositionDetails : BaseEntityPortalPage<InternshipProvider>
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
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.Positions);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                Response.Redirect("~/Secure/InternshipProviders/Default.aspx");
            }

            if (!Config.InternshipPositionCreationAllowed)
            {
                mvInternshipPosition.SetActiveView(vPositionCreationNotAllowed);
            }

            if (CurrentGroup != null && CurrentGroup.PositionGroupStatus == enPositionGroupStatus.Published && CurrentGroup.AvailablePositions != CurrentGroup.TotalPositions)
            {
                Response.Redirect(string.Format("EditPreAssignedPosition.aspx?gID={0}", CurrentGroup.ID));
            }

            divNote.Visible = false;
            ucPositionGroupInput.CountryID = Entity.CountryID.Value;
            if (!IsPostBack)
            {
                if (CurrentGroup != null)
                {
                    ucPositionGroupInput.Entity = CurrentGroup;                   
                    ucPositionGroupInput.Bind();

                    bool showNote;
                    if (bool.TryParse(Request.QueryString["showNote"], out showNote))
                    {
                        divNote.Visible = showNote;
                    }
                }
            }             
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("InternshipPositions.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (CurrentGroup == null)
            {
                CurrentGroup = new InternshipPositionGroup();
                CurrentGroup.ProviderID = Entity.ID;
                CurrentGroup.AvailablePositions = 0;
                CurrentGroup.PreAssignedPositions = 0;
                CurrentGroup.PositionGroupStatus = enPositionGroupStatus.UnPublished;
                UnitOfWork.MarkAsNew(CurrentGroup);

                ucPositionGroupInput.Fill(CurrentGroup);

                for (int i = 0; i < CurrentGroup.TotalPositions; i++)
                {
                    InternshipPosition position = new InternshipPosition();
                    position.InternshipPositionGroup = CurrentGroup;
                }
            }
            else
            {
                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var oldPositionCount = CurrentGroup.TotalPositions;

                ucPositionGroupInput.Fill(CurrentGroup);

                var newPositionCount = CurrentGroup.TotalPositions;

                if (newPositionCount > oldPositionCount)
                {
                    for (int i = 0; i < newPositionCount - oldPositionCount; i++)
                    {
                        InternshipPosition position = new InternshipPosition();
                        position.InternshipPositionGroup = CurrentGroup;

                        if (CurrentGroup.PositionGroupStatus == enPositionGroupStatus.Published)
                        {
                            InternshipPositionLog logEntry = new InternshipPositionLog();
                            logEntry.InternshipPosition = position;
                            logEntry.OldStatus = enPositionStatus.UnPublished;
                            logEntry.NewStatus = enPositionStatus.Available;
                            logEntry.CreatedAt = DateTime.Now;
                            logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                            logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                            UnitOfWork.MarkAsNew(logEntry);

                            position.PositionStatus = enPositionStatus.Available;
                            CurrentGroup.AvailablePositions++;
                        }
                    }
                }
                else if (newPositionCount < oldPositionCount)
                {
                    var positions = CurrentGroup.Positions.ToList();

                    for (int i = positions.Count - 1; i >= newPositionCount; i--)
                    {
                        positions[i].LogEntries.Load();
                        var logEntries = positions[i].LogEntries.ToList();

                        for (int j = logEntries.Count - 1; j >= 0; j--)
                        {
                            UnitOfWork.MarkAsDeleted(logEntries[j]);
                        }

                        UnitOfWork.MarkAsDeleted(positions[i]);

                        if (CurrentGroup.PositionGroupStatus == enPositionGroupStatus.Published)
                        {
                            CurrentGroup.AvailablePositions--;
                        }
                    }
                }

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

            Response.Redirect(string.Format("PositionPhysicalObject.aspx?gID={0}", CurrentGroup.ID));
        }
    }
}
