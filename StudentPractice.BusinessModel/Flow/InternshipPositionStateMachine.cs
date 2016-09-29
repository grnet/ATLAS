using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Imis.Domain;
using Stateless;
using StudentPractice.Utils;

namespace StudentPractice.BusinessModel.Flow
{
    public class InternshipPositionStateMachine : StateMachine<enPositionStatus, enInternshipPositionTriggers>
    {
        protected InternshipPosition Position { get; set; }

        public InternshipPositionStateMachine(InternshipPosition position)
            : base(position.PositionStatus)
        {
            Position = position;
            ConfigurePublish();
            ConfigureAssignment();
        }

        #region [ Triggers ]

        Dictionary<enInternshipPositionTriggers, TriggerWithParameters<InternshipPositionTriggersParams>> _triggers =
            new Dictionary<enInternshipPositionTriggers, TriggerWithParameters<InternshipPositionTriggersParams>>();

        public TriggerWithParameters<InternshipPositionTriggersParams> TriggerFor(enInternshipPositionTriggers trigger)
        {
            if (!_triggers.ContainsKey(trigger))
            {
                _triggers.Add(trigger, SetTriggerParameters<InternshipPositionTriggersParams>(trigger));
            }
            return _triggers[trigger];
        }

        #endregion

        #region [ Configuration Methods ]

        private void ConfigurePublish()
        {
            #region [ UnPublished ]
            Configure(enPositionStatus.UnPublished)
                .Permit(enInternshipPositionTriggers.Cancel, enPositionStatus.Canceled)
                .PermitIf(enInternshipPositionTriggers.Publish, enPositionStatus.Available,
                    () =>
                    {
                        return Position.InternshipPositionGroup.PhysicalObjects.Count > 0 && (Position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault() || Position.InternshipPositionGroup.Academics.Count > 0);
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.UnPublish),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        if (transition.Source == enPositionStatus.Assigned)
                        {
                            Position.AssignedToStudent.IsAssignedToPosition = false;
                            Position.AssignedAt = null;
                            Position.AssignedToStudentID = null;
                            Position.PreAssignedAt = null;
                            Position.PreAssignedByOfficeID = null;
                            Position.PreAssignedByMasterAccountID = null;
                            Position.PreAssignedForAcademicID = null;
                            Position.DaysLeftForAssignment = null;
                            Position.ImplementationStartDate = null;
                            Position.ImplementationEndDate = null;
                        }

                        else if (transition.Source == enPositionStatus.PreAssigned)
                        {
                            Position.PreAssignedForAcademic.PreAssignedPositions -= 1;
                            Position.PreAssignedAt = null;
                            Position.PreAssignedByOfficeID = null;
                            Position.PreAssignedByMasterAccountID = null;
                            Position.PreAssignedForAcademicID = null;
                            Position.DaysLeftForAssignment = null;
                        }

                        if (transition.Source == enPositionStatus.Available)
                            Position.InternshipPositionGroup.AvailablePositions--;

                        if (transition.Source == enPositionStatus.PreAssigned || transition.Source == enPositionStatus.Assigned)
                            Position.InternshipPositionGroup.PreAssignedPositions--;

                        Position.PositionStatus = transition.Destination;
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.RollbackRevoke),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        Position.CancellationReason = enCancellationReason.None;
                        Position.PositionStatus = transition.Destination;
                    });
            #endregion

            #region [ Available ]

            Configure(enPositionStatus.Available)
                .Permit(enInternshipPositionTriggers.UnPublish, enPositionStatus.UnPublished)
                .Permit(enInternshipPositionTriggers.Cancel, enPositionStatus.Canceled)
                .Permit(enInternshipPositionTriggers.PreAssign, enPositionStatus.PreAssigned)
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.Publish),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        if (transition.Source == enPositionStatus.UnPublished)
                            Position.InternshipPositionGroup.AvailablePositions++;

                        Position.PositionStatus = transition.Destination;
                    })
                 .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.RollbackPreAssignment),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        if (triggerParams.Username != "sysadmin")
                        {
                            logEntry.UnPreAssignedByOfficeID = triggerParams.OfficeID;
                            logEntry.UnPreAssignedByMasterAccountID = triggerParams.MasterAccountID;
                        }
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        if (triggerParams.BlockingReason != enBlockingReason.None)
                        {
                            //Εάν δεν έχει ήδη προστεθεί το group στις ποινές τότε πρόσθεσε το
                            if (!new BlockedPositionGroupRepository(uow).BlockedPositionGroupExists(Position.GroupID, Position.PreAssignedByMasterAccountID.Value)
                                && !new InternshipPositionGroupRepository(uow).IsAvailableToOnlyOneAcademic(Position.GroupID))
                            {
                                BlockedPositionGroup masterBlockedPositionGroup = new BlockedPositionGroup();
                                masterBlockedPositionGroup.GroupID = Position.GroupID;
                                masterBlockedPositionGroup.MasterAccountID = triggerParams.MasterAccountID;
                                masterBlockedPositionGroup.BlockingReason = triggerParams.BlockingReason;
                                masterBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                                uow.MarkAsNew(masterBlockedPositionGroup);

                                BlockedPositionGroup cascadedBlockedPositionGroup;

                                //Μετέφερε την ποινή σε τυχόν συμπληρωματικό Ιδρυματικό ΓΠΑ
                                if (Position.PreAssignedByMasterAccount.OfficeType != enOfficeType.Institutional)
                                {
                                    var institutionalOffice = new InternshipOfficeRepository(uow).GetVerifiedInstitutionalOffice(Position.PreAssignedByMasterAccount.InstitutionID.Value);

                                    if (institutionalOffice != null)
                                    {
                                        cascadedBlockedPositionGroup = new BlockedPositionGroup();
                                        cascadedBlockedPositionGroup.MasterBlock = masterBlockedPositionGroup;
                                        cascadedBlockedPositionGroup.GroupID = Position.GroupID;
                                        cascadedBlockedPositionGroup.MasterAccountID = institutionalOffice.ID;
                                        cascadedBlockedPositionGroup.BlockingReason = enBlockingReason.BlockCascade;
                                        cascadedBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                                        uow.MarkAsNew(cascadedBlockedPositionGroup);
                                    }
                                }
                                //Μετέφερε την ποινή σε τυχόν συμπληρωματικό Τμηματικό ή Πολυ-Τμηματικό ΓΠΑ
                                else
                                {
                                    var departmentalOffices = new InternshipOfficeRepository(uow).GetVerifiedDepartmentalOffices(Position.PreAssignedByMasterAccount.InstitutionID.Value);

                                    foreach (var office in departmentalOffices)
                                    {
                                        cascadedBlockedPositionGroup = new BlockedPositionGroup();
                                        cascadedBlockedPositionGroup.MasterBlock = masterBlockedPositionGroup;
                                        cascadedBlockedPositionGroup.GroupID = Position.GroupID;
                                        cascadedBlockedPositionGroup.MasterAccountID = office.ID;
                                        cascadedBlockedPositionGroup.BlockingReason = enBlockingReason.BlockCascade;
                                        cascadedBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                                        uow.MarkAsNew(cascadedBlockedPositionGroup);
                                    }
                                }
                            }
                        }

                        Position.PreAssignedForAcademic.PreAssignedPositions -= 1;

                        Position.PreAssignedAt = null;
                        Position.PreAssignedByOfficeID = null;
                        Position.PreAssignedByMasterAccountID = null;
                        Position.PreAssignedForAcademicID = null;
                        Position.DaysLeftForAssignment = null;

                        Position.InternshipPositionGroup.AvailablePositions += 1;
                        Position.InternshipPositionGroup.PreAssignedPositions -= 1;

                        Position.PositionStatus = transition.Destination;
                    })
                 .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.DeleteAssignment),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.UnAssignedByOfficeID = triggerParams.OfficeID;
                        logEntry.UnAssignedByMasterAccountID = triggerParams.MasterAccountID;
                        logEntry.UnAssignedStudentID = Position.AssignedToStudentID;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        //Εάν δεν έχει ήδη προστεθεί το group στις ποινές τότε πρόσθεσε το
                        if (!new BlockedPositionGroupRepository(uow).BlockedPositionGroupExists(Position.GroupID, Position.PreAssignedByMasterAccountID.Value)
                            && !new InternshipPositionGroupRepository(uow).IsAvailableToOnlyOneAcademic(Position.GroupID))
                        {
                            BlockedPositionGroup masterBlockedPositionGroup = new BlockedPositionGroup();
                            masterBlockedPositionGroup.GroupID = Position.GroupID;
                            masterBlockedPositionGroup.MasterAccountID = triggerParams.MasterAccountID;
                            masterBlockedPositionGroup.BlockingReason = triggerParams.BlockingReason;
                            masterBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                            uow.MarkAsNew(masterBlockedPositionGroup);

                            BlockedPositionGroup cascadedBlockedPositionGroup;

                            //Μετέφερε την ποινή σε τυχόν συμπληρωματικό Ιδρυματικό ΓΠΑ
                            if (Position.PreAssignedByMasterAccount.OfficeType != enOfficeType.Institutional)
                            {
                                var institutionalOffice = new InternshipOfficeRepository(uow).GetVerifiedInstitutionalOffice(Position.PreAssignedByMasterAccount.InstitutionID.Value);

                                if (institutionalOffice != null)
                                {
                                    cascadedBlockedPositionGroup = new BlockedPositionGroup();
                                    cascadedBlockedPositionGroup.MasterBlock = masterBlockedPositionGroup;
                                    cascadedBlockedPositionGroup.GroupID = Position.GroupID;
                                    cascadedBlockedPositionGroup.MasterAccountID = institutionalOffice.ID;
                                    cascadedBlockedPositionGroup.BlockingReason = enBlockingReason.BlockCascade;
                                    cascadedBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                                    uow.MarkAsNew(cascadedBlockedPositionGroup);
                                }
                            }
                            //Μετέφερε την ποινή σε τυχόν συμπληρωματικό Τμηματικό ή Πολυ-Τμηματικό ΓΠΑ
                            else
                            {
                                var departmentalOffices = new InternshipOfficeRepository(uow).GetVerifiedDepartmentalOffices(Position.PreAssignedByMasterAccount.InstitutionID.Value);

                                foreach (var office in departmentalOffices)
                                {
                                    cascadedBlockedPositionGroup = new BlockedPositionGroup();
                                    cascadedBlockedPositionGroup.MasterBlock = masterBlockedPositionGroup;
                                    cascadedBlockedPositionGroup.GroupID = Position.GroupID;
                                    cascadedBlockedPositionGroup.MasterAccountID = office.ID;
                                    cascadedBlockedPositionGroup.BlockingReason = enBlockingReason.BlockCascade;
                                    cascadedBlockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays;
                                    uow.MarkAsNew(cascadedBlockedPositionGroup);
                                }
                            }
                        }

                        var student = new StudentRepository(uow).Load(Position.AssignedToStudentID.Value);
                        student.IsAssignedToPosition = false;

                        Position.AssignedAt = null;
                        Position.AssignedToStudentID = null;
                        Position.PreAssignedAt = null;
                        Position.PreAssignedByOfficeID = null;
                        Position.PreAssignedByMasterAccountID = null;
                        Position.PreAssignedForAcademicID = null;
                        Position.DaysLeftForAssignment = null;
                        Position.ImplementationStartDate = null;
                        Position.ImplementationEndDate = null;

                        Position.InternshipPositionGroup.AvailablePositions += 1;
                        Position.InternshipPositionGroup.PreAssignedPositions -= 1;

                        Position.PositionStatus = transition.Destination;

                        uow.Commit();
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.RollbackRevoke),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        Position.CancellationReason = enCancellationReason.None;
                        Position.PositionStatus = transition.Destination;
                    });
            #endregion
        }

        private void ConfigureAssignment()
        {
            #region [ PreAssigned ]

            Configure(enPositionStatus.PreAssigned)
                .Permit(enInternshipPositionTriggers.RollbackPreAssignment, enPositionStatus.Available)
                .PermitIf(enInternshipPositionTriggers.Assign, enPositionStatus.Assigned,
                    () =>
                    {
                        return Position.ImplementationStartDate > DateTime.Now.Date;
                    })
                .PermitIf(enInternshipPositionTriggers.AssignAndBeginImplementation, enPositionStatus.UnderImplementation,
                    () =>
                    {
                        return Position.ImplementationStartDate <= DateTime.Now.Date;
                    })
                .Permit(enInternshipPositionTriggers.UnPublish, enPositionStatus.UnPublished)
                .Permit(enInternshipPositionTriggers.Cancel, enPositionStatus.Canceled)
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.PreAssign),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        Position.PreAssignedAt = triggerParams.ExecutionDate.Date;
                        Position.PreAssignedByOfficeID = triggerParams.OfficeID;
                        Position.PreAssignedByMasterAccountID = triggerParams.MasterAccountID;
                        Position.PreAssignedForAcademicID = triggerParams.Academic.ID;
                        Position.DaysLeftForAssignment = StudentPracticeConstants.Default_MaxDaysForAssignment;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.PreAssignedByOfficeID = triggerParams.OfficeID;
                        logEntry.PreAssignedByMasterAccountID = triggerParams.MasterAccountID;
                        logEntry.PreAssignedForAcademicID = triggerParams.Academic.ID;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        Position.PositionStatus = transition.Destination;

                        triggerParams.Academic.PreAssignedPositions++;

                        uow.Commit();
                    })
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.DeleteAssignment),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.UnAssignedByOfficeID = triggerParams.OfficeID;
                        logEntry.UnAssignedByMasterAccountID = triggerParams.MasterAccountID;
                        logEntry.UnAssignedStudentID = Position.AssignedToStudentID;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        var student = new StudentRepository(uow).Load(Position.AssignedToStudentID.Value);
                        student.IsAssignedToPosition = false;

                        Position.AssignedAt = null;
                        Position.AssignedToStudentID = null;
                        Position.ImplementationStartDate = null;
                        Position.ImplementationEndDate = null;

                        Position.PreAssignedForAcademic.PreAssignedPositions++;

                        Position.PositionStatus = transition.Destination;

                        uow.Commit();
                    });
            #endregion

            #region [ Assigned ]

            Configure(enPositionStatus.Assigned)
                .PermitIf(enInternshipPositionTriggers.DeleteAssignment, enPositionStatus.PreAssigned,
                    () =>
                    {
                        return Position.DaysLeftForAssignment > 0;
                    })
                .PermitIf(enInternshipPositionTriggers.DeleteAssignment, enPositionStatus.Available,
                    () =>
                    {
                        return Position.DaysLeftForAssignment == 0;
                    })
                .PermitIf(enInternshipPositionTriggers.BeginImplementation, enPositionStatus.UnderImplementation,
                    () =>
                    {
                        return Position.ImplementationStartDate.HasValue && Position.ImplementationEndDate.HasValue;
                    })
                .Permit(enInternshipPositionTriggers.UnPublish, enPositionStatus.UnPublished)
                .Permit(enInternshipPositionTriggers.Cancel, enPositionStatus.Canceled)
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.Assign),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        Position.AssignedAt = triggerParams.ExecutionDate.Date;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.AssignedByOfficeID = triggerParams.OfficeID;
                        logEntry.AssignedByMasterAccountID = triggerParams.MasterAccountID;
                        logEntry.AssignedToStudentID = Position.AssignedToStudentID;
                        logEntry.ImplementationStartDate = Position.ImplementationStartDate;
                        logEntry.ImplementationEndDate = Position.ImplementationEndDate;
                        logEntry.FundingType = Position.FundingType;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        Position.PreAssignedForAcademic.PreAssignedPositions--;

                        Position.PositionStatus = transition.Destination;

                        uow.Commit();
                    });
            #endregion

            #region [ UnderImplementation ]

            Configure(enPositionStatus.UnderImplementation)
               .Permit(enInternshipPositionTriggers.CompleteImplementation, enPositionStatus.Completed)
               .Permit(enInternshipPositionTriggers.Cancel, enPositionStatus.Canceled)
               .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.BeginImplementation),
                   (triggerParams, transition) =>
                   {
                       IUnitOfWork uow = triggerParams.UnitOfWork;

                       InternshipPositionLog logEntry = new InternshipPositionLog();
                       logEntry.InternshipPositionID = Position.ID;
                       logEntry.OldStatus = Position.PositionStatus;
                       logEntry.NewStatus = transition.Destination;
                       logEntry.FundingType = Position.FundingType;
                       logEntry.CreatedAt = triggerParams.ExecutionDate;
                       logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                       logEntry.CreatedBy = triggerParams.Username;
                       uow.MarkAsNew(logEntry);

                       Position.PositionStatus = transition.Destination;

                       uow.Commit();
                   })
               .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.AssignAndBeginImplementation),
                    (triggerParams, transition) =>
                    {
                        IUnitOfWork uow = triggerParams.UnitOfWork;

                        Position.AssignedAt = triggerParams.ExecutionDate.Date;

                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPositionID = Position.ID;
                        logEntry.OldStatus = Position.PositionStatus;
                        logEntry.NewStatus = transition.Destination;
                        logEntry.FundingType = Position.FundingType;
                        logEntry.AssignedByOfficeID = triggerParams.OfficeID;
                        logEntry.AssignedByMasterAccountID = triggerParams.MasterAccountID;
                        logEntry.AssignedToStudentID = Position.AssignedToStudentID;
                        logEntry.ImplementationStartDate = Position.ImplementationStartDate;
                        logEntry.ImplementationEndDate = Position.ImplementationEndDate;
                        logEntry.FundingType = Position.FundingType;
                        logEntry.CreatedAt = triggerParams.ExecutionDate;
                        logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                        logEntry.CreatedBy = triggerParams.Username;
                        uow.MarkAsNew(logEntry);

                        Position.PreAssignedForAcademic.PreAssignedPositions--;

                        Position.PositionStatus = transition.Destination;

                        uow.Commit();
                    })
               .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.RollbackCompletion),
                   (triggerParams, transition) =>
                   {
                       IUnitOfWork uow = triggerParams.UnitOfWork;

                       Position.CompletedAt = null;
                       Position.CompletionComments = null;
                       Position.AssignedToStudent.IsAssignedToPosition = true;

                       InternshipPositionLog logEntry = new InternshipPositionLog();
                       logEntry.InternshipPositionID = Position.ID;
                       logEntry.OldStatus = Position.PositionStatus;
                       logEntry.NewStatus = transition.Destination;                       
                       logEntry.UnCompletedByOfficeID = triggerParams.OfficeID;
                       logEntry.UnCompletedByMasterAccountID = triggerParams.MasterAccountID;
                       logEntry.CreatedAt = triggerParams.ExecutionDate;
                       logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                       logEntry.CreatedBy = triggerParams.Username;
                       uow.MarkAsNew(logEntry);

                       Position.PositionStatus = transition.Destination;

                       uow.Commit();
                   })
               .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.RollbackCancellation),
                   (triggerParams, transition) =>
                   {
                       IUnitOfWork uow = triggerParams.UnitOfWork;

                       Position.CompletedAt = null;
                       Position.CompletionComments = null;
                       Position.AssignedToStudentID = Position.CanceledStudentID;
                       Position.CanceledStudent.IsAssignedToPosition = true;
                       Position.CanceledStudentID = null;

                       InternshipPositionLog logEntry = new InternshipPositionLog();
                       logEntry.InternshipPositionID = Position.ID;
                       logEntry.OldStatus = Position.PositionStatus;
                       logEntry.NewStatus = transition.Destination;                       
                       logEntry.UnCompletedByOfficeID = triggerParams.OfficeID;
                       logEntry.UnCompletedByMasterAccountID = triggerParams.MasterAccountID;
                       logEntry.CreatedAt = triggerParams.ExecutionDate;
                       logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                       logEntry.CreatedBy = triggerParams.Username;
                       uow.MarkAsNew(logEntry);

                       Position.PositionStatus = transition.Destination;

                       uow.Commit();
                   });

            #endregion

            #region [ Completed ]

            Configure(enPositionStatus.Completed)
               .Permit(enInternshipPositionTriggers.RollbackCompletion, enPositionStatus.UnderImplementation)
               .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.CompleteImplementation),
                   (triggerParams, transition) =>
                   {
                       IUnitOfWork uow = triggerParams.UnitOfWork;

                       Position.CompletedAt = triggerParams.ExecutionDate.Date;                       
                       Position.AssignedToStudent.IsAssignedToPosition = false;

                       InternshipPositionLog logEntry = new InternshipPositionLog();
                       logEntry.InternshipPositionID = Position.ID;
                       logEntry.OldStatus = Position.PositionStatus;
                       logEntry.NewStatus = transition.Destination;
                       logEntry.FundingType = Position.FundingType;
                       logEntry.CompletedAt = triggerParams.ExecutionDate.Date;
                       logEntry.CompletedByOfficeID = triggerParams.OfficeID;
                       logEntry.CompletedByMasterAccountID = triggerParams.MasterAccountID;
                       logEntry.CompletionComments = triggerParams.Comment;
                       logEntry.CreatedAt = triggerParams.ExecutionDate;
                       logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                       logEntry.CreatedBy = triggerParams.Username;
                       uow.MarkAsNew(logEntry);

                       Position.PositionStatus = transition.Destination;
                   });
            #endregion

            #region [ Canceled ]

            Configure(enPositionStatus.Canceled)
                .Permit(enInternshipPositionTriggers.RollbackCancellation, enPositionStatus.UnderImplementation)
                .PermitIf(enInternshipPositionTriggers.RollbackRevoke, enPositionStatus.UnPublished, () =>
                    {
                        return (Position.CancellationReason == enCancellationReason.CanceledGroupCascade
                             || Position.CancellationReason == enCancellationReason.FromHelpdesk
                             || Position.CancellationReason == enCancellationReason.FromProvider)
                             && Position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished;
                    })
                .PermitIf(enInternshipPositionTriggers.RollbackRevoke, enPositionStatus.Available, () =>
                     {
                         return (Position.CancellationReason == enCancellationReason.CanceledGroupCascade
                             || Position.CancellationReason == enCancellationReason.FromHelpdesk
                             || Position.CancellationReason == enCancellationReason.FromProvider)
                             && Position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Published;
                     })
                .OnEntryFrom(TriggerFor(enInternshipPositionTriggers.Cancel),
                   (triggerParams, transition) =>
                   {
                       IUnitOfWork uow = triggerParams.UnitOfWork;

                       InternshipPositionLog logEntry = new InternshipPositionLog();
                       logEntry.InternshipPositionID = Position.ID;
                       logEntry.OldStatus = Position.PositionStatus;
                       logEntry.NewStatus = transition.Destination;
                       if (Position.FundingTypeInt.HasValue)
                       {
                           logEntry.FundingType = Position.FundingType;
                       }
                       logEntry.CancellationReason = triggerParams.CancellationReason;
                       logEntry.CreatedAt = triggerParams.ExecutionDate;
                       logEntry.CreatedAtDateOnly = triggerParams.ExecutionDate.Date;
                       logEntry.CreatedBy = triggerParams.Username;

                       if (transition.Source == enPositionStatus.Available)
                       {
                           Position.InternshipPositionGroup.AvailablePositions--;
                       }

                       else if (transition.Source == enPositionStatus.UnderImplementation)
                       {
                           logEntry.CompletedAt = triggerParams.ExecutionDate.Date;
                           if (triggerParams.CancellationReason == enCancellationReason.FromOffice)
                           {
                               logEntry.CompletedByOfficeID = triggerParams.OfficeID;
                               logEntry.CompletedByMasterAccountID = triggerParams.MasterAccountID;
                           }
                           logEntry.CompletionComments = triggerParams.Comment;
                           Position.CompletedAt = triggerParams.ExecutionDate.Date;
                           Position.CompletionComments = triggerParams.Comment;
                           Position.CancellationReason = triggerParams.CancellationReason;
                           Position.AssignedToStudent.IsAssignedToPosition = false;
                           Position.CanceledStudentID = Position.AssignedToStudentID;
                       }

                       else if (transition.Source == enPositionStatus.Assigned)
                       {
                           Position.InternshipPositionGroup.PreAssignedPositions--;
                           if (triggerParams.CancellationReason == enCancellationReason.FromOffice)
                           {
                               logEntry.UnAssignedByOfficeID = triggerParams.OfficeID;
                               logEntry.UnAssignedByMasterAccountID = triggerParams.MasterAccountID;
                           }
                           logEntry.UnAssignedStudentID = Position.AssignedToStudentID;
                           Position.AssignedToStudent.IsAssignedToPosition = false;
                           Position.AssignedAt = null;
                           Position.AssignedToStudentID = null;
                           Position.PreAssignedAt = null;
                           Position.PreAssignedByOfficeID = null;
                           Position.PreAssignedByMasterAccountID = null;
                           Position.PreAssignedForAcademicID = null;
                           Position.DaysLeftForAssignment = null;
                           Position.ImplementationStartDate = null;
                           Position.ImplementationEndDate = null;
                       }

                       else if (transition.Source == enPositionStatus.PreAssigned)
                       {
                           Position.PreAssignedForAcademic.PreAssignedPositions--;
                           Position.InternshipPositionGroup.PreAssignedPositions--;
                           Position.PreAssignedAt = null;
                           Position.PreAssignedByOfficeID = null;
                           Position.PreAssignedByMasterAccountID = null;
                           Position.PreAssignedForAcademicID = null;
                           Position.DaysLeftForAssignment = null;
                           if (triggerParams.CancellationReason == enCancellationReason.FromOffice)
                           {
                               logEntry.UnPreAssignedByOfficeID = triggerParams.OfficeID;
                               logEntry.UnPreAssignedByMasterAccountID = triggerParams.MasterAccountID;
                           }
                       }

                       Position.CancellationReason = triggerParams.CancellationReason;
                       Position.PositionStatus = transition.Destination;
                       uow.MarkAsNew(logEntry);
                   });
            #endregion
        }

        #endregion

        #region [ Shortcut Methods ]

        public void Publish(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.Publish), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void UnPublish(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.UnPublish), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void PreAssign(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.PreAssign), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackPreAssignment(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.RollbackPreAssignment), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void Assign(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.Assign), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void DeleteAssignment(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.DeleteAssignment), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void AssignAndBeginImplementation(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.AssignAndBeginImplementation), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void BeginImplementation(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.BeginImplementation), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void CompleteImplementation(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.CompleteImplementation), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void Cancel(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.Cancel), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackCompletion(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.RollbackCompletion), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackCancellation(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.RollbackCancellation), triggersParams);
            }
            catch (InvalidOperationException exception)
            {
                /* Δεν μπορει να γίνει η μετάβαση */
                LogHelper.LogError(exception, this);
            }
        }

        public void RollbackRevoke(InternshipPositionTriggersParams triggersParams)
        {
            try
            {
                Fire(TriggerFor(enInternshipPositionTriggers.RollbackRevoke), triggersParams);
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
