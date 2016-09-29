using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Imis.Domain;
using StudentPractice.Utils;

namespace StudentPractice.BusinessModel
{
    public static class ServiceWorker
    {
        #region [ Public Update Methods ]

        public static void UpdateStudentInfoFromStudentCard(int studentID, bool async = false, bool useQueue = true)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAcademicIDService"]))
            {
                GenericUpdateMethod(studentID, UpdateStudentInfoFromStudentCardMethod, async, useQueue);
            }
        }

        #endregion

        #region [ Private Update Methods ]

        private static void UpdateStudentInfoFromStudentCardMethod(int studentID, bool useQueue)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                if (useQueue && QueueWorker.Current.IsInitialized)
                {
                    var tmpEntry = new QueueEntry() { QueueDataID = studentID, QueueEntryType = enQueueEntryType.UpdateStudentInfoFromStudentCard };
                    try
                    {
                        if (QueueWorker.IsStudentQueued(studentID, enQueueEntryType.UpdateStudentInfoFromStudentCard))
                            QueueWorker.Current.RemoveFromQueue(tmpEntry, enQueueEntryType.UpdateStudentInfoFromStudentCard);

                        BusinessHelper.UpdateStudentInfoFromStudentCard(uow, studentID);
                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex, typeof(ServiceWorker), "Failed to update student info. Adding to Queue.");
                        try
                        {
                            QueueWorker.Current.AddToQueue(tmpEntry.QueueDataID, ex.Message, enQueueEntryType.UpdateStudentInfoFromStudentCard);
                        }
                        catch (Exception ex2)
                        {
                            LogHelper.LogError(ex2, typeof(ServiceWorker), string.Format("Fail to add to queue studentApplication with ID : {0}", studentID.ToString()));
                        }
                    }
                }
                else
                {
                    try
                    {
                        BusinessHelper.UpdateStudentInfoFromStudentCard(uow, studentID);
                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError(ex, typeof(ServiceWorker), "Failed to update student info");
                    }
                }
            }
        }

        #endregion

        #region [ Helpers ]

        private class GenericMethodData
        {
            public int StudentID { get; set; }
            public bool UseQueue { get; set; }
        }

        private static void GenericUpdateMethod(int id, Action<int, bool> updateMethod, bool async, bool useQueue)
        {
            if (async)
            {
                if (!QueueWorker.Current.IsInitialized)
                    return;
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    var data = (GenericMethodData)x;
                    updateMethod(data.StudentID, data.UseQueue);
                }, new GenericMethodData()
                {
                    StudentID = id,
                    UseQueue = useQueue
                });
            }
            else
            {
                updateMethod(id, useQueue);
            }
        }

        #endregion
    }
}
