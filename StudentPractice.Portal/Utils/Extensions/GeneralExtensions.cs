using System;
using System.Web;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;
using Imis.Domain;
using System.Text;

namespace StudentPractice.Portal
{
    public static class GeneralExtensions
    {
        public static string GetRequesterIP(this HttpRequest request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
                return ipList.Split(',')[0];

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static string CrLfToBr(this string s)
        {
            return s != null ? s.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br />") : "";
        }

        public static string[] SplitLines(this string s)
        {
            return s.Replace("\r", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToNull(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            while (s.Contains("  "))
                s = s.Replace("  ", " ");

            return s.Trim();
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
    }
}
