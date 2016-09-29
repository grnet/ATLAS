using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.CacheManagerExtensions;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    /// <summary>
    /// Summary description for Office Certification
    /// </summary>
    public class GenerateOfficeCertificationPDF : IHttpHandler
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
        protected InternshipOffice CurrentOffice { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            context.Response.Clear();
            LoadData();
            context.Response.ContentType = "application/octet-stream";
            Context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Office-{0}-Certification.pdf", CurrentOffice.CertificationNumber));
            CreatePDF();
        }

        private void LoadData()
        {
            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Context.User.Identity.Name, x => x.Academics);
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
            switch (CurrentOffice.OfficeType)
            {
                case enOfficeType.Institutional:
                    localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/OfficeCertification.rdlc");
                    break;
                case enOfficeType.Departmental:
                    localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/DepartmentCertification.rdlc");
                    break;
                case enOfficeType.MultipleDepartmental:
                    localReport.ReportPath = HttpContext.Current.Server.MapPath("~/_rdlc/MultipleDepartmentCertification.rdlc");
                    break;
                default:
                    break;
            }

            List<ReportParameter> parameters = new List<ReportParameter>();

            var institution = CacheManager.Institutions.Get(CurrentOffice.InstitutionID.Value);
            parameters.Add(new ReportParameter("CertificationNumber", string.Format("Αριθμός Βεβαίωσης: {0} / {1}", CurrentOffice.CertificationNumber, CurrentOffice.CertificationDate.Value.ToString("dd-MM-yyyy"))));
            parameters.Add(new ReportParameter("Institution", institution.Name));

            if (CurrentOffice.OfficeType != enOfficeType.Institutional)
            {
                string academics = string.Empty;

                foreach (var academic in CurrentOffice.Academics.OrderBy(x => x.Department))
                {
                    academics += academic.Department + ", ";
                }

                academics = academics.Substring(0, academics.LastIndexOf(", "));

                parameters.Add(new ReportParameter("Department", academics));
            }

            switch (CurrentOffice.CertifierType)
            {
                case enCertifierType.AEIPresident:
                    parameters.Add(new ReportParameter("CertifierType", "ΠΡΥΤΑΝΗ"));
                    break;
                case enCertifierType.TEIPresident:
                    parameters.Add(new ReportParameter("CertifierType", "ΠΡΟΕΔΡΟΥ ΙΔΡΥΜΑΤΟΣ"));
                    break;
                case enCertifierType.DepartmentPresident:
                    parameters.Add(new ReportParameter("CertifierType", "ΠΡΟΕΔΡΟΥ ΤΜΗΜΑΤΟΣ"));
                    break;
                case enCertifierType.DepartmentChief:
                    parameters.Add(new ReportParameter("CertifierType", "ΠΡΟΙΣΤΑΜΕΝΟΥ ΤΜΗΜΑΤΟΣ"));
                    break;
            }

            parameters.Add(new ReportParameter("CertifierName", CurrentOffice.CertifierName));

            if (CurrentOffice.OfficeType == enOfficeType.Institutional)
            {
                parameters.Add(new ReportParameter("OfficeDetails", string.Format("Βεβαιώνεται ότι το ως άνω Ίδρυμα συμμετέχει στο πρόγραμμα «ΑΤΛΑΣ - Ηλεκτρονική Υπηρεσία Πρακτικής Άσκησης Φοιτητών ΑΕΙ» με το username: <{0}>.", CurrentOffice.UserName)));
            }
            else if (CurrentOffice.OfficeType == enOfficeType.MultipleDepartmental)
            {
                parameters.Add(new ReportParameter("OfficeDetails", string.Format("Βεβαιώνεται ότι τα ως άνω Τμήματα συμμετέχουν στο πρόγραμμα «ΑΤΛΑΣ - Ηλεκτρονική Υπηρεσία Πρακτικής Άσκησης Φοιτητών ΑΕΙ» με το username: <{0}>.", CurrentOffice.UserName)));
            }
            else
            {
                parameters.Add(new ReportParameter("OfficeDetails", string.Format("Βεβαιώνεται ότι το ως άνω Τμήμα συμμετέχει στο πρόγραμμα «ΑΤΛΑΣ - Ηλεκτρονική Υπηρεσία Πρακτικής Άσκησης Φοιτητών ΑΕΙ» με το username: <{0}>.", CurrentOffice.UserName)));
            }


            if (CurrentOffice.AlternateContactName != null)
            {
                if (CurrentOffice.OfficeType == enOfficeType.Institutional)
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, το Ίδρυμα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>, τον/την οποίο/οποία αναπληρώνει ο/η <{3}> με e-mail <{4}> και τηλέφωνο <{5}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone, CurrentOffice.AlternateContactName, CurrentOffice.AlternateContactEmail, CurrentOffice.AlternateContactPhone)));
                }
                else if (CurrentOffice.OfficeType == enOfficeType.MultipleDepartmental)
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, τα Τμήματα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>, τον/την οποίο/οποία αναπληρώνει ο/η <{3}> με e-mail <{4}> και τηλέφωνο <{5}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone, CurrentOffice.AlternateContactName, CurrentOffice.AlternateContactEmail, CurrentOffice.AlternateContactPhone)));
                }
                else
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, το Τμήμα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>, τον/την οποίο/οποία αναπληρώνει ο/η <{3}> με e-mail <{4}> και τηλέφωνο <{5}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone, CurrentOffice.AlternateContactName, CurrentOffice.AlternateContactEmail, CurrentOffice.AlternateContactPhone)));
                }

            }
            else
            {
                if (CurrentOffice.OfficeType == enOfficeType.Institutional)
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, το Ίδρυμα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone)));
                }
                else if (CurrentOffice.OfficeType == enOfficeType.MultipleDepartmental)
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, τα Τμήματα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone)));
                }
                else
                {
                    parameters.Add(new ReportParameter("ContactPersonDetails", string.Format("Για τις ανάγκες του συγκεκριμένου προγράμματος, το Τμήμα εκπροσωπεί ο/η <{0}> με e-mail <{1}> και τηλέφωνο <{2}>.", CurrentOffice.ContactName, CurrentOffice.ContactEmail, CurrentOffice.ContactPhone)));
                }

            }

            switch (CurrentOffice.CertifierType)
            {
                case enCertifierType.AEIPresident:
                    parameters.Add(new ReportParameter("CertifierTypeFooter", "Πρύτανη"));
                    break;
                case enCertifierType.TEIPresident:
                    parameters.Add(new ReportParameter("CertifierTypeFooter", "Προέδρου Ιδρύματος"));
                    break;
                case enCertifierType.DepartmentPresident:
                    parameters.Add(new ReportParameter("CertifierTypeFooter", "Προέδρου Τμήματος"));
                    break;
                case enCertifierType.DepartmentChief:
                    parameters.Add(new ReportParameter("CertifierTypeFooter", "Προϊσταμένου Τμήματος"));
                    break;
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