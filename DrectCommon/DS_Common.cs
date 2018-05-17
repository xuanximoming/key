using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DrectSoft.Core;
using DrectSoft.DSSqlHelper;
using ICSharpCode.SharpZipLib.Zip;
/***********************************************************************************************************************别文进插件*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DrectSoft.Common
{
    /// <summary>
    /// 功能描述：通用操作类
    ///    本类的中大部分方法是对原先代码中整出的
    ///    目的是集中常用操作
    /// 创 建 者：bwj
    /// 创建日期：20121018
    /// </summary>
    public sealed class DS_Common
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static IUser currentUser;

        #region 普通方法 ↓　bwj 2012-10-19
        /// <summary>
        /// 判断是否能PING通数据库服务器--收录并改进
        /// </summary>
        /// <param name="strIp"></param>
        /// <returns></returns>
        public static bool CmdPing(string strIp)
        {
            try
            {
                bool result = false;

                // 参数处理
                if (strIp == null ||
                    strIp.Trim() == string.Empty)
                {
                    return result;
                }


                Process aProcess = new Process();
                //初始化调用进程参数
                aProcess.StartInfo.FileName = "cmd.exe";
                aProcess.StartInfo.UseShellExecute = false;
                aProcess.StartInfo.RedirectStandardInput = true;
                aProcess.StartInfo.RedirectStandardOutput = true;
                aProcess.StartInfo.RedirectStandardError = true;
                aProcess.StartInfo.CreateNoWindow = true;
                //启动
                aProcess.Start();
                //输入ping命令
                aProcess.StandardInput.WriteLine("ping -n 1 " + strIp);
                //输入exit命令
                aProcess.StandardInput.WriteLine("exit");
                //取输出
                string strRst = aProcess.StandardOutput.ReadToEnd();

                if (strRst.IndexOf("(0%") != -1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                aProcess.Close();

                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 根据换行符将字符串转换成字符串数组--新增
        /// 创建人:bwj   2012-10-19
        /// </summary>
        /// <param name="_str">待处理字符串</param>
        /// <returns>返回字符串数组</returns>
        public static string[] StringConvrtToArrBy_N(string _str)
        {
            try
            {
                //参数处理
                if (_str.Trim() == null || _str == string.Empty)
                {
                    return null;
                }
                return _str.Split('\n');

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        #endregion 普通方法 ↑

        /// <summary>
        /// 设置控件背景色
        /// 注：目前只针对 横线框
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="color">设置的颜色</param>
        public static void setBackColor(object sender, Color color)
        {
            if (sender is TextEdit)
            {
                (sender as TextEdit).BackColor = color;
            }
            else if (sender is MemoEdit)
            {
                (sender as MemoEdit).BackColor = color;
            }
            else if (sender is LookUpEdit)
            {
                (sender as LookUpEdit).BackColor = color;
            }
        }

        /// <summary>
        /// 将控件背景色设置为白色
        /// 注：目前只针对 横线框
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="flag">true-进入；false-离开</param>
        public static void setBackColor(object sender, bool flag)
        {
            if (flag)
            {
                setBackColor(sender, Color.LemonChiffon);
            }
            else
            {
                setBackColor(sender, Color.White);
            }
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-11</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void win_KeyPress(KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// 选中-不选中
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void cbx_KeyPress(object sender)
        {
            try
            {
                if (sender is CheckEdit)
                {
                    CheckEdit ch = (CheckEdit)sender;
                    ch.Checked = !ch.Checked;
                }
                else if (sender is CheckBox)
                {
                    CheckBox ch = (CheckBox)sender;
                    ch.Checked = !ch.Checked;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 文本框Enter事件 --- 获取焦点选中内容
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void txt_Enter(object sender)
        {
            try
            {
                if (sender is TextEdit)
                {
                    TextEdit con = (TextEdit)sender;
                    con.SelectAll();
                }
                else if (sender is CheckEdit)
                {
                    CheckEdit con = (CheckEdit)sender;
                    con.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 序号(自增长)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AutoIndex(DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 压缩-------------收录
        /// <auth>sunbwj</auth>
        /// <date>2011-11-1</date>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string ZipEmrXml(string emrContent)
        {
            try
            {
                byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
                MemoryStream ms = new MemoryStream();
                GZipStream dfs = new GZipStream(ms, CompressionMode.Compress, true);
                dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
                dfs.Close();
                ms.Seek(0, SeekOrigin.Begin);
                byte[] buffZipXml = new byte[ms.Length];
                ms.Read(buffZipXml, 0, buffZipXml.Length);
                return Convert.ToBase64String(buffZipXml);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 解压缩-------------收录
        /// <auth>sunbwj</auth>
        /// <date>2011-11-1</date>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string UnzipEmrXml(string emrContent)
        {
            try
            {
                byte[] rbuff = Convert.FromBase64String(emrContent);
                MemoryStream ms = new MemoryStream(rbuff);
                GZipStream dfs = new GZipStream(ms, CompressionMode.Decompress, true);
                StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
                string sXml = sr.ReadToEnd();
                sr.Close();
                dfs.Close();
                return sXml;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 树工具提示(TreeList)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-13</date>
        /// </summary>
        /// <param name="toolTipController">提示工具</param>
        /// <param name="tipTreeList">树</param>
        /// <param name="point">位置</param>
        /// <param name="displayName">提示内容</param>
        public static void InitToolTip(ToolTipController toolTipController, TreeList tipTreeList, Point point, string displayName)
        {
            try
            {
                if (!string.IsNullOrEmpty(displayName.Trim()))
                {
                    toolTipController.SetToolTip(tipTreeList, displayName);
                    toolTipController.SetToolTipIconType(tipTreeList, DevExpress.Utils.ToolTipIconType.Exclamation);
                    toolTipController.ShowBeak = true;
                    toolTipController.ShowShadow = true;
                    toolTipController.Rounded = true;
                    toolTipController.ShowHint(displayName, point);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 图片加载 - 性别
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-15</date>
        /// </summary>
        /// <param name="repositoryItemImageXB">GridList引用对象</param>
        /// <param name="imageListBrxb">图片集</param>
        public static void InitializeImage_XB(RepositoryItemImageComboBox repositoryItemImageXB, ImageList imageListBrxb)
        {
            try
            {
                ImageHelper.GetImageListIllness();
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repositoryItemImageXB.SmallImages = imageListBrxb;
                ImageComboBoxItem ImageComboItemMale = new ImageComboBoxItem("男", "1", 1);
                ImageComboBoxItem ImageComboItemFemale = new ImageComboBoxItem("女", "2", 0);
                ImageComboBoxItem ImageComboItemWeizhi = new ImageComboBoxItem("未知", "3", 2);
                repositoryItemImageXB.Items.Add(ImageComboItemMale);
                repositoryItemImageXB.Items.Add(ImageComboItemFemale);
                repositoryItemImageXB.Items.Add(ImageComboItemWeizhi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 图片加载 - 危重级别
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-15</date>
        /// </summary>
        /// <param name="repositoryItemImageXB">GridList引用对象</param>
        /// <param name="imageListBrxb">图片集</param>
        public static void InitializeImage_WZJB(RepositoryItemImageComboBox repositoryItemImageWZJB, ImageList imageListWZJB)
        {
            try
            {
                ImageHelper.GetImageListIllness();
                imageListWZJB = ImageHelper.GetImageListIllness();
                repositoryItemImageWZJB.SmallImages = imageListWZJB;
                DataTable dt = null;
                if ((dt == null) || (dt.Rows.Count <= 0))
                {
                    ImageComboBoxItem item1 = new ImageComboBoxItem("一般病人", "0", 0);
                    ImageComboBoxItem item2 = new ImageComboBoxItem("危重病人", "1", 1);
                    ImageComboBoxItem item3 = new ImageComboBoxItem("病重病人", "2", 2);
                    repositoryItemImageWZJB.Items.AddRange(new ImageComboBoxItem[] { item1, item2, item3 });
                }
                else
                {
                    ImageComboBoxItem[] imageCombo = new ImageComboBoxItem[dt.Rows.Count];
                    for (int index = 0; index < imageCombo.Length; index++)
                    {
                        ImageComboBoxItem item = new ImageComboBoxItem(dt.Rows[index]["name"].ToString().Trim(), dt.Rows[index]["mxdm"].ToString().Trim(), Convert.ToInt16(dt.Rows[index]["mxdm"].ToString().Trim()));
                        imageCombo[index] = item;
                    }
                    repositoryItemImageWZJB.Items.AddRange(imageCombo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 显示等待框
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-12</date>
        /// <param name="m_WaitDialog">等待框</param>
        /// <param name="caption">提示信息</param>
        public static void SetWaitDialogCaption(WaitDialogForm m_WaitDialog, string caption)
        {
            try
            {
                if (null != m_WaitDialog)
                {
                    if (!m_WaitDialog.Visible)
                    {
                        m_WaitDialog.Visible = true;
                    }
                    m_WaitDialog.Caption = caption;
                }
                else
                {
                    m_WaitDialog = new WaitDialogForm(caption, "请稍候");
                    m_WaitDialog.Show();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 隐藏等待框
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-12</date>
        /// <param name="m_WaitDialog">等待框</param>
        public static void HideWaitDialog(WaitDialogForm m_WaitDialog)
        {
            try
            {
                if (null != m_WaitDialog)
                {
                    m_WaitDialog.Hide();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 读/写InI文件 sunbwj add 2012-11-21
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        private const int BUFFERSIZE = 5000;//缓冲区大小
        /// <summary>
        /// 读InI配置文件
        /// <auth>sunbwj</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        /// <param name="_section">节点</param>
        /// <param name="_key">项</param>
        /// <param name="_path">路径</param>
        /// <returns></returns>
        public static string ReadInIValue(string _section, string _key, string _path)
        {
            try
            {
                string result = "";

                StringBuilder strErrMsg = ReadOrWriteCheck(_section, _key, _path);
                if (strErrMsg.Length != 0)
                {
                    strErrMsg.Insert(0, "读配置文件:\r\n");
                    strErrMsg.AppendLine();
                    strErrMsg.Append("YD_Common.cs → ReadIniValue()");
                    throw new Exception(strErrMsg.ToString());
                }

                StringBuilder temp = new StringBuilder(BUFFERSIZE);

                GetPrivateProfileString(_section, _key, "", temp, BUFFERSIZE, _path);

                result = temp.ToString();

                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 写InI配置文件 
        /// <auth>sunbwj</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        /// <param name="_section">节点</param>
        /// <param name="_key">项</param>
        /// <param name="_value">写入的值</param>
        /// <param name="_path">路径</param>
        public static bool WriteInIValue(string _section, string _key, string _value, string _path)
        {
            try
            {
                bool result = false;

                StringBuilder strErrMsg = ReadOrWriteCheck(_section, _key, _path);
                if (strErrMsg.Length != 0)
                {
                    strErrMsg.Insert(0, "写配置文件:\r\n");
                    strErrMsg.AppendLine();
                    strErrMsg.Append("YD_Common.cs → WriteIniValue()");
                    throw new Exception(strErrMsg.ToString());
                }

                WritePrivateProfileString(_section, _key, _value, _path);

                result = true;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 读/写配置文件前检查
        /// <auth>sunbwj</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        /// <param name="_section">节点</param>
        /// <param name="_key">项</param>
        /// <param name="_path">路径</param>
        /// <returns>错误信息</returns>
        private static StringBuilder ReadOrWriteCheck(string _section, string _key, string _path)
        {
            try
            {
                StringBuilder strErrMsg = new StringBuilder();
                if (_section.Trim() == string.Empty)
                {
                    if (strErrMsg.Length != 0)
                    {
                        strErrMsg.AppendLine();
                    }
                    strErrMsg.Append("节点--必指定");
                }
                if (_key.Trim() == string.Empty)
                {
                    if (strErrMsg.Length != 0)
                    {
                        strErrMsg.AppendLine();
                    }
                    strErrMsg.Append("项--必指定");
                }
                if (_path.Trim() == string.Empty)
                {
                    if (strErrMsg.Length != 0)
                    {
                        strErrMsg.AppendLine();
                    }
                    strErrMsg.Append("路径--必指定");
                }
                else if (!File.Exists(_path))
                {
                    if (strErrMsg.Length != 0)
                    {
                        strErrMsg.AppendLine();
                    }
                    strErrMsg.Append("配置文件不存在");
                }
                return strErrMsg;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        #endregion
        #region ZIP文件压缩解压 sunbwj add 2012-11-22
        /// <summary>
        /// 压缩前处理
        /// <auth>sunbwj add</auth>
        /// <date>2012-11-22</date>
        /// </summary>
        /// <param name="_srcFile"></param>
        /// <param name="_objFile"></param>
        /// <param name="iscompresultsion"></param>
        /// <returns></returns>
        private static StringBuilder PreZipCheck(string _srcFile, string _objFile, bool iscompresultsion)
        {
            try
            {
                StringBuilder strErr = new StringBuilder();
                if (_srcFile.Trim() == "")
                {
                    if (strErr.Length != 0)
                    {
                        strErr.AppendLine();
                    }
                    if (iscompresultsion)
                    {
                        strErr.Append("待压缩文件--必须指定");
                    }
                    else
                    {
                        strErr.Append("待解压文件--必须指定");
                    }
                }
                else if (!File.Exists(_srcFile))
                {
                    if (strErr.Length != 0)
                    {
                        strErr.AppendLine();
                    }
                    if (iscompresultsion)
                    {
                        strErr.Append("待压缩文件--不存在");
                    }
                    else
                    {
                        strErr.Append("待解压文件--不存在");
                    }
                }
                if (_objFile.Trim() == "")
                {
                    if (strErr.Length != 0)
                    {
                        strErr.AppendLine();
                    }
                    if (iscompresultsion)
                    {
                        strErr.Append("压缩后文件名--必须指定");
                    }
                    else
                    {
                        strErr.Append("解压后文件路径(或名称)--必须指定");
                    }
                }
                return strErr;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 压缩或解压密码
        /// <auth>sunbwj add</auth>
        /// <date>2012-11-22</date>
        /// </summary>
        public static string ZipPwd = "";

        /// <summary>
        /// 解压文件
        /// <auth>sunbwj add</auth>
        /// <date>2012-11-22</date>
        /// </summary>
        /// <param name="_zipfile">压缩文件的名称，如：C:\123\123.zip</param>
        /// <param name="_objdir">要解压的文件夹路径</param>
        /// <returns></returns>
        public static bool DecompressionZip(string _zipfile, string _objdir)
        {
            try
            {
                bool result = false;

                StringBuilder strErr = PreZipCheck(_zipfile, _objdir, false);
                if (strErr.Length != 0)
                {
                    strErr.Insert(0, "ZIP解压异常:\r\n");
                    strErr.AppendLine();
                    strErr.Append("YD_Common.cs ->DecompressionZip()");

                    throw new Exception(strErr.ToString());
                }

                _objdir = _objdir.Replace("/", "\\");
                if (!_objdir.EndsWith("\\"))
                {
                    _objdir += "\\";
                }

                if (!Directory.Exists(_objdir))
                {
                    Directory.CreateDirectory(_objdir);
                }
                ZipInputStream aZipInputStream = new ZipInputStream(File.OpenRead(_zipfile));
                if (ZipPwd != "")
                {
                    aZipInputStream.Password = ZipPwd;
                }
                ZipEntry theEntry;
                while ((theEntry = aZipInputStream.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(_objdir + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(_objdir + theEntry.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = aZipInputStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                aZipInputStream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 压缩文件
        /// <auth>sunbwj add</auth>
        /// <date>2012-11-22</date> 
        /// </summary>
        /// <param name="_fileToZip"></param>
        /// <param name="_zipedFile"></param>
        /// <returns></returns>
        public static bool CompresultsionZip(string _fileToZip, string _zipedFile)
        {
            FileStream ZipFile = null;
            ZipOutputStream ZipStream = null;
            ZipEntry ZipEntry = null;
            try
            {

                bool result = false;

                StringBuilder strErr = PreZipCheck(_fileToZip, _zipedFile, true);

                if (strErr.Length != 0)
                {
                    strErr.Insert(0, "ZIP压缩异常:\r\n");
                    strErr.AppendLine();
                    strErr.Append("YD_Common.cs ->CompresultsionZip()");

                    throw new Exception(strErr.ToString());
                }

                //读待压缩文件
                ZipFile = File.OpenRead(_fileToZip);
                byte[] buffer = new byte[ZipFile.Length];
                ZipFile.Read(buffer, 0, buffer.Length);
                ZipFile.Close();
                //生成缩文件
                ZipFile = File.Create(_zipedFile);
                ZipStream = new ZipOutputStream(ZipFile);
                if (ZipPwd.Trim() != "")
                {
                    ZipStream.Password = ZipPwd;
                }
                ZipEntry = new ZipEntry(Path.GetFileName(_fileToZip));
                ZipStream.PutNextEntry(ZipEntry);
                ZipStream.SetLevel(6);//压缩比例
                ZipStream.Write(buffer, 0, buffer.Length);
                ZipStream.Flush();
                //ZipFile.Close();
                result = true;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                if (ZipEntry != null)
                {
                    ZipEntry = null;
                }

                if (ZipStream != null)
                {
                    ZipStream.Finish();
                    ZipStream.Close();
                }

                if (ZipFile != null)
                {
                    ZipFile.Close();
                    ZipFile = null;
                }

                GC.Collect();
                GC.Collect(1);
            }
        }
        #endregion

        /// <summary>
        /// 判断是否为指定带几位小数的数字   收录
        /// </summary>
        /// <param name="strNumber">判断的数据</param>
        /// <param name="intdecimals">小数位数，9999为任意数</param>
        /// <returns> true是数字，false非数字</returns>
        public static bool IsNumber(String strNumber, int intdecimals)
        {
            try
            {

                string[] pattern = new string[]
                {
                    @"^[0-9]*$", //intdecimals=0
                    @"^[0-9]*[.]?[0-9]?$", // @"(^[0-9]*$)|(^[0-9]*[.]?[0-9]$)", //intdecimals=0
                    @"^[0-9]*[.]?[0-9]*$"  //default @"^[0-9]*.[0-9]*$" 
                };

                Match match;

                switch (intdecimals)
                {
                    case 0: //整数
                        {
                            match = Regex.Match(strNumber, pattern[0]);   // 匹配正则表达式    
                            break;
                        }
                    case 1: //一位小数
                        {
                            match = Regex.Match(strNumber, pattern[1]);   // 匹配正则表达式    
                            break;
                        }
                    default: //任意数字（21 、23.33）
                        {
                            match = Regex.Match(strNumber, pattern[pattern.Length - 1]);   // 匹配正则表达式    
                            break;
                        }
                }

                return match.Success;//false不是数字,true是数字
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将List拼接成查询字符串
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-12</date>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string CombineSQLStringByList(List<string> list)
        {
            try
            {
                string sqlStr = string.Empty;
                if (null == list || list.Count() == 0)
                {
                    return sqlStr;
                }
                foreach (string str in list)
                {
                    sqlStr += "'" + str + "',";
                }
                if (!string.IsNullOrEmpty(sqlStr))
                {
                    sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
                }
                return sqlStr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-18</date>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterSpecialCharacter(string content)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    content = content.Replace("'", "''");
                    string guid = Guid.NewGuid().ToString();
                    if (content.Contains("]"))
                    {
                        content = content.Replace("]", guid);
                    }
                    if (content.Contains("["))
                    {
                        content = content.Replace("[", "[[]");
                    }
                    if (content.Contains(guid))
                    {
                        content = content.Replace(guid, "[]]");
                    }
                    content = content.Replace("*", "[*]").Replace("%", "[%]");
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除特殊字符
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-18</date>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string DeleteSpecialCharacter(string content)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    content = content.Replace("'", "").Replace("[", "").Replace("]", "").Replace("*", "").Replace("%", "");
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将时间间隔转格式(天,时,分)
        ///  xlb 2013-01-15
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string TimeSpanToLocal(TimeSpan timeSpan)
        {
            try
            {
                int days = timeSpan.Days;
                int hours = timeSpan.Hours;
                int mins = timeSpan.Minutes;
                string chnSpan = string.Empty;
                if (days > 0)
                {
                    chnSpan += days.ToString() + "天";
                }
                else
                {
                    chnSpan += "0" + "天";
                }
                if (hours > 0)
                {
                    chnSpan += hours.ToString() + "时";
                }
                else
                {
                    chnSpan += "0" + "时";
                }
                if (mins > 0)
                {
                    chnSpan += mins.ToString() + "分";
                }
                else
                {
                    chnSpan += "0" + "分";
                }
                return chnSpan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清除指定区域内容
        ///    Dev控件
        /// xlb 2013-01-15
        /// </summary>
        /// <param name="control"></param>
        public static void ClearControl(Control control)
        {
            try
            {
                if (control.Controls.Count <= 0)
                {
                    return;
                }
                foreach (Control ctrl in control.Controls)
                {
                    Type type = ctrl.GetType();
                    if (type == typeof(TextEdit))
                    {
                        TextEdit textEdit = ctrl as TextEdit;
                        if (textEdit != null)
                        {
                            textEdit.Text = string.Empty;
                        }
                    }
                    else if (type == typeof(ComboBoxEdit))
                    {
                        ComboBoxEdit cmbEdit = ctrl as ComboBoxEdit;
                        if (cmbEdit != null)
                        {
                            cmbEdit.SelectedIndex = -1;
                        }
                    }
                    else if (type == typeof(LookUpEdit))
                    {
                        LookUpEdit lookUpEdit = ctrl as LookUpEdit;
                        if (lookUpEdit != null)
                        {
                            lookUpEdit.EditValue = string.Empty;
                        }
                    }
                    else if (type == typeof(SpinEdit))
                    {
                        SpinEdit spinEdit = ctrl as SpinEdit;
                        if (spinEdit != null)
                        {
                            spinEdit.Text = string.Empty;
                        }
                    }
                    else if (type == typeof(MemoEdit))
                    {
                        MemoEdit memoEdit = ctrl as MemoEdit;
                        if (memoEdit != null)
                        {
                            memoEdit.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 屏蔽第三方控件右键菜单方法
        /// Add xlb 2013-02-29
        /// 常用控件屏蔽 可添加
        /// </summary>
        /// <param name="control"></param>
        /// <param name="contextMenuStrip">windows右键菜单控件</param>
        public static void CancelMenu(Control control, ContextMenuStrip contextMenuStrip)
        {
            try
            {
                foreach (Control ctrl in control.Controls)
                {
                    Type type = ctrl.GetType();
                    if (type == typeof(LabelControl))
                    {
                        continue;
                    }
                    else if (type == typeof(TextEdit))
                    {
                        TextEdit textEdit = ctrl as TextEdit;
                        if (textEdit != null)
                        {
                            textEdit.Properties.ContextMenuStrip = contextMenuStrip;
                        }
                    }
                    else if (type == typeof(MemoEdit))
                    {
                        MemoEdit memoEdit = ctrl as MemoEdit;
                        if (memoEdit != null)
                        {
                            memoEdit.Properties.ContextMenuStrip = contextMenuStrip;
                        }
                    }
                    else if (type == typeof(SpinEdit))
                    {
                        SpinEdit spinEdit = ctrl as SpinEdit;
                        if (spinEdit != null)
                        {
                            spinEdit.Properties.ContextMenuStrip = contextMenuStrip;
                        }
                    }
                    else if (type == typeof(DateEdit))
                    {
                        DateEdit dateEdit = ctrl as DateEdit;
                        if (dateEdit != null)
                        {
                            dateEdit.Properties.ContextMenuStrip = contextMenuStrip;
                        }
                    }
                    else if (type == typeof(ComboBoxEdit))
                    {
                        ComboBoxEdit cmbEdit = ctrl as ComboBoxEdit;
                        if (cmbEdit != null)
                        {
                            cmbEdit.Properties.ContextMenuStrip = contextMenuStrip;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-21</date>
        /// <returns>192.168.1.100 格式</returns>
        public static string GetIPHost()
        {
            try
            {
                string ipStr = string.Empty;
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                if (null != IpEntry && null != IpEntry.AddressList)
                {
                    IPAddress ipAddr = null;
                    if (IpEntry.AddressList.Length > 2)
                    {//本机IP
                        ipAddr = IpEntry.AddressList[2];
                    }
                    else if (IpEntry.AddressList.Length > 0)
                    {//物理IP
                        ipAddr = IpEntry.AddressList[0];
                    }
                    ipStr = null == ipAddr ? "" : ipAddr.ToString();
                }
                return ipStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// 检测一个字符串是不是以汉字开始  
        /// </summary>  
        /// <param name="str">要检测的字符串</param>  
        /// <returns>是否为汉字开始</returns>  
        private static bool isChinese(string str)
        {
            if ((int)str[0] > 0x4E00 && (int)str[0] < 0x9FA5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 将字符串截取成打印宽度一致的List
        /// 标点后置 单位前置
        /// xll 2013-03-28
        /// </summary>
        /// <param name="graphics">用于画图的界面</param>
        /// <param name="allStr">原字符</param>
        /// <param name="pixnum">画布宽度</param>
        /// <param name="DanWeilist">单位集合</param>
        /// <param name="BiaoDianList">标点集合</param>
        /// <returns></returns>
        public static List<string> GetStrEquallong(Graphics graphics, string allStr, float pixnum, List<string> DanWeilist, List<char> BiaoDianList)
        {
            try
            {
                if (allStr == null || string.IsNullOrEmpty(allStr))
                {
                    return new List<string>();
                }
                Font font = new System.Drawing.Font("宋体", 10);
                float widthf = 0f;
                string duanStr = ""; //截取字符串
                StringBuilder strbu = new StringBuilder();
                for (int i = 0; i < allStr.Length; i++)
                {
                    if (allStr[i] == '\n')
                    {
                        duanStr += allStr[i];
                        strbu.Append(duanStr);
                        widthf = 0f;
                        duanStr = "";
                        continue;
                    }
                    float widthtmp = 0f;
                    string lastStr = allStr.Substring(i); //剩下的字符
                    if (!isChinese(allStr[i].ToString())) //字母和数字为汉字的一半 by Ukey 2017-03-28
                    {
                        widthtmp = graphics.MeasureString("夺", font).Width / 2;
                    }
                    else
                    {
                        widthtmp = graphics.MeasureString(allStr[i].ToString(), font).Width;
                    }

                    widthf += widthtmp;//graphics.MeasureString(allStr[i].ToString(), font).Width;
                    duanStr += allStr[i];

                    bool findDanwei = false;
                    if (DanWeilist != null && DanWeilist.Count > 0)
                    {
                        foreach (var item in DanWeilist)
                        {
                            int aa = lastStr.ToUpper().IndexOf(item.ToUpper());
                            if (aa == 1)
                            {
                                float hiwidth = widthf + graphics.MeasureString(item, font).Width;
                                if (hiwidth > pixnum)
                                {
                                    strbu.AppendLine(duanStr);
                                    widthf = 0f;
                                    duanStr = "";
                                    findDanwei = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (findDanwei)
                    {
                        continue;
                    }

                    if (widthf > pixnum && i != allStr.Length - 1)  //最后一个字符不做以下判断
                    {
                        if (BiaoDianList != null && BiaoDianList.Count > 0)
                        {
                            if (BiaoDianList.Contains(allStr[i + 1]))
                            {
                                duanStr += allStr[i + 1];
                                i++;
                            }
                        }

                        strbu.AppendLine(duanStr);
                        widthf = 0f;
                        duanStr = "";
                    }
                    else if (i == allStr.Length - 1)
                    {
                        strbu.Append(duanStr);
                        widthf = 0f;
                        duanStr = "";
                    }
                }
                return strbu.ToString().Split('\n').ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 设置控件属性方法参照IemControlDefault表 ↓ Add by xlb 2013-07-01

        /// <summary>
        /// <Auth>XLB</Auth>
        /// <date>2013-07-01</date>
        /// 设置控件属性默认值
        /// </summary>
        public static void SetDefaultValue(Form form)
        {
            try
            {
                Type type = form.GetType();
                //获取指定路径的配置控件集合
                DataTable dataControls = GetControls(type.ToString());
                object obj;
                if (dataControls != null && dataControls.Rows.Count > 0)
                {
                    foreach (DataRow item in dataControls.Rows)
                    {
                        obj = Assembly.Load("Common.Ctrs").CreateInstance(item["CONTROLSTYLE"].ToString().Trim(), true);
                        //匹配指定名属性
                        Control control = form.Controls.Find(item["Controlname"].ToString(), true).FirstOrDefault();
                        if (control == null)
                        {
                            return;
                        }
                        //设置指定属性默认值
                        ReflectionSetProperty(control as object, item["PROPERTY"].ToString().Trim(), (object)item["CONTROLVALUE"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置控件属性默认值
        /// Add by xlb 2013-07-01
        /// </summary>
        /// <param name="userControl">用户控件</param>
        public static void SetDefaultValue(UserControl userControl)
        {
            try
            {
                Type type = userControl.GetType();
                //获取指定路径下的表控件集合
                DataTable dataControls = GetControls(type.ToString());
                object obj;
                if (dataControls != null && dataControls.Rows.Count > 0)
                {
                    foreach (DataRow item in dataControls.Rows)
                    {
                        obj = Assembly.Load("Common.Ctrs").CreateInstance(item["CONTROLSTYLE"].ToString().Trim(), true);
                        //匹配指定名属性
                        Control control = userControl.Controls.Find(item["Controlname"].ToString(), true).FirstOrDefault()/*返回结果中的第一个元素或默认值*/;
                        if (control == null)
                        {
                            return;
                        }
                        //设置指定属性默认值
                        ReflectionSetProperty(control as object, item["PROPERTY"].ToString().Trim(), (object)item["CONTROLVALUE"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改指定控件指定属性值
        /// </summary>
        /// <param name="objclass">当前控件</param>
        /// <param name="propertyName">属性值</param>
        /// <param name="value">属性值</param>
        private static void ReflectionSetProperty(object objclass, string propertyName, object value)
        {
            try
            {
                //获取当前对象的所有属性
                PropertyInfo[] infos = objclass.GetType().GetProperties();
                foreach (PropertyInfo item in infos)
                {
                    //遍历属性找到指定属性
                    if (item.Name == propertyName && item.CanWrite/*属性是否可写*/)
                    {
                        if (!item.PropertyType.IsGenericType)
                        {
                            //获取当前属性的类型若为非泛型则转换成属性类型的对象
                            if (item.PropertyType.IsEnum)
                            {
                                //枚举类型转换
                                object objValue = Enum.Parse(item.PropertyType, value.ToString(), true);
                                item.SetValue(objclass, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(objValue, item.PropertyType), null);
                            }
                            else if (item.PropertyType == typeof(System.Drawing.Color))
                            {
                                /*设置值作为颜色结构体一个属性并返回*/
                                item.SetValue(objclass, CreateColor(value), null);
                            }
                            else
                            {
                                item.SetValue(objclass, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(value, item.PropertyType), null);
                            }
                        }
                        else
                        {
                            //泛型
                            Type type = item.PropertyType.GetGenericTypeDefinition();
                            if (type == typeof(Nullable<>))
                            {
                                item.SetValue(objclass, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(item.PropertyType)), null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据字符生成对应的颜色
        /// <auth>XLB</auth>
        /// <date>2013-07-01</date>
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        private static Color CreateColor(object colorName)
        {
            try
            {
                //获取结构体类型
                Type colorType = typeof(Color);

                //匹配字符串指定的属性
                PropertyInfo property = colorType.GetProperty
                (colorName.ToString(), BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static
                | BindingFlags.DeclaredOnly);

                if (property == null)
                {
                    throw new Exception("属性配置错误");
                }
                //返回字符串指定的颜色
                return (Color)property.GetValue(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取路径获取路径下所有的控件
        /// Add by xlb 2013-07-01
        /// 后期若有底层库应提取此方法
        /// 由于Service已引用当前dll故不能互相引用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DataTable GetControls(string path)
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                DataTable controlData = DS_SqlHelper.ExecuteDataTable(@"select * from iemcontroldefault where valid='1' and controlsrc=@controlSrc",
                new SqlParameter[] { new SqlParameter("@controlSrc", path) }, CommandType.Text);
                return controlData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// <Date>2013-07-15</Date>
        /// 数据集为空时显示自定义字符
        /// <Auth>XLB</Auth>
        /// </summary>
        /// <param name="str">自定义字符</param>
        /// <param name="font">文本格式</param>
        /// <param name="brush">刷子</param>
        /// <param name="e"></param>
        public static void CustomDrawEmptyDataSource(string str, Font font, Brush brush, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            try
            {
                //定义文本布局方式
                using (StringFormat sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Rectangle rect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawString(str, font, brush, rect, sf);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
