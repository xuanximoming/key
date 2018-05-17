using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    internal static class UICommon
    {

        internal static string ShowOpenPictureDialog()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.AddExtension = false;
                ofd.CheckFileExists = true;
                ofd.DefaultExt = "bmp";
                ofd.Filter = "所有图形文件(*.bmp, *.jpg, *.jpeg, *.tiff, *.tif, *.gif)|*.bmp;*.jpg;*.jpeg;*.tiff;*.tif;*.gif|位图(*.bmp)|*.bmp|Jpeg(*.jpg, *.jpeg)|*.jpg;*.jpeg|Gif(*.gif)|*.gif|Tiff(*.tiff, *.tif)|*.tiff;*.tif";
                ofd.FilterIndex = 0;
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = "更换图片"; //Modified by wwj 2013-04-19 将“加载背景图片” 改为“更换图片” 
                DialogResult dr = ofd.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return string.Empty;
                else
                    return ofd.FileName;
            }
        }

        internal static Brush GetBrush(string name,
            Color foreColor, Color backColor,
            NamedHatchStyles nhs, NamedTextureStyles nts)
        {
            if (nhs != null)
            {
                HatchStyle? hatch = nhs.GetHatchStyle(name);
                if (hatch.HasValue)
                    return new HatchBrush(hatch.Value, foreColor, backColor);
            }
            if (nts != null)
            {
                Picture pic = nts.GetTexture(name);
                if (pic != null)
                {
                    Image img = pic.GetImage();
                    if (img != null)
                        return new TextureBrush(img);
                }
            }
            return new SolidBrush(foreColor);
        }

    }
}
