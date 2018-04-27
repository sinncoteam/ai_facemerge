using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AICore.Utils;
using Newtonsoft.Json;

namespace AICore.Api.FaceMerge
{
    public class FaceMergeApi
    {
        // 获取结果数据
        public static FaceMergeModel getResult(string filename, string imgmodel)
        {
            
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            string noces = new Random(DateTime.Now.GetHashCode()).Next(1000000, 100000000).ToString();
            string fil = HttpRequestHelper.UrlEncode(ImageBase64Helper.ImgToBase64String(filename));
            string stamp = TimeHelper.GetTimeStamp(DateTime.Now, 10);
            //stamp = "1493468759";
            //noces = "fa577ce340859f9fe";
            TPOAuthApi tpo = new TPOAuthApi();
            
            //dict.Add("app_id", tpo.app_id.ToString());            
            //dict.Add("image", fil);
            //dict.Add("model", "4");
            //dict.Add("nonce_str", noces);
            
            //dict.Add("time_stamp", stamp);
            string datas = "app_id=" + tpo.app_id + "&image=" + fil + "&model="+imgmodel+"&nonce_str=" + noces + "&time_stamp=" + stamp;
            //string k1 = HttpRequestHelper.UrlEncode("腾讯AI开放平台");
            //string k2 = HttpRequestHelper.UrlEncode("示例仅供参考");
            //string datas = "app_id=10000&key1="+ k1+"&key2="+ k2+"&nonce_str=20e3408a79&time_stamp=1493449657";
            string sign = tpo.getReqSign(datas);

            datas += "&sign=" + sign;

            //string res = HttpRequestHelper.getHttpRequest("https://api.ai.qq.com/fcgi-bin/ptu/ptu_facemerge",
              //  new { app_id = tpo.app_id, time_stamp = sst, nonce_str = noces, model = 4, image = fil, sign = tpo.sign });
            string res = HttpRequestHelper.getHttpRequest("https://api.ai.qq.com/fcgi-bin/ptu/ptu_facemerge", datas);
            
            FaceMergeModel mol = JsonConvert.DeserializeObject<FaceMergeModel>(res);
            return mol;
        }
    }
}