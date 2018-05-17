using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;
using DrectSoft.Common;

namespace DrectSoft.Common.SystemAppManager
{
    public partial class FormManager : Form, IStartPlugIn
    {
        IEmrHost m_app;
        #region 已注释 by xlb 2012-12-28 改用参数化查询语句 避免报错
        //private const string sql_str = "select * from APPCFG where hide='0' ";//无效的和隐藏标识为1的不显示------ywk
        //private const string sql_strQuery = @"select * from APPCFG where hide='0'  and (upper(configkey) like '%{0}%' or upper(name) like '%{0}%' or upper(descript) like '%{0}%' or '{0}' is null )";
        #endregion
        private const string sql_Query = @"select * from APPCFG where hide='0' "
            + " and (upper(configkey) like '%'||@conName||'%' or upper(name) like '%'||@conName||'%' or "
            + " upper(descript) like '%'||@conName||'%' or @conName is null )";//add by xlb 2012-12-28
        //private DataTable sourceTable = null;
        //IDataAccess sql_helper;
        AppConfigDalc appdal;
        IAppConfigDesign _currentConfigDesign;
        Dictionary<string, Control> _cacheUI = new Dictionary<string, Control>();


        public FormManager()
        {
            InitializeComponent();
            DS_SqlHelper.CreateSqlHelper();
        }

        /// <summary>
        ///根据传入的字符串查询结果绑定数据
        ///王冀 2012 10 24
        ///edit zyx 2012-12-17  系统参数配置中，当查询不到结果时给出提示框
        ///edit by xlb 2012-12-28 改用参数化查询
        /// </summary>
        /// <param name="key"></param>
        private void BindData(string sql, string key)
        {
            try
            {
                //sql_helper = DataAccessFactory.DefaultDataAccess;
                appdal = new AppConfigDalc();
                //sourceTable = sql_helper.ExecuteDataTable(sql_str);
                SqlParameter[] sps = { new SqlParameter("@conName", key) };
                //DataTable sourceTable = sql_helper.ExecuteDataTable(key);
                DataTable sourceTable = DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                if (sourceTable == null || sourceTable.Rows.Count <= 0)
                {
                    MessageBox.Show("没有符合条件的数据");
                }
                gridControl1.DataSource = sourceTable;
                //if (sourceTable == null || sourceTable.Rows.Count <= 0)
                //{
                //    MessageBox.Show("没有符合您查询的系统参数配置的相关数据！");
                //}
                m_app.PublicMethod.ConvertGridDataSourceUpper(gridView1);
                //绑定数据  查询结构加载时 changed事件获取不到句柄 不能自动绑定数据需要手动加载 2012-10-25
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                SetApp(dataRow);
                AddControlToMyPanel(dataRow);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private Dictionary<string, EmrAppConfig> GetGroupConfigs(EmrAppConfig eac)
        {
            try
            {
                Dictionary<string, EmrAppConfig> configs = new Dictionary<string, EmrAppConfig>();
                configs.Add(eac.Key, eac);
                return configs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ShowDesignIntface(EmrAppConfig eac)
        {
            try
            {
                this.SuspendLayout();
                Dictionary<string, EmrAppConfig> configs = GetGroupConfigs(eac);
                string assInfo = eac.DesignClass;
                IAppConfigDesign iacd = null;
                if (string.IsNullOrEmpty(assInfo))
                {
                    //用通用的设置类
                    StandardAppConfigDesign sacd = new StandardAppConfigDesign();
                    iacd = sacd as IAppConfigDesign;
                }
                else
                {
                    _currentConfigDesign = null;
                    Type t = Type.GetType(assInfo);
                    if (t != null)
                    {
                        iacd = (IAppConfigDesign)Activator.CreateInstance(t);
                    }
                }

                _currentConfigDesign = iacd;
                if (iacd == null) throw new ArgumentException("指定的配置类未实现接口(IAppConfigDesign).");
                if (!_cacheUI.ContainsKey(eac.Key)) _cacheUI.Add(eac.Key, iacd.DesignUI);
                AddDesignIntface(eac.Name, iacd.DesignUI);
                iacd.Load(m_app, configs);
            }
            catch (Exception e)
            {
                m_app.CustomMessageBox.MessageShow(e.Message, CustomMessageBoxKind.ErrorOk);
            }
            finally
            { this.ResumeLayout(); }
        }

        private void AddDesignIntface(string title, Control ctrl)
        {
            try
            {
                ClearDesignIntface();

                if (ctrl == null) throw new ArgumentNullException("ctrl");
                ctrl.Dock = DockStyle.Fill;
                panelUI.Controls.Add(ctrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ClearDesignIntface()
        {
            try
            {
                panelUI.Controls.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetApp(DataRow dataRow)
        {
            try
            {
                if (dataRow == null)
                {
                    return;
                }
                EmrAppConfig appcfg = appdal.DataRow2EmrAppConfig(dataRow);
                ShowDesignIntface(appcfg);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            try
            {
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                m_app = host;
                return plg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle < 0) return;

                DataRow dataRow = gridView1.GetDataRow(e.FocusedRowHandle);
                if (dataRow == null) return;
                SetApp(dataRow);
                //mykey = dataRow["configkey"].ToString();
                //下面将其他的几列的值赋给相应控件加到mypanel中
                //cmdHide.SelectedText = "";
                //cmdValid.SelectedText = "";
                AddControlToMyPanel(dataRow);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 将控件加到mypanel中
        /// </summary>
        /// <param name="dataRow"></param>
        private void AddControlToMyPanel(DataRow dataRow)
        {
            try
            {
                txtcanshu.Text = dataRow["configkey"].ToString();//参数
                txtcanshuname.Text = dataRow["name"].ToString();//参数名
                txtcanshudesp.Text = dataRow["descript"].ToString();//参数描述
                txtcanshutype.Text = dataRow["paramtype"].ToString();//参数类型
                if (dataRow["hide"].ToString() == "0")
                {
                    cmdHide.SelectedIndex = 0;
                }
                else
                {
                    cmdHide.SelectedIndex = 1;
                }
                //cmdHide.SelectedText = dataRow["hide"].ToString();//隐藏标志
                //cmdValid.SelectedText = dataRow["valid"].ToString();//是否有效
                //xll 2013-1-22 不用有效标记
                //if (dataRow["valid"].ToString() == "0")
                //{
                //    cmdValid.SelectedIndex = 0;
                //}
                //else
                //{
                //    cmdValid.SelectedIndex = 1;
                //}
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 窗体加载事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormManager_Load(object sender, EventArgs e)
        {
            try
            {
                BindData(sql_Query, "");//edit by xlb 2012-12-28
            }
            catch (Exception ex)
            {
                //m_app.CustomMessageBox.MessageShow(ex.Message);
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtcanshu.Text) || string.IsNullOrEmpty(txtcanshudesp.Text) || string.IsNullOrEmpty(txtcanshuname.Text) || string.IsNullOrEmpty(txtcanshutype.Text))
                {
                    m_app.CustomMessageBox.MessageShow("请确认所有项已经填写完毕");
                    return;
                }
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (dataRow == null) return;

                if (_currentConfigDesign != null)
                {
                    _currentConfigDesign.Save();
                    if (_currentConfigDesign.ChangedConfigs != null)
                    {
                        EmrAppConfig eac = new EmrAppConfig();
                        foreach (string key in _currentConfigDesign.ChangedConfigs.Keys)
                        {
                            eac = _currentConfigDesign.ChangedConfigs[key];//key
                            dataRow["value"] = eac.Config;
                        }
                        //以前的是ConfigKey改变了，才进行更新，此处改为更新一处就更新操作
                        eac.Config = dataRow["value"].ToString();
                        eac.Key = txtcanshu.Text;//参数
                        eac.Name = txtcanshuname.Text;//参数名称
                        eac.Descript = txtcanshudesp.Text;//参数描述
                        // bool isvalid = false;//定义是否有效
                        //if (txtvalid.Text == "1")
                        //if (cmdValid.SelectedText == "1")
                        //{
                        //    isvalid = true;
                        //}
                        eac.Valid = true;//是否有效
                        //if (cmdValid.SelectedIndex == 1)
                        //{
                        eac.Valid1 = "1";//是否有效
                        //}
                        //else
                        //{
                        //    eac.Valid1 = "0";
                        //}

                        if (cmdHide.SelectedIndex == 1)
                        {
                            eac.IsHide = "1";//隐藏标志
                        }
                        else
                        {
                            eac.IsHide = "0";
                        }
                        //eac.Valid1 = cmdValid.SelectedText;//是否有效
                        //eac.IsHide = cmdHide.SelectedText;//隐藏标志
                        appdal.UpdateEmrAppConfig(eac);
                        m_app.CustomMessageBox.MessageShow("保存成功");
                        txtQuery.Text = string.Empty;
                        BindData(sql_Query, "");//王冀//edit by xlb 2012-12-28
                    }

                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void myPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 清理
        /// 王冀2012 10 24
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtQuery.Text = "";
                this.txtQuery.Focus();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 重置查询结果，刷新为重新进行一次查询
        /// 王冀2012 10 24
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                FormManager_Load(sender, e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 查询
        /// 王冀2012 10 24
        /// edit by xlb 2012-12-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPage();
                //string key = this.txtQuery.Text.ToUpper().Replace("'", "''");
                string key = this.txtQuery.Text.ToUpper();
                //if (key.Contains(";"))//add by xlb 2012-12-27
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("含有特殊字符");
                //    return;
                //}
                this.txtQuery.Text = string.Empty;//add by xlb 2012-12-27
                BindData(sql_Query, key);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空页面
        /// <auth>张业兴</auth>
        /// <date>2012-12-17</date>
        /// </summary>
        private void ClearPage()
        {
            try
            {
                panelUI.Controls.Clear();
                txtcanshu.Text = string.Empty;
                txtcanshudesp.Text = string.Empty;
                txtcanshuname.Text = string.Empty;
                txtcanshutype.Text = string.Empty;
                cmdValid.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检索事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-01</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //string searchStr = txtQuery.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]").Replace("[", "[[]");
                string searchStr = DS_Common.FilterSpecialCharacter(txtQuery.Text);
                string filter = string.Format(" configkey like '%{0}%' or name like '%{0}%' or value like '%{0}%' or descript like '%{0}%' ", searchStr);
                DataTable dv = gridControl1.DataSource as DataTable;
                if (null != dv && dv.Rows.Count > 0)
                {
                    dv.DefaultView.RowFilter = filter;
                }
                //重新绑定数据          add by wangj  2013 2 26
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dataRow)
                {
                    ClearPage();
                }
                else
                {
                    SetApp(dataRow);
                    AddControlToMyPanel(dataRow);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
