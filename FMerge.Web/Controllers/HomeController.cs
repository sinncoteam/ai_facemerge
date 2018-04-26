using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FMerge.Web.Models;
using AICore.Api.FaceMerge;
using AICore.Utils;

namespace FMerge.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //FaceMergeModel sss = FaceMergeApi.getResult();
            //ViewBag.mm = sss.data.image;
            ViewBag.kk = ConfigManager.AppSettings("model");
            var us = new AICore.Domain.Service.UserService();
            var dt = us.getModel(10);
            ViewBag.kk2 = dt.Id;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
