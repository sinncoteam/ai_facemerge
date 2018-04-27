using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FMerge.Web.Models;
using AICore.Api.FaceMerge;
using AICore.Utils;
using Microsoft.AspNetCore.Http;
using FMerge.Web.Component;
using Newtonsoft.Json;
using AICore.Domain.Service;
using AICore.Domain.Model;

namespace FMerge.Web.Controllers
{
    public class WxController : BaseController
    {
        public ActionResult RecLogin()
        {
            string json = Request.Form["jsondata"];
            string data = AESHelper.Decode(json);
            //Logging4net.WriteInfo(data);
            if (!string.IsNullOrEmpty(data))
            {
                WxUserInfo wxu = JsonConvert.DeserializeObject<WxUserInfo>(data);
                UserService x_userService = new UserService();
                var item = x_userService.getModel(wxu.openid);
                if (item != null)
                {
                    // if (item.IsValid == 1)
                    // {
                        Authentication.Instance.SetAuth(item);
                        return RedirectToAction("index","home");
                    // }
                }
                else
                {
                    UserModel uInfo = new UserModel();
                    uInfo.openid = wxu.openid;
                    uInfo.NickName = wxu.nickname;
                    uInfo.UserLogo = wxu.headimgurl;
                    uInfo.CreateTime = DateTime.Now;
                   
                    uInfo.Id = x_userService.setModel(uInfo);
                    Authentication.Instance.SetAuth(uInfo);
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("index","home");
        }
    }
}