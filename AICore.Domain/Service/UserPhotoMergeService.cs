using System;
using AICore.Domain.Model;
using System.Linq;
using ViData;
using AICore.Api.FaceMerge;
using System.Threading;

namespace AICore.Domain.Service
{
    public class UserPhotoMergeService : Repository<UserPhotoMergeModel, UserPhotoMergeModel>
    {
        public int addUserPhoto(UserPhotoMergeModel photo)
        {
            return Convert.ToInt32(this.Insert(photo));
        }

        public UserPhotoMergeModel getPhotoWithResult(int id)
        {
            var item = this.GetById(id);
            return item;
        }

        public int PhotoMergeJob()
        {
            string sql = "select * from t_d_userphotomerge where status = 0 and photomodel <> '' order by id limit 10";
            var list = DataHelper.Fill<UserPhotoMergeModel>(sql);
            foreach(var item in list)
            {
                FaceMergeModel pmodel = FaceMergeApi.getResult(item.LocalPhotoUrl, item.PhotoModel);
                if(pmodel.ret == 0)
                {
                    
                    this.Update(() => new UserPhotoMergeModel(){ MergeTime = DateTime.Now, Status = 1, PhotoReusltUrl = pmodel.data.image }, a => a.Id == item.Id);
                }
                else{
                    this.Update(() => new UserPhotoMergeModel(){ MergeTime = DateTime.Now, Status = 2 }, a => a.Id == item.Id);
                }
                Thread.Sleep(100);
            }
            return list.Count;
        }
    }
}