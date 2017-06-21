using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grab.Common
{
    public class Proxy
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Proxy));
        /// <summary>
        /// http://www.proxylists.net/cn_0.html
        /// </summary>
        /// <returns></returns>
        IList<string> GetProxyList0()
        {
            _logger.Info("Init proxy list.");
            List<string> list = new List<string>();
            try
            {
                var urlFormat = "http://www.proxylists.net/cn_{0}.html";
                bool flag = true;
                for (int i = 0; i < MaxCount && flag; i++)
                {
                    var url = string.Format(urlFormat, i);
                    string html = RequestHelper.HttpGet(url, Encoding.UTF8);
                    string pattern = @"<tr>\s*<td>\s*<script type='text/javascript'>eval\(unescape\('([^']*?)'\)\);</script><noscript>[^<>]*?</noscript>\s*</td>\s*<td>\s*(\d{1,5})\s*</td>\s*</tr>";
                    Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                    MatchCollection collection = re.Matches(html);
                    if (collection.Count > 0)
                    {
                        foreach (Match m in collection)
                        {
                            var js = HttpUtility.UrlDecode(m.Groups[1].Value.Trim());
                            var ip = RegexMatch(@"\d*\.\d*\.\d*\.\d*", js, 0);
                            list.Add(ip + ":" + m.Groups[2].Value.Trim());
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (list.Count == 0)
                {
                    Console.WriteLine("获取代理 失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取代理文件异常：{0}", ex.Message);
            }
            finally
            {
                if (list.Count <= 0)
                {
                    list = GetDefaultProxyList();
                }
            }
            return list.Distinct().ToList();
        }

        /// <summary>
        /// http://www.kuaidaili.com/free/inha/{0}/
        /// </summary>
        /// <returns></returns>
        IList<string> GetProxyList2()
        {
            Console.WriteLine("初始化代理服务器。。。");
            var MaxCount = 5;
#if DEBUG
            MaxCount = 5;
#endif
            List<string> list = new List<string>();
            try
            {
                var urlFormat = "http://www.kuaidaili.com/free/inha/{0}/";
                bool flag = true;
                int retryTimes = 5;
                for (int i = 1; i <= MaxCount && flag; i++)
                {
                    Console.WriteLine("正在加载代理源第{0}页。。。", i);
                    var url = string.Format(urlFormat, i);
                    string responseData = HttpGet(url, Encoding.UTF8);
                    //string pattern = @" <tr>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+<td>([\s\S]*)</td>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+<td>([^<]*)</td>[\s\t\n]+</tr>";
                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(responseData);
                    //获取列表页职位链接
                    var proxyies = html.DocumentNode.SelectNodes("//div[@id='list']/table/tbody/tr");
                    //Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                    //MatchCollection collection = re.Matches(html);
                    if (proxyies.Count > 0)
                    {
                        foreach (var line in proxyies)
                        {
                            var ip = line.SelectSingleNode("./td[@data-title='IP']").InnerHtml;
                            var port = line.SelectSingleNode("./td[@data-title='PORT']").InnerHtml;
                            list.Add(ip + ":" + port);
                        }
                        retryTimes = 5;
                    }
                    else if (retryTimes > 0)
                    {
                        i--;
                        retryTimes--;
                    }
                    Thread.Sleep(1000);
                }
                if (list.Count == 0)
                {
                    Console.WriteLine("获取代理 失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取代理文件异常：{0}", ex.Message);
            }
            finally
            {
                if (list.Count <= 0)
                {
                    list = GetDefaultProxyList();
                }
            }
            return list.Distinct().ToList();
        }

        /// <summary>
        /// http://www.kuaidaili.com/free/intr/{0}/
        /// </summary>
        /// <returns></returns>
        IList<string> GetProxyList3()
        {
            Console.WriteLine("初始化代理服务器。。。");
            var MaxCount = 5;
#if DEBUG
            MaxCount = 5;
#endif
            List<string> list = new List<string>();
            try
            {
                var urlFormat = "http://www.kuaidaili.com/free/intr/{0}/";
                bool flag = true;
                int retryTimes = 5;
                for (int i = 1; i <= MaxCount && flag; i++)
                {
                    Console.WriteLine("正在加载代理源第{0}页。。。", i);
                    var url = string.Format(urlFormat, i);
                    string responseData = RequestHelper.HttpGet(url, Encoding.UTF8);                    
                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(responseData);
                    //获取列表页职位链接
                    var proxyies = html.DocumentNode.SelectNodes("//div[@id='list']/table/tbody/tr");
                    if (proxyies.Count > 0)
                    {
                        foreach (var line in proxyies)
                        {
                            var ip = line.SelectSingleNode("./td[@data-title='IP']").InnerHtml;
                            var port = line.SelectSingleNode("./td[@data-title='PORT']").InnerHtml;
                            list.Add(ip + ":" + port);
                        }
                        retryTimes = 5;
                    }
                    else if (retryTimes > 0)
                    {
                        i--;
                        retryTimes--;
                    }
                    Thread.Sleep(1000);
                }
                if (list.Count == 0)
                {
                    Console.WriteLine("获取代理 失败");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取代理文件异常：{0}", ex.Message);
            }
            finally
            {
                if (list.Count <= 0)
                {
                    list = GetDefaultProxyList();
                }
            }
            return list.Distinct().ToList();
        }
    }
}
