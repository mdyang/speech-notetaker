using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    static class Util
    {
        public static IEnumerable<string> GetNormalizedTokens(this string rawText)
        {
            return rawText.Split(' ').Select(s => s.Trim(' ', '.', ',').ToLowerInvariant()).Where(s => !string.IsNullOrEmpty(s));
        }

        public static IEnumerable<string> GetOriginalTokens(this string rawText)
        {
            return rawText.Split(' ').Where(s => !string.IsNullOrEmpty(s));
        }

        public static string GetNormalized(this string token)
        {
            return token.Trim(' ', '.', ',').ToLowerInvariant();
        }

        public static string MakeHttpRequest(string method, string url, string contentType, Tuple<string, string>[] headers = null, string payload = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            request.ContentType = contentType;

            if (headers != null)
            {
                foreach (Tuple<string, string> header in headers)
                {
                    request.Headers[header.Item1] = header.Item2;
                }
            }

            if (payload != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(payload);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
