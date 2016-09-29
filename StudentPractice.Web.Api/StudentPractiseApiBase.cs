using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Security.Principal;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Net;
using System.Web;
using System.Web.Security;
using Imis.Domain;
using StudentPractice.Utils;
using log4net;
using StudentPractice.Mails;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public abstract class StudentPractiseApiBase
    {
        protected ServiceCallerDetails ApiCaller { get { return _caller; } }
        private readonly ServiceCallerDetails _caller = new ServiceCallerDetails();

        protected StudentPractiseApiBase()
        {
            _reporterRepository = new Lazy<ReporterRepository>(() => new ReporterRepository(UnitOfWork));
            _officeRepository = new Lazy<InternshipOfficeRepository>(() => new InternshipOfficeRepository(UnitOfWork));
            _positionGroupRepository = new Lazy<InternshipPositionGroupRepository>(() => new InternshipPositionGroupRepository(UnitOfWork));
            _positionRepository = new Lazy<InternshipPositionRepository>(() => new InternshipPositionRepository(UnitOfWork));
            _providerRepository = new Lazy<InternshipProviderRepository>(() => new InternshipProviderRepository(UnitOfWork));
            _studentRepository = new Lazy<StudentRepository>(() => new StudentRepository(UnitOfWork));
        }

        protected readonly Lazy<ReporterRepository> _reporterRepository;
        protected readonly Lazy<InternshipOfficeRepository> _officeRepository;
        protected readonly Lazy<InternshipPositionGroupRepository> _positionGroupRepository;
        protected readonly Lazy<InternshipPositionRepository> _positionRepository;
        protected readonly Lazy<InternshipProviderRepository> _providerRepository;
        protected readonly Lazy<StudentRepository> _studentRepository;

        private IUnitOfWork _UnitOfWork = null;
        protected IUnitOfWork UnitOfWork
        {
            get
            {
                if (_UnitOfWork == null)
                    _UnitOfWork = UnitOfWorkFactory.Create();
                return _UnitOfWork;
            }
        }

        #region [RequestValidation]

        protected virtual void Validate(bool checkActive = false)
        {
            var headers = HttpContext.Current.Request.Headers;
            var decryptedCookie = (FormsAuthenticationTicket)null;

            if (!string.IsNullOrWhiteSpace(headers["access_token"]))
                ApiCaller.AccessToken = headers["access_token"];
            else
                throw new WebFaultException(HttpStatusCode.BadRequest);

            try
            {
                decryptedCookie = FormsAuthentication.Decrypt(ApiCaller.AccessToken.Replace("\"", ""));
            }
            catch (Exception)
            {
                HttpContext.Current.Items["APIUnauthorized"] = true;
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
            if (decryptedCookie == null)
            {
                HttpContext.Current.Items["APIUnauthorized"] = true;
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }

            UserValidation(decryptedCookie.Name);
        }

        protected virtual void UserValidation(string username)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
            var reporter = _reporterRepository.Value.FindByUsername(username);
            if (reporter != null)
                ApiCaller.ID = reporter.ID;
            else
            {
                HttpContext.Current.Items["APIUnauthorized"] = true;
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }

        #endregion

        #region [ErrorLogging]

        protected void LogException(Exception ex)
        {
            if (ex != null)
                LogHelper.LogError(ex, typeof(StudentPractiseApiOffice), ex.Message);

            else if (HttpContext.Current == null || HttpContext.Current.Error == null)
                LogHelper.LogError(new Exception("Unknown Error - No Context"), typeof(StudentPractiseApiOffice));

            else
                LogHelper.LogError(new Exception("Unknown error"), typeof(StudentPractiseApiOffice));
        }

        protected ServiceResponse UnhandledException(Exception ex, string srvName, int callerID)
        {
            LogException(ex);
            StudentPractiseApiLog.Log(enServiceCaller.Office, srvName, callerID, enErrorCode.InternalServerError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
            return Failure(ErrorMessages.InternalServerError, enErrorCode.InternalServerError);
        }

        protected ServiceResponse<T> UnhandledException<T>(Exception ex, string srvName, int callerID)
        {
            LogException(ex);
            StudentPractiseApiLog.Log(enServiceCaller.Office, srvName, callerID, enErrorCode.InternalServerError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
            return Failure<T>(ErrorMessages.InternalServerError, enErrorCode.InternalServerError);
        }

        #endregion

        #region [Results]

        protected ServiceResponse Success()
        {
            return new ServiceResponse(true);
        }

        protected ServiceResponse Failure(string message, enErrorCode errorcode)
        {
            return new ServiceResponse(false, message, (int)errorcode);
        }

        protected ServiceResponse<T> Success<T>(T value)
        {
            return new ServiceResponse<T>(true, value);
        }

        protected ServiceResponse<T> Failure<T>(string message, enErrorCode errorcode)
        {
            return new ServiceResponse<T>(false, default(T), message, (int)errorcode); ;
        }

        #endregion
    }
}
