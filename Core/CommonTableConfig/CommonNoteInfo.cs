using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig
{
    /// <summary>
    /// xll
    /// 2012-12-17
    /// 新增修改单据信息
    /// </summary>
    public partial class CommonNoteInfo : DevBaseForm
    {
        CommonNoteEntity m_CommonNoteEntity;
        IEmrHost m_app;
        CommonNoteBiz m_CommonNoteBiz;
        List<CommonNote_TabEntity> commonNote_TabEntityDel = new List<CommonNote_TabEntity>();
        CommonNoteCountEntity m_commonNoteCountEntity;
        public CommonNoteInfo(CommonNoteEntity commonNoteEntity, IEmrHost app)
        {
            try
            {
                m_CommonNoteEntity = commonNoteEntity;
                m_app = app;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取统计数据 xll 20130620
        /// </summary>
        private void InitCommonCount()
        {
            try
            {
                chkCount.Checked = false;
                if (string.IsNullOrEmpty(m_CommonNoteEntity.CommonNoteFlow))
                {
                    return;
                }
                m_commonNoteCountEntity = CommonNoteBiz.GetCommonNoteCount(m_CommonNoteEntity.CommonNoteFlow);
                if (m_commonNoteCountEntity == null) return;
                txtitemCount.Text = m_commonNoteCountEntity.ItemCount;
                txt12Name.Text = m_commonNoteCountEntity.Hour12Name == null ? "12小时统计" : m_commonNoteCountEntity.Hour12Name;
                txt24Name.Text = m_commonNoteCountEntity.Hour24Name == null ? "24小时统计" : m_commonNoteCountEntity.Hour24Name;
                chkCount.Checked = m_commonNoteCountEntity.Valide == "1" ? true : false;
                tim12.Time = string.IsNullOrEmpty(m_commonNoteCountEntity.Hour12Time) ? Convert.ToDateTime("2013-06-20 07:00:00") : Convert.ToDateTime(m_commonNoteCountEntity.Hour12Time);
                tim24.Time = string.IsNullOrEmpty(m_commonNoteCountEntity.Hour24Time) ? Convert.ToDateTime("2013-06-20 19:00:00") : Convert.ToDateTime(m_commonNoteCountEntity.Hour24Time);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void UCCommonNoteInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitDataBase();
                InitCommonNoteData();
                if (String.IsNullOrEmpty(m_CommonNoteEntity.CommonNoteFlow))
                {
                    this.Text = " 新增" + m_CommonNoteEntity.CommonNoteName;
                }
                else
                {
                    bool canedit = CanEditCommonNote(m_CommonNoteEntity);
                    if (canedit)
                    {
                        this.Text = "编辑" + m_CommonNoteEntity.CommonNoteName;
                    }
                    else
                    {
                        this.Text = m_CommonNoteEntity.CommonNoteName + "已被使用，无法修改。如需修改，请重新添加通用单据";
                        SetFormEdit(canedit);
                    }
                }

                InitCommonCount();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 初始化信息 包括所有的科室和病区等
        /// </summary>
        private void InitDataBase()
        {
            try
            {
                //chcomboxDepart
                if (m_CommonNoteBiz == null)
                    m_CommonNoteBiz = new CommonNoteBiz(m_app);
                var basedictoryListAll = m_CommonNoteBiz.GetAllDepartAndAreas();
                List<BaseDictory> basedictoryListDepart = basedictoryListAll["01"];
                List<BaseDictory> basedictoryListAreas = basedictoryListAll["02"];
                InitChcombox(chListDept, basedictoryListDepart);
                InitChcombox(chlistWard, basedictoryListAreas);
                DataTable dtPrint = m_CommonNoteBiz.GetPrintTem();
                cboxPrint.Properties.Items.Clear();
                if (dtPrint != null)
                {
                    foreach (DataRow item in dtPrint.Rows)
                    {
                        string tempDesc = item["TEMPDESC"].ToString();
                        if (item["TEMPDESC"].ToString().Length > 10)
                        {
                            tempDesc = tempDesc.Substring(0, 10);
                        }
                        string itemValue = item["TEMPNAME"].ToString() + "--" + tempDesc;
                        cboxPrint.Properties.Items.Add(itemValue);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化CommonNoteEntity中的信息到控件上
        /// </summary>
        private void InitCommonNoteData()
        {
            try
            {
                if (!string.IsNullOrEmpty(m_CommonNoteEntity.CommonNoteFlow))
                {
                    m_CommonNoteEntity = m_CommonNoteBiz.GetDetailCommonNote(m_CommonNoteEntity.CommonNoteFlow);
                }
                txtNoteName.Text = m_CommonNoteEntity.CommonNoteName;

                DataTable dtPrint = m_CommonNoteBiz.GetPrintTem();
                //处理打印模板 显示模板名和描述
                string printShowName = m_CommonNoteEntity.PrinteModelName;
                if (dtPrint != null && dtPrint.Rows.Count > 0)
                {
                    foreach (DataRow dritem in dtPrint.Rows)
                    {
                        int myIndex = dritem["TEMPNAME"].ToString().IndexOf(".");
                        if (myIndex > 0 && dritem["TEMPNAME"].ToString().Substring(0, myIndex) == m_CommonNoteEntity.PrinteModelName)
                        {
                            printShowName = dritem["TEMPNAME"].ToString() + "--" + dritem["TEMPDESC"].ToString();
                            break;
                        }
                    }
                }
                cboxPrint.Text = printShowName;
                comboxShowType.Text = m_CommonNoteEntity.ShowType;
                chboxUsing.Checked = m_CommonNoteEntity.UsingFlag == "1" ? true : false;
                chboxPicEdit.Checked = m_CommonNoteEntity.UsingPicSign == "1" ? true : false;
                //chboxCheck.Checked = m_CommonNoteEntity.UsingCheckDoc == "1" ? true : false;
                //加载科室信息
                InitChcomBoxChecked(chListDept, m_CommonNoteEntity.BaseDepartments);
                //加载病区信息
                InitChcomBoxChecked(chlistWard, m_CommonNoteEntity.BaseAreas);
                //加载commonNoteTab
                InitCommonNoteTab(m_CommonNoteEntity.CommonNote_TabList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitCommonNoteTab(List<CommonNote_TabEntity> commonNote_TabEntityList)
        {
            try
            {
                if (commonNote_TabEntityList == null)
                {
                    commonNote_TabEntityList = new List<CommonNote_TabEntity>();
                    commonNote_TabEntityList.Add(new CommonNote_TabEntity());
                }
                for (int i = 0; i < commonNote_TabEntityList.Count; i++)
                {
                    AddCommonNoteTab(commonNote_TabEntityList[i], i + 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddCommonNoteTab(CommonNote_TabEntity commonNote_TabEntity, int index)
        {
            try
            {
                UCCommonNoteTab uCCommonNoteTab = new UCCommonNoteTab(commonNote_TabEntity, m_app);
                uCCommonNoteTab.groupControlTab.Text = "表格" + index;
                if (index == 1)
                {
                    uCCommonNoteTab.hyLinkRemove.Visible = false;
                }
                uCCommonNoteTab.hyLinkRemove.Click += delegate(object sender, EventArgs e)
                {
                    if (!string.IsNullOrEmpty(uCCommonNoteTab.m_commonNote_TabEntity.CommonNote_Tab_Flow))
                    {
                        uCCommonNoteTab.m_commonNote_TabEntity.Valide = "0";
                        commonNote_TabEntityDel.Add(uCCommonNoteTab.m_commonNote_TabEntity);
                    }
                    xScorlTablist.Controls.Remove(uCCommonNoteTab);
                    SetBiaoGeXuHao();
                };
                uCCommonNoteTab.Dock = DockStyle.Top;
                xScorlTablist.Controls.Add(uCCommonNoteTab);
                uCCommonNoteTab.BringToFront();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 设置表格的序号
        /// </summary>
        private void SetBiaoGeXuHao()
        {
            try
            {
                for (int i = 0; i < xScorlTablist.Controls.Count; i++)
                {
                    UCCommonNoteTab uCCommonNoteTab = xScorlTablist.Controls[i] as UCCommonNoteTab;
                    if (uCCommonNoteTab == null) continue;
                    uCCommonNoteTab.groupControlTab.Text = "表格" + (i + 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化多选控件所有选项
        /// </summary>
        /// <param name="chboxedit"></param>
        /// <param name="baseDictoryList"></param>
        private void InitChcombox(CheckedListBoxControl chboxList, List<BaseDictory> baseDictoryList)
        {
            try
            {
                chboxList.Items.Clear();
                foreach (var item in baseDictoryList)
                {
                    CheckedListBoxItem checkedListBoxItem = new CheckedListBoxItem(item, item.Name);
                    chboxList.Items.Add(checkedListBoxItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //根据配置单中科室和病区状况初始化多选控件选中状态
        private void InitChcomBoxChecked(CheckedListBoxControl chboxList, List<BaseDictory> baseDictoryListChecked)
        {
            try
            {
                if (chboxList.Items == null) return;
                foreach (CheckedListBoxItem boxitem in chboxList.Items)
                {
                    boxitem.CheckState = CheckState.Unchecked;
                    BaseDictory baseDictoryChbox = boxitem.Value as BaseDictory;
                    if (baseDictoryListChecked == null) continue;
                    foreach (var baseitem in baseDictoryListChecked)
                    {
                        if (baseitem.Id == baseDictoryChbox.Id)
                        {
                            boxitem.CheckState = CheckState.Checked;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取控件选中的科室或病区
        /// </summary>
        /// <param name="chboxedit"></param>
        /// <returns></returns>
        private List<BaseDictory> GetDepartAndAreas(CheckedListBoxControl chboxedit)
        {
            try
            {
                List<Object> objectList = chboxedit.Items.GetCheckedValues();
                List<BaseDictory> baseDictoryList = new List<BaseDictory>();
                if (objectList == null) return baseDictoryList;
                foreach (var item in objectList)
                {
                    baseDictoryList.Add(item as BaseDictory);
                }
                return baseDictoryList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 追加表格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNote_TabEntity commonNote_TabEntity = new CommonNote_TabEntity();
                AddCommonNoteTab(commonNote_TabEntity, xScorlTablist.Controls.Count + 1);
            }
            catch (Exception ex)
            {
                //m_app.CustomMessageBox.MessageShow(ex.Message + ex.StackTrace);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 验证通用单
        /// </summary>
        /// <returns></returns>
        public bool ValidateCommonNote()
        {
            try
            {
                if (chkCount.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtitemCount.Text))
                    {
                        m_app.CustomMessageBox.MessageShow("统计项不能为空");
                        txtitemCount.Focus();
                        return false;
                    }
                    else if (string.IsNullOrEmpty(txt12Name.Text))
                    {
                        m_app.CustomMessageBox.MessageShow("12小时统计名称不能为空");
                        txt12Name.Focus();
                        return false;
                    }
                    else if (string.IsNullOrEmpty(txt24Name.Text))
                    {
                        m_app.CustomMessageBox.MessageShow("24小时统计名称不能为空");
                        txt24Name.Focus();
                        return false;
                    }
                }
                if (string.IsNullOrEmpty(txtNoteName.Text))
                {
                    m_app.CustomMessageBox.MessageShow("通用单名称不能为空");
                    txtNoteName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(cboxPrint.Text))
                {

                    m_app.CustomMessageBox.MessageShow("打印模板名称不能为空");
                    cboxPrint.Focus();
                    return false;
                }
                //else if (string.IsNullOrEmpty(comboxShowType.Text))
                //{
                //    m_app.CustomMessageBox.MessageShow("展示形式不能为空");
                //    comboxShowType.Focus();
                //    return false;
                //}   //暂时不使用展示形式
                List<string> orderTabList = new List<string>(); //表格的排序码
                foreach (UCCommonNoteTab item in xScorlTablist.Controls)
                {
                    bool isValidateTab = item.ValidateTab();
                    if (!isValidateTab)
                    {
                        return false;
                    }
                    if (chboxUsing.Checked == true)
                    {
                        bool noBieMing = item.HasBieMingNull();
                        if (noBieMing)
                        {
                            m_app.CustomMessageBox.MessageShow("别名为空时，不能启用快速录入功能");
                            return false;
                        }
                    }
                    orderTabList.Add(item.txtOrdeCode.Text);
                }
                for (int i = 0; i < orderTabList.Count; i++)
                {
                    for (int q = i + 1; q < orderTabList.Count; q++)
                    {
                        if (orderTabList[i] == orderTabList[q])
                        {
                            m_app.CustomMessageBox.MessageShow("表格排序码不能相同");
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

        public CommonNoteEntity SaveCommonNote()
        {
            try
            {
                m_CommonNoteEntity.CommonNoteName = txtNoteName.Text;

                string printShowName = cboxPrint.Text;
                int jiequIndex = printShowName.IndexOf("."); //截取--之前的保存 如SSJLD--手术记录单 只保存SSJLD
                if (jiequIndex > 0)
                {
                    printShowName = printShowName.Substring(0, jiequIndex);
                }

                m_CommonNoteEntity.PrinteModelName = printShowName;
                m_CommonNoteEntity.ShowType = comboxShowType.Text;
                m_CommonNoteEntity.UsingFlag = chboxUsing.Checked == true ? "1" : "0";

                m_CommonNoteEntity.UsingPicSign = chboxPicEdit.Checked == true ? "1" : "0";
                //m_CommonNoteEntity.UsingCheckDoc =  chboxCheck.Checked == true ? "1" : "0";

                m_CommonNoteEntity.BaseAreas = GetDepartAndAreas(chlistWard);
                m_CommonNoteEntity.BaseDepartments = GetDepartAndAreas(chListDept);
                m_CommonNoteEntity.Valide = "1";
                m_CommonNoteEntity.CommonNote_TabList = new List<CommonNote_TabEntity>();
                foreach (UCCommonNoteTab item in xScorlTablist.Controls)
                {
                    item.SaveCommonNoteTab();
                    m_CommonNoteEntity.CommonNote_TabList.Add(item.m_commonNote_TabEntity);
                }
                m_CommonNoteEntity.CommonNote_TabList.AddRange(commonNote_TabEntityDel);
                return m_CommonNoteEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CommonNoteCountEntity SaveCommonCount()
        {
            try
            {
                if (m_commonNoteCountEntity == null)
                    m_commonNoteCountEntity = new CommonNoteCountEntity();
                if (chkCount.Checked == true)
                {
                    m_commonNoteCountEntity.Valide = "1";
                }
                else
                {
                    m_commonNoteCountEntity.Valide = "0";
                }
                m_commonNoteCountEntity.Hour12Name = txt12Name.Text;
                m_commonNoteCountEntity.Hour24Name = txt24Name.Text;
                m_commonNoteCountEntity.ItemCount = txtitemCount.Text;
                m_commonNoteCountEntity.Hour12Time = tim12.Time.ToString("yyyy-MM-dd HH:mm:ss");
                m_commonNoteCountEntity.Hour24Time = tim24.Time.ToString("yyyy-MM-dd HH:mm:ss");
                m_commonNoteCountEntity.CommonNoteFlow = m_CommonNoteEntity.CommonNoteFlow;
                return m_commonNoteCountEntity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //科室checked时的操作
        private void chListDept_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                string checkedtxt = "";
                foreach (CheckedListBoxItem item in chListDept.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        checkedtxt += item.Description + ",";
                    }
                }
                if (checkedtxt.Length > 0)
                    checkedtxt = checkedtxt.Substring(0, checkedtxt.Length - 1);
                popEditDept.Text = checkedtxt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //病区选择时的操作
        private void chlistWard_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                string checkedtxt = "";
                foreach (CheckedListBoxItem item in chlistWard.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        checkedtxt += item.Description + ",";
                    }
                }
                if (checkedtxt.Length > 0)
                    checkedtxt = checkedtxt.Substring(0, checkedtxt.Length - 1);
                popEditWard.Text = checkedtxt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 设置窗体不可编辑
        /// </summary>
        /// <param name="canEdit"></param>
        public void SetFormEdit(bool canEdit)
        {
            try
            {
                txtNoteName.Properties.ReadOnly = !canEdit;
                cboxPrint.Properties.ReadOnly = !canEdit;
                popEditDept.Properties.ReadOnly = false;
                popEditWard.Properties.ReadOnly = false;
                chboxUsing.Properties.ReadOnly = false;
                chboxPicEdit.Properties.ReadOnly = !canEdit;
                //chboxCheck.Properties.ReadOnly = !canEdit;
                btnAddTable.Enabled = canEdit;
                btnSave.Enabled = true;
                foreach (var item in xScorlTablist.Controls)
                {
                    UCCommonNoteTab uCCommonNoteTab = (item as UCCommonNoteTab);
                    if (uCCommonNoteTab == null) continue;
                    uCCommonNoteTab.SetFormEdit(canEdit);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断是否可编辑 如果该条单据已经使用 则不可再对该条单据进行修改
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <returns></returns>
        private bool CanEditCommonNote(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(commonNoteEntity.CommonNoteFlow))
                {
                    return true;
                }
                else
                {
                    string sql = string.Format("select count(*) from incommonnote i where i.commonnoteflow='{0}' and i.valide='1'", commonNoteEntity.CommonNoteFlow);
                    DataTable dtCount = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                    if (dtCount == null || dtCount.Rows == null | dtCount.Rows.Count <= 0)
                    {
                        return true;
                    }
                    else if (dtCount.Rows[0][0].ToString() == "0")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkCount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCount.Checked == true)
            {
                gcCount.Visible = true;
            }
            else
            {
                gcCount.Visible = false;
            }
        }



    }
}
