using System;

namespace AICore.Domain.Model
{
    public class UserPhotoMergeModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string openid { get; set; }

        public string LocalPhotoUrl { get; set; }

        public string PhotoModel { get; set; }

        public string PhotoResultUrl { get; set; }
        public string PhotoExt { get; set; }

        // 0 未执行， 1 已合成， 2 合成失败
        public int Status { get; set; }

        public string StatusSummary { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime MergeTime { get; set; }

        public string RealName { get; set; }
        public string School { get; set; }
        public string DateYear { get; set; }
    }
}