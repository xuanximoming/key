using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace CommonTableConfig
{
    /// <summary>
    /// 模板维护界面
    /// 用于查询新增删除修改模板信息
    /// By 项令波 2012-12-14
    /// </summary>
    public partial class CommonNote_ModelWeiHu : DevBaseForm
    {
        #region 变量和构造
        IEmrHost m_app;
        WaitDialogForm m_WaitForm;//等待窗体
        public CommonNote_ModelWeiHu(IEmrHost app)
        {
            try
            {
                m_app = app;
                InitializeComponent();
                DS_SqlHelper.CreateSqlHelper();
                m_WaitForm = new WaitDialogForm("正在加载窗体", "请稍后");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 查询事件
        /// Add by XLB 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow focusRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (focusRow == null)
                {
                    //throw new Exception("没有选中编辑行");
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有选中要编辑的行");
                    return;
                }
                CommonNote_ModelInfo ucModelInfo = new CommonNote_ModelInfo(m_app, focusRow);
                if (ucModelInfo == null)
                {
                    return;
                }
                //避免弹出窗体窜到别的页面上方打开
                ucModelInfo.TopMost = true;
                if (ucModelInfo.ShowDialog() == DialogResult.OK)
                {
                    ReSet();
                    Search();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件 valide改为0并非真正删除
        /// by 项令波 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                int index = this.gridView1.FocusedRowHandle;
                DataTable dt = (DataTable)this.gcCommonModel.DataSource;
                if (dt == null || dt.Rows.Count <= 0)
                {
                    //throw new Exception("没有数据");
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有数据");
                    return;
                }

                string id = dt.Rows[index]["TEMPFLOW"].ToString().Trim();
                if (string.IsNullOrEmpty(id))
                {
                    return;
                }
                string SqlStr = "update CommonnotePrintTemp set valide='0' where tempflow=@tempid ";
                SqlParameter[] sps = { new SqlParameter("@tempid", id) };
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请确认删除吗？", "志扬软件", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    DS_SqlHelper.ExecuteNonQuery(SqlStr, sps, CommandType.Text);
                    ReSet();
                    Search();
                    gridView1.FocusedRowHandle = index;//删除后焦点定位到下一条数据上
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新 增 事 件
        /// by  项令波
        /// 2012-12-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNote_ModelInfo ucModelInfo = new CommonNote_ModelInfo(m_app);
                if (ucModelInfo == null)
                {
                    return;
                }
                ucModelInfo.TopMost = true;
                if (ucModelInfo.ShowDialog() == DialogResult.OK)
                {
                    ReSet();
                    Search();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// by xlb 2012-12-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcCommonModel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataRow datarow = gridView1.GetFocusedDataRow();
                if (datarow == null)
                {
                    return;
                }
                CommonNote_ModelInfo ucModelInfo = new CommonNote_ModelInfo(m_app, datarow);
                if (ucModelInfo == null)
                {
                    return;
                }
                ucModelInfo.TopMost = true;
                if (ucModelInfo.ShowDialog() == DialogResult.OK)
                {
                    ReSet();
                    Search();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 下载事件
        /// 解压并下载
        /// by xlb 2012-12-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow focusRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (focusRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                string QueryStr = "select tempname,tempcontent from  CommonnotePrintTemp where  tempflow=@tempflow";
                SqlParameter[] sps = { new SqlParameter("@tempflow", focusRow["TEMPFLOW"]) };
                DbDataReader dbreander = DS_SqlHelper.ExecuteDataReader(QueryStr, sps, CommandType.Text);
                while (dbreander.Read())
                {
                    FileStream fstrem = null;
                    string content = DS_Common.UnzipEmrXml(dbreander["TEMPCONTENT"].ToString());
                    byte[] bytecontent = Convert.FromBase64String(content);
                    //MemoryStream ms = new MemoryStream(bytecontent);
                    SaveFileDialog saveModel = new SaveFileDialog();
                    saveModel.Title = "下载到本地文件";
                    //saveModel.Filter = "模板文件(*.xrp)|*.xrp";
                    saveModel.FileName = dbreander["TEMPNAME"].ToString();
                    if (saveModel.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileinfo = new FileInfo(saveModel.FileName);
                        fstrem = fileinfo.OpenWrite();
                        fstrem.Write(bytecontent, 0, bytecontent.Length);
                        fstrem.Close();
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("下载成功");
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 列表序号
        /// by xlb 2012-12-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender,
        DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// by xlb 2012-12-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonNote_ModelWeiHu_Load(object sender, EventArgs e)
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitForm, "正在加载数据");
                txtFileName.Focus();
                DS_Common.HideWaitDialog(m_WaitForm);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// by xlb 2012-12-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReSet_Click(object sender, EventArgs e)
        {
            try
            {
                ReSet();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 查询方法
        /// by XLB 2012-12-14 9:41
        /// </summary>
        private void Search()
        {
            try
            {
                string strSql = @"select ct.TempFlow,ct.TempName,ct.TempDesc,ct.CreateDateTime,ct.modifydatetime from CommonnotePrintTemp ct 
                where 1=1 and ct.Valide='1' and ct.TempName like '%'||@tname||'%' and ct.TempDesc like '%'||@desca||'%'order by  createdatetime desc ";
                SqlParameter[] sps = { new SqlParameter("@tname", txtFileName.Text.Trim()),
                                       new SqlParameter("@desca", txtDescription.Text.Trim())
                                     };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(strSql, sps, CommandType.Text);
                // DataTable dt = m_app.SqlHelper.ExecuteDataTable(strSql, sps,CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    gcCommonModel.DataSource = dt;
                }
                else
                {
                    gcCommonModel.DataSource = null;
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的数据");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 已弃用 YD_Common存在该方法 by xlb 2012-12-25
        /// <summary>
        /// 设置等待窗体
        /// by xlb 2012-12-19
        /// </summary>
        /// <param name="caption"></param>
        //private void SetWaitForm(string caption)
        //{
        //    try
        //    {
        //        if (m_WaitForm != null)
        //        {
        //            if (!m_WaitForm.Visible)
        //            {
        //                m_WaitForm.Visible = true;
        //            }
        //            m_WaitForm.Caption = caption;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        ///// <summary>
        ///// 隐藏等待窗体
        ///// by xlb 2012-12-19
        ///// </summary>
        //private void HideWaitForm()
        //{
        //    try
        //    {
        //        if (m_WaitForm != null)
        //        {
        //            m_WaitForm.Hide();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 重置
        /// by xlb 2012-12-20
        /// </summary>
        private void ReSet()
        {
            try
            {
                txtFileName.Text = string.Empty;
                txtDescription.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}