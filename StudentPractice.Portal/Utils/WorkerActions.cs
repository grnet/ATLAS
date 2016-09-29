using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using StudentPractice.Portal;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.BusinessModel.Flow;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using StudentPractice.Mails;
using log4net;
using StudentPractice.Portal.UserControls.Exporters;

namespace StudentPractice.Utils.Worker
{
    public static class WorkerActions
    {

        readonly static ILog s_Log = LogManager.GetLogger(typeof(AsyncWorker));

        #region [ ProposalData ]

        public static AsyncWorkerItem UpdateStatisticsByDay()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = UpdateStatisticsByDayTask;
            item.Name = "UpdateStatisticsByDay";
            return item;
        }

        public static AsyncWorkerItem CheckPreAssignedPositions()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = CheckPreAssignedPositionsTask;
            item.Name = "CheckPreAssignedPositions";
            return item;
        }

        public static AsyncWorkerItem CheckAssignedPositions()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = CheckAssignedPositionsTask;
            item.Name = "CheckAssignedPositions";
            return item;
        }

        public static AsyncWorkerItem CheckBlockedPositions()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = CheckBlockedPositionsTask;
            item.Name = "CheckBlockedPositions";
            return item;
        }

        public static AsyncWorkerItem CheckNewlyPublishedPositions()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = CheckNewlyPublishedPositionsTask;
            item.Name = "CheckNewlyPublishedPositions";
            return item;
        }

        public static AsyncWorkerItem GenerateReportFiles()
        {
            AsyncWorkerItem item = new AsyncWorkerItem();
            item.Task = GenerateReportFilesTask;
            item.Name = "GenerateReportFiles";
            return item;
        }

        public static void UpdateStatisticsByDayTask()
        {
            LogHelper.LogMessage("Starting task UpdateStatisticsByDayTask", typeof(WorkerActions).FullName);
            using (var sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                using (var sqlCom = new SqlCommand("sp_UpdateStatisticsByDay", sqlCon))
                {
                    sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCom.CommandTimeout = 300;
                    sqlCom.Connection.Open();
                    sqlCom.ExecuteNonQuery();
                    sqlCom.Connection.Close();
                }
            }
            LogHelper.LogMessage("Finished task UpdateStatisticsByDayTask", typeof(WorkerActions).FullName);
        }

        public static void CheckPreAssignedPositionsTask()
        {
            LogHelper.LogMessage("Starting task CheckPreAssignedPositionsTask", typeof(WorkerActions).FullName);

            int batchNumber = 0;
            int batchSize = 500;

            while (true)
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    List<InternshipPosition> positions = new InternshipPositionRepository(uow).FindPreAssignedInternshipPositions(batchNumber++ * batchSize, batchSize,
                        x => x.InternshipPositionGroup,
                        x => x.PreAssignedByMasterAccount.Academics,
                        x => x.PreAssignedForAcademic);

                    if (positions.Count != 0)
                        CheckPreAssignedPositionsTask(uow, positions);
                    else
                        break;
                }
            }

            LogHelper.LogMessage("Finished task CheckPreAssignedPositionsTask", typeof(WorkerActions).FullName);
        }

        private static void CheckPreAssignedPositionsTask(IUnitOfWork uow, List<InternshipPosition> positions)
        {
            foreach (var position in positions)
            {
                if (position.PreAssignedAt.HasValue)
                    position.DaysLeftForAssignment = StudentPracticeConstants.Default_MaxDaysForAssignment - (int)DateTime.Now.Subtract(position.PreAssignedAt.Value).TotalDays;
                if (position.DaysLeftForAssignment <= 0)
                    position.DaysLeftForAssignment = 0;

                if (position.PositionStatus == enPositionStatus.PreAssigned && position.DaysLeftForAssignment <= 0)
                {
                    InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                    triggersParams.Username = "sysadmin";
                    triggersParams.ExecutionDate = DateTime.Now;
                    triggersParams.UnitOfWork = uow;
                    var stateMachine = new InternshipPositionStateMachine(position);

                    if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Revoked)
                    {
                        triggersParams.CancellationReason = enCancellationReason.CanceledGroupCascade;
                        stateMachine.Cancel(triggersParams);
                        uow.Commit();
                    }
                    else if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                    {
                        stateMachine.UnPublish(triggersParams);
                        uow.Commit();
                    }
                    else
                    {
                        triggersParams.MasterAccountID = position.PreAssignedByMasterAccountID.Value;
                        triggersParams.BlockingReason = enBlockingReason.TimeForAssignmentExpired;
                        stateMachine.RollbackPreAssignment(triggersParams);
                        uow.Commit();
                    }
                }
                else
                {
                    uow.Commit();
                }
            }
        }

        public static void CheckAssignedPositionsTask()
        {
            LogHelper.LogMessage("Starting task CheckAssignedPositionsTask", typeof(WorkerActions).FullName);

            int batchNumber = 0;
            int batchSize = 500;

            while (true)
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    List<InternshipPosition> positions = new InternshipPositionRepository(uow).FindAssignedInternshipPositions(batchNumber++ * batchSize, batchSize);

                    if (positions.Count != 0)
                        CheckAssignedPositionsTask(uow, positions);
                    else
                        break;
                }
            }

            LogHelper.LogMessage("Finished task CheckAssignedPositionsTask", typeof(WorkerActions).FullName);
        }

        private static void CheckAssignedPositionsTask(IUnitOfWork uow, List<InternshipPosition> positions)
        {
            foreach (var position in positions)
            {
                if (position.ImplementationStartDate.Value <= DateTime.Now.Date)
                {
                    InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();

                    triggersParams.Username = "sysadmin";
                    triggersParams.ExecutionDate = DateTime.Now;
                    triggersParams.UnitOfWork = uow;

                    var stateMachine = new InternshipPositionStateMachine(position);
                    stateMachine.BeginImplementation(triggersParams);
                }
            }
        }

        public static void CheckBlockedPositionsTask()
        {
            LogHelper.LogMessage("Starting task CheckBlockedPositionsTask", typeof(WorkerActions).FullName);

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                Criteria<BlockedPositionGroup> criteria = new Criteria<BlockedPositionGroup>();

                criteria.UsePaging = false;
                criteria.Include(x => x.CascadedBlocks);
                criteria.Expression = criteria.Expression.IsNull(x => x.MasterBlockID);

                int blockedPositionGroupCount;
                List<BlockedPositionGroup> blockedPositionGroups = new BlockedPositionGroupRepository(uow).FindWithCriteria(criteria, out blockedPositionGroupCount);

                foreach (var blockedPositionGroup in blockedPositionGroups)
                {
                    try
                    {
                        blockedPositionGroup.DaysLeft = StudentPracticeConstants.Default_BlockingDays - (int)DateTime.Today.Date.Subtract(blockedPositionGroup.CreatedAtDateOnly).TotalDays;
                        if (blockedPositionGroup.DaysLeft <= 0)
                            blockedPositionGroup.DaysLeft = 0;

                        foreach (var item in blockedPositionGroup.CascadedBlocks)
                            item.DaysLeft = blockedPositionGroup.DaysLeft;

                        if (blockedPositionGroup.DaysLeft <= 0)
                        {
                            blockedPositionGroup.DaysLeft = 0;
                            var cascadedBlocks = blockedPositionGroup.CascadedBlocks.ToList();
                            for (int i = cascadedBlocks.Count - 1; i >= 0; i--)
                                uow.MarkAsDeleted(cascadedBlocks[i]);

                            uow.MarkAsDeleted(blockedPositionGroup);
                        }

                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex, typeof(WorkerActions).FullName, string.Format("Exception: {0} at task CheckBlockedPositionsTask", ex.Message));
                    }
                }
            }
            LogHelper.LogMessage("Finished task CheckBlockedPositionsTask", typeof(WorkerActions).FullName);
        }

        public static void CheckNewlyPublishedPositionsTask()
        {
            LogHelper.LogMessage("Starting task CheckNewlyPublishedPositionsTask", typeof(WorkerActions).FullName);

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                List<InternshipPositionGroup> positions = new InternshipPositionGroupRepository(uow).FindPublishedPositions(DateTime.Now.Date, x => x.Academics);
                List<InternshipOffice> offices = new InternshipOfficeRepository(uow).FindVerifiedOffices(x => x.Academics);

                if (positions.Count() > 0)
                {
                    foreach (var office in offices)
                    {
                        int positionCount = 0;
                        List<Academic> officeAcademics;

                        if (office.CanViewAllAcademics.GetValueOrDefault())

                            officeAcademics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == office.InstitutionID.Value).ToList();
                        else
                            officeAcademics = office.Academics.ToList();

                        foreach (var position in positions)
                        {
                            if (position.IsVisibleToAllAcademics.GetValueOrDefault())
                                positionCount++;
                            else if (position.Academics.Select(x => x.ID).Intersect(officeAcademics.Select(x => x.ID)).Count() > 0)
                                positionCount++;
                        }

                        if (positionCount > 0)
                        {
                            var email = MailSender.SendNewlyPublishedPositions(office.ID, office.ContactEmail, office.UserName, positionCount);
                            uow.MarkAsNew(email);
                        }
                    }

                    uow.Commit();
                }
            }

            LogHelper.LogMessage("Finished task CheckNewlyPublishedPositionsTask", typeof(WorkerActions).FullName);
        }

        public static void GenerateReportFilesTask()
        {
            LogHelper.LogMessage("Starting task GenerateReportFilesTask", typeof(WorkerActions).FullName);
            
            CreateInternshipPositionGroupsFile();
            CreateInternshipPositionsFile();

            LogHelper.LogMessage("Finished task GenerateReportFilesTask", typeof(WorkerActions).FullName);
        }

        #endregion

        #region [ Helper Methods ]

        public static string CreateInternshipPositionGroupsFile()
        {
            var iRep = new InternshipPositionGroupRepository();
            var iExporter = new InternshipPositionGroupsExporter();

            var filePath = string.Format("{0}\\InternshipPositionGroups.xls", StudentPractice.Portal.Config.ReportFilesDirectory);
            iRep.GetInternshipPositionGroupsAsReader((reader) =>
            {
                iExporter.ExportToFile(reader, filePath);
            });

            return filePath;
        }

        public static string CreateInternshipPositionsFile()
        {
            var iRep = new InternshipPositionRepository();
            var iExporter = new InternshipPositionsExporter();

            var filePath = string.Format("{0}\\InternshipPositions.xls", StudentPractice.Portal.Config.ReportFilesDirectory);
            iRep.GetInternshipPositionsAsReader((reader) =>
            {
                iExporter.ExportToFile(reader, filePath);
            });

            return filePath;
        }

        #endregion
    }
}