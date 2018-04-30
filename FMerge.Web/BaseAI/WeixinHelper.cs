using AICore.Utils;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Text;

namespace FMerge.Web.BaseAI
{
    public class WeixinHelper
    {
        private static TenPayV3Info _tenPayV3Info;
        public static TenPayV3Info tenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[ConfigManager.AppSettings("TenPayV3_MchId")];
                }
                return _tenPayV3Info;
            }
        }
        public static WxJsInfo wxjsApiPay()
        {
            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();
            JsApiTicketResult wxTicket = CacheService.GetCache(ConstHelper.WxTicketKey) as JsApiTicketResult;
            if (wxTicket == null)
            {
                wxTicket = CommonApi.GetTicket(tenPayV3Info.AppId, tenPayV3Info.AppSecret);
                CacheService.SetChache(ConstHelper.WxTicketKey, wxTicket, wxTicket.expires_in - 120);
            }
            string url =  HttpContext.Current.Request.Path;
            Senparc.Weixin.MP.TenPayLib.RequestHandler nativeHandler = new Senparc.Weixin.MP.TenPayLib.RequestHandler(null);
            nativeHandler.SetParameter("jsapi_ticket", wxTicket.ticket);
            nativeHandler.SetParameter("noncestr", nonceStr);
            nativeHandler.SetParameter("timestamp", timeStamp);
            nativeHandler.SetParameter("url", url);
            string sign = nativeHandler.CreateSHA1Sign();
            return new WxJsInfo()
            {
                AppId = tenPayV3Info.AppId,
                Noncestr = nonceStr,
                Timestamp = timeStamp,
                Signature = sign
            };
        }
    }

    public class WxJsInfo
    {
        public string AppId { get; set; }
        public string Noncestr { get; set; }
        public string Timestamp { get; set; }
        public string Signature { get; set; }
    }
}
