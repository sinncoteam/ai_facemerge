using System;
using ViCore;
using ViData;

namespace AICore.Domain.Model
{
    public class UserModelMap : DMClassMap<UserModel>
    {
        public UserModelMap()
        {
            Table("t_d_user");
            Id(a => a.Id,"id").Identity();
            Map(a => a.openid);
            Map(a => a.UserName,"username");
            Map(a => a.NickName,"nickname");
            Map(a => a.UserLogo,"userlogo");
            Map(a => a.RealName,"realname");
            Map(a => a.School,"school");
            Map(a => a.DateYear,"dateyear");
            Map(a => a.CreateTime,"createtime");
        }
    }
}