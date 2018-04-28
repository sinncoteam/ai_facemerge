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
using AICore.Domain.Service;
using AICore.Domain.Model;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FMerge.Web.Controllers
{
    // [AuthLogin]
    public class ApiController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public ApiController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public JsonResult getWxUser()
        {
            AjaxMsgResult msg = new AjaxMsgResult();
            msg.success = 1;
            msg.source = new WxUserModel()
            {
                id = this.CurrentUser.Id,
                openid = this.CurrentUser.openid,
                nickname = this.CurrentUser.NickName,
                userlogo = this.CurrentUser.UserLogo
            };
            return Json(msg);
        }

        public JsonResult setUserInfo(string openid, string realname, string school, string dateyear)
        {
            AjaxMsgResult msg = new AjaxMsgResult();
            UserService x_uService = new UserService();
            int i = 1;
            //int i = x_uService.Update(() => new UserModel() { RealName = realname, School = school, DateYear = dateyear }, a => a.openid == openid);
            if (i > 0)
            {
                msg.success = 1;
                msg.code = "success";
            }
            else
            {
                msg.code = "fail";
                msg.msg = "设置用户信息失败，请稍后再试";
            }
            return Json(msg);
        }

        public async Task<IActionResult> setOriPhoto()
        {
            var files = Request.Form.Files;
            // long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            // string contentRootPath = _hostingEnvironment.ContentRootPath;
            int i = 0;
            foreach (var formFile in files)
            {

                string fileExt = ImageBase64Helper.getFileExt(formFile.FileName); //文件扩展名，不含“.”
                long fileSize = formFile.Length; //获得文件大小，以字节为单位
                string uid = this.CurrentUser.Id.ToString();
                string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名

                string oriPath = "/upload/" + uid + "/";
                string filePath = webRootPath + oriPath;
                if (Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += newFileName;
                oriPath += newFileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                UserPhotoMergeModel x_upModel = new UserPhotoMergeModel();
                x_upModel.UserId = CurrentUser.Id;
                x_upModel.CreateTime = DateTime.Now;
                x_upModel.LocalPhotoUrl = oriPath;
                x_upModel.Status = 0;
                UserPhotoMergeService x_upService = new UserPhotoMergeService();
                
                // i = x_upService.Insert(x_upModel);


            }
            AjaxMsgResult msg = new AjaxMsgResult() { success = 1, source = i };
            return Json(msg);
        }
    }
}