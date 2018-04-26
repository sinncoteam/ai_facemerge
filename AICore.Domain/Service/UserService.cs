using AICore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AICore.Domain.Service
{
    public class UserService
    {
        public UserModel getModel(int id)
        {

            Dapper.SqlMapper.Query<UserModel>(null, "");
            return null;
        }
    }
}
