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
    public class PhotoResultController : BaseController
    {
        public IActionResult Index(string photoid)
        {
            return Redirect("/#/myGraduationPhoto?photoid="+photoid);
        }
    }
}