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
using System.DrawingCore;

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

        public IActionResult T()
        {
            // FileMergeSize fs = new FileMergeSize();
            // fs.Width = 579;
            // fs.Height = 680;
            // fs.FileName = "e:\\1.jpg";
            // List<FileMergeSize> fslist = new List<FileMergeSize>();
            // FileMergeSize fs1 = new FileMergeSize();
            // fs1.FileName = "e:\\0.jpg";
            // FileMergeSize fs2 = new FileMergeSize();
            // fs2.FileName = "e:\\qr.png";
            // fs2.X = 420;
            // fs2.Y = 540;
            // fslist.Add(fs1);
            // fslist.Add(fs2);
            // List<FontTextSize> ftlist = new List<FontTextSize>();
            // FontTextSize ft1 = new FontTextSize();
            // ft1.Text = "我是"; 
            // ft1.Y = 540;
            // FontTextSize ft2 = new FontTextSize();
            // ft2.Text = "大学生";
            // ft2.TextBrush = new SolidBrush(Color.Red);
            // ft2.X = 64;
            // ft2.Y = 540;
            // FontTextSize ft3 = new FontTextSize();
            // ft3.Text = "2018年";
            // ft3.Y = 580;
            // ft3.TextBrush = new SolidBrush(Color.Red);
            // FontTextSize ft4 = new FontTextSize();
            // ft4.Text = "毕业于";
            // //ft4.X = ft3.Text.Length * 30;
            // ft4.Y = 620;
            // FontTextSize ft5 = new FontTextSize();
            // ft5.Text = "重庆大学";
            // ft5.X = 96;
            // ft5.Y = 620;
            // ft5.TextBrush = new SolidBrush(Color.Red);

            // ftlist.Add(ft1);
            // ftlist.Add(ft2);
            // ftlist.Add(ft3);
            // ftlist.Add(ft4);
            // ftlist.Add(ft5);
            // ImageHelper.MergeImage(fs, fslist, ftlist);
            return Content("yes");
        }

        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
