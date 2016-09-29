using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;
using System.Web;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Threading;
using System.Web.Hosting;
using Imis.Domain;
using StudentPractice.BusinessModel;
using log4net;

namespace StudentPractice.Mails
{
    public static class SMSSender
    {

        private static readonly ILog log = LogManager.GetLogger("SMSSender");

        /// <summary>
        /// When true, no SMS are sent, they are only logged.
        /// </summary>
        public static bool DebugMode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["EnableSMS"] != "true";
            }
        }

        #region Private methods for constructing and sending SMS messages

        private static void Send(string msg, string recipID, string recipNumber, string[] fieldValues)
        {
            Dictionary<string, string> recipients = new Dictionary<string, string>();
            recipients.Add(recipID, recipNumber);
            List<string[]> fieldValuesList = null;
            if (fieldValues != null && fieldValues.Length > 0)
            {
                fieldValuesList = new List<string[]>();
                fieldValuesList.Add(fieldValues);
            }
            Send(msg, recipients, fieldValuesList);
        }

        #endregion

        #region LocoTel SMS service

        public static void Send(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList)
        {
            //Perform the necessary tests. We don't want to waste money on invalid SMSs, right?
            if (String.IsNullOrEmpty(msg))
                throw new ArgumentException("Cannot send empty SMS");
            if (recipients == null || recipients.Count == 0)
                throw new ArgumentException("Cannot send SMS because no recipients were defined");
            if (fieldValuesList != null && fieldValuesList.Count != recipients.Count)
                throw new ArgumentException("The list of field values must have as many items as the list of recipients");

            MatchCollection matches = Regex.Matches(msg, "#field.#");
            //Validate the length of the SMS. No longer than 160
            int baseLength = Regex.Replace(msg, "#field.#", String.Empty).Length;
            if (baseLength > 160)
                throw new ArgumentException("The base length of the SMS cannot surpass 160 characters. It now is : " + baseLength);
            if (matches.Count > 0)
                foreach (var fieldValues in fieldValuesList)
                {
                    int smsLength = baseLength;
                    foreach (var field in fieldValues)
                        smsLength += field.Length;
                    if (smsLength > 160)
                        throw new ArgumentException("The resulting length of the SMS cannot surpass 160 characters. An sms has length of " + smsLength);
                }

            if (fieldValuesList != null)
            {
                foreach (var fieldValues in fieldValuesList)
                    if ((fieldValues == null && matches.Count > 0) || (fieldValues.Length != matches.Count))
                        throw new ArgumentException("The number of field values for at least one recipient is different than the number of fields in the message");
            }
            else
                if (matches.Count > 0)
                    throw new ArgumentException("Cannot send null fieldValuesList when the message has field values");

            log.Debug("Before actually calling the CallSendService method");
            CallSendService(msg, recipients, fieldValuesList, matches);
        }


        /// <summary>
        /// For Locotel
        /// </summary>
        private static XDocument ConstructSmsXml(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList, int totalFields)
        {
            log.Debug("Starting the Xml construction method");
            //Create an XDocument and add the basic elements
            XDocument xd = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("msg",
                    new XElement("username", new XText(ConfigurationManager.AppSettings["LocotelUsername"])),
                    new XElement("password", new XText(ConfigurationManager.AppSettings["LocotelPassword"])),
                    new XElement("text", new XText(msg)),
                    new XElement("totalfields", new XText(totalFields.ToString()))
                )
            );

            for (int i = 0; i < recipients.Count; i++)
            {
                var recipient = recipients.ElementAt(i);

                //Add an element for each recipient and also add sub-elements fir uid and msisdn
                xd.Element("msg").Add(new XElement("recipient",
                    new XElement("uid", new XText(recipient.Key)),
                    new XElement("msisdn", new XText(recipient.Value)),
                    new XElement("mobile", new XText("ATLAS"))));

                //Add the values for fields for the recipient just added
                if (fieldValuesList != null && fieldValuesList.Count > 0)
                    for (int j = 1; j <= fieldValuesList[i].Length; j++)
                        ((XElement)xd.Element("msg").LastNode).Add(new XElement("field" + j, new XText(fieldValuesList[i][j - 1])));
            }
            log.Debug("Xml Constructed");
            return xd;
        }

        /// <summary>
        /// For Locotel
        /// </summary>
        private static void CallSendService(string msg, IDictionary<string, string> recipients, IList<string[]> fieldValuesList, MatchCollection matches)
        {
            string url = "https://www.locosms.gr/xmlsend.php";

            XDocument xd = ConstructSmsXml(msg, recipients, fieldValuesList, matches.Count);

            log.Debug("Before constructing the post to the webpage to be sent");
            if (!DebugMode)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "text/xml";
                request.Method = "POST";

                string data = xd.Declaration.ToString() + xd.ToString(SaveOptions.DisableFormatting);

                byte[] postData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = postData.Length;

                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    requestStream.Close();

                    log.Debug("Before doing the post");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    string strResponse = new StreamReader(responseStream).ReadToEnd();
                    XDocument xdStatus = (XDocument)XDocument.Parse(strResponse.Substring(strResponse.IndexOf('<')));

                    //Before parsing the response, check that it is correct
                    if (xdStatus.Element("results") == null || xdStatus.Element("results").Element("status") == null ||
                        (xdStatus.Element("results").Element("status").Value != "OK" && xdStatus.Element("results").Element("status").Value != "FAIL"))
                    {
                        log.ErrorFormat("The response from the SMS service was not the expected. Response : {0}", xdStatus.ToString(SaveOptions.DisableFormatting));
                        throw new ApplicationException("The response from the SMS service was not the expected");
                    }

                    if (xdStatus.Element("results").Element("status").Value == "FAIL")
                    {
                        log.ErrorFormat("Received FAIL status for SMS sending. Reason : [{0}].", (xdStatus.Element("results").Element("reason") != null ? xdStatus.Element("results").Element("reason").Value : "NO REASON WAS GIVEN"));
                        throw new ApplicationException("The SMS service failed in sending the SMS. Reason : " + (xdStatus.Element("results").Element("reason") != null ? xdStatus.Element("results").Element("reason").Value : "NO REASON WAS GIVEN"));
                    }

                    responseStream.Close();
                }
                catch (WebException wex)
                {
                    //Log the exception and rethrow it
                    log.Error("Failure sending the SMS : [" + xd.ToString(SaveOptions.DisableFormatting) + "]", wex);
                    throw;
                }
            }
        }

        #endregion

        #region Methods for sending SMSs to Customers

        public static int maxFullNameLength = 25;
        public static int maxProviderNameLength = 28;

        public static void SendCustomMessage(Student student, string customMessage)
        {
            //Declare and initialize the necessary values for the SMS sending
            string message = SMSDetailsReader.GetSMSDetails(AV_CustomMessage).Message;

            if (String.IsNullOrEmpty(student.ContactMobilePhone) || student.ContactMobilePhone.Length != 10)
                throw new ArgumentException("The student with ID : " + student.ID + " does not have a valid mobile phone number");

            SMS sms = new SMS()
            {
                SendID = student.ID.ToString().AddZeroPadding(8) + DateTime.Now.ToString("yyMMddhhmmss") + Convert.ToInt32(enSMSType.CustomMessage),
                Reporter = student,
                ReporterNumber = "30" + student.ContactMobilePhone,
                Type = (int)enSMSType.CustomMessage,
                Msg = message,
                FieldValues = new string[] { customMessage }
            };

            //Send the SMS
            Send(message, sms.SendID, sms.ReporterNumber, sms.FieldValues);

            //Since the send is successfull create the SMS objects in the DB
            sms.Status = !DebugMode ? (int)enSMSStatus.Sent : (int)enSMSStatus.CreatedOnly;
            sms.SentAt = DateTime.Now;
        }

        public static SMS SendInternshipPositionAssignment(int studentID, string firstName, string lastName, string providerName, string mobilePhone)
        {
            //Declare and initialize the necessary values for the SMS sending
            string message = SMSDetailsReader.GetSMSDetails(AV_InternshipPositionAssignment).Message;

            if (String.IsNullOrEmpty(mobilePhone) || mobilePhone.Length != 10)
                throw new ArgumentException("The student with ID : " + studentID + " does not have a valid mobile phone number");

            string fullNameField = string.Format("{0} {1}", BusinessHelper.NameTrim(lastName, maxFullNameLength), firstName.SubstringByLength(1));
            string providerNameField = BusinessHelper.NameTrim(providerName, maxProviderNameLength).Replace("&", "").Replace("\"", "");

            SMS sms = new SMS()
            {
                SendID = studentID.ToString().AddZeroPadding(8) + DateTime.Now.ToString("yyMMddhhmmss") + Convert.ToInt32(enSMSType.InternshipPositionAssignment),
                ReporterID = studentID,
                ReporterNumber = "30" + mobilePhone,
                Type = (int)enSMSType.InternshipPositionAssignment,
                Msg = message,
                FieldValues = new string[] { fullNameField, providerNameField }
            };

            //Send the SMS
            Send(message, sms.SendID, sms.ReporterNumber, sms.FieldValues);

            //Since the send is successfull create the SMS objects in the DB
            sms.Status = !DebugMode ? (int)enSMSStatus.Sent : (int)enSMSStatus.CreatedOnly;
            sms.SentAt = DateTime.Now;

            return sms;
        }

        public static string AddZeroPadding(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            int sLength = s.Length;
            int numberOfZeros = maxLength - sLength;

            string sPadded = "";

            for (int i = 0; i < numberOfZeros; i++)
                sPadded += "0";

            sPadded += s;

            return sPadded;
        }

        #endregion

        #region XML SMSs configuration

        // Element names in XML file
        private static readonly string E_SMS = "SMS";
        private static readonly string E_Message = "Message";
        // Attribute keys in XML file
        private static readonly string AK_Category = "Category";
        // Attribute values in XML file
        private static readonly string AV_CustomMessage = "CustomMessage";
        private static readonly string AV_InternshipPositionAssignment = "InternshipPositionAssignment";

        private class SMSDetails
        {
            public string Message;
        }

        private static class SMSDetailsReader
        {
            public static SMSDetails GetSMSDetails(string category)
            {
                return (from z in CachedSMSDetails.Descendants(E_SMS)
                        where (string)z.Attributes(AK_Category).Single() == category
                        select new SMSDetails()
                        {
                            Message = z.Descendants(E_Message).Single().Value
                        }).Single();
            }

            private static XElement cachedSMSDetails = null;

            /// <summary>
            /// Returns the XML configuration for the e-mails, found in App_Data/Mails.xml.
            /// The resulting XElement is cached in a static private field.
            /// </summary>
            private static XElement CachedSMSDetails
            {
                get
                {
                    if (HttpContext.Current == null)
                        cachedSMSDetails = XElement.Parse(File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/SMSs.xml")));
                    else
                        cachedSMSDetails = XElement.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/SMSs.xml")));
                    return cachedSMSDetails;
                }
            }
        }
        #endregion
    }
}
