using AICore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViData;

namespace AICore.Domain.Service
{
    public class UserService : Repository<UserModel, UserModel>
    {
        public UserModel getModel(int id)
        {
            UserModel um = this.Get(a => a.Id == id).FirstOrDefault();    
            return um;
        }

        
        public UserModel getModel(string openid)
        {
            UserModel um = this.Get(a => a.openid == openid).FirstOrDefault();
            if( um != null)
            {
                return um;
            }
            return null;
        }

        // 通过opneid注册帐号
        public int setModel(UserModel model)
        {
            // var model = getModel(openid);
            // if( model == null)
            // {
            //     model = new UserModel();
            //     model.openid = openid;
            //     model.NickName = nickname;
            //     model.UserLogo = userlogo;
            //     model.CreateTime = DateTime.Now;
            // }
            return Convert.ToInt32(this.Insert(model));
        }
    }
}
