using System;
using System.Collections.Generic;
using System.Text;

namespace AICore.Domain.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }

        public string UserLogo { get; set; }

        public string openid { get; set; }

        public string RealName { get; set; }
        public string School { get; set; }
        public string DateYear { get; set; }

        public DateTime CreateTime { get; set; }


    }
}
