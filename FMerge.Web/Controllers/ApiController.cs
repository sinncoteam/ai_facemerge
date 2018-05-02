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
using FMerge.Web.BaseAI;

namespace FMerge.Web.Controllers
{
    //[AuthLogin]
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
            //msg.source = new WxUserModel()
            //{
            //    id = 1,
            //    openid = "xxxxxxxxxx",
            //    nickname = "常玉生",
            //    userlogo = ""
            //};
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
            //int i = 1;
            //i = x_uService.Update(() => new UserModel() { RealName = realname, School = school, DateYear = dateyear }, a => a.openid == openid);
            Authentication.Instance.setSession(new UserModel()
            {
                openid = CurrentUser.openid,
                RealName = realname,
                School = school,
                DateYear = dateyear
            });
            // if (i > 0)
            // {
            msg.success = 1;
            msg.code = "success";
            // }
            // else
            // {
            //     msg.code = "fail";
            //     msg.msg = "设置用户信息失败，请稍后再试";
            // }
            return Json(msg);
        }


        public IActionResult setOriPhoto()
        {
            var files = Request.Form.Files;
            // long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            int i = 0;
            string uid = this.CurrentUser.Id.ToString();
            //string uid = "1";
            string oriPath = "/upload/" + uid + "/";
            string thumlPath = oriPath;
            foreach (var formFile in files)
            {
                string fileExt = ImageBase64Helper.getFileExt(formFile.FileName); //文件扩展名，不含“.”
                //long fileSize = formFile.Length; //获得文件大小，以字节为单位

                string guid = System.Guid.NewGuid().ToString();
                string newFileName = guid + "." + fileExt; //随机生成新的文件名
                string thbumlName = guid + "_s." + fileExt;


                string filePath = webRootPath + oriPath;
                string flethumlPath = webRootPath + oriPath;
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += newFileName;
                oriPath += newFileName;
                flethumlPath += thbumlName;
                thumlPath += thbumlName;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
                int img = ThumbnailHelper.MakeThumbnailImage(filePath, flethumlPath, 600);
                if( img == 0)
                {
                    thumlPath = oriPath;
                }

                UserPhotoMergeModel x_upModel = new UserPhotoMergeModel();
                //x_upModel.UserId = 99;
                x_upModel.UserId = CurrentUser.Id;
                x_upModel.openid = CurrentUser.openid;
                x_upModel.CreateTime = DateTime.Now;
                x_upModel.LocalPhotoUrl = thumlPath;
                x_upModel.PhotoExt = fileExt;
                x_upModel.Status = 0;
                UserPhotoMergeService x_upService = new UserPhotoMergeService();

                i = Convert.ToInt32(x_upService.Insert(x_upModel));
            }
            if (files.Count > 0)
            {
                AjaxMsgResult msg = new AjaxMsgResult() { success = 1, source = new { photoid = i, purl = thumlPath } };
                return Json(msg);
            }
            AjaxMsgResult msg2 = new AjaxMsgResult() { success = 0, msg = "请上传1张照片" };
            return Json(msg2);
        }

        public IActionResult setMerge(string openid, int photoid, string photomodel)
        {
            UserPhotoMergeService x_upService = new UserPhotoMergeService();
            x_upService.Update(() => new UserPhotoMergeModel()
            {
                PhotoModel = photomodel
            }, a => a.Id == photoid && a.openid == openid);

            AjaxMsgResult result = new AjaxMsgResult();
            result.success = 1;
            result.msg = "照片正在合成中，请稍候";
            result.source = new { realname = CurrentUser.RealName, school = CurrentUser.School, dateyear = CurrentUser.DateYear };
            
            return Json(result);
        }

        public IActionResult getMergeResult(string openid, int photoid)
        {
            AjaxMsgResult result = new AjaxMsgResult();
            UserPhotoMergeService x_upService = new UserPhotoMergeService();
            var item = x_upService.Get(a => a.Status == 1 && a.Id == photoid).FirstOrDefault();
            if (item != null)
            {
                result.success = 1;
                result.msg = "合成成功";
                result.source = new { realname = CurrentUser.RealName, school = CurrentUser.School, dateyear = CurrentUser.DateYear, photo = item.PhotoResultUrl };
            }
            else
            {
                result.success = 0;
                result.msg = "照片合成中";
                result.source = new { realname = CurrentUser.RealName, school = CurrentUser.School, dateyear = CurrentUser.DateYear, photo = "" };
            }
            //result.success = 1;
            //    result.msg = "合成成功";
            //    result.source = "/upload/99/xx.jpg";
            return Json(result);
        }

        public IActionResult getWxJsSDK()
        {
            AjaxMsgResult result = new AjaxMsgResult();
            result.success = 1;
            result.source = WeixinHelper.wxjsApiPay();
            return Json(result);
        }
    }
}