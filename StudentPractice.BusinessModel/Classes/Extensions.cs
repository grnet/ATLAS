using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StudentPractice.BusinessModel
{
    public static class Extensions
    {
        private static int SplitAt(int totalItems, int splitEvery, out int takeExtra)
        {
            int timesToIterate = totalItems / splitEvery;
            takeExtra = totalItems % splitEvery;
            return timesToIterate;
        }

        public static List<List<T>> Split<T>(this List<T> list, int splitEvery)
        {
            int takeExtra;
            int timesToIterate = SplitAt(list.Count, splitEvery, out takeExtra);

            List<List<T>> newLists = new List<List<T>>();
            for (int i = 0; i < timesToIterate; i++)
            {
                int skip = i * splitEvery;
                newLists.Add(list.Skip(skip).Take(splitEvery).ToList());
            }
            if (takeExtra > 0)
            {
                int skip = timesToIterate * splitEvery;
                newLists.Add(list.Skip(skip).Take(takeExtra).ToList());
            }
            return newLists;
        }

        public static string EncodeShibString(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return String.Empty;
            var uUtf8 = new UTF8Encoding();
            Encoding x = Encoding.GetEncoding("utf-8");
            byte[] b = x.GetBytes(input);
            return uUtf8.GetString(b);
        }

        public static bool ContainsLatinCharacters(this string s)
        {
            if (Regex.IsMatch(s, @"^[a-zA-Z\s\-]+$"))
                return true;
            else
                return false;
        }

        public static string[] SplitByLength(this string s, int splitAt)
        {
            string[] result = new[] { string.Empty, string.Empty };

            if (s.Length > splitAt)
            {
                int index = s.Substring(0, splitAt - 1).LastIndexOf(' ');
                result[0] = s.Substring(0, index);
                result[1] = s.Substring(index + 1);
            }
            else
            {
                result[0] = s;
            }

            return result;
        }

        public static string SubstringByLength(this string s, int length)
        {
            string result = string.Empty;

            if (s.Length > length)
            {
                result = s.Substring(0, length);
            }
            else
            {
                result = s;
            }

            return result;
        }
    }
}
