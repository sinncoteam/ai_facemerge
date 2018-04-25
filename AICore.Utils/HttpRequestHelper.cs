using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Security;

namespace AICore.Utils
{
    public class HttpRequestHelper
    {
        /// <summary>
        /// 获取外部请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string getHttpRequest(string url, string obj, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            //webRequest.Credentials = new NetworkCredential("AKIDCRbdVCW5IJDyYDtFbmss00paFpHXehWC", AuthrizeKey);
            //webRequest.Headers.Add("Authorization", authkey);
            webRequest.ContentType = contentType;
            //string jsonData = jss.Serialize(obj);

           

            string jsonData = obj;
            //webRequest.
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
            webRequest.ContentLength = byteArray.Length;
            using (Stream newStream = webRequest.GetRequestStream())//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
            {
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string content = reader.ReadToEnd();
                        return content;
                    }
                }
            }
        }

        public static string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString()).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
    }
}
