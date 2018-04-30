using System;
using AICore.Domain.Model;
using System.Linq;
using ViData;
using AICore.Api.FaceMerge;
using System.Threading;
using AICore.Utils;

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
            try
            {
                string sql = "select * from t_d_userphotomerge where status = 0 and photomodel <> '' order by id limit 10";
                var list = DataHelper.Fill<UserPhotoMergeModel>(sql);
                foreach (var item in list)
                {
                    try
                    {
                        string localurl = ConfigManager.AppSettings("webroot") + item.LocalPhotoUrl;
                        FaceMergeModel pmodel = FaceMergeApi.getResult(localurl, item.PhotoModel);
                        if (pmodel.ret == 0)
                        {
                            string guid = System.Guid.NewGuid().ToString();
                            string resultpath = "/upload/" + item.UserId + "/" + guid + "." + item.PhotoExt;
                            string resulturl = ConfigManager.AppSettings("webroot") + resultpath;


                            AICore.Utils.ImageBase64Helper.Base64StringToImage(pmodel.data.image, resulturl);
                            this.Update(() => new UserPhotoMergeModel() { MergeTime = DateTime.Now, Status = 1, PhotoResultUrl = resultpath }, a => a.Id == item.Id);
                        }
                        else
                        {
                            this.Update(() => new UserPhotoMergeModel() { MergeTime = DateTime.Now, Status = 2 }, a => a.Id == item.Id);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(100);
                }
                return list.Count;
            }
            catch(Exception ex)
            {
            }
            return 0;
        }
    }
}