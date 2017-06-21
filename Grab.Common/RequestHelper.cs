using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Grab.Common
{
    public static class RequestHelper
    {

        #region Timeout Check
        public static TResult TimeoutAfter<TResult>(this Func<TResult> func, TimeSpan timeout)
        {
            var task = Task.Run(func);
            return TimeoutAfterAsync(task, timeout).GetAwaiter().GetResult();
        }

        private static async Task<TResult> TimeoutAfterAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var result = await Task.WhenAny(task, Task.Delay(timeout));
            if (result == task)
            {
                // Task completed within timeout.
                return task.GetAwaiter().GetResult();
            }
            else
            {
                // Task timed out.
                throw new TimeoutException();
            }
        }
        #endregion

        public static string HttpGet(string url, Encoding encoding, bool exceptionBackToUpper = false)
        {
            try
            {
                string html = string.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 20000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = GetStreamReader(encoding, response);

                html = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                response.Close();
                response.Dispose();
                request = null;
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                Console.WriteLine("Request Error!" + ex.Message);
                return string.Empty;
            }
        }

        public static string HttpGetWithProxy(string proxy, string url, Encoding encoding)
        {
            var timeOut = 10;
#if DEBUG
            timeOut = 5;
#endif
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            try
            {
                string html = string.Empty;
                request = (HttpWebRequest)HttpWebRequest.Create(url);

                request.Proxy = new WebProxy(proxy, true);
                request.Timeout = timeOut * 1000;
                request.ReadWriteTimeout = timeOut * 1000;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception e)
            {
                Console.WriteLine("Request Error!" + e.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        #region Private Method

        private static StreamReader GetStreamReader(Encoding encoding, WebResponse response)
        {
            var contentEncoding = response.Headers.GetValues("Content-Encoding");
            if (contentEncoding != null && contentEncoding[0].Equals("gzip"))
            {
                Stream stream = null;
                if (contentEncoding[0].ToLower().Equals("gzip"))
                {
                    stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (contentEncoding[0].ToLower().Equals("deflate"))
                {
                    stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
                }

                return new StreamReader(stream, encoding);
            }
            else
            {
                return new StreamReader(response.GetResponseStream(), encoding);
            }
        }

        #endregion
    }
}
