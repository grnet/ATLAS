using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Imis.Domain;
using StudentPractice.Queue;

namespace StudentPractice.BusinessModel
{
    public class QueueWorker : IQueueWorker
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static QueueWorker Current
        {
            get
            {
                return Nested.dispatcher;
            }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly QueueWorker dispatcher = new QueueWorker();
        }

        #endregion

        internal bool IsInitialized { get; set; }

        public static void Inititalize()
        {
            QueueWorker.Current.IsInitialized = true;
            ServiceQueue.Instance.Initialize(Current);
        }

        public static bool IsStudentQueued(int studentID, enQueueEntryType type)
        {
            return new QueueEntryRepository().LoadAll().Any(x => x.QueueDataID == studentID && x.QueueEntryTypeInt == (int)type);
        }

        void IQueueWorker.AddToQueue(IQueueEntry entry) { throw new NotImplementedException(); }

        void IQueueWorker.RemoveFromQueue(IQueueEntry entry) { throw new NotImplementedException(); }

        public void AddToQueue(int studentID, string message, enQueueEntryType type)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var entry = new QueueEntry();
                entry.QueueDataID = studentID;
                entry.NoOfRetries = 0;
                entry.MaxNoOfRetries = ServiceQueue.Instance.Config.MaxNoOfRetries;
                entry.LastAttemptDate = DateTime.Now;
                entry.QueueEntryType = type;
                ((IQueueWorker)this).SetQueueData(entry, new GenericQueueDataCollection() { new GenericQueueData() { 
                    NoOfRetry = 0, 
                    Message = message, 
                    ServerName = Environment.MachineName                    
                } });
                uow.MarkAsNew(entry);
                uow.Commit();
            }
        }

        public void RemoveFromQueue(IQueueEntry entry)
        {
            using (DBEntities uow = new DBEntities())
            {
                var dbEntry = new QueueEntryRepository(uow).LoadAll().FirstOrDefault(x => x.QueueDataID == (int)entry.QueueDataID && x.QueueEntryTypeInt == entry.QueueEntryTypeInt);
                if (dbEntry != null)
                {
                    uow.MarkAsDeleted(dbEntry);
                    uow.SaveChanges();
                }
            }
        }

        public void RemoveFromQueue(IQueueEntry entry, enQueueEntryType type)
        {
            using (DBEntities uow = new DBEntities())
            {
                var dbEntry = new QueueEntryRepository(uow).LoadAll().FirstOrDefault(x => x.QueueDataID == (int)entry.QueueDataID && x.QueueEntryTypeInt == (int)type);
                if (dbEntry != null)
                {
                    uow.MarkAsDeleted(dbEntry);
                    uow.SaveChanges();
                }
            }
        }

        T IQueueWorker.GetQueueData<T>(IQueueEntry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.QueueDataXml))
                return default(T);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(new StringReader(entry.QueueDataXml));
        }

        void IQueueWorker.SetQueueData(IQueueEntry queueEntry, object queueData)
        {
            XmlSerializer xs = new XmlSerializer(queueData.GetType());
            StringBuilder sb = new StringBuilder();
            xs.Serialize(XmlTextWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true }), queueData);
            queueEntry.QueueDataXml = sb.ToString();
        }

        public void ProcessQueueEntry(IQueueEntry entry)
        {
            if (entry.MaxNoOfRetries == entry.NoOfRetries)
                return;

            using (DBEntities uow = new DBEntities())
            {
                var e = uow.QueueEntries.Single(x => x.QueueDataID == (int)entry.QueueDataID && x.QueueEntryTypeInt == entry.QueueEntryTypeInt);
                try
                {
                    var data = ((IQueueWorker)this).GetQueueData<GenericQueueDataCollection>(e);

                    switch (e.QueueEntryType)
                    {
                        case enQueueEntryType.UpdateStudentInfoFromStudentCard:
                            BusinessHelper.UpdateStudentInfoFromStudentCard(uow, e.QueueDataID);
                            break;
                        default:
                            break;
                    }

                    uow.QueueEntries.DeleteObject(e);
                }
                catch (Exception ex)
                {
                    e.NoOfRetries++;
                    e.LastAttemptDate = DateTime.Now;
                    var data = ((IQueueWorker)this).GetQueueData<GenericQueueDataCollection>(e);
                    data.Add(new GenericQueueData()
                    {
                        NoOfRetry = e.NoOfRetries,
                        Message = ex.Message,
                        ServerName = Environment.MachineName
                    });
                    ((IQueueWorker)this).SetQueueData(e, data);
                }
                uow.SaveChanges();
            }
        }

        bool QueueEntryExists(int queueDataID, int type)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                return new QueueEntryRepository(uow).LoadAll().Any(x => x.QueueDataID == queueDataID && x.QueueEntryTypeInt == type);
            }
        }

        public Student GetStudentForUpdate(IQueueEntry queueEntry, bool checkIfExistsInQueue)
        {
            if (checkIfExistsInQueue)
            {
                if (QueueEntryExists((int)queueEntry.QueueDataID, queueEntry.QueueEntryTypeInt))
                    return new StudentRepository().Load((int)queueEntry.QueueDataID);
                else
                    return null;
            }
            else
            {
                return new StudentRepository().Load((int)queueEntry.QueueDataID);
            }
        }

        public void ProcessQueue(Action<bool> callback)
        {
            bool queueProcessed = new QueueEntryRepository().LoadAll().Any(x => x.NoOfRetries < x.MaxNoOfRetries);

            if (queueProcessed)
            {
                var entries = new QueueEntryRepository().LoadAll().Where(x => x.NoOfRetries < x.MaxNoOfRetries);
                foreach (var entry in entries)
                {
                    ProcessQueueEntry(entry);
                }

            }

            callback.Invoke(queueProcessed);
        }
    }
}
