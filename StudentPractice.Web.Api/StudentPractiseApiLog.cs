using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using StudentPractice.Utils;
using StudentPractice.BusinessModel;
using Imis.Domain;

namespace StudentPractice.Web.Api
{
    class StudentPractiseApiLog
    {
        public static void Log(enServiceCaller serviceCaller, string serviceMethodCalled, int serviceCallerID, string errorCode, bool success, string request, string ip)
        {
            if (ApiConfig.Current.TypeOfLogging == "Full" || (ApiConfig.Current.TypeOfLogging == "FailsOnly" && success == false))
            {
                ThreadPool.QueueUserWorkItem(response =>
                {
                    try
                    {
                        using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                        {
                            StudentPracticeApiLog logEntry = new StudentPracticeApiLog();

                            logEntry.ServiceCaller = (int)serviceCaller;
                            logEntry.ServiceCalledAt = DateTime.Now;
                            logEntry.ServiceMethodCalled = serviceMethodCalled;
                            logEntry.ServiceCallerID = serviceCallerID;
                            logEntry.ErrorCode = errorCode;
                            logEntry.Success = success;
                            logEntry.Request = request;
                            logEntry.IP = ip;

                            uow.MarkAsNew(logEntry);
                            uow.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError<StudentPractiseApiLog>(ex, null, string.Format("Error while saving internal service response for submissionCode {0}", errorCode));
                    }
                }, null);
            }
        }
    }
}
