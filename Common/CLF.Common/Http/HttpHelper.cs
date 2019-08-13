using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CLF.Common.Http
{
    public partial class HttpHelper
    {
        /// <summary>  
        /// 证书路径  
        /// </summary>  
        public static string CertFilePath { get; set; }
        /// <summary>  
        /// 证书口令  
        /// </summary>  
        public static string CertFilePwd { get; set; }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static string DoPost(string url, IDictionary<string, string> parameters, string charset = "utf-8")
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;

            byte[] postData = Encoding.GetEncoding(charset).GetBytes(BuildQuery(parameters, charset));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet ?? charset);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="headers">请求头部数据</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static string DoPost(string url, string json, Dictionary<string, string> headers = null, string charset = "utf-8")
        {
            HttpWebRequest req = GetWebRequest(url, "POST", headers);
            req.ContentType = "application/json;charset=" + charset;

            byte[] postData = Encoding.GetEncoding(charset).GetBytes(json);
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet ?? charset);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行POST请求（可以带数字证书）
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="sendData">请求的数据</param>
        /// <param name="contentType">请求内容格式</param>
        /// <param name="headers">请求头部</param>
        /// <param name="charset">编码格式</param>
        /// <param name="isLoadCert">是否带数字证书</param>
        /// <returns></returns>
        public static string DoPost(string url, string sendData, string contentType, Dictionary<string, string> headers = null, string charset = "utf-8", bool isLoadCert = false)
        {
            HttpWebRequest req = GetWebRequest(url, "POST", headers, isLoadCert);
            req.ContentType = contentType;

            byte[] postData = Encoding.GetEncoding(charset).GetBytes(sendData);
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(charset);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求。(异步)
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static async Task<string> DoAsyncPost(string url, string json, string charset = "utf-8")
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/json;charset=" + charset;

            byte[] postData = Encoding.GetEncoding(charset).GetBytes(json);
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp =
                (HttpWebResponse)
                    await Task<WebResponse>.Factory.FromAsync(req.BeginGetResponse, req.EndGetResponse, null);
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet ?? charset);
            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static string DoGet(string url, IDictionary<string, string> parameters, string charset = "utf-8")
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters, charset);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters, charset);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        public static HttpWebRequest GetWebRequest(string url, string method, Dictionary<string, string> headers = null, bool isLoadCert = false)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.Timeout = 100000;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    req.Headers[header.Key] = header.Value;
                }
            }
            if (isLoadCert)
            {
                X509Certificate2 cert = CreateX509Certificate2();
                req.ClientCertificates.Add(cert);
            }
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    string encodedValue = HttpUtility.UrlEncode(value, Encoding.GetEncoding(charset));

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        private static bool ServerCertificateValidationCallback(object obj, X509Certificate cer, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        private static X509Certificate2 CreateX509Certificate2()
        {
            X509Certificate2 cert = null;
            try
            {
                cert = new X509Certificate2(CertFilePath, CertFilePwd);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ServerCertificateValidationCallback);
            }
            catch (Exception)
            {
                cert = null;
            }
            return cert;
        }
    }
}
