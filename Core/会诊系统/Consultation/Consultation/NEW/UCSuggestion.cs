using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.DLG;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common;

namespace Consultation.NEW
{
    /// <summary>
    /// 意见填写界面用于多科会诊时动态产生表格
    /// Add xlb 2013-02-22
    /// </summary>
    public partial class UCSuggestion : DevExpress.XtraEditors.XtraUserControl
    {
        DataRow recordConsultRow;//实际会诊信息
        IEmrHost app;
        string consultApplySns;

        #region 方法 Add by xlb 2013-02-25

        public UCSuggestion()
        {
            try
            {
                InitializeComponent();
                if (!DesignMode)
                {
                    #region 屏蔽右键菜单 by xlb 2013-03-21
                    ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                    DS_Common.CancelMenu(groupControl1, contextMenuStrip1);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="consultApplySn"></param>
        /// <param name="host"></param>
        public UCSuggestion(string consultApplySn, IEmrHost host)
            : this()
        {
            try
            {
                consultApplySns = consultApplySn;
                app = host;
                Register();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UCSuggestion(string consultApplySn,DataRow row,IEmrHost host):this()
        {
            try
            {
                recordConsultRow = row;
                consultApplySns = consultApplySn;
                app = host;
                Register();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 提交会诊完成时校验是否填写了会诊意见
        /// Add xlb 2013-03-06
        /// </summary>
        /// <returns></returns>
        private bool ValidateDate()
        {
            try
            {
                if (string.IsNullOrEmpty(memoEditSuggestion.Text.Trim()))
                {
                    memoEditSuggestion.Focus();
                    MessageBox.Show("请填写会诊意见");
                    return false;
                }
                else if (memoEditSuggestion.Text.Trim().Length > 1500)
                {
                    memoEditSuggestion.Focus();
                    MessageBox.Show("会诊意见不能超过1500字");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件
        /// Add xlb 2013-02-25
        /// </summary>
        private void Register()
        {
            try
            {
                btnSave.Click+=new EventHandler(btnSave_Click);
                btnCompelete.Click+=new EventHandler(btnCompelete_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法 用来保存受邀医师填写的会诊意见
        /// Add xlb 2013-02-25
        /// </summary>
        private void Save(string status)
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                string sql = @"select consultsuggestion  from consultsuggestion where 
                       consultapplysn=@consultapplySn and createuser=@createUser and valid='1'";
                SqlParameter[] sps ={new SqlParameter("@consultapplySn",consultApplySns),
                                    new SqlParameter("@createUser",app.User.Id),
                                    };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (dt != null&&dt.Rows.Count <= 0)
                {
                    string sqlInsert = @"insert into consultsuggestion(consultapplysn,createuser,
                                        createtime,consultsuggestion,valid,state)
                           values(@consultapplysn,@createuser,@createtime,@consultsuggestion,1,@status)";
                    SqlParameter[] spr ={
                                           new SqlParameter("@consultapplysn",consultApplySns),
                                           new SqlParameter("@createuser",app.User.Id),
                                           new SqlParameter("@createtime",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                                           new SqlParameter("@consultsuggestion",memoEditSuggestion.Text.Trim()),
                                           new SqlParameter("@status",status)
                                         };
                    DS_SqlHelper.ExecuteNonQuery(sqlInsert, spr, CommandType.Text);
                }
                else
                {
                    string sqlupdate = @"update consultsuggestion set consultsuggestion=@consultsuggestion,state=@state
                                      where createuser=@createuser and consultapplysn=@consultApplySn";
                    SqlParameter[] spr ={
                                            new SqlParameter("@consultsuggestion",memoEditSuggestion.Text.Trim()),
                                            new SqlParameter("@state",status),
                                            new SqlParameter("@createuser",app.User.Id),
                                            new SqlParameter("@consultApplySn",consultApplySns)
                                       };
                    DS_SqlHelper.ExecuteNonQuery(sqlupdate, spr, CommandType.Text);
                }
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 Add by xlb 2013-03-06

        /// <summary>
        /// 保存事件 保存受邀医师填写的会诊意见
        /// Add xlb 2013-02-25 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender,EventArgs e)
        {
            try
            {
                Save("10");//10表示会诊意见保存为草稿
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊意见完成事件
        /// Add xlb 2013-03-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCompelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool result=ValidateDate();
                if (result)
                {
                    Save("20");//表示会诊意见已完成
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
