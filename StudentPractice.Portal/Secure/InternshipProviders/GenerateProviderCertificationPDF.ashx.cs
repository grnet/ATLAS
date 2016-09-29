using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.CacheManagerExtensions;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    /// <summary>
    /// Summary description for Provider Certification
    /// </summary>
    public class GenerateProviderCertificationPDF : IHttpHandler
    {

        IUnitOfWork _UnitOfWork = null;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_UnitOfWork == null)
                    _UnitOfWork = UnitOfWorkFactory.Create(); ;
                return _UnitOfWork;
            }
            set
            {
                _UnitOfWork = value;
            }
        }

        protected HttpContext Context { get; set; }
        protected InternshipProvider CurrentProvider { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            context.Response.Clear();
            LoadData();
            context.Response.ContentType = "application/octet-stream";
            Context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Provider-{0}-Certification.pdf", CurrentProvider.CertificationNumber));
            CreatePDF();
        }

        private void LoadData()
        {
            CurrentProvider = new InternshipProviderRepository(UnitOfWork).FindByUsername(Context.User.Identity.Name);
        }

        private void CreatePDF()
        {
            using (LocalReport lr = new LocalReport())
            {
                ConfigureReport(lr);
                //Proposal Label Has to Be LandScape
                string deviceInfo = @"<DeviceInfo>
            <OutputFormat>PDF</OutputFormat>
            <PageWidth>21cm</PageWidth>
            <PageHeight>29.7cm</PageHeight>
            <MarginTop>0.5in</MarginTop>
            <MarginLeft>0.0in</MarginLeft>
            <MarginRight>0.0in</MarginRight>
            <MarginBottom>0.5in</MarginBottom>
            </DeviceInfo>";

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                //var reportBytes = lr.Render("PDF", deviceInfo);
                Context.Response.BinaryWrite(renderedBytes);
            }
        }

        private void ConfigureReport(LocalReport localReport)
        {
            if (LanguageService.GetUserLanguage() == enLanguage.English)
                localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/ProviderCertificationEN.rdlc");
            else
                localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/ProviderCertification.rdlc");

            List<ReportParameter> parameters = new List<ReportParameter>();

            parameters.Add(new ReportParameter("CertificationNumber", string.Format("{0}: {1} / {2}", Resources.ProviderLiterals.CertificationNumber, CurrentProvider.CertificationNumber, CurrentProvider.CertificationDate.Value.ToString("dd-MM-yyyy"))));


            if (CurrentProvider.LegalPersonIdentificationType == enIdentificationType.ID)
            {
                parameters.Add(new ReportParameter("ProviderDetails", string.Format(Resources.ProviderLiterals.ProviderDetails1, CurrentProvider.Name, CurrentProvider.AFM, CurrentProvider.DOY, CurrentProvider.LegalPersonName, CurrentProvider.LegalPersonIdentificationNumber, CurrentProvider.LegalPersonIdentificationIssueDate, CurrentProvider.LegalPersonIdentificationIssuer, CurrentProvider.LegalPersonPhone, CurrentProvider.UserName)));
            }
            else
            {
                parameters.Add(new ReportParameter("ProviderDetails", string.Format(Resources.ProviderLiterals.ProviderDetails2, CurrentProvider.Name, CurrentProvider.AFM, CurrentProvider.DOY, CurrentProvider.LegalPersonName, CurrentProvider.LegalPersonIdentificationNumber, CurrentProvider.LegalPersonPhone, CurrentProvider.UserName)));
            }

            if (CurrentProvider.AlternateContactName != null)
            {
                parameters.Add(new ReportParameter("ContactPersonDetails", string.Format(Resources.ProviderLiterals.ContactPersonDetails1, CurrentProvider.ContactName, CurrentProvider.ContactEmail, CurrentProvider.ContactPhone, CurrentProvider.AlternateContactName, CurrentProvider.AlternateContactEmail, CurrentProvider.AlternateContactPhone)));
            }
            else
            {
                parameters.Add(new ReportParameter("ContactPersonDetails", string.Format(Resources.ProviderLiterals.ContactPersonDetails2, CurrentProvider.ContactName, CurrentProvider.ContactEmail, CurrentProvider.ContactPhone)));
            }

            if (CurrentProvider.LegalPersonIdentificationType == enIdentificationType.ID)
            {
                parameters.Add(new ReportParameter("IdentificationCopy", Resources.ProviderLiterals.IdentificationCopy1));
            }
            else
            {
                parameters.Add(new ReportParameter("IdentificationCopy", Resources.ProviderLiterals.IdentificationCopy2));
            }

            localReport.DataSources.Add(new ReportDataSource() { Name = "Dummy", Value = new List<Academic>() { new Academic() } });
            localReport.SetParameters(parameters);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}