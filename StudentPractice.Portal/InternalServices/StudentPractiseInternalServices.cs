using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using Imis.Domain;
using StudentPractice.BusinessModel;
using log4net;
using System.Net;

namespace StudentPractice.Portal.InternalServices
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class StudentPractiseInternalServices : IDisposable
    {
        private readonly IUnitOfWork _uow;

        public StudentPractiseInternalServices(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public StudentPractiseInternalServices() : this(UnitOfWorkFactory.Create()) { }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public ServiceResponse UpdateStudentAcademicIDNumber(AcademicIDCardRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.StudentNumber) || request.AcademicID <= 0 || string.IsNullOrWhiteSpace(request.NewAcademicIDNumber))
                {
                    string message = "Δεν στείλατε τις σωστές παραμέτρους κατά την κλήση του Web Service";
                    InternalServiceLogger.Log(enServiceCaller.StudentCard, "UpdateStudentAcademicIDNumber", request.ServiceCallerID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString());
                    return Failure(message);
                }
                else
                {
                    var student = new StudentRepository(_uow).FindActiveByStudentNumberAndAcademicID(request.StudentNumber, request.AcademicID);

                    if (student == null)
                    {
                        string message = "Τα στοιχεία που δηλώσατε δεν είναι έγκυρα";
                        InternalServiceLogger.Log(enServiceCaller.StudentCard, "UpdateStudentAcademicIDNumber", request.ServiceCallerID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString());
                        return Failure(message);
                    }
                    else
                    {
                        student.AcademicIDNumber = request.NewAcademicIDNumber;
                        student.AcademicIDStatus = (enAcademicIDApplicationStatus)request.AcademicIDStatus;
                        student.AcademicIDSubmissionDate = request.AcademicIDSubmissionDate;
                        _uow.Commit();
                        return Success();
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex);
                return Failure(ex.Message);
            }
        }

        private ServiceResponse Success()
        {
            return new ServiceResponse
            {

                Success = true
            };
        }

        private ServiceResponse Failure(string message = "")
        {
            return new ServiceResponse
            {
                Success = false,
                Message = message
            };
        }

        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}