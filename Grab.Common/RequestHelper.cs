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
                return task.GetAwaiter().GetResult();
            }
            else
            {
                throw new TimeoutException();
            }
        }
        #endregion

        public static string HttpGet(string url, Encoding encoding, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpGet(string url, Encoding encoding, CookieCollection cookie, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;                
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.Add(cookie);
                request.CookieContainer = cookieContainer;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpGet(string url, Encoding encoding, CookieContainer cookieContainer, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.CookieContainer = cookieContainer;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpGet(string url, Encoding encoding, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);

                request.Proxy = new WebProxy(proxy, true);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception e)
            {
                if (exceptionBackToUpper)
                    throw e;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpGet(string url, Encoding encoding, CookieCollection cookie, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.Proxy = new WebProxy(proxy, true);
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.Add(cookie);
                request.CookieContainer = cookieContainer;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpGet(string url, Encoding encoding, CookieContainer cookieContainer, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.Proxy = new WebProxy(proxy, true);
                request.CookieContainer = cookieContainer;
                response = (HttpWebResponse)request.GetResponse();
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }


        public static string HttpPost(string posturl, string postData, Encoding encoding, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                byte[] data = encoding.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(posturl);
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = request.GetResponse() as HttpWebResponse;
                reader = GetStreamReader(encoding, response);               
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpPost(string posturl, string postData, Encoding encoding, CookieContainer cookieContainer, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                byte[] data = encoding.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(posturl);
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = request.GetResponse() as HttpWebResponse;
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string HttpPost(string posturl, string postData, Encoding encoding, CookieCollection cookie, bool exceptionBackToUpper = true, int timeOut = 20000)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            string html = string.Empty;
            try
            {
                byte[] data = encoding.GetBytes(postData);
                request = (HttpWebRequest)WebRequest.Create(posturl);
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                response = request.GetResponse() as HttpWebResponse;
                reader = GetStreamReader(encoding, response);
                html = reader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
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
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        #region Private Method

        private static StreamReader GetStreamReader(Encoding encoding, HttpWebResponse response)
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
