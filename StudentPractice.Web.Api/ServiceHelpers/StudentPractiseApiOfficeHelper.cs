using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Security.Principal;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Security;
using Imis.Domain;
using StudentPractice.Utils;
using log4net;
using StudentPractice.Mails;
using StudentPractice.BusinessModel;
using StudentPractice.BusinessModel.Flow;
using System.IO;


namespace StudentPractice.Web.Api
{
    public partial class StudentPractiseApiOffice : StudentPractiseApiBase
    {
        public InternshipOffice OfficeCaller;

        private AuthenticationResult BuildAuthenticationResult(InternshipOffice office)
        {
            AuthenticationResult result = new AuthenticationResult();
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, office.UserName, DateTime.Now, DateTime.Now.AddDays(ApiConfig.Current.TicketExpiration), true, "");
            result.AuthToken = FormsAuthentication.Encrypt(ticket);
            return result;
        }

        private bool CanAccessServices(InternshipOffice office)
        {
            bool isApproved = office.IsApiAppoved.GetValueOrDefault();
            if (!office.IsMasterAccount && office.MasterAccountID.HasValue)
            {
                var masterAccount = new InternshipOfficeRepository().Load(office.MasterAccountID.Value);
                isApproved = masterAccount.IsApiAppoved.GetValueOrDefault();
            }
            return isApproved && office.VerificationStatus == enVerificationStatus.Verified;
        }

        protected override void UserValidation(string username)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
            OfficeCaller = new InternshipOfficeRepository().FindByUsername(username, x => x.Academics);
            
            if (OfficeCaller == null)                
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            ApiCaller.ID = OfficeCaller.ID;
        }       
    }
}
