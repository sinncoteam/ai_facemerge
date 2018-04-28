using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace AICore.Utils
{
    public class ImageBase64Helper
    {
        //图片转为base64编码的字符串  
        public static string ImgToBase64String(string Imagefilename)
        {
                Bitmap bmp =new Bitmap(Imagefilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);     
        }

        // 转换为base64
        public static string ImgToBase64String(Bitmap file)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                file.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);  
            }   
        }

        //threeebase64编码的字符串转为图片  
        public static Bitmap Base64StringToImage(string strbase64)
        {
            
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                bmp.Save(@"d:\test.jpg", ImageFormat.Jpeg);

                ms.Close();
                return bmp;
           
        }

        public static string getFileExt(string filename)
        {
            string[] strArr = filename.Split('.');
            string name = strArr[strArr.Length-1];
            return "."+ name;
        }
    }
}
