using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

#if __MOBILE__
using Newtonsoft.Json;
#endif

namespace CloudWTO.Services
{
    public class EasyWebRequest
    {

        public static string sendGetHttpWebRequest(string url)
        {

            ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //req.Host = "example.com";
            req.Headers.Add(HttpRequestHeader.Authorization, App.token);//给请求添加权限
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string result = sr.ReadToEnd();
            Console.WriteLine("ex:" + result);

            return result;
        }

        public static string sendPOSTHttpWebRequest(string url, string param)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
                byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(param);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                //req.Headers.Add(HttpRequestHeader.Authorization,@"string");//给请求添加权限
                req.ContentLength = bs.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bs, 0, bs.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());
                string result = sr.ReadToEnd();
                sr.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
                return ex.Message;
            }
        }

        public static string sendOtherPOSTHttpWebRequest(string url, string param)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
                byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(param);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add(HttpRequestHeader.Authorization, App.token);//给请求添加权限
                req.ContentLength = bs.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bs, 0, bs.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());
                string result = sr.ReadToEnd();
                sr.Close();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
                return ex.Message;
            }
        }

        public static async Task<string> sendAwaitePOSTHttpWebRequest(string url, string param,string token)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyCertHandler;
                byte[] bs = Encoding.GetEncoding("UTF-8").GetBytes(param);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                if(!string.IsNullOrWhiteSpace(token))req.Headers.Add(HttpRequestHeader.Authorization, token);//给请求添加权限
                req.ContentLength = bs.Length;
                try
                {
                    Stream requestStream = await req.GetRequestStreamAsync();
                    await requestStream.WriteAsync(bs, 0, bs.Length);
                    requestStream.Close();
                }
                catch (WebException ex){

                }
                WebResponse wr = await req.GetResponseAsync();
                HttpWebResponse res = wr as HttpWebResponse;
                StreamReader sr = new StreamReader(res.GetResponseStream());
                var result = await sr.ReadToEndAsync();
                sr.Close();
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
                return ex.Message;
            }
        }

        static bool MyCertHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            Console.WriteLine("hi");
            // Ignore errors
            return true;
        }


    }
}
