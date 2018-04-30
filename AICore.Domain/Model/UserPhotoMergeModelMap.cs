using System;
using ViCore;
using ViData;

namespace AICore.Domain.Model
{
    public class UserPhotoMergeModelMap : DMClassMap<UserPhotoMergeModel>
    {
        public UserPhotoMergeModelMap()
        {
            Table("t_d_userphotomerge");
            Id(a => a.Id,"id").Identity();
            Map(a => a.UserId,"userid");
            Map(a => a.openid);
            Map(a => a.LocalPhotoUrl,"localphotourl");
            Map(a => a.PhotoModel,"photomodel");
            Map(a => a.PhotoResultUrl,"photoresulturl");
            Map(a => a.PhotoExt, "photoext");
            Map(a => a.Status,"status");
            Map(a => a.StatusSummary,"statussummary");
            Map(a => a.CreateTime,"createtime");
            Map(a => a.MergeTime,"mergetime");
        }
    }
}