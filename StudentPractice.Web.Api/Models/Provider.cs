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
    public class ProviderResult
    {
        public int ID { get; set; }
        public string AFM { get; set; }
        public string Name { get; set; }

        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
    }

    public static class Provider
    {
        public static ProviderResult ToProviderResult(this InternshipProvider model)
        {
            return new ProviderResult()
            {
                ID = model.ID,
                AFM = model.AFM,
                Name = model.Name,
                

                ContactName = model.ContactName,
                ContactPhone = model.ContactPhone,
                ContactEmail = model.ContactEmail
            };
        }
    }
}
