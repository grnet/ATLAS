using System;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Utils;
using StudentPractice.Web.Api;
using WebFormsMvp.Autofac;
using WebFormsMvp.Binder;
using StudentPractice.Utils.Worker;
using System.Reflection;
using StudentPractice.Portal.InternalServices;
using System.Threading;
using System.Web.Security;

namespace StudentPractice.Portal
{
    public class Global : HttpApplication
    {
        public static string VersionNumber { get; private set; }
        protected void Application_Start(object sender, EventArgs e)
        {
            VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            log4net.Config.XmlConfigurator.Configure();

            QueueWorker.Inititalize();

            if (Config.EnableAPI)
                RouteTable.Routes.Add(new ServiceRoute("Api/Offices/v1", new WebServiceHostFactory(), typeof(StudentPractiseApiOffice)));
            RouteTable.Routes.Add(new ServiceRoute("InternalServices", new WebServiceHostFactory(), typeof(StudentPractiseInternalServices)));
            AsyncWorker.Instance.Items.Add(WorkerActions.UpdateStatisticsByDay());
            AsyncWorker.Instance.Items.Add(WorkerActions.CheckBlockedPositions());
            AsyncWorker.Instance.Items.Add(WorkerActions.CheckPreAssignedPositions());
            AsyncWorker.Instance.Items.Add(WorkerActions.CheckAssignedPositions());
            AsyncWorker.Instance.Items.Add(WorkerActions.GenerateReportFiles());
            AsyncWorker.Initialize();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                AsyncWorker.Instance.Dispose();
            }
            catch (Exception)
            {
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
                LogHelper.LogError(e.ExceptionObject as Exception, "Global", string.Format("Unhandler Exception. Is Application Terminating:{0}", e.IsTerminating));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception error = Server.GetLastError();

            if (error != null)
            {
                LogHelper.LogError(error, "Global");
            }
            else if (HttpContext.Current == null || HttpContext.Current.Error == null)
            {
                LogHelper.LogError(new Exception("Unknown Error - No Context"), "Global");
            }
            else
            {
                LogHelper.LogError(new Exception("Unknown error"), "Global");
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //LanguageService.InitUserLanguage();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var sessionCookieKey = Response.Cookies.AllKeys.SingleOrDefault(c => c.ToLower() == "asp.net_sessionid");
            var sessionCookie = Response.Cookies.Get(sessionCookieKey);
            if (sessionCookie != null)
            {
                //sessionCookie.Secure = true;
                sessionCookie.HttpOnly = true;
            }

            if (Request.Url.AbsolutePath.ToLower().Contains("api/offices/v1"))
            {
                Response.Cookies.Clear();

                if (HttpContext.Current.Items.Contains("APIUnauthorized"))
                {
                    Response.StatusDescription = "Invalid access token";
                    Response.StatusCode = 401;
                }
            }
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            LanguageService.InitUserLanguage();

            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var data = AuthenticationService.GetUserData();
                if (data.ReporterID == 0 && string.IsNullOrEmpty(data.ContactName))
                {
                    AuthenticationService.LoginReporter(Thread.CurrentPrincipal.Identity.Name);
                    data = AuthenticationService.GetUserData();
                }

                if (!AuthenticationService.IsCookieValid())
                {
                    AuthenticationService.InvalidateCookie(Thread.CurrentPrincipal.Identity.Name, false);
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }

                var newIdentity = new StudentPracticeIdentity(Thread.CurrentPrincipal.Identity.Name, Thread.CurrentPrincipal.Identity.AuthenticationType)
                {
                    ReporterID = data.ReporterID,
                    ContactName = data.ContactName
                };

                var newPrincipal = new StudentPracticePrincipal(newIdentity, Roles.GetRolesForUser(Thread.CurrentPrincipal.Identity.Name)) { Identity = newIdentity };

                Thread.CurrentPrincipal = newPrincipal;
                HttpContext.Current.User = newPrincipal;
            }
        }
    }
}