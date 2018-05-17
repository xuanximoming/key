using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;
using DevExpress.XtraGrid.Columns;


namespace IemMainPageExtension
{
    /// <summary>
    /// 病案扩展维护界面
    /// </summary>
    public partial class FormIemConfig : DevBaseForm, IStartPlugIn
    {
        private SqlUti sqlUtil;
        IEmrHost m_app;

        List<IemMainPageExcept> iemMainPageExceptDel/*删除对象集合*/ = new List<IemMainPageExcept>();

        #region  Methods

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormIemConfig()
        {
            try
            {
                InitializeComponent();
                RegisterEvent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="host"></param>
        public FormIemConfig(IEmrHost host)
        {
            try
            {
                InitializeComponent();
                RegisterEvent();
                m_app = host;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件方法
        /// 避免设计模式时事件丢失
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnAdd.Click += new EventHandler(btnAdd_Click);
                btnDel.Click += new EventHandler(btnDel_Click);
                btnDown.Click += new EventHandler(btnDown_Click);
                btnSaveIem.Click += new EventHandler(btnSaveIem_Click);
                btnUp.Click += new EventHandler(btnUp_Click);
                this.Load += new EventHandler(FormIemConfig_Load);
                repositoryItemSearchLookUpEdit1.EditValueChanged += new EventHandler(repositoryItemSearchLookUpEdit1_EditValueChanged);
                gvMainPage.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(gvMainPage_CustomDrawRowIndicator);
                gvMainPage.Click += new EventHandler(gvMainPage_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取所有数据元
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        private void GetAllDateElement()
        {
            try
            {
                sqlUtil = new SqlUti();
                List<DateElementEntity> dataList = sqlUtil.GetAllDateElement();
                repositoryItemSearchLookUpEdit1.DataSource = dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化列表
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        private void InitDateList()
        {
            try
            {
                if (sqlUtil == null)
                {
                    sqlUtil = new SqlUti();
                }
                Dictionary<string, DateElementEntity> dateElement = sqlUtil.GetDataElement();
                List<IemMainPageExcept> iemMainPageList = sqlUtil.GetIemMainPageExcept(dateElement);
                gridControl1.DataSource = iemMainPageList;
                GetAllDateElement();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验方法
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            try
            {
                List<IemMainPageExcept> iemMainPageExcept = gvMainPage.DataSource as List<IemMainPageExcept>;
                if (iemMainPageExcept == null || iemMainPageExcept.Count <= 0)
                {
                    return true;
                }
                foreach (var item in iemMainPageExcept)
                {
                    if (string.IsNullOrEmpty(item.IemExName))
                    {
                        MessageBox.Show("列名不能为空");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(item.IemOtherName))
                    {
                        MessageBox.Show("标签名不能为空");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(item.DateElementFlow))
                    {
                        MessageBox.Show("数据元ID不能为空");
                        return false;
                    }
                    else if (string.IsNullOrEmpty(item.IsOtherLine))
                    {
                        MessageBox.Show("是否换行不能为空");
                        return false;
                    }
                }
                for (int i = 0; i < iemMainPageExcept.Count; i++)
                {
                    if (!Tool.IsDigitalOrLetter(iemMainPageExcept[i].IemExName))
                    {
                        MessageBox.Show("列名必须为纯英文");
                        return false;
                    }
                    for (int j = 1+i; j < iemMainPageExcept.Count; j++)
                    {
                        if (iemMainPageExcept[i].IemExName.Equals(iemMainPageExcept[j].IemExName))
                        {
                            MessageBox.Show("列名不能重复");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置界面可编辑性
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        private void SetFormEdit(bool isCanEdit)
        {
            try
            {
                IEMEXNAME.OptionsColumn.AllowEdit = isCanEdit;
                IEMOTHERNAME.OptionsColumn.AllowEdit = isCanEdit;
                ISOTHERLINE.OptionsColumn.AllowEdit = isCanEdit;

                IEMCONTROL.OptionsColumn.AllowEdit = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        ///<purpose>保存方法</purpose>
        /// </summary>
        private void SaveIemExcept()
        {
            try
            {
                List<IemMainPageExcept> iemMainPageExcept = gridControl1.DataSource as List<IemMainPageExcept>;

                if (iemMainPageExcept == null)
                {
                    iemMainPageExcept = new List<IemMainPageExcept>();
                }

                int index = 0;
                iemMainPageExcept.AddRange(iemMainPageExceptDel);
                foreach (var item in iemMainPageExcept)
                {
                    item.OrderCode = (++index).ToString();//排序码
                    sqlUtil = new SqlUti();
                    sqlUtil.SaveIemMainPageExcept(item);
                }
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败");
            }
        }

        #region IStartup Members

        public IPlugIn Run(IEmrHost application)
        {
            try
            {
                if (application == null)
                {
                    throw new ArgumentNullException("application");
                }

                //FormIemConfig formIemConfig = new FormIemConfig(application);
                m_app = application;
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region    Events 

        /// <summary>
        /// 窗体加载事件
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormIemConfig_Load(object sender, EventArgs e)
        {
            try
            {
                InitDateList();
                SetFormEdit(true);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 联动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void repositoryItemSearchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit searchLookUpEdit = sender as SearchLookUpEdit;
                DateElementEntity dataElementEntity = searchLookUpEdit.Properties.View.GetFocusedRow() as DateElementEntity;
                IemMainPageExcept iemMainPageExcept = gvMainPage.GetFocusedRow() as IemMainPageExcept;
                
                if (iemMainPageExcept == null || string.IsNullOrEmpty(iemMainPageExcept.IemExId))
                {
                    ELEMENTNAME.OptionsColumn.AllowEdit = true;
                }
                int count = sqlUtil.GetIsBeenUse(iemMainPageExcept.IemExId);
                if (count > 0)
                {
                    ELEMENTNAME.OptionsColumn.AllowEdit = false;
                    return;
                }
                else
                {
                    ELEMENTNAME.OptionsColumn.AllowEdit = true;
                }
                if (dataElementEntity != null && iemMainPageExcept != null)
                {

                    iemMainPageExcept.ElementType = dataElementEntity.ElementType;
                    iemMainPageExcept.DateElementFlow = dataElementEntity.ElementFlow;
                    iemMainPageExcept.DateElement = dataElementEntity;
                    iemMainPageExcept.ElementName = dataElementEntity.ElementName;
                    iemMainPageExcept.IemOtherName = dataElementEntity.ElementName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 单击列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMainPage_Click(object sender, EventArgs e)
        {
            try
            {
                int index = gvMainPage.FocusedRowHandle;
                ELEMENTNAME.OptionsColumn.AllowEdit = true;
                List<IemMainPageExcept> iemMainPageExcept = gridControl1.DataSource as List<IemMainPageExcept>;
                if (index == 0)
                {
                    btnUp.Enabled = false;
                    btnDown.Enabled = true;
                }
                else if (index == iemMainPageExcept.Count - 1)
                {
                    btnDown.Enabled = false;
                    btnUp.Enabled = true;
                }
                else
                {
                    btnUp.Enabled = true;
                    btnDown.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMainPage_CustomDrawRowIndicator(
            object sender, 
            DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 上移事件
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                btnDown.Enabled = true;/*针对焦点行在最后一行上移后仍不可编辑*/
                List<IemMainPageExcept> iemMainPageExceptList/*数据源集合*/ = gridControl1.DataSource as List<IemMainPageExcept>;
                if (iemMainPageExceptList == null)
                {
                    return;
                }
                IemMainPageExcept iemMainPageExcept = gvMainPage.GetFocusedRow() as IemMainPageExcept;
                if (iemMainPageExcept == null)
                {
                    return;
                }
                int index = gvMainPage.FocusedRowHandle;/*焦点行*/
                if (index <= 0)
                {
                    btnUp.Enabled = false;
                    return;
                }

                iemMainPageExceptList.Remove(iemMainPageExcept);/*集合中移除焦点行对象*/
                iemMainPageExceptList.Insert(index - 1, iemMainPageExcept);/*重新排序*/
                gridControl1.DataSource = new List<IemMainPageExcept>(iemMainPageExceptList);/*重新绑定*/
                gvMainPage.MoveBy(index - 1);/*焦点移到上一行*/
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 添加行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<IemMainPageExcept> iemMainPageList = gridControl1.DataSource as List<IemMainPageExcept>;

                if (iemMainPageList == null)
                {
                    iemMainPageList = new List<IemMainPageExcept>();
                }
                IemMainPageExcept iemMainPageExcept = new IemMainPageExcept();
                iemMainPageExcept.IsOtherLine = "0";
                iemMainPageExcept.IemControl = "";
                iemMainPageExcept.Vailde = "1";//是否有效
                iemMainPageList.Add(iemMainPageExcept);
                gridControl1.DataSource = new List<IemMainPageExcept>(iemMainPageList);
                gvMainPage.MoveBy(iemMainPageList.Count - 1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 删除行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                //维护对象
                IemMainPageExcept iemExcept = gvMainPage.GetFocusedRow() as IemMainPageExcept;

                if (iemExcept == null)
                {
                    return;
                }
                int index = gvMainPage.FocusedRowHandle;

                if (index < 0)
                {
                    return;
                }
                //列表数据源集合
                List<IemMainPageExcept> iemDataSource = gridControl1.DataSource as List<IemMainPageExcept>;

                if (!string.IsNullOrEmpty(iemExcept.IemExId))
                {
                    iemExcept.Vailde = "0";

                    iemExcept.ModifyDocId = m_app.User.Id; //记录作废人ID和时间方便追溯
                    iemExcept.ModifyDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    iemMainPageExceptDel.Add(iemExcept);
                }
                iemDataSource.Remove(iemExcept);//去除待删除对象
                gridControl1.DataSource = new List<IemMainPageExcept>(iemDataSource);
                gvMainPage.MoveBy(index);//焦点定位到删除行的下一行
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 下移事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                btnUp.Enabled = true;
                //维护对象集合
                List<IemMainPageExcept> iemMainPageExceptList = gridControl1.DataSource as List<IemMainPageExcept>;

                if (iemMainPageExceptList == null)
                {
                    return;
                }
                IemMainPageExcept iemMainPageExcept = gvMainPage.GetFocusedRow() as IemMainPageExcept;

                if (iemMainPageExcept == null)
                {
                    return;
                }
                int index = gvMainPage.FocusedRowHandle;

                if (index < 0)
                {
                    return;
                }
                int max = iemMainPageExceptList.Count;

                if (index >= max - 1)
                {
                    btnDown.Enabled = false;//下移最后一行置灰按钮
                    return;
                }
                iemMainPageExceptList.Remove(iemMainPageExcept);
                iemMainPageExceptList.Insert(index + 1, iemMainPageExcept);
                gridControl1.DataSource = new List<IemMainPageExcept>(iemMainPageExceptList);
                gvMainPage.MoveBy(index + 1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <auth>XLB</auth>
        /// <date>2013-04-10</date>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveIem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    SaveIemExcept();
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