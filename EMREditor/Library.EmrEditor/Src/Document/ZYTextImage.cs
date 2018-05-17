using System;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Common;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using DrectSoft.Basic.Paint.NET;
using System.IO;
//using ZYCommon ;
namespace ZYTextDocumentLib
{
    /// <summary>
    /// 电子病历文本文档图片对象,用于表示文档中的一个图片对象
    /// </summary>
    /// <seealso>ZYTextDocumentLib.ZYTextObject</seealso>
    public class ZYTextImage : ZYTextObject, System.IDisposable
    {
        //todo zhouhui
        private PaintPanel m_temppanel = null;
        private System.Drawing.Image myImage = null;

        private string imagecontent = null;

        /// <summary>
        /// 图片宽度和高度是否保持比例
        /// </summary>
        public bool ImageSizeRectLock
        {
            get
            {
                if (System.Math.Abs(base.dblWidthHeightRate - (double)myImage.Width / myImage.Height) < 0.04)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value && myImage != null)
                {
                    base.dblWidthHeightRate = (double)myImage.Width / myImage.Height;
                }
                else
                    base.dblWidthHeightRate = 0;
            }
        }

        /// <summary>
        /// 对象使用的图片对象
        /// </summary>
        public System.Drawing.Image Image
        {
            get { return myImage; }
            set
            {
                myImage = value;
                if (myImage != null)
                {
                    this.Width = myImage.Size.Width;
                    this.Height = myImage.Size.Height;
                }
            }
        }
        /// <summary>
        /// 对象图片来源的URL
        /// </summary>
        public string Src
        {
            get { return myAttributes.GetString(ZYTextConst.c_Src); }
            set { myAttributes.SetValue(ZYTextConst.c_Src, value); }
        }

        /// <summary>
        /// 图片数据是否保存到文档中
        /// </summary>
        public bool SaveInFile
        {
            get { return myAttributes.GetBool(ZYTextConst.c_SaveInFile); }
            set { myAttributes.SetValue(ZYTextConst.c_SaveInFile, value); }
        }
        /// <summary>
        /// 图片类型
        /// </summary>
        public string ImageType
        {
            get { return myAttributes.GetString(ZYTextConst.c_Type); }
            set { myAttributes.SetValue(ZYTextConst.c_Type, value); }
        }

        /// <summary>
        /// 返回XML节点名称
        /// </summary>
        /// <returns>XML节点名称</returns>
        public override string GetXMLName()
        {
            return ZYTextConst.c_Img;
        }


        /// <summary>
        /// 已重载:从XML节点加载对象数据
        /// </summary>
        /// <param name="myElement">XML节点</param>
        /// <returns>操作是否成功</returns>
        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (base.FromXML(myElement))
            {

                this.Width = 30;
                this.Height = 30;
                if (myImage != null)
                {
                    myImage.Dispose();
                    myImage = null;
                    imagecontent = string.Empty;
                }
                if (this.SaveInFile)
                {
                    try
                    {
                        myImage = StringCommon.ImageFromBase64String(myElement.InnerText);
                    }
                    catch
                    {
                        //如果不能加载出图片，则说明是该图片已经被修改过了
                        //需要使用图片编辑器
                        myImage = null;
                    }
                    if (myImage == null)
                    {
                        imagecontent = myElement.InnerText;
                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(imagecontent));
                        m_temppanel.LoadImage(ms);
                        myImage = m_temppanel.CurrentImage;
                    }
                }
                else
                {
                    myImage = ZYTextConst.ImageFromURL(this.Src);
                    //todo
                }

                if (myImage != null)
                {
                    this.Width = myImage.Size.Width;
                    this.Height = myImage.Size.Height;
                }
                if (myElement.HasAttribute(ZYTextConst.c_Width))
                    this.Width = StringCommon.ToInt32Value(myElement.GetAttribute(ZYTextConst.c_Width), intWidth);
                if (myElement.HasAttribute(ZYTextConst.c_Height))
                    this.Height = StringCommon.ToInt32Value(myElement.GetAttribute(ZYTextConst.c_Height), intHeight);
                myAttributes.SetValue(ZYTextConst.c_Width, intWidth.ToString());
                myAttributes.SetValue(ZYTextConst.c_Height, intHeight.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 已重载:保存对象数据到XML节点
        /// </summary>
        /// <param name="myElement">XML节点</param>
        /// <returns>操作是否成功</returns>
        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            switch (myOwnerDocument.Info.SaveMode)
            {
                case 0: // 保存所有数据
                    if (myImage != null)
                    {
                        if (myImage.Size.Width != intWidth)
                            myAttributes.SetValue(ZYTextConst.c_Width, intWidth.ToString());
                        else
                            myAttributes.RemoveAttribute(ZYTextConst.c_Width);

                        if (myImage.Size.Height != intHeight)
                            myAttributes.SetValue(ZYTextConst.c_Height, intHeight.ToString());
                        else
                            myAttributes.RemoveAttribute(ZYTextConst.c_Height);
                    }
                    myAttributes.ToXML(myElement);
                    if (this.SaveInFile && myImage != null)
                    {
                        if (string.IsNullOrEmpty(imagecontent))
                            myElement.InnerText = StringCommon.ImageToBase64String(myImage, System.Drawing.Imaging.ImageFormat.Png);
                        //todo
                        else
                            myElement.InnerText = imagecontent;
                    }
                    return true;
                case 1: // 只保存文本数据
                    break;
                case 2: // 只保存结构化数据
                    break;
            }
            return false;
        }

        /// <summary>
        /// 已重载:图片元素不能转换为文本数据
        /// </summary>
        /// <returns></returns>
        public override bool isTextElement()
        {
            return false;
        }


        /// <summary>
        /// 已重载:处理鼠标移动事件
        /// </summary>
        /// <param name="x">鼠标光标在文档中的X坐标</param>
        /// <param name="y">鼠标光标在文档中的Y坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>是否处理了该事件</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextObject.HandleMouseMove</seealso>
        public override bool HandleMouseMove(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            if (base.HandleMouseMove(x, y, Button))
                return true;
            else
            {
                if (myImage != null && Bounds.Contains(x, y))
                {
                    //Debug.WriteLine("图片宽度:" + myImage.Size.Width + " 高度:" + myImage.Size.Height + "\r\n格式:" + myImage.PixelFormat.ToString()+DateTime.Now.Ticks.ToString());
                    //myOwnerDocument.SetToolTip("图片宽度:" + myImage.Size.Width + " 高度:" + myImage.Size.Height + "\r\n格式:" + myImage.PixelFormat.ToString() , -1 , new System.Drawing.Rectangle(x,y,5,5) );
                    return true;
                }
                return false;
            }
        }


        public override bool HandleMouseDown(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            //MessageBox.Show("HandleMouseDown");
            if (Button == MouseButtons.Right)
            {
                if (this.OwnerDocument.Info.DocumentModel != DocumentModel.Read && this.OwnerDocument.Info.DocumentModel != DocumentModel.Clear)
                {
                    contextMenu.Show(Control.MousePosition);
                }
            }
            return base.HandleMouseDown(x, y, Button);
        }

        public override void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "编辑图片":
                    this.EditImage();
                    this.OwnerDocument.OwnerControl.Refresh();//add by bwy
                    break;
                case "图片另存为...":
                    contextMenu.Close();
                    this.SaveImageAs();
                    break;
                case "1":
                    break;
                case "2":
                    break;
                default:
                    break;
            }
            base.contextMenu_ItemClicked(sender, e);
        }
        /// <summary>
        /// 已重载:重新绘制对象
        /// </summary>
        /// <returns>操作是否成功</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextObject.RefreshView</seealso>
        public override bool RefreshView()
        {
            int x = this.RealLeft;
            int y = this.RealTop;
            if (myImage != null)
            {
                myOwnerDocument.View.DrawImage(myImage, x, y, intWidth, intHeight);
            }
            else
                myOwnerDocument.View.DrawString(ZYTextConst.ERR_ImgFileNotFined, x, y, intWidth, intHeight);
            return base.RefreshView();
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <returns>根据对象属性设置加载图片对象</returns>
        public bool LoadImage()
        {
            switch (this.ImageType)
            {
                case ZYTextConst.c_EMRTextDoc:
                    using (ZYTextDocument myDoc = new ZYTextDocument())
                    {
                        myDoc.View = new DocumentView();
                        myDoc.FromXMLFile(this.Src);
                        myImage = myDoc.GetPreViewImage();
                    }
                    break;
                default:
                    myImage = ZYTextConst.ImageFromURL(this.Src);
                    break;
            }
            if (myImage != null)
            {
                intWidth = myOwnerDocument.PixelToDocumentUnit(myImage.Size.Width);
                intHeight = myOwnerDocument.PixelToDocumentUnit(myImage.Size.Height);
            }
            return true;
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// 
        static ZYTextImage()
        {

        }

        public ZYTextImage()
        {
            myBorder = new ZYTextBorder();
            intWidth = 90;
            intHeight = 90;
            m_temppanel = new PaintPanel();

            //加右键菜单选项
            contextMenu.Items.Clear();
            contextMenu.Items.Add("编辑图片");
            contextMenu.Items.Add("图片另存为...");
        }

        public bool EditImage()
        {
            using (System.Drawing.Bitmap myBMP = new System.Drawing.Bitmap(myImage))
            {
                using (EditImageForm dlg = new EditImageForm())
                {
                    //if (!string.IsNullOrEmpty(imagecontent))
                    //{
                    //    if (dlg.LoadImages(imagecontent) == DialogResult.OK)
                    //    {
                    //        myImage.Dispose();
                    //        myImage = dlg.CurrentImage;
                    //        imagecontent = dlg.ImageContent;

                    //        myOwnerDocument.Modified = true;
                    //        return true;
                    //    }

                    //}
                    //else
                    {
                        #region 解决编辑器中图片尺寸和图片编辑窗体中图片不一致的问题 Modified by wwj 2013-04-22
                        int width = DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(this.Width, GraphicsUnit.Document, GraphicsUnit.Pixel);
                        int height = DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(this.Height, GraphicsUnit.Document, GraphicsUnit.Pixel);
                        Image bmpCopy = new Bitmap(width, height);
                        Graphics g = Graphics.FromImage(bmpCopy);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawImage(myBMP, 0, 0, bmpCopy.Width, bmpCopy.Height);
                        #endregion

                        if (dlg.LoadImages2(bmpCopy) == DialogResult.OK)
                        {
                            myImage.Dispose();
                            myImage = dlg.CurrentImage;
                            imagecontent = dlg.ImageContent;

                            #region 解决在图片编辑窗体中修改图片大小后，在编辑器中没有改变的问题 Modified by wwj 2013-04-22
                            myOwnerDocument.BeginContentChangeLog();
                            width = DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(myImage.Width, GraphicsUnit.Pixel, GraphicsUnit.Document);
                            height = DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(myImage.Height, GraphicsUnit.Pixel, GraphicsUnit.Document);
                            myOwnerDocument.EndContentChangeLog();
                            myOwnerDocument.RefreshLine();
                            myOwnerDocument.UpdateView();
                            #endregion

                            myOwnerDocument.Modified = true;
                            return true;
                        }
                    }

                }
                //using( frmEditImage dlg = new frmEditImage())
                //{
                //    dlg.ShowInTaskbar = false;
                //    dlg.ContentBMP = myBMP ;
                //    dlg.MinimizeBox = false;

                //    dlg.ShowDialog();
                //    if( dlg.BMPChanged )
                //    {
                //        myImage.Dispose();
                //        myImage = dlg.EditedBMP ;
                //        myOwnerDocument.Modified = true;
                //        return true;
                //    }
                //}
            }
            return false;
        }

        public bool SaveImageAs()
        {
            //System.Drawing.Image myImg = (this.EMRDoc.Content.CurrentSelectElement as ZYTextImage).Image;

            using (System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog())
            {
                dlg.OverwritePrompt = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "PNG图片|*.png";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Image.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            return true;
        }

        #region IDisposable 成员

        /// <summary>
        /// 实现 System.IDisposable.Dispose 接口,销毁内置的图片对象
        /// </summary>
        public void Dispose()
        {
            if (myImage != null)
            {
                myImage.Dispose();
                myImage = null;

            }
        }

        #endregion
    }
}
