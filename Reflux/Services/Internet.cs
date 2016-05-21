using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;
using static System.String;

namespace Reflux.Services
{
    public static class Internet
    {
        public static string Get(string url, string apiToken)
        {
            return Execute(url, apiToken);
        }

        public static string Put(string url, string apiToken, string body = null)
        {
            return Execute(url, apiToken, "PUT", body);
        }

        public static string Post(string url, string apiToken, string body = null)
        {
            return Execute(url, apiToken, "POST", body);
        }

        private static string Execute(string url, string apiToken, string method = null, string body = null)
        {
            try
            {
                var request = WebRequest.Create(url);

                request.ContentType = "application/json";

                var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(apiToken + ":"));

                request.Headers.Add("Authorization", "Basic " + encoded);

                if (!IsNullOrEmpty(body))
                {
                    request.Method = method;

                    byte[] bytes = new ASCIIEncoding().GetBytes(body);

                    request.ContentLength = bytes.Length;

                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var encoding = Encoding.ASCII;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                LogManager.GetCurrentClassLogger().Error($"Flowdock request error: {url}-{method}-{body}");

                throw;
            }
        }
    }
}
