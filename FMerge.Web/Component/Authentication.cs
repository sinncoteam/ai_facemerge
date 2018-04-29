using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AICore.Domain.Model;
using AICore.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FMerge.Web.Component
{
    public class Authentication
    {
        public const string UserSessionKey = "xx_user_info";
        static Authentication()
        {
        }
        static UserService x_uService = new UserService();
        private static Authentication instance = new Authentication();
        public static Authentication Instance
        {
            get
            {
                return instance;
            }
        }

        public UserModel CurrentUser
        {
            get
            {
                if (!IsLogin)
                {
                    return null;
                }

                string umodel = BaseAI.HttpContext.Current.Session.GetString(UserSessionKey);
                UserModel u = JsonConvert.DeserializeObject<UserModel>(umodel);
                return u;
                // int uid;
                // if (int.TryParse(BaseAI.HttpContext.Current.User.Identity.Name, out uid))
                // {
                //     if (string.IsNullOrEmpty(umodel))
                //     { 
                //         SetSession(uid);
                //     }
                //     return JsonConvert.DeserializeObject<UserModel>(umodel);
                // }
                // return null;
            }
        }

        public bool IsLogin
        {
            get
            {
                //return BaseAI.HttpContext.Current.User.Identity.IsAuthenticated;
                return !string.IsNullOrEmpty(BaseAI.HttpContext.Current.Session.GetString(UserSessionKey));
                //return HttpContext.Current.Request.IsAuthenticated;
            }
        }

        public void SignOut()
        {
            // BaseAI.HttpContext.Current.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            BaseAI.HttpContext.Current.Session.Clear();
            // FormsAuthentication.SignOut();
            // HttpContext.Current.Session[UserSessionKey] = null;
            // HttpContext.Current.Session.Clear();
         
        }

        public void SetAuth(UserModel user)
        {
            int uid = user.Id;
            // var cc = new System.Security.Principal.GenericIdentity(uid.ToString());
            // var dd = new System.Security.Claims.ClaimsIdentity(cc);
            // BaseAI.HttpContext.Current.User.AddIdentity(dd);     
            // BaseAI.HttpContext.Current.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(){ 
                
            //  } );
            SetSession(uid);
        }

        public void RefreshSession(int userId)
        {
            SetSession(userId);           
        }



        /// <summary>
        ///保存用户状态
        /// </summary>
        /// <param name="userId"></param>
        void SetSession(int userId)
        {
            //     UserInfo u = omService.GetById(userId);
            //     if (u == null)
            //     {
            //         if (System.Web.HttpContext.Current.Request.Path != null && !System.Web.HttpContext.Current.Request.Path.ToLower().Contains("user/logout"))
            //             System.Web.HttpContext.Current.Response.Redirect("/user/logout");
            //         return;
            //     }
            //     HttpContext.Current.Session[UserSessionKey] = u;
            UserModel um = new UserModel(){ Id = userId, NickName = "常玉生", openid = "xxxxxxxxxxxx"};
            string umstr = JsonConvert.SerializeObject(um);
            BaseAI.HttpContext.Current.Session.SetString(UserSessionKey,umstr);
        }

    }
}