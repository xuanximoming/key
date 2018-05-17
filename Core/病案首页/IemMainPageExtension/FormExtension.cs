using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;

namespace IemMainPageExtension
{
    /// <summary>
    /// 病案首页扩展信息编辑界面
    /// Add by xlb 2013-04-09
    /// </summary>
    public partial class FormExtension : UserControl
    {
        private List<IemMainPageExcept> iemExcept;//病案首页个性配置维护对象
        private List<IemMainPageExceptUse> iemExceptUse;//病案首页个性配置对象使用对象
        private SqlUti sqlUtil;
        private string noofInpat = "";
        private DataTable m_extensionTable;

        /// <summary>
        /// 字段表对应字段和字段值
        /// </summary>
        public DataTable ExtensionTable
        {
            get
            {
                if (m_extensionTable == null)
                {
                    m_extensionTable = new DataTable();
                    AddDataElement(m_extensionTable, "iemName");
                    AddDataElement(m_extensionTable, "iemValue");
                }
                return m_extensionTable;
            }
        }

        #region 方法 Add by xlb 2013-04-09

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormExtension()
        {
            try
            {
                InitializeComponent();
                sqlUtil = new SqlUti();
                SetStyle(ControlStyles.UserPaint, true);/*控件自行绘制*/
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); /*禁止擦除背景*/
                SetStyle(ControlStyles.DoubleBuffer, true); /*双缓冲(绘制在缓冲区进行完成后输出到屏幕上)*/
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="noofpat">病案号</param>
        public FormExtension(string noofpat)
            : this()
        {
            try
            {
                this.noofInpat = noofpat;
                sqlUtil = new SqlUti();
                InitIemExcept();
                GetIemMainPageExceptUsePat();
                InitControl(iemExcept, iemExceptUse);
                SetStyle(ControlStyles.UserPaint, true);/*控件自行绘制*/
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); /*禁止擦除背景*/
                SetStyle(ControlStyles.DoubleBuffer, true); /*双缓冲(绘制在缓冲区进行完成后输出到屏幕上)*/
                RegisterEvent();/*注册事件*/
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取使用对象集合
        /// </summary>
        private void GetIemMainPageExceptUsePat()
        {
            try
            {
                if (sqlUtil == null)
                {
                    sqlUtil = new SqlUti();
                }
                iemExceptUse = sqlUtil.GetIemExceptUse(noofInpat);
                if (iemExceptUse == null)
                {
                    iemExceptUse = new List<IemMainPageExceptUse>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化维护对象
        /// </summary>
        private void InitIemExcept()
        {
            try
            {
                Dictionary<string, DateElementEntity> dataList = sqlUtil.GetDataElement();
                iemExcept = sqlUtil.GetIemMainPageExcept(dataList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 给指定表添加列
        /// <auth>XLB</auth>
        /// <date></date>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        private void AddDataElement(DataTable dt, string columnName)
        {
            try
            {
                if (dt == null)
                {
                    dt = new DataTable();
                }
                DataColumn dtColumn = new DataColumn(columnName, typeof(string));
                dt.Columns.Add(dtColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册相关事件
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnSave.Click += new EventHandler(btnSave_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化界面
        /// 控件名预留
        /// Add by xlb 2013-04-09
        /// 便于扩展后期工程
        /// </summary>
        private void InitControl(List<IemMainPageExcept> iemList/*维护对象集合*/, List<IemMainPageExceptUse> iemMainPageExceptUseList)
        {
            try
            {
                foreach (var item in iemList)/*遍历集合中每个对象，每个对象对应一个数据元*/
                {
                    if (string.IsNullOrEmpty(item.IemControl))/*控件名为空则按数据元类型进行处理*/
                    {
                        IemMainPageExceptUse iemMainPageExceptUse = null;

                        var ieExceptUseList = (from obj in iemMainPageExceptUseList where obj.IemexId == item.IemExId select obj).ToList();
                        if (ieExceptUseList == null || ieExceptUseList.Count <= 0)
                        {
                            iemMainPageExceptUse = new IemMainPageExceptUse();
                            iemMainPageExceptUse.Noofinpat = this.noofInpat;
                        }
                        else
                        {
                            //返回序列中的第一个对象
                            iemMainPageExceptUse = ieExceptUseList.First();
                        }
                        if (string.IsNullOrEmpty(item.IemOtherName))
                        {
                            item.IemOtherName = "未指定列";
                        }

                        TextEdit textEdit = new TextEdit();
                        textEdit.Text = item.IemOtherName + "：";
                        textEdit.BackColor = Color.Transparent;
                        textEdit.ForeColor = Color.Black;
                        textEdit.Width = 150;
                        textEdit.Enabled = false;
                        textEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                        textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        this.flowLayoutPanel1.Controls.Add(textEdit);

                        if (item.IemOtherName == "是")
                        {
                            this.flowLayoutPanel1.SetFlowBreak(textEdit, true);
                        }
                        /*通过数据元类型返回相应用户控件*/
                        UserControl userControl = CreateControlFactory.CreateControl(item, item.DateElement, iemMainPageExceptUse);
                        userControl.Width = 105;
                        this.flowLayoutPanel1.Controls.Add(userControl);
                        //确定用户控件是否需换行
                        if (item.IsOtherLine/*1:表示换行,0:不换行*/ == "是")
                        {
                            //流布局是否换行
                            this.flowLayoutPanel1.SetFlowBreak(userControl/*控件*/, true/*Bool类型表示是否换行*/);
                        }
                    }
                    else
                    {
                        //SetControl("","");//预留的接口用于以后需要维护特定数据添加对应用户控件
                    }
                }
                //控制流方向
                this.flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
                this.flowLayoutPanel1.Margin = new Padding(0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        private void Save()
        {
            try
            {
                IemMainPageExceptUse iemExceptUse = null;

                /*编辑界面用户控件选择值并记录相应的病案维护对象*/
                foreach (Control control in this.flowLayoutPanel1.Controls)
                {
                    if (control is IIemMainPageExcept)
                    {
                        /*记录每个用户控件对应的使用对象信息*/
                        iemExceptUse = (control as IIemMainPageExcept).GetIemMainPageExceptUse();
                    }
                    sqlUtil.SaveIemUse(iemExceptUse);
                }

                MessageBox.Show("更新成功");
                GetUIInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新失败");
                throw ex;
            }
        }

        /// <summary>
        /// 获取界面信息填充表
        /// </summary>
        /// <returns></returns>
        private DataTable GetUIInfo()
        {
            try
            {
                if (m_extensionTable == null)
                {
                    m_extensionTable = new DataTable();
                }
                if (!m_extensionTable.Columns.Contains("iemName"))
                {
                    AddDataElement(m_extensionTable, "iemName");
                }
                if (!m_extensionTable.Columns.Contains("iemValue"))
                {
                    AddDataElement(m_extensionTable, "iemValue");
                }
                if (m_extensionTable.Rows.Count > 0)
                {
                    m_extensionTable.Rows.Clear();
                }

                foreach (Control control in this.flowLayoutPanel1.Controls)
                {
                    IemMainPageExceptUse iemUse = null;
                    if (control is IIemMainPageExcept)
                    {
                        /*记录每个用户控件对应的使用对象信息*/
                        iemUse = (control as IIemMainPageExcept).GetIemMainPageExceptUse();
                    }
                    if (iemUse == null)
                    {
                        iemUse = new IemMainPageExceptUse();
                    }
                    //var iemUseEntity=from a in iemExcept  select a.IemExId=iemUse.IemexId;
                    IemMainPageExcept iemExceptEntity = null;
                    foreach (var item in iemExcept)
                    {
                        if (item.IemExId == iemUse.IemexId)
                        {
                            iemExceptEntity = item;
                            break;
                        }
                    }
                    if (iemExceptEntity == null)
                    {
                        iemExceptEntity = new IemMainPageExcept();
                    }
                    DataRow dataRow = m_extensionTable.NewRow();
                    dataRow["iemValue"] = iemUse.Value;
                    dataRow["iemName"] = iemExceptEntity.IemExName;
                    m_extensionTable.Rows.Add(dataRow);
                }

                return m_extensionTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建远程实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object CreateInstance(Type type, object[] param)
        {
            try
            {
                if (type == null)
                {
                    throw new ArgumentException("无法获取类型");
                }
                return Activator.CreateInstance(type, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据维护对象对应控件地址创建对应用户控件
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public UserControl SetControl(string dllName/*dll地址*/, string fullName/*全地址*/)
        {
            try
            {
                Assembly assemblyInfo = Assembly.LoadFrom(dllName);

                if (assemblyInfo == null)
                {
                    throw new Exception("查找插件失败");
                }
                Type type = assemblyInfo.GetType(fullName);//跨Dll时直接获取类型会失败
                UserControl userControl = (UserControl)CreateInstance(type, null);
                return userControl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 Add by xlb 2013-04-09

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

    }
}