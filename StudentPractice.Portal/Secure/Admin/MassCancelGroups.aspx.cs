using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.BusinessModel.Flow;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class MassCancelGroups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            var gIDs = txtGroupIDs.Text.Split(',').Select(x => int.Parse(x)).ToList();

            var idBatches = gIDs.Split(50);

            foreach (var batch in idBatches)
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    var groups = new InternshipPositionGroupRepository(uow).LoadMany(batch, x => x.Positions).ToList();

                    foreach (var group in groups)
                    {
                        var machine = new InternshipPositionGroupStateMachine(group);
                        if (machine.CanFire(enInternshipPositionGroupTriggers.UnPublish))
                        {
                            //group.Positions.Load();
                            machine.UnPublish(new InternshipPositionGroupTriggerParams()
                            {
                                ExecutionDate = DateTime.Now,
                                Username = Thread.CurrentPrincipal.Identity.Name,
                                UnitOfWork = uow
                            });
                        }
                        else if (machine.CanFire(enInternshipPositionGroupTriggers.Revoke))
                        {
                            var positions = new InternshipPositionRepository(uow).FindByGroupID(group.ID,
                                x => x.InternshipPositionGroup,
                                x => x.InternshipPositionGroup.PhysicalObjects,
                                x => x.InternshipPositionGroup.Academics,
                                x => x.InternshipPositionGroup.LogEntries,
                                x => x.AssignedToStudent);

                            machine.Revoke(new InternshipPositionGroupTriggerParams()
                            {
                                Positions = positions,
                                CancellationReason = enCancellationReason.FromHelpdesk,
                                ExecutionDate = DateTime.Now,
                                Username = Thread.CurrentPrincipal.Identity.Name,
                                UnitOfWork = uow
                            });
                        }
                    }

                    uow.Commit();
                }
            }
        }
    }
}