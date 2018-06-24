using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#pragma warning disable 0618

namespace DrectSoft.Core.ImageManager
{
    public partial class ImageManager : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {
        private IEmrHost m_app;
        private DataTable m_ImageLibrary;
        private string file;

        /// <summary>
        /// ctor
        /// </summary>
        public ImageManager()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        public ImageManager(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        /// <summary>
        /// 创建值控件
        /// </summary>
        private void CreateValueControl()
        {
            try
            {
                Control c = CreateValueControlCore();
                c.Dock = DockStyle.Fill;
                if (c != null)
                {
                    this.panelImage.Controls.Add(c);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private Control CreateValueControlCore()
        {
            Control result = new Control(); ;
            return result;
        }

        private void InitData()
        {
            try
            {
                string selstr = "select ID,Name,Memo,Valid,Image from ImageLibrary where Valid=1";
                m_ImageLibrary = m_app.SqlHelper.ExecuteDataTable(selstr);
                gridImage.DataSource = m_ImageLibrary;
                gridImage.Refresh();
            }
            catch (Exception)
            {

                throw;
            }


        }


        private void UpdateImageContent(byte[] image, int indx)
        {

            OracleLob lob;
            OracleTransaction txn = null;
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;
            string strSql = string.Empty;
            string content = string.Empty;
            Exception exception = null;
            try
            {
                conn = new OracleConnection(m_app.SqlHelper.GetDbConnection().ConnectionString);
                conn.Open();
                txn = conn.BeginTransaction();
                cmd = new OracleCommand(strSql, conn, txn);
                cmd.CommandText = "select image from imagelibrary  where ID=" + indx + " FOR UPDATE";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lob = dr.GetOracleLob(0);
                    lob.Erase();
                    if (lob != OracleLob.Null)
                    {
                        //content = lob.Value.ToString();

                        lob.Write(image, 0, image.Length);
                    }
                }
                txn.Commit();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                exception = ex;
            }
            finally
            {
                dr.Close();
                conn.Close();
                cmd.Dispose();
            }

            if (exception != null)
            {
                throw exception;
            }
        }


        private void ExecProc(int Sort, string Name, string Content, string Memo, byte[] image, int ID)
        {
            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@Sort", SqlDbType.Int);
            sqlParam[1] = new SqlParameter("@Name", SqlDbType.VarChar);
            sqlParam[2] = new SqlParameter("@Memo", SqlDbType.VarChar);
            sqlParam[3] = new SqlParameter("@ID", SqlDbType.Int);
            sqlParam[0].Value = Sort;
            sqlParam[1].Value = Name;
            sqlParam[2].Value = Memo;
            sqlParam[3].Value = ID;

            DataTable table = m_app.SqlHelper.ExecuteDataTable("usp_InsertImage", sqlParam, CommandType.StoredProcedure);
            if ((table != null) && (table.Rows.Count > 0))
            {
                int index = Convert.ToInt32(table.Rows[0][0]);

                UpdateImageContent(image, index);
            }
        }

        #region Events
        //private void ButtonSelect_Click(object sender, EventArgs e)
        //{
        //    pictureEdit.Image = null;
        //    if (openFileDialogImage.ShowDialog() == DialogResult.OK)
        //    {
        //        string fileType = Path.GetExtension(this.openFileDialogImage.FileName);
        //        if (fileType.ToLower() == ".jpg" || fileType.ToLower() == ".jpeg" || fileType.ToLower() == ".jpe" || fileType.ToLower() == ".jpif" || fileType.ToLower() == ".gif" || fileType.ToLower() == ".tiff" || fileType.ToLower() == ".tif" || fileType.ToLower() == ".png" || fileType.ToLower() == ".ico")
        //        {
        //            file = this.openFileDialogImage.FileName;
        //            pictureEdit.Image = Image.FromFile(file);
        //        }
        //        else
        //        {
        //            m_app.CustomMessageBox.MessageShow("请选择合适的图片文件");
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// 保存新建图片
        /// </summary>
        private bool SaveNew()
        {
            try
            {
                if (textName.Text.Trim() == "" || pictureEdit.Image == null)
                {
                    m_app.CustomMessageBox.MessageShow("没有选择图片或输入名称");
                    return false;
                }

                MemoryStream ms = new MemoryStream();
                CreateNewImage(pictureEdit.Image, out ms);
                byte[] byteImage = ms.ToArray();
                string content = string.Empty;
                ExecProc(0, textName.Text.Trim(), content, memoEdit.Text.Trim(), byteImage, 1);
                InitData();
                m_app.CustomMessageBox.MessageShow("保存成功");

                return true;
            }
            catch (Exception)
            {

                throw;
            }


        }
        /// <summary>
        /// 新增 try
        /// add by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
                btnAdd.Text = "新增";
                EnabledButton(true);
                textName.Text = string.Empty;
                memoEdit.Text = string.Empty;
                pictureEdit.Image = null;
            }
            catch (Exception)
            {

                throw;
            }

        }


        private void CreateNewImage(Image originalImage, out MemoryStream ms)
        {


            string imgTempPath = CompressImage.GetTempPath + "\\" + DateTime.Now.ToString("yyyyMMss_hhmmssms") + ".jpg";
            int towidth = originalImage.Width;
            int toheight = originalImage.Height;
            ms = new MemoryStream();
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
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
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
            }
        }
        /// <summary>
        /// /修改操作 
        /// edit by ywk 2012年12月18日13:25:47 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonModify_Click(object sender, EventArgs e)
        {
            try
            {
                string imgTempPath = CompressImage.GetTempPath + "\\" + DateTime.Now.ToString("yyyyMMss_hhmmssms") + ".jpg";
                if (layoutViewImage.FocusedRowHandle < 0)
                    return;
                else if (textName.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("请输入图片名称");
                    return;
                }
                else
                {


                    DataRow focusRow = layoutViewImage.GetDataRow(layoutViewImage.FocusedRowHandle);
                    MemoryStream ms = new MemoryStream();
                    CreateNewImage(pictureEdit.Image, out ms);

                    if (ms.Length > 0)
                    {
                        byte[] byteImage = ms.ToArray();
                        string content = string.Empty;
                        ExecProc(1, textName.Text.Trim(), content, memoEdit.Text.Trim(), byteImage, Convert.ToInt32(focusRow["ID"]));
                        InitData();
                        m_app.CustomMessageBox.MessageShow("修改成功");
                    }


                }

            }
            catch (Exception ex)
            {

                m_app.CustomMessageBox.MessageShow(ex.Message);
            }

        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// edit by ywk 2012年12月18日13:21:50 异常捕捉
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (layoutViewImage.FocusedRowHandle < 0)
                    return;
                DataRow focusRow = layoutViewImage.GetDataRow(layoutViewImage.FocusedRowHandle);
                string str = string.Empty;
                if (focusRow != null)
                {
                    str = "update ImageLibrary set Valid=0 where ID=" + focusRow["ID"];
                }
                if (m_app.CustomMessageBox.MessageShow(string.Format("是否确认删除？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                {
                    m_app.SqlHelper.ExecuteNoneQuery(str);
                    m_ImageLibrary.Rows.Remove(focusRow);
                    InitData();
                    m_app.CustomMessageBox.MessageShow("删除成功");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void layoutViewImage_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (layoutViewImage.FocusedRowHandle < 0)
                return;
            DataRow focusRow = layoutViewImage.GetDataRow(layoutViewImage.FocusedRowHandle);
            if (focusRow.IsNull("Image") == true) return;

            textName.Text = focusRow["Name"].ToString();
            memoEdit.Text = focusRow["Memo"].ToString();
            MemoryStream ms = new MemoryStream((byte[])focusRow["Image"]);
            Image image = Image.FromStream(ms);
            ms.Close();
            //PP.LoadOriginalImage((byte[])focusRow["Image"]);
            pictureEdit.Image = image;
            file = "";
        }

        private void ImageManager_Load(object sender, EventArgs e)
        {
            CreateValueControl();
            InitData();
            this.textName.Focus();
        }
        #endregion

        #region IStartup 成员
        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            ImageManager imageManager = new ImageManager(application);
            PlugIn plg = new PlugIn(this.GetType().ToString(), imageManager);
            return plg;
        }
        #endregion
        /// <summary>
        /// 新增按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //增加try catch 
            //add by ywk 2012年12月18日13:20:51
            try
            {
                if (btnAdd.Text.Equals("新增"))
                {
                    btnAdd.Text = "保存";
                    EnabledButton(false);
                    textName.Text = string.Empty;
                    memoEdit.Text = string.Empty;
                    //PP.LoadOriginalImage(null);
                }
                else//保存
                {

                    if (SaveNew())
                    {
                        btnAdd.Text = "新增";
                        EnabledButton(true);
                    }
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }

        }


        /// <summary>
        /// 禁止/启用编辑按钮
        /// </summary>
        /// <param name="bolEnabled"></param>
        private void EnabledButton(bool bolEnabled)
        {
            ButtonSaveNew.Enabled = !bolEnabled;
            ButtonModify.Enabled = bolEnabled;
            ButtonDel.Enabled = bolEnabled;
            btn_LoadImage.Enabled = !bolEnabled;
        }

        private void btn_LoadImage_Click(object sender, EventArgs e)
        {
            if (openFileDialogImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(openFileDialogImage.FileName);
                    pictureEdit.Image = img;

                }
                catch (Exception ex)
                {
                    m_app.CustomMessageBox.MessageShow(ex.Message);
                }
            }
        }

















    }
}