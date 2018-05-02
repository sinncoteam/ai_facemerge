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
using AICore.Domain.Model;

namespace FMerge.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            //Authentication.Instance.SetAuth(new UserModel(){ Id = 101, openid ="xxxxxxxxxxx", NickName="长羽生", UserLogo = "http://aaaaaaaaa.com/x.jpg" });
            //FaceMergeModel sss = FaceMergeApi.getResult();
            //ViewBag.mm = sss.data.image;
            // ViewBag.kk = ConfigManager.AppSettings("model");
            // var us = new AICore.Domain.Service.UserService();
            // var dt = us.getModel(10);
            // ViewBag.kk2 = dt.Id;
            return View();
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
