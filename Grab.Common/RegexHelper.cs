using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grab.Common
{
    public class RegexHelper
    {
        public const string Ip = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
        public const string SubstringFormat = "(?<=({0})).+(?=({1}))";
        public const string Email = @"[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+";
        public const string Unicode = @"[\u4E00-\u9FA5\uE815-\uFA29]+";
        public const string Url = @"(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*";
        public const string MobileNumber = @"[1][3-8]\d{9}";
        public const string ChineseWords = @"[\u4e00-\u9fff]+";

        public static MatchCollection GetMatchedCollection(string input, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matchcollection = regex.Matches(input);
            return matchcollection;
        }

        public static string RegexMatch(string pattern, string html, int grpIndex = 1, int rsltMinLength = 0)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(html))
                return "";
            if (rsltMinLength <= 0)
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match m = regex.Match(html);
                if (!string.IsNullOrEmpty(m.Groups[grpIndex].Value))
                {
                    return m.Groups[grpIndex].Value.Trim();
                }
            }
            else
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection ms = regex.Matches(html);
                if (ms.Count > 0)
                {
                    foreach (Match m in ms)
                    {
                        var val = m.Groups[grpIndex].Value.Trim();
                        if (val.Length >= rsltMinLength)
                            return val;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Regex Match, 使用预编译义好的Regex 对象
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="html"></param>
        /// <param name="grpIndex"></param>
        /// <param name="rsltMinLength"></param>
        /// <returns></returns>
        public static string RegexMatch(Regex regex, string html, int grpIndex = 1, int rsltMinLength = 0)
        {
            if (regex == null)
                return "";
            if (rsltMinLength <= 0)
            {
                Match m = regex.Match(html);
                if (!string.IsNullOrEmpty(m.Groups[grpIndex].Value))
                {
                    return m.Groups[grpIndex].Value.Trim();
                }
            }
            else
            {
                MatchCollection ms = regex.Matches(html);
                if (ms.Count > 0)
                {
                    foreach (Match m in ms)
                    {
                        var val = m.Groups[grpIndex].Value.Trim();
                        if (val.Length >= rsltMinLength)
                            return val;
                    }
                }
            }

            return string.Empty;
        }
    }
}
