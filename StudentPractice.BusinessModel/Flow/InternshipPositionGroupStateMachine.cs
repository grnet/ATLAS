using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;
using StudentPractice.Utils;
using Imis.Domain;
using System.Threading;

namespace StudentPractice.BusinessModel.Flow
{
    public class InternshipPositionGroupStateMachine : StateMachine<enPositionGroupStatus, enInternshipPositionGroupTriggers>
    {
        #region [ Trigger Helpers ]

        #region [ Triggers ]

        Dictionary<enInternshipPositionGroupTriggers, TriggerWithParameters<InternshipPositionGroupTriggerParams>> _triggers =
            new Dictionary<enInternshipPositionGroupTriggers, TriggerWithParameters<InternshipPositionGroupTriggerParams>>();

        public TriggerWithParameters<InternshipPositionGroupTriggerParams> TriggerFor(enInternshipPositionGroupTriggers trigger)
        {
            if (!_triggers.ContainsKey(trigger))
            {
                _triggers.Add(trigger, SetTriggerParameters<InternshipPositionGroupTriggerParams>(trigger));
            }
            return _triggers[trigger];
        }

        #endregion

        #endregion

        public InternshipPositionGroupStateMachine(InternshipPositionGroup group)
            : base(group.PositionGroupStatus)
        {
            PositionGroup = group;
            ConfigureStates();
        }

        protected InternshipPositionGroup PositionGroup { get; set; }

        private void ConfigureStates()
        {
            Configure(enPositionGroupStatus.UnPublished)
                .PermitIf(enInternshipPositionGroupTriggers.Delete, enPositionGroupStatus.Deleted,
                    () =>
                    {
                        return !PositionGroup.LogEntries.Any(x => x.NewStatusInt == (int)enPositionGroupStatus.Published && x.OldStatusInt == (int)enPositionGroupStatus.UnPublished);
                    })
                .PermitIf(enInternshipPositionGroupTriggers.Revoke, enPositionGroupStatus.Revoked,
                    () =>
                    {
                        return PositionGroup.LogEntries.Any(x => x.NewStatusInt == (int)enPositionGroupStatus.Published && x.OldStatusInt == (int)enPositionGroupStatus.UnPublished);
                    })
                .PermitIf(enInternshipPositionGroupTriggers.Publish, enPositionGroupStatus.Published,
                    () =>
                    {
                        return PositionGroup.PhysicalObjects.Count > 0 && (PositionGroup.IsVisibleToAllAcademics.GetValueOrDefault() || PositionGroup.Academics.Count > 0);
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.UnPublish),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;

                        foreach (var position in PositionGroup.Positions)
                        {
                            var stateMachine = new InternshipPositionStateMachine(position);
                            if (position.PositionStatus == enPositionStatus.Available && stateMachine.CanFire(enInternshipPositionTriggers.UnPublish))
                                stateMachine.UnPublish(tParams);
                        }
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.RollbackDelete),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.RollbackRevoke),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;

                        foreach (var item in triggerParams.Positions)
                        {
                            var stateMachine = new InternshipPositionStateMachine(item);
                            if (item.PositionStatus == enPositionStatus.Canceled && item.CancellationReason != enCancellationReason.FromOffice
                                && stateMachine.CanFire(enInternshipPositionTriggers.RollbackRevoke))
                            {
                                stateMachine.RollbackRevoke(tParams);
                                //PositionGroup.AvailablePositions++;
                            }
                        }
                    });

            Configure(enPositionGroupStatus.Published)
                .PermitIf(enInternshipPositionGroupTriggers.Revoke, enPositionGroupStatus.Revoked,
                    () =>
                    {
                        return PositionGroup.Positions.Any(x => x.PositionStatusInt >= (int)enPositionStatus.PreAssigned);
                    })
                .PermitIf(enInternshipPositionGroupTriggers.UnPublish, enPositionGroupStatus.UnPublished,
                    () =>
                    {
                        return PositionGroup.PreAssignedPositions == 0 && PositionGroup.Positions.All(x => x.PositionStatus == enPositionStatus.Available);
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.Publish),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        if (PositionGroup.FirstPublishedAt == null)
                        {
                            PositionGroup.FirstPublishedAt = triggerParams.ExecutionDate.Date;
                            PositionGroup.LastPublishedAt = triggerParams.ExecutionDate.Date;
                        }
                        else
                        {
                            PositionGroup.LastPublishedAt = triggerParams.ExecutionDate.Date;
                        }

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;

                        foreach (var position in PositionGroup.Positions)
                        {
                            var stateMachine = new InternshipPositionStateMachine(position);
                            if (position.PositionStatus == enPositionStatus.UnPublished && stateMachine.CanFire(enInternshipPositionTriggers.Publish))
                                stateMachine.Publish(tParams);
                        }
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.RollbackRevokeNPublish),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;

                        foreach (var item in triggerParams.Positions)
                        {
                            var stateMachine = new InternshipPositionStateMachine(item);
                            if (item.PositionStatus == enPositionStatus.Canceled && item.CancellationReason != enCancellationReason.FromOffice
                                && stateMachine.CanFire(enInternshipPositionTriggers.RollbackRevoke))
                            {
                                stateMachine.RollbackRevoke(tParams);
                            }
                        }
                    }); ;

            Configure(enPositionGroupStatus.Deleted)
                .Permit(enInternshipPositionGroupTriggers.RollbackDelete, enPositionGroupStatus.UnPublished)
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.Delete),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;

                        //foreach (var position in triggerParams.Positions)
                        //{
                        //    var stateMachine = new InternshipPositionStateMachine(position);
                        //    if (stateMachine.CanFire(enInternshipPositionTriggers.UnPublish))
                        //        stateMachine.UnPublish(tParams);
                        //}
                    });

            Configure(enPositionGroupStatus.Revoked)
                .PermitIf(enInternshipPositionGroupTriggers.RollbackRevoke, enPositionGroupStatus.UnPublished,
                    () =>
                    {
                        return !PositionGroup.Positions.Any(x => x.PositionStatus > enPositionStatus.Available && x.PositionStatus < enPositionStatus.Canceled);
                    })
                .PermitIf(enInternshipPositionGroupTriggers.RollbackRevokeNPublish, enPositionGroupStatus.Published,
                    () =>
                    {
                        return PositionGroup.Positions.Any(x => x.PositionStatus > enPositionStatus.Available && x.PositionStatus < enPositionStatus.Canceled);
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionGroupTriggers.Revoke),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        PositionGroup.PositionGroupStatus = transition.Destination;

                        InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                        gLog.CreatedAt = triggerParams.ExecutionDate;
                        gLog.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        gLog.CreatedBy = triggerParams.Username;
                        gLog.GroupID = PositionGroup.ID;
                        gLog.OldStatus = transition.Source;
                        gLog.NewStatus = transition.Destination;
                        uow.MarkAsNew(gLog);

                        InternshipPositionTriggersParams tParams = new InternshipPositionTriggersParams();
                        tParams.Username = triggerParams.Username;
                        tParams.ExecutionDate = triggerParams.ExecutionDate;
                        tParams.UnitOfWork = uow;
                        tParams.CancellationReason = triggerParams.CancellationReason;                        

                        foreach (var position in triggerParams.Positions)
                        {
                            var stateMachine = new InternshipPositionStateMachine(position);
                            if ((position.PositionStatus == enPositionStatus.Available || position.PositionStatus == enPositionStatus.UnPublished) && stateMachine.CanFire(enInternshipPositionTriggers.Cancel))
                                stateMachine.Cancel(tParams);
                        }
                    });
        }


        #region [ Shortcut Methods ]

        public void Publish(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.Publish), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void UnPublish(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.UnPublish), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void Delete(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.Delete), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void Revoke(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.Revoke), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackDelete(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.RollbackDelete), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackRevoke(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.RollbackRevoke), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }
        
        public void RollbackRevokeNPublish(InternshipPositionGroupTriggerParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionGroupTriggers.RollbackRevokeNPublish), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        #endregion

    }
}
