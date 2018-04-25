using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AICore.Utils;

namespace AICore.Api.FaceMerge
{
    public class TPOAuthApi
    {
        public int app_id { get; set; }
        public int time_stamp { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string appkey { get; set; }
        public TPOAuthApi()
        {
            app_id = 1106781813;
           // app_id = 10000;
            appkey = "9DkwckMXDSIszGxF";
           // appkey = "a95eceb1ac8c24ee28b70f7dbba912bf";
        }

        public TPOAuthApi(int appid, string appKey)
        {
            app_id = appid;
            appkey = appKey;
        }
       

        public string getReqSign(string str)
        {
            if (!string.IsNullOrEmpty(sign)) { return sign; }

 
            str += "&app_key="+ appkey;

            // 4. MD5运算+转换大写，得到请求签名
            sign = MD5Helper.Md5(str);
            return sign;
        }
    }
}