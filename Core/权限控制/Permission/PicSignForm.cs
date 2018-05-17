using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

using System.IO;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
namespace DrectSoft.Core.Permission
{
    /// <summary>
    /// 编辑图片签名窗体  
    /// <summary>
    /// <auth>zyx</auth>
    /// <date>2012-12-12</date>
    /// </summary>
    /// </summary>
    public partial class PicSignForm : DevBaseForm
    {
        private string picString = string.Empty;//图片转换后的字符串

        private DataRow dataRowPicSign;//图片签名信息

        private IUser m_User;

        private IEmrHost m_app;

        /// <summary>
        /// 窗体无参构造
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="app"></param>
        public PicSignForm(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_User = app.User;
                m_app = app;
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 有参构造
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dataRow"></param>
        public PicSignForm(IEmrHost app, DataRow dataRow)
        {
            try
            {
                InitializeComponent();
                m_User = app.User;
                m_app = app;
                dataRowPicSign = dataRow;
                BtnState();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicSignForm_Load(object sender, EventArgs e)
        {
            try
            {

                InitForm(dataRowPicSign);
                BtnState();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        #region 事件
        /// <summary>
        /// 浏览事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = true;
                string srcPath = string.Empty;//图片路径
                ofdBrowse.Filter = "图片文件(*.jpg,*.png)|*.jpg;*.png";
                if (ofdBrowse.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofdBrowse.FileName);
                    if (fi.Length <= 10240)
                    {
                        srcPath = ofdBrowse.FileName.ToString();
                        picString = GetImageFromString(srcPath);
                        pePicSign.Image = CreateImageFromString(picString);
                    }
                    else
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请上传小于10KB的图片！");
                    }
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 保存事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string userPicFlow = dataRowPicSign["USERPICFLOW"].ToString();
                if (string.IsNullOrEmpty(userPicFlow))
                {
                    Insert();
                }
                else
                {
                    Update();
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 把图片转换成字符串
        ///  <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="fromPath">图片路径</param>
        private string GetImageFromString(string fromPath)
        {
            try
            {
                Stream s = File.Open(fromPath, FileMode.Open);
                int leng = 0;
                if (s.Length < Int32.MaxValue)
                {
                    leng = (int)s.Length;
                }
                byte[] by = new byte[leng];
                s.Read(by, 0, leng);//把图片读到字节数组中
                s.Close();

                string str = Convert.ToBase64String(by);//把字节数组转换成字符串
                return str;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 把字符串还原成图片
        /// <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private Image CreateImageFromString(string content)
        {
            try
            {
                if (String.IsNullOrEmpty(content))
                {
                    return null;
                }
                byte[] buf = Convert.FromBase64String(content);//把字符串读到字节数组中
                MemoryStream ms = new MemoryStream(buf);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ms.Close();
                ms.Dispose();
                Bitmap bmp = new Bitmap(img);
                return bmp;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化窗体
        /// <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        /// <param name="dataRow"></param>
        private void InitForm(DataRow dataRow)
        {
            try
            {
                txtUserId.Focus();
                if (dataRow != null)
                {
                    txtUserId.Text = dataRow["UserId"].ToString();
                    txtUserName.Text = dataRow["UserName"].ToString();
                    txtDepartment.Text = dataRow["DepartmentName"].ToString();
                    GetUserPic(dataRow["userpicflow"].ToString());
                    if (!string.IsNullOrEmpty(picString))
                    {
                        pePicSign.Image = CreateImageFromString(picString);
                    }
                }
                else
                {
                    txtUserId.Text = m_app.User.Id;
                    txtUserName.Text = m_app.User.Name;
                    txtDepartment.Text = m_app.User.CurrentDeptName;
                    GetUserPic(txtUserId.Text.Trim().ToString());
                    if (!string.IsNullOrEmpty(picString))
                    {
                        pePicSign.Image = CreateImageFromString(picString);
                    }
                }

            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 增加图片签名
        /// <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        private void Insert()
        {
            try
            {
                string sqlInsert = "insert into userpicsign (userpicflow,userid,createdatetime,createuserid,createusername,valide,userpic) values(@userpicflow,@userid,@createdatetime,@createuserid,@createusername,@valide,@userpic)";
                SqlParameter[] parms = new SqlParameter[] 
                {
                    new SqlParameter("@userpicflow",SqlDbType.NVarChar,50),
                    new SqlParameter("@userid",SqlDbType.NVarChar,20),
                    new SqlParameter("@createdatetime",SqlDbType.NVarChar,20),
                    new SqlParameter("@createuserid",SqlDbType.NVarChar,20),
                    new SqlParameter("@createusername",SqlDbType.NVarChar,20),
                    new SqlParameter("@valide",SqlDbType.NVarChar,1),
                    new SqlParameter("@userpic",SqlDbType.Text)
                };
                parms[0].Value = Guid.NewGuid().ToString();
                parms[1].Value = txtUserId.Text.Trim();
                DateTime now = DateTime.Now;
                parms[2].Value = now.ToString();
                parms[3].Value = m_app.User.Id;
                parms[4].Value = m_app.User.Name;
                if (string.IsNullOrEmpty(picString))
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("签名图片不存在，你确定要保存吗？", "签名图片", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        parms[5].Value = 0;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    parms[5].Value = 1;
                }

                parms[6].Value = picString;
                DS_SqlHelper.ExecuteNonQuery(sqlInsert, parms, CommandType.Text);
                dataRowPicSign["USERPICFLOW"] = parms[0].Value;
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 修改图片签名
        /// <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        private void Update()
        {
            try
            {
                string sqlUpdatePicSign = "update userpicsign set createdatetime=@createdatetime,createuserid=@createuserid,createusername=@createusername, valide=@valide,userpic=@userpic where userpicflow=@userpicflow";
                SqlParameter[] parms = new SqlParameter[] 
                {
                    new SqlParameter("@createdatetime",SqlDbType.NVarChar),
                    new SqlParameter("@createuserid",SqlDbType.NVarChar),
                    new SqlParameter("@createusername",SqlDbType.NVarChar),
                    new SqlParameter("@valide",SqlDbType.NVarChar),
                    new SqlParameter("@userpic",SqlDbType.Text),
                    new SqlParameter("@userpicflow",SqlDbType.NVarChar)
                };
                DateTime now = DateTime.Now;
                parms[0].Value = now.ToString();
                parms[1].Value = m_app.User.Id;
                parms[2].Value = m_app.User.Name;
                if (string.IsNullOrEmpty(picString))
                {
                    parms[3].Value = 0;
                }
                else
                {
                    parms[3].Value = 1;
                }
                parms[4].Value = picString;
                parms[5].Value = dataRowPicSign["userpicflow"].ToString();
                DS_SqlHelper.ExecuteNonQuery(sqlUpdatePicSign, parms, CommandType.Text);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 获取图片签名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private void GetUserPic(string userpicflow)
        {
            try
            {
                string sqlGetUserPic = "select userpic from userpicsign where userpicflow=@userpicflow";
                SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@userpicflow", SqlDbType.NVarChar) };
                parms[0].Value = userpicflow;
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlGetUserPic, parms, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    picString = dt.Rows[0]["userpic"].ToString();
                }
                else
                {
                    picString = string.Empty;
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }

        /// <summary>
        /// 绑定控件状态
        /// <auth>张业兴</auth>
        /// <date>2012-12-12</date>
        /// </summary>
        private void BtnState()
        {
            try
            {
                btnBrowse.Enabled = true;
                btnSave.Enabled = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}