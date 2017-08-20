using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Grab.Common
{
    public class HtmlHelper
    {
        public static string RemoveLabel(string input)
        {
            Regex labelRemoveRegex = new Regex("(<[^>]*>)", RegexOptions.IgnoreCase);
            return labelRemoveRegex.Replace(input, string.Empty);
        }

        /// <summary>
        /// 去掉包住的东西，比如用于去掉高亮效果的标签，保留中间的内容(@"<FONT color=red>([\s\S]*?)</FONT>")
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RemoveOuterLabel(string input, string pattern)
        {
            string returnStr = string.Empty;
            Regex highlightRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection hightlightMatchs = highlightRegex.Matches(input);
            if (hightlightMatchs.Count > 0)
            {
                //逐个匹配替换中间内容
                returnStr = highlightRegex.Replace(input, new MatchEvaluator(x =>
                {
                    return x.Value.Replace(x.Value, x.Groups[1].Value);
                }));
            }
            return returnStr;

        }

        public static string ReplaceNewLine(string input)
        {
            return ReplaceNewLine(input, "\r\n");
        }

        public static string ReplaceNewLine(string input, string replaceStr)
        {
            string returnStr = string.Empty;
            Regex regex = new Regex(@"(\r\n)+", RegexOptions.IgnoreCase);
            returnStr = regex.Replace(input, replaceStr);
            return returnStr;
        }

        public static string RemoveCss(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value, @"<style[^>]*>([\s\S]*?)</style>", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"style=['""][^'""]*['""]", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"class=['""][^'""]*['""]", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"<script[^>]*>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"</?[!vosi\?][^>]*>", "", RegexOptions.IgnoreCase).Trim();
            }

            return value;
        }

        /// <summary>
        /// 格式化富文本标签字段，去标签
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string FormatRichText(string description)
        {
            if (string.IsNullOrEmpty(description))
                return "";

            description = HttpUtility.HtmlDecode(description);
            description = Regex.Replace(description, @"</?[lusovh\!xfi\?][^>]*>", string.Empty, RegexOptions.IgnoreCase);

            List<string> brTagList = new List<string>() { "p", "div", "ul", "li", "dl" };
            brTagList.ForEach(tag =>
            {
                description = Regex.Replace(description, string.Format("<{0}[^<>]*?>", tag), "", RegexOptions.IgnoreCase);
                description = Regex.Replace(description, string.Format("</{0}>", tag), "&&&&&&&&", RegexOptions.IgnoreCase);
            });

            List<string> tableTagList = new List<string>() { "tr", "table", "td", "thead", "tbody", "th" };
            tableTagList.ForEach(tag =>
            {
                description = Regex.Replace(description, string.Format("<{0}[^<>]*?>", tag), string.Format("####{0}|||", tag), RegexOptions.IgnoreCase);
                description = Regex.Replace(description, string.Format("</{0}>", tag), string.Format("/////####{0}|||", tag), RegexOptions.IgnoreCase);
            });

            description = Regex.Replace(description, @"</?br\s*/?\s*>", "&&&&&&&&", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, @"</?br\s*/?\s*>", "&&&&&&&&", RegexOptions.IgnoreCase);
            description = Regex.Replace(description, "</?[^<>]*?>", "");

            description = description.Replace("&&&&&&&&", "<br/>");
            tableTagList.ForEach(tag =>
            {
                description = description.Replace(string.Format("/////####{0}|||", tag), string.Format("</{0}>", tag));
                description = description.Replace(string.Format("####{0}|||", tag), string.Format("<{0}>", tag));
            });

            description = Regex.Replace(description, @"</?[a-zA-Z][^<>]*?$", "");
            return description;
        }

        /// <summary>
        /// 移除掉中文括号，及 空白
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceBracesAndTrim(string value)
        {
            return value.Replace("（", "(").Replace("）", ")").Replace(" ", "").Trim();
        }

        /// <summary>
        /// 移除Pattern所匹配到的
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RemoveMatched(string html, string pattern)
        {
            if (string.IsNullOrEmpty(html)) return "";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection col = regex.Matches(html);
            if (col.Count > 0)
            {
                foreach (Match m in col)
                {
                    html = html.Replace(m.Groups[1].Value, "");
                }
            }
            return html;
        }

        /// <summary>
        /// 去除Class
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveClassAttr(string html)
        {
            string pattern = @"(class\s*=\s*""[^""]*?"")";
            return RemoveMatched(html, pattern);
        }

        /// <summary>
        /// 去除样式
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveStyleAttr(string html)
        {
            string pattern = @"(style\s*=\s*""[^""]*?"")";
            return RemoveMatched(html, pattern);
        }

        /// <summary>
        /// 移除html标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="patternList">eg.移除font标签"(</?font[^>]*>)"</param>
        /// <returns></returns>
        public static string RemoveTag(string html, List<string> patternList)
        {
            if (string.IsNullOrEmpty(html)) return "";
            patternList.ForEach(p => html = RemoveMatched(html, p));
            return html.Trim();
        }

        /// <summary>
        /// 移除所有标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveAllTag(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            return Regex.Replace(html, "<[^<>]*?>", "").Trim();
        }

        /// <summary>
        ///移除所有a标签
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveLink(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Regex.Replace(value, "(</?a[^>]*>)", "", RegexOptions.IgnoreCase);
            }

            return string.Empty;
        }

        /// <summary>
        /// 移除空白、html注释、d* 、p*标签
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ProcessingLabel(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value, "&nbsp;", " ", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"</?[lusovh\!xfi\?][^>]*>", string.Empty, RegexOptions.IgnoreCase);
                value = Regex.Replace(value, "</?[dp][^>]*>", new MatchEvaluator(x =>
                {
                    if (x.Value.Contains("</") || x.Value.Contains("<br"))
                    {
                        return "<br /><br />";
                    }
                    return string.Empty;
                }), RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"</?b(?(\s+)(?:\s+[^>]*)|\s*)>", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"(<br[^>]*>\s*){3,}", "<br /><br />", RegexOptions.IgnoreCase).Trim();
            }

            return value;
        }

        /// <summary>
        /// 移除html中的空格和空白
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSpace(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value.ToLower(), "&quot;", @"""", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), "&amp;", "&", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), "&nbsp;|&nbsp|nbsp", " ", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), ";", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"\s+", " ", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, " ", "", RegexOptions.IgnoreCase);
                if (!string.IsNullOrEmpty(value))
                {
                    return value.Trim();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 把html的空格变成文本的空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceHtmlSpace(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value.ToLower(), "&quot;", @"""", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), "&amp;", "&", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), "&nbsp;|&nbsp|nbsp", " ", RegexOptions.IgnoreCase);
                value = Regex.Replace(value.ToLower(), ";", "", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, @"\s+", " ", RegexOptions.IgnoreCase);
                value = Regex.Replace(value, " ", "", RegexOptions.IgnoreCase);
                if (!string.IsNullOrEmpty(value))
                {
                    return value.Trim();
                }
            }
            return string.Empty;
        }

        public static string RemoveUrlHttp(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value, "http://|http:|http|//", "", RegexOptions.IgnoreCase);
            }

            return value;
        }

        public static string ReplaceBracketToCN(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("(", "（").Replace(")", "）").Trim();
            }

            return value;
        }

    }
}
