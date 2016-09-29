using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.Profile;
using StudentPractice.BusinessModel;
using log4net;

namespace StudentPractice.Mails
{
    public static class MailSender
    {
        private static readonly ILog log = LogManager.GetLogger("MailSender");

        /// <summary>
        /// When true, no mails are sent, they are only logged.
        /// </summary>
        public static bool DebugMode = false;

        /// <summary>
        /// Replaces all possible variables in mail body templates.
        /// </summary>
        /// <param name="body">The body of the Message</param>
        /// <param name="values">A dictionary with the values to replace. ex: Key: Username,Value: "djsolid" </param>
        /// <returns></returns>
        public static string ReplaceVars(string body, Dictionary<string, string> values)
        {
            string bodyReplaced = body;
            foreach (var value in values)
            {
                bodyReplaced = bodyReplaced.Replace(string.Format("%{0}%", value.Key.ToUpper()), value.Value);
            }
            return bodyReplaced;
        }

        /// <summary>
        /// Sends mail
        /// </summary>
        /// <param name="From">Sender e-mail address</param>
        /// <param name="To">Receiver e-mail address</param>
        /// <param name="Subject">E-mail subject</param>
        /// <param name="Body">E-mail body</param>
        private static void Send(string from, string to, string subject, string body, string footer, bool htmlBody)
        {
            try
            {
                if (string.IsNullOrEmpty(from))
                    throw new ArgumentException("Will not send email from invalid address");
                else if (string.IsNullOrEmpty(to))
                    throw new ArgumentException("Will not send email to invalid address");
                else
                {
                    if (DebugMode)
                    {
                        log.InfoFormat("From {0}, to {1}, subject {2}, body {3}", from, to, subject, body + footer);
                    }
                    else
                    {
                        MailMessage m = new MailMessage(from, to, subject, body + footer);
                        SmtpClient sc = new SmtpClient();

                        m.IsBodyHtml = htmlBody;

                        sc.Send(m);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private static void Send(string to, string subject, string body, string footer, bool htmlBody)
        {
            body = body.Replace("«", "\"").Replace("»", "\"").Replace("&#171;", "\"").Replace("&#187;", "\"");
            Send(GetNoReplyMail(), to, subject, body, footer, htmlBody);
        }

        public static void SendCustomMessage(string to, string subject, string message, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_CustomMessage, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", subject);

            MailSender.Send(to, subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);
        }

        public static Email SendCustomMessageToReporter(int reporterID, string to, string subject, string message, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_CustomMessage, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("message", message);
            values.Add("subject", subject);

            MailSender.Send(to, subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.CustomMessage,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendIncidentReportSubmitConfirmation(int reporterID, string to, string question, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_IncidentReportSubmitConfirmation, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("reportText", question);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.IncidentReportSubmitConfirmation,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendIncidentReportAnswer(int reporterID, string to, string reportID, string question, string answer, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_IncidentReportAnswer, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("reportID", reportID);
            values.Add("reportText", question);
            values.Add("reportAnswer", answer);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.IncidentReportAnswer,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendEmailVerification(int reporterID, string to, string name, Uri uri, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_EmailVerification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("link", uri.ToString());
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.EmailVerification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendProviderUserEmailVerification(int reporterID, string to, string name, Uri uri, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_ProviderUserEmailVerification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("link", uri.ToString());
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.EmailVerification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendOfficeUserEmailEmailVerification(int reporterID, string to, string name, Uri uri, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_OfficeUserEmailVerification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("link", uri.ToString());
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.EmailVerification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendForgotPassword(int reporterID, string to, string username, string password, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_ForgotPassword, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", username);
            values.Add("password", password);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.ForgotPassword,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendProviderVerification(int reporterID, string to, string username, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_ProviderVerification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", username);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.ProviderVerification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendOfficeVerification(int reporterID, string to, string username, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_OfficeVerification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", username);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.ProviderVerification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendCompletedPositionStudentNotification(int reporterID, string to, string name, int positionID, string positionTitle, string providerName, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_CompletedPositionStudentNotification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("positionID", positionID.ToString());
            values.Add("positionTitle", positionTitle);
            values.Add("providerName", providerName);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.CompletedPositionStudentNotification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendAssignedPositionStudentNotification(int reporterID, string to, string name, int positionID, string positionTitle, string providerName, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_AssignedPositionStudentNotification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("name", name);
            values.Add("positionID", positionID.ToString());
            values.Add("positionTitle", positionTitle);
            values.Add("providerName", providerName);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.AssignedPositionStudentNotification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendAssignedPositionProviderNotification(int reporterID, string to, string username, int positionID, string positionTitle, string studentName, string institution, string school, string department, string studentNumber, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_AssignedPositionProviderNotification, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", username);
            values.Add("positionID", positionID.ToString());
            values.Add("positionTitle", positionTitle);
            values.Add("studentName", studentName);
            values.Add("institution", institution);
            values.Add("school", school);
            values.Add("department", department);
            values.Add("studentNumber", studentNumber);
            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.AssignedPositionProviderNotification,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static Email SendNewlyPublishedPositions(int reporterID, string to, string username, int positionCount, enLanguage lang = enLanguage.Greek)
        {
            MailDetails mailDetails = MailDetailsReader.GetMailDetails(AV_NewlyPublishedPositions, lang);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("username", username);

            if (positionCount == 1)
            {
                values.Add("positionCount", string.Format("Θα θέλαμε να σας ενημερώσουμε ότι προστέθηκε στην εφαρμογή {0} Θέση Πρακτικής Άσκησης που μπορεί να προδεσμευτεί από το Γραφείο σας.", positionCount));
            }
            else
            {
                values.Add("positionCount", string.Format("Θα θέλαμε να σας ενημερώσουμε ότι προστέθηκαν στην εφαρμογή {0} Θέσεις Πρακτικής Άσκησης που μπορούν να προδεσμευτούν από το Γραφείο σας.", positionCount));
            }

            MailSender.Send(to, mailDetails.Subject, ReplaceVars(mailDetails.Body, values), MailDetailsReader.GetMailFooter(lang), false);

            Email email = new Email()
            {
                ReporterID = reporterID,
                Type = (int)enEmailType.NewlyPublishedPositions,
                SentAt = DateTime.Now,
                EmailAddress = to,
                Subject = mailDetails.Subject,
                Body = ReplaceVars(mailDetails.Body, values)
            };

            return email;
        }

        public static string GetNoReplyMail()
        {
            return "no-reply@minedu.gov.gr";
        }

        #region XML emails configuration

        // Element names in XML file
        private static readonly string E_Mail = "Mail";
        private static readonly string E_Subject = "Subject";
        private static readonly string E_Body = "Body";
        private static readonly string E_Footer = "Footer";
        // Attribute keys in XML file
        private static readonly string AK_Category = "Category";
        // Attribute values in XML file        
        private static readonly string AV_CustomMessage = "CustomMessage";
        private static readonly string AV_IncidentReportSubmitConfirmation = "IncidentReportSubmitConfirmation";
        private static readonly string AV_IncidentReportAnswer = "IncidentReportAnswer";
        private static readonly string AV_EmailVerification = "EmailVerification";
        private static readonly string AV_OfficeUserEmailVerification = "OfficeUserEmailVerification";
        private static readonly string AV_ProviderUserEmailVerification = "ProviderUserEmailVerification";
        private static readonly string AV_ForgotPassword = "ForgotPassword";
        private static readonly string AV_ProviderVerification = "ProviderVerification";
        private static readonly string AV_OfficeVerification = "OfficeVerification";
        private static readonly string AV_CompletedPositionStudentNotification = "CompletedPositionStudentNotification";
        private static readonly string AV_AssignedPositionStudentNotification = "AssignedPositionStudentNotification";
        private static readonly string AV_AssignedPositionProviderNotification = "AssignedPositionProviderNotification";
        private static readonly string AV_NewlyPublishedPositions = "NewlyPublishedPositions";

        private class MailDetails
        {
            public string Subject;
            public string Body;
        }

        private static class MailDetailsReader
        {
            private static XElement cachedMailsDetails = null;
            private static XElement CachedMailsDetails
            {
                get
                {
                    if (cachedMailsDetails == null)
                    {
                        if (HttpContext.Current == null)
                            cachedMailsDetails = XElement.Parse(File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Mails.xml")));
                        else
                            cachedMailsDetails = XElement.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/Mails.xml")));
                    }
                    return cachedMailsDetails;
                }
            }

            public static MailDetails GetMailDetails(string category, enLanguage lang)
            {
                MailDetails md = null;
                if (lang == enLanguage.English)
                {
                    var enCategory = category + "_EN";
                    md = (from z in CachedMailsDetails.Descendants(E_Mail)
                          where (string)z.Attributes(AK_Category).Single() == enCategory
                          select new MailDetails()
                          {
                              Subject = z.Descendants(E_Subject).Single().Value,
                              Body = z.Descendants(E_Body).Single().Value
                          }).Single();
                }
                return md ?? (from z in CachedMailsDetails.Descendants(E_Mail)
                              where (string)z.Attributes(AK_Category).Single() == category
                              select new MailDetails()
                              {
                                  Subject = z.Descendants(E_Subject).Single().Value,
                                  Body = z.Descendants(E_Body).Single().Value
                              }).Single();
            }

            public static string GetMailFooter(enLanguage lang)
            {
                if (lang == enLanguage.English)
                    return (from z in CachedMailsDetails.Elements(E_Footer + "_EN")
                            select ((XCData)z.FirstNode).Value).Single();
                else
                    return (from z in CachedMailsDetails.Elements(E_Footer)
                            select ((XCData)z.FirstNode).Value).Single();
            }
        }

        #endregion
    }
}
