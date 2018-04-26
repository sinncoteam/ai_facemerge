using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AICore.Utils
{
    public class QRCodeHelper
    {
        public static string getQRCode(string content, int width, int height)
        {
            //BarcodeWriter<Bitmap> writer = new BarcodeWriter<Bitmap>();
            //writer.Format = BarcodeFormat.QR_CODE;
            //QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            //{
            //    DisableECI = true,//设置内容编码
            //    CharacterSet = "UTF-8",  //设置二维码的宽度和高度
            //    Width = width,
            //    Height = height,
            //    Margin = 1//设置二维码的边距,单位不是固定像素
            //};
            //writer.Options = options;

            //Bitmap map = writer.Write(content);
            //string map64 = ImageBase64Helper.ImgToBase64String(map);
            //return map64;
            return null;

        }
        
    }
}