﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.DrawingCore.Drawing2D;
using System.IO;
using System.DrawingCore.Text;
using System.Text.RegularExpressions;

namespace AICore.Utils
{
    public enum ThumbnailType
    {
        /// <summary>
        /// 头像
        /// </summary>
        Logo,
        /// <summary>
        /// 照片
        /// </summary>
        Photo
    }

    public class ThumbnailHelper
    {
        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="fileName">源图片</param>
        /// <param name="newFileName">缩略图保存路径</param>
        /// <param name="width">缩略图宽度</param>
        public static int MakeThumbnailImage(string fileName, string newFileName, int width)
        {
            Bitmap bmp = new Bitmap(fileName);
            int rotate = 0;
            foreach (var prop in bmp.PropertyItems)
            {
                if (prop.Id == 0x112)
                {
                    if (prop.Value[0] == 6)
                        rotate = 90;
                    if (prop.Value[0] == 8)
                        rotate = -90;
                    if (prop.Value[0] == 3)
                        rotate = 180;
                    break;
                }
            }
            switch (rotate)
            {
                case 90: bmp.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                case -90: bmp.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                case 180: bmp.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
            }

            if (bmp.Width > width)
            {
                double scale = (double)bmp.Width / width;
                double height = (double)bmp.Height / scale;

                //Image.ThumbMode mode = mode = Image.ThumbMode.Max;
                ContentAlignment contentAlignment = ContentAlignment.TopLeft;
                Bitmap image = ImageHelper.Thumbnail(bmp, new Size(width, (int)height), contentAlignment, ThumbnailType.Photo);
                ImageHelper.SaveIamge(image, 80L, newFileName);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 按用户指定长宽缩放图片
        /// </summary>
        /// <param name="fileName">源图片</param>
        /// <param name="newFileName">缩略图保存路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int width, int height, ThumbnailType type = ThumbnailType.Photo)
        {
            Bitmap bmp = new Bitmap(fileName);
            ImageHelper.ThumbMode mode = mode = ImageHelper.ThumbMode.Max;
            ContentAlignment contentAlignment = ContentAlignment.TopLeft;
            Bitmap image = ImageHelper.Thumbnail(bmp, new Size(width, height), contentAlignment, type);
            ImageHelper.SaveIamge(image, 100L, newFileName);
        }


    }//end class

    public class ImageHelper
    {
        /// <summary>
        /// 缩略模式。
        /// </summary>
        public enum ThumbMode : byte
        {
            /// <summary>
            /// 完整模式
            /// </summary>
            Full = 1,
            /// <summary>
            /// 最大尺寸
            /// </summary>
            Max
        }

        /// <summary>
        /// 缩略图。
        /// </summary>
        /// <param name="image">要缩略的图片</param>
        /// <param name="size">要缩放的尺寸</param>
        /// <param name="mode">缩略模式</param>
        /// <param name="contentAlignment">对齐方式</param>
        /// <returns>返回已经缩放的图片。</returns>
        public static Bitmap Thumbnail(Bitmap image, Size size, ContentAlignment contentAlignment, ThumbnailType type)
        {

            if (image.Width <= size.Width && image.Height <= size.Height)
            {
                if (type == ThumbnailType.Photo)
                    return image;
            }

            if (!size.IsEmpty && !image.Size.IsEmpty && !size.Equals(image.Size))
            {
                //先取一个宽比例。
                double scale = (double)image.Width / (double)size.Width;

                if (image.Height > image.Width)
                    scale = (double)image.Height / (double)size.Height;
                //缩略模式
                //switch (mode)
                //{
                //    case ThumbMode.Full:
                //        if (image.Height > image.Width)
                //            scale = (double)image.Height / (double)size.Height;
                //        break;
                //    case ThumbMode.Max:
                //        if (image.Height / scale < size.Height)
                //            scale = (double)image.Height / (double)size.Height;
                //        break;
                //}

                Size newSzie = new Size((int)(image.Width / scale), (int)(image.Height / scale));
                Bitmap result = new Bitmap(newSzie.Width, newSzie.Height);

                if (type == ThumbnailType.Logo)
                {
                    newSzie = new Size(size.Width, size.Height);
                    result = new Bitmap(newSzie.Width, newSzie.Height);
                }

                using (Graphics g = Graphics.FromImage(result))
                {
                    //背景颜色
                    g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), size));
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    //对齐方式
                    Rectangle destRect;
                    switch (contentAlignment)
                    {
                        case ContentAlignment.TopCenter:
                            destRect = new Rectangle(new Point(-((newSzie.Width - size.Width) / 2), 0), newSzie);
                            break;
                        case ContentAlignment.TopRight:
                            destRect = new Rectangle(new Point(-(newSzie.Width - size.Width), 0), newSzie);
                            break;
                        case ContentAlignment.MiddleLeft:
                            destRect = new Rectangle(new Point(0, -((newSzie.Height - size.Height) / 2)), newSzie);
                            break;
                        case ContentAlignment.MiddleCenter:
                            destRect = new Rectangle(new Point(-((newSzie.Width - size.Width) / 2), -((newSzie.Height - size.Height) / 2)), newSzie);
                            break;
                        case ContentAlignment.MiddleRight:
                            destRect = new Rectangle(new Point(-(newSzie.Width - size.Width), -((newSzie.Height - size.Height) / 2)), newSzie);
                            break;
                        case ContentAlignment.BottomLeft:
                            destRect = new Rectangle(new Point(0, -(newSzie.Height - size.Height)), newSzie);
                            break;
                        case ContentAlignment.BottomCenter:
                            destRect = new Rectangle(new Point(-((newSzie.Width - size.Width) / 2), -(newSzie.Height - size.Height)), newSzie);
                            break;
                        case ContentAlignment.BottomRight:
                            destRect = new Rectangle(new Point(-(newSzie.Width - size.Width), -(newSzie.Height - size.Height)), newSzie);
                            break;
                        default:
                            destRect = new Rectangle(new Point(0, 0), newSzie);
                            break;
                    }
                    g.DrawImage(image, destRect, new Rectangle(new Point(0, 0), image.Size), GraphicsUnit.Pixel);
                    image.Dispose();
                }
                return result;
            }
            else
                return image;
        }

        /// <summary>
        /// 保存图片。
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="quality">品质（1L~100L之间，数值越大品质越好）</param>
        /// <param name="filename">保存路径</param>
        public static void SaveIamge(Bitmap image, long quality, string filename)
        {
            using (EncoderParameters encoderParams = new EncoderParameters(1))
            {
                using (EncoderParameter parameter = (encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality)))
                {
                    ImageCodecInfo encoder = null;
                    //取得扩展名
                    string ext = Path.GetExtension(filename);
                    if (string.IsNullOrEmpty(ext))
                        ext = ".jpg";
                    //根据扩展名得到解码、编码器
                    foreach (ImageCodecInfo codecInfo in ImageCodecInfo.GetImageEncoders())
                    {
                        if (Regex.IsMatch(codecInfo.FilenameExtension, string.Format(@"(;|^)\*\{0}(;|$)", ext), RegexOptions.IgnoreCase))
                        {
                            encoder = codecInfo;
                            break;
                        }
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    image.Save(filename, encoder, encoderParams);
                }
            }
        }

        /// <summary>
        /// 保存图片。
        /// </summary>
        /// <param name="stream">要保存的流</param>
        /// <param name="quality">品质（1L~100L之间，数值越大品质越好）</param>
        /// <param name="filename">保存路径</param>
        public static void SaveIamge(Stream stream, long quality, string filename)
        {
            using (Bitmap bmpTemp = new Bitmap(stream))
            {
                SaveIamge(bmpTemp, quality, filename);
            }
        }

        /// <summary>
        /// 合成图片
        /// </summary>
        /// <param name="file">要合成的图片信息</param>
        /// <param name="fileList">合成的图片列表</param>
        /// <param name="textList">合成的文字列表</param>
        public static void MergeImage(FileMergeSize file, IList<FileMergeSize> fileList, IList<FontTextSize> textList)
        {
            Bitmap bit0 = new Bitmap(file.Width, file.Height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bit0);
            //设置画布背景颜色为白色
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, file.Width, file.Height));
            foreach (var item in fileList)
            {
                using (Bitmap bit1 = new Bitmap(item.FileName))
                {
                    g1.DrawImage(bit1, item.X, item.Y, bit1.Width, bit1.Height);
                }
            }
            foreach (var item in textList)
            {
                g1.DrawString(item.Text, item.TextFont, item.TextBrush, item.X, item.Y);
            }
            bit0.Save(file.FileName);
            bit0.Dispose();
        }
    }

    public class FileMergeSize
    {
        public string FileName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class FontTextSize
    {
        public FontTextSize()
        {
            this.TextBrush = new SolidBrush(Color.Black);
            this.TextFont = new Font("微软雅黑",24);

        }
        public string Text { get; set; }
        public Brush TextBrush { get; set; }
        public Font TextFont { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
