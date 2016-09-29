using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class InternshipPopup : System.Web.UI.Page
    {

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.Academics)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.PreAssignedByMasterAccount.Academics)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.AssignedToStudent.Academic);

            enPositionCreationType creationType;
            if (Enum.TryParse<enPositionCreationType>(Request.QueryString["crType"], out creationType))
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt, (int)creationType);
            else
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt, (int)enPositionCreationType.FromProvider);


            int cancellationReason;
            if (int.TryParse(Request.QueryString["cReason"], out cancellationReason))
            {
                switch (cancellationReason)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.CancellationReasonInt, (int)enCancellationReason.FromOffice);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.CancellationReasonInt, (int)enCancellationReason.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                        break;
                }
            }

            int positionStatus;
            if (int.TryParse(Request.QueryString["pStatus"], out positionStatus))
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, positionStatus);
                if (positionStatus == 0)
                    criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatusInt, (int)enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            }

            int groupStatus;
            if (int.TryParse(Request.QueryString["gStatus"], out groupStatus))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatusInt, groupStatus);
            }

            DateTime createdAt;
            if (DateTime.TryParse(Request.QueryString["createdAt"], out createdAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAtDateOnly, createdAt);
            }

            DateTime availableAt;
            if (DateTime.TryParse(Request.QueryString["availableAt"], out availableAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAtDateOnly, availableAt);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatus, enPositionStatus.Available, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
            }

            DateTime preassignedAt;
            if (DateTime.TryParse(Request.QueryString["preAssignedAt"], out preassignedAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedAt, preassignedAt);
            }

            DateTime assignedAt;
            if (DateTime.TryParse(Request.QueryString["assignedAt"], out assignedAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedAt, assignedAt);
            }

            DateTime underImplementationAt;
            if (DateTime.TryParse(Request.QueryString["underImplementationAt"], out underImplementationAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ImplementationStartDate, underImplementationAt);
            }

            DateTime completedAt;
            if (DateTime.TryParse(Request.QueryString["completedAt"], out completedAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CompletedAt, completedAt);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Completed);
            }

            DateTime canceledAt;
            if (DateTime.TryParse(Request.QueryString["canceledAt"], out canceledAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CompletedAt, canceledAt);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Canceled);
                criteria.Expression = criteria.Expression.Where(x => x.CancellationReasonInt, (int)enCancellationReason.FromOffice);
            }

            DateTime revokedAt;
            if (DateTime.TryParse(Request.QueryString["revokedAt"], out revokedAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.UpdatedAt, revokedAt, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                criteria.Expression = criteria.Expression.Where(x => x.UpdatedAt, revokedAt.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatus, enPositionStatus.Canceled);
                criteria.Expression = criteria.Expression.Where(x => x.CancellationReasonInt, (int)enCancellationReason.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            }

            DateTime deletedAt;
            if (DateTime.TryParse(Request.QueryString["deletedAt"], out deletedAt))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.UpdatedAt, deletedAt, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.UpdatedAt, deletedAt.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatusInt, (int)enPositionGroupStatus.Deleted);
            }


            e.InputParameters["criteria"] = criteria;
        }
    }
}