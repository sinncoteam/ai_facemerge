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
            UserModel um = new UserModel();
            um.Id =  ViData.DataHelper.Fill("select * from t_d_user").Rows.Count;

            return um;
        }
    }
}
