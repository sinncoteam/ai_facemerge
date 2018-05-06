using System;
using AICore.Domain.Model;
using System.Linq;
using ViData;
using AICore.Api.FaceMerge;
using System.Threading;
using AICore.Utils;
using System.IO;
using System.Collections.Generic;
using System.DrawingCore;

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
                        string webroot = ConfigManager.AppSettings("webroot");
                        string localurl = webroot + item.LocalPhotoUrl;
                        if (!File.Exists(localurl))
                        {
                            this.Delete(a => a.Id == item.Id);
                        }
                        FaceMergeModel pmodel = FaceMergeApi.getResult(localurl, item.PhotoModel);
                        if (pmodel.ret == 0)
                        {
                            string guid = System.Guid.NewGuid().ToString();
                            string resultpath = "/upload/" + item.UserId + "/" + guid + "." + item.PhotoExt;
                            string resulturl = webroot + resultpath;

                            string resultpath2 = "/upload/" + item.UserId + "/" + guid + "_x." + item.PhotoExt;
                            string resulturl2 = webroot + resultpath2;

                            AICore.Utils.ImageBase64Helper.Base64StringToImage(pmodel.data.image, resulturl);
                            try
                            {
                                setLocalMerge(resulturl2, resulturl, webroot + "/qr.png", item.RealName, item.DateYear, item.School);
                            }
                            catch { }
                            this.Update(() => new UserPhotoMergeModel() { MergeTime = DateTime.Now, Status = 1, PhotoResultUrl = resultpath2 }, a => a.Id == item.Id);
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
            catch (Exception ex)
            {
            }
            return 0;
        }

        void setLocalMerge(string newfilepath, string photopath, string qrpath, string realname, string dateyear, string school)
        {
            FileMergeSize fs = new FileMergeSize();
            fs.Width = 579;
            fs.Height = 680;
            fs.FileName = newfilepath;
            List<FileMergeSize> fslist = new List<FileMergeSize>();
            FileMergeSize fs1 = new FileMergeSize();
            fs1.FileName = photopath;
            FileMergeSize fs2 = new FileMergeSize();
            fs2.FileName = qrpath;
            fs2.X = 420;
            fs2.Y = 540;
            fslist.Add(fs1);
            fslist.Add(fs2);
            List<FontTextSize> ftlist = new List<FontTextSize>();
            FontTextSize ft1 = new FontTextSize();
            ft1.Text = "我是";
            ft1.Y = 540;
            FontTextSize ft2 = new FontTextSize();
            ft2.Text = realname;
            ft2.TextBrush = new SolidBrush(Color.Red);
            ft2.X = 64;
            ft2.Y = 540;
            FontTextSize ft3 = new FontTextSize();
            ft3.Text = dateyear;
            ft3.Y = 580;
            ft3.TextBrush = new SolidBrush(Color.Red);
            FontTextSize ft4 = new FontTextSize();
            ft4.Text = "毕业于";
            //ft4.X = ft3.Text.Length * 30;
            ft4.Y = 620;
            FontTextSize ft5 = new FontTextSize();
            ft5.Text = school;
            ft5.X = 96;
            ft5.Y = 620;
            ft5.TextBrush = new SolidBrush(Color.Red);

            ftlist.Add(ft1);
            ftlist.Add(ft2);
            ftlist.Add(ft3);
            ftlist.Add(ft4);
            ftlist.Add(ft5);
            ImageHelper.MergeImage(fs, fslist, ftlist);
        }
    }
}