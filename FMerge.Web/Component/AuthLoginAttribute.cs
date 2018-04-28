using System;
using System.Web;
using AICore.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using FMerge.Web.BaseAI;


namespace FMerge.Web.Component
{
    public class AuthLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            //string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            //string caName = controlName + "." + actionName;
            ////不排除，都需登录 V2.0.1(3-10)
            //string[] unAuth = new string[] { "my.sendsmsreg", "my.sendsmsforget" };
            //if (unAuth.Contains(caName))
            //{
            //    return;
            //}

            if (!Authentication.Instance.IsLogin)
            {
                if (filterContext.HttpContext.Request.IsAjax())
                {
                    AjaxMsgResult msg = new AjaxMsgResult()
                    {
                        success = 0,
                        code = "-99",
                        msg = "登录失效"
                    };
                    filterContext.Result = new JsonResult(msg);
                    return;
                }
                setoAuth(filterContext, "请先登录");
                return;
            }
        }

        private void setoAuth(ActionExecutingContext filterContext, string msg, int type = 0)
        {

            string pq = filterContext.HttpContext.Request.Path;
            // if (filterContext.HttpContext.Request.Path != null)
            // {
            //     pq = filterContext.HttpContext.Request.Path;
            // }
            //string host = filterContext.HttpContext.Request.Url.Host;
            //host = filterContext.HttpContext.Server.UrlEncode("http://" + host + "/user/wxlogin");
            //string loginurl = filterContext.HttpContext.Server.UrlEncode("/wx/login?s=" + pq);
            //host = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + tenPayV3Info.AppId + "&redirect_uri=" + host + "&response_type=code&scope=snsapi_base&state=" + loginurl + "#wechat_redirect";
            string host = ConfigManager.AppSettings("host");
            //string host = "xianyunsoft.xicp.cn";
            string hostUrl = "http://wxin2.cqnews.net/authorize.aspx?gp=53f6b7a0975642e9801f0d91d6042a70&ga=0682ed39d45745ae879f43d06e267ed0&opa=";
            hostUrl += HttpUtility.UrlEncode("http://" + host + "/wx/reclogin?s=" + pq);

            filterContext.Result = new RedirectResult(hostUrl);

        }


    }
}