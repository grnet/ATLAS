using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using System.Web.Script.Serialization;
using Imis.Domain;

namespace StudentPractice.BusinessModel
{
    public class AcademicIDCardRequest
    {
        public string StudentNumber { get; set; }
        public int AcademicID { get; set; }
        public int ServiceCallerID { get; set; }
    }

    public class StudentCardNumberService
    {
        static StudentCardNumberService()
        {
            academicIDServiceUrl = ConfigurationManager.AppSettings["AcademicIDServiceURL"];
        }

        private static string academicIDServiceUrl = string.Empty;

        //Search by academic id and student number
        public static StudentDetailsFromAcademicID GetStudentInfo(AcademicIDCardRequest requestData)
        {
            var endpoint = academicIDServiceUrl + "GetStudentCardNumber";
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
            requestWriter.Write(new JavaScriptSerializer().Serialize(requestData));
            requestWriter.Close();

            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                return new JavaScriptSerializer().Deserialize<StudentDetailsFromAcademicID>(data);
            }
        }

        //Search by academic card number
        public static StudentDetailsFromAcademicID GetStudentInfoByAcademicCardNumber(int reporterId, string academicCardId)
        {
            var endpoint = academicIDServiceUrl + "GetStudentDetails/";
            var url = string.Format("{0}{1}/{2}", endpoint, reporterId, academicCardId);
            var req = (HttpWebRequest)WebRequest.Create(url);
            var response = req.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                return new JavaScriptSerializer().Deserialize<StudentDetailsFromAcademicID>(data);
            }
        }
    }
}
