using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
namespace CommonTableConfig
{
    /// <summary>
    /// 新增编辑上传文件信息界面
    /// by xlb 2012-12-17
    /// </summary>
    public partial class CommonNote_ModelInfo : DevBaseForm
    {
        DataRow dataRowFile;//模板信息
        IEmrHost m_app;
        //int index;//用来判断保存时是修改或者新增 0：修改 1：新增
        EditState m_State;//操作状态
        OpenFileDialog ofDialog = new OpenFileDialog();
        #region 构造
        public CommonNote_ModelInfo()
        {
            try
            {
                InitializeComponent();
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 有参数构造
        /// Add by XLB 2012-12-14
        /// </summary>
        /// <param name="app"></param>
        /// <param name="row"></param>
        public CommonNote_ModelInfo(IEmrHost app, DataRow row)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                dataRowFile = row;
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonNote_ModelInfo(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region enum by xlb 2012-12-26
        /// <summary>
        /// 区分操作状态
        /// </summary>
        public enum EditState
        {
            /// <summary>
            /// 新增
            /// </summary>
            Add = 1,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit = 2

        }
        #endregion

        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonNote_ModelInfo_Load(object sender, EventArgs e)
        {
            try
            {
                if (dataRowFile == null)
                {
                    this.Text = "新增";
                    m_State = EditState.Add;
                }
                else
                {
                    this.Text = dataRowFile["TEMPNAME"] == null ? "编辑" : "编辑" + dataRowFile["TEMPNAME"].ToString();
                    txtTempName.Text = dataRowFile["TEMPNAME"] == null ? "" : dataRowFile["TEMPNAME"].ToString();
                    txtDesc.Text = dataRowFile["TEMPDESC"] == null ? "" : dataRowFile["TEMPDESC"].ToString();
                    m_State = EditState.Edit;
                    SetFormEdit(false);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 浏览事件
        /// by xlb 2012-12-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofDialog == null)
                {
                    return;
                }
                if (m_State == EditState.Edit)
                {
                    int index = txtTempName.Text.IndexOf(".");
                    if (index > 0)
                    {
                        string filterName = txtTempName.Text.Substring(index);
                        ofDialog.Filter = "模板文件(*" + filterName + ")|*" + filterName;
                    }
                    else
                    {
                        ofDialog.Filter = "模板文件(*.xrp)|*.xrp";
                    }
                }
                else
                {
                    ofDialog.Filter = "模板文件(*.xrp,*.xml)|*.xrp;*.xml";
                }
                ofDialog.Title = "选择文件";
                if (ofDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSearch.Text = ofDialog.FileName;
                    if (m_State == EditState.Add)
                    {
                        string filename = ofDialog.FileName.Substring(ofDialog.FileName.LastIndexOf("\\") + 1);
                        txtTempName.Text = filename;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessageBox.Show("您确认需要关闭么？", "志扬软件", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Check())
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 设置窗体控件是否只读的方法
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="CanEdit"></param>
        private void SetFormEdit(bool canEdit)
        {
            try
            {
                //xll 2013-1-18 永远只读
                txtTempName.Properties.ReadOnly = true;
                //txtSearch.Properties.ReadOnly = !CanEdit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将上传文件进行压缩
        /// by xlb 2012-12-17
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string ToStream(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    return null;
                }
                FileStream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader bReader = new BinaryReader(fStream);
                byte[] byteFile = bReader.ReadBytes((int)fStream.Length);
                string btyeToStr = Convert.ToBase64String(byteFile);
                return DS_Common.ZipEmrXml(btyeToStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验方法
        /// by xlb 2012-12-18
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            try
            {
                bool result = true;
                if (this.txtTempName.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("文件名不能为空");
                    txtTempName.Focus();
                    result = false;
                }
                else if (this.txtDesc.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("请输入备注");
                    txtDesc.Focus();
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法
        /// 新增或修改
        /// EditState.Add:新增,EditState.Edit:修改
        /// by xlb 2012-12-27
        /// </summary>
        private void Save()
        {
            try
            {
                DataTable dt = DS_SqlHelper.ExecuteDataTable("select name from users where id=@ID", new SqlParameter[] { new SqlParameter("ID", m_app.User.Id) }, CommandType.Text);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                string dtRowsToStringTrim = dt.Rows[0]["NAME"].ToString().Trim();
                if (m_State == EditState.Add)
                {
                    if (txtSearch.Text.Trim() == null || txtSearch.Text.Trim().Length <= 0 && ofDialog.FileName.Length <= 0)
                    {
                        btnSearch.Focus();
                        MessageBox.Show("请选择上传文件");
                        return;
                    }
                    string sqlcompare = "select tempname from CommonnotePrintTemp where tempname like @tempname||'%' and valide='1'";
                    string temptext = txtTempName.Text.Trim().Substring(0, txtTempName.Text.Trim().IndexOf(".") + 1);
                    SqlParameter[] spr = { new SqlParameter("@tempname", temptext) };
                    DbDataReader dbr = DS_SqlHelper.ExecuteDataReader(sqlcompare, spr, CommandType.Text);
                    if (dbr.Read())
                    {
                        txtTempName.Focus();
                        MessageBox.Show("该文件名已存在");
                        return;
                    }
                    string btyetostr = ToStream(ofDialog.FileName);
                    string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string stradd = @" insert into CommonnotePrintTemp (tempflow,tempname,tempcontent,tempdesc,createdatetime,createuserid,createusername,valide,modifydatetime)
                                values(@tempflow,@tempname,@tempcontent,@tempdesc,@createdatetime,@createuserid,@createusername,@valide,@modifydatetime) ";
                    SqlParameter[] sps ={new SqlParameter("@tempflow",Guid.NewGuid().ToString()),
                                         new SqlParameter("@tempname",txtTempName.Text.Trim()),
                                         new SqlParameter("@tempcontent",SqlDbType.Text),
                                         new SqlParameter("@tempdesc",txtDesc.Text.Trim()),
                                         new SqlParameter("@createdatetime",nowTime),
                                         new SqlParameter("@createuserid",m_app.User.Id),
                                         new SqlParameter("@createusername",dtRowsToStringTrim),
                                         new SqlParameter("@valide","1"),
                                         new SqlParameter("@modifydatetime",nowTime)
                                        };
                    sps[2].Value = btyetostr;
                    DS_SqlHelper.ExecuteNonQuery(stradd, sps, CommandType.Text);
                    MessageBox.Show("添加成功");
                }
                else if (m_State == EditState.Edit)
                {
                    string strupdate = "update CommonnotePrintTemp set tempcontent=@tempcontent,tempdesc=@tempdesc,modifydatetime=@modifydatetime,createuserid=@createuserid,createusername=@createusername where tempflow=@tempflow";
                    SqlParameter[] sps ={ 
                                               new SqlParameter("@tempcontent", SqlDbType.Text),
                                               new SqlParameter("@tempdesc",txtDesc.Text.Trim()),
                                               new SqlParameter("@modifydatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@createuserid", m_app.User.Id),
                                               new SqlParameter("@createusername", dtRowsToStringTrim),
                                               new SqlParameter("@tempflow",dataRowFile["TEMPFLOW"]==null?"":dataRowFile["TEMPFLOW"].ToString())
                                           };
                    if (txtSearch.Text.Trim() != null && txtSearch.Text.Trim().Length > 0 && ofDialog.FileName.Length > 0)
                    {
                        string btyetostr = ToStream(ofDialog.FileName);
                        sps[0].Value = btyetostr;
                    }
                    else
                    {
                        SqlParameter[] spr = { new SqlParameter("@tempflow", dataRowFile["TEMPFLOW"] == null ? "" : dataRowFile["TEMPFLOW"].ToString()) };
                        string content = DS_SqlHelper.ExecuteDataTable("select tempcontent from CommonnotePrintTemp where tempflow=@tempflow", spr, CommandType.Text).Rows[0]["TEMPCoNTENT"].ToString();
                        sps[0].Value = content;
                    }
                    DS_SqlHelper.ExecuteNonQuery(strupdate, sps, CommandType.Text);
                    MessageBox.Show("修改成功");
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
