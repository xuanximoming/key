using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace DrectSoft.Core.ImageManager
{
    public abstract class CompressImage
    {

        #region 临时文件夹Tmep
        /// <summary>
        /// 临时文件夹Tmep
        /// </summary>
        public static string GetTempPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("TEMP");
            }
        }
        #endregion

        #region  压缩图片
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="byteImage">原始图片</param>
        /// <param name="size">压缩大小，单位:KB</param>
        /// <returns></returns>
        public static bool CompressBegin( ref byte[] byteImage,int size)
        {
            try
            {
                string imgTempPath = GetTempPath + "\\" + DateTime.Now.ToString("yyyyMMss_hhmmssms") + ".jpg";
                MemoryStream ms = (new MemoryStream(byteImage));
                Image tempImage = Image.FromStream(ms);
                tempImage.Save(imgTempPath);
                ms.Dispose();

                FileInfo fInfo = new FileInfo(imgTempPath);

                if (fInfo.Length / 1024 > size)
                {
                    for (int i = 100; i > 0; i = i - 3)
                    {
                        string imgNewPath = GetTempPath + "\\" + DateTime.Now.ToString("yyyyMMss_hhmmssms") + i + ".jpg";
                        MakeThumbnail(imgTempPath, imgNewPath, i);

                        fInfo = new FileInfo(imgNewPath);
                        if (fInfo.Length / 1024 < size)
                        {
                            FileStream fs = new FileStream(imgNewPath, FileMode.Open);
                            int length = (int)fs.Length;
                            byteImage = new byte[length];
                            fs.Read(byteImage, 0, length);
                            fs.Dispose();

                            File.Delete(imgNewPath);
                            break;
                        }
                        else
                        {
                            File.Delete(imgNewPath);
                        }

                    }

                }
                File.Delete(imgTempPath);

                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region  生成jpg类型缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="scale">缩图比例</param>    
        private static void MakeThumbnail(string originalImagePath, string thumbnailPath, int scale)
        {
            Image originalImage = Image.FromFile(originalImagePath);


            int towidth = originalImage.Width;
            int toheight = originalImage.Height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //长宽等比例
            toheight = originalImage.Height * scale / 100;
            towidth = originalImage.Width * scale / 100;
                

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion
    }
}
