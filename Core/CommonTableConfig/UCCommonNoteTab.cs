using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig
{
    /// <summary>
    /// 单据表格项界面
    /// </summary>
    public partial class UCCommonNoteTab : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        CommonNoteBiz m_CommonNoteBiz;
        public CommonNote_TabEntity m_commonNote_TabEntity;
        GridHitInfo downHitInfo = null;
        public List<CommonNote_ItemEntity> commonNote_ItemEntityDelList = new List<CommonNote_ItemEntity>();
        public UCCommonNoteTab(CommonNote_TabEntity commonNote_TabEntity, IEmrHost app)
        {
            try
            {
                m_app = app;
                m_commonNote_TabEntity = commonNote_TabEntity;
                InitializeComponent();
                RegisteEnvent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// edit by xlb
        /// 2013-01-12
        /// </summary>
        /// <param name="commonNote_TabEntity"></param>
        private void InitCommonNoteTabData(CommonNote_TabEntity commonNote_TabEntity)
        {
            try
            {
                if (commonNote_TabEntity == null) return;
                txtCommonTabName.Text = commonNote_TabEntity.CommonNoteTabName;
                txtOrdeCode.Text = commonNote_TabEntity.OrderCode;
                cboShowType.Text = commonNote_TabEntity.ShowType;
                cboUseRole.Text = commonNote_TabEntity.UsingRole;
                txtMaxRow.Text = commonNote_TabEntity.MaxRows;
                //初始化数据元
                gridControlCommomItem.DataSource = commonNote_TabEntity.CommonNote_ItemList;
                GetAllDataElement();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有数据元
        /// </summary>
        private void GetAllDataElement()
        {
            try
            {
                //将所有数据元都取出来 需要改进 点find按钮是获取 
                DataElementEntity dataElementEntity = new DataElementEntity();
                dataElementEntity.ElementId = "";
                dataElementEntity.ElementName = "";
                dataElementEntity.ElementPYM = "";
                dataElementEntity.ElementClass = "";
                DataElemntBiz dataElemntBiz = new CommonTableConfig.DataElemntBiz(m_app);
                if (CommonTabHelper.DataElementListAll == null)
                    CommonTabHelper.DataElementListAll = dataElemntBiz.GetDataElement(dataElementEntity);
                repositoryItemSearchLookUpEdit1.DataSource = CommonTabHelper.DataElementListAll;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增事件
        /// edit by xlb 203-01-21
        /// 对单据项进行排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<CommonNote_ItemEntity> commonNote_ItemEntityList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemEntityList == null)
                    commonNote_ItemEntityList = new List<CommonNote_ItemEntity>();
                CommonNote_ItemEntity commonNote_ItemEntity = new CommonNote_ItemEntity();
                commonNote_ItemEntity.IsValidate = "是";
                commonNote_ItemEntity.Valide = "1";
                commonNote_ItemEntityList.Add(commonNote_ItemEntity);
                gridControlCommomItem.DataSource = new List<CommonNote_ItemEntity>(commonNote_ItemEntityList);
                gvCommonItem.MoveBy(commonNote_ItemEntityList.Count - 1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 列表行上移事件
        /// xlb 2013-01-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                List<CommonNote_ItemEntity> commonNote_ItemList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemList == null)
                {
                    return;
                }
                CommonNote_ItemEntity commonNote_Item = gvCommonItem.GetFocusedRow() as CommonNote_ItemEntity;
                if (commonNote_Item == null)
                {
                    return;
                }
                int index = gvCommonItem.FocusedRowHandle;
                if (index < 1)
                {
                    //throw new Exception("第一行无法上移");
                    MessageBox.Show("第一行无法上移");
                    return;
                }
                commonNote_ItemList.Remove(commonNote_Item);
                commonNote_ItemList.Insert(index - 1, commonNote_Item);
                gridControlCommomItem.DataSource = new List<CommonNote_ItemEntity>(commonNote_ItemList);
                gvCommonItem.FocusedRowHandle = index - 1;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 列表行下移事件
        /// xlb 2013-01-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                List<CommonNote_ItemEntity> commonNote_ItemList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemList == null)
                {
                    return;
                }
                CommonNote_ItemEntity commonNote_Item = gvCommonItem.GetFocusedRow() as CommonNote_ItemEntity;
                if (commonNote_Item == null)
                {
                    return;
                }
                int index = gvCommonItem.FocusedRowHandle;
                int max = commonNote_ItemList.Count;
                if (index + 1 >= max)
                {
                    //throw new Exception("最后一行无法下移");
                    MessageBox.Show("最后一行无法下移");
                    return;
                }
                commonNote_ItemList.Remove(commonNote_Item);
                commonNote_ItemList.Insert(index + 1, commonNote_Item);
                gridControlCommomItem.DataSource = new List<CommonNote_ItemEntity>(commonNote_ItemList);
                gvCommonItem.FocusedRowHandle = index + 1;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号列
        /// xlb 2013-01-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCommonItem_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// edit xlb 2013-01-21
        /// 单据项排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNote_ItemEntity commonNote_ItemEntity = gvCommonItem.GetFocusedRow() as CommonNote_ItemEntity;
                int index = gvCommonItem.GetFocusedDataSourceRowIndex();
                List<CommonNote_ItemEntity> commonNote_ItemEntityListData = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemEntity == null) return;
                if (!string.IsNullOrEmpty(commonNote_ItemEntity.CommonNote_Item_Flow))
                {
                    commonNote_ItemEntity.Valide = "0";
                    commonNote_ItemEntityDelList.Add(commonNote_ItemEntity);
                }
                commonNote_ItemEntityListData.Remove(commonNote_ItemEntity);
                gridControlCommomItem.DataSource = new List<CommonNote_ItemEntity>(commonNote_ItemEntityListData);
                gvCommonItem.MoveBy(index);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void gvCommonItem_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.Name == "排序")
                {
                    int ordercode;
                    bool validate = Int32.TryParse(e.Value.ToString(), out ordercode);
                    if (!validate)
                    {
                        m_app.CustomMessageBox.MessageShow("排序码必须为数字");
                    }
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 验证
        /// edit by xlb 2013-01-12
        /// </summary>
        /// <returns></returns>
        public bool ValidateTab()
        {
            try
            {
                int intOrderCode;
                if (string.IsNullOrEmpty(txtCommonTabName.Text))
                {
                    m_app.CustomMessageBox.MessageShow("名称不能为空！");
                    txtCommonTabName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(cboUseRole.Text))
                {
                    m_app.CustomMessageBox.MessageShow("使用角色不能为空！");
                    cboUseRole.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(cboShowType.Text))
                {
                    m_app.CustomMessageBox.MessageShow("显示格式不能为空！");
                    cboShowType.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(txtOrdeCode.Text))
                {
                    m_app.CustomMessageBox.MessageShow("排序码不能为空！");
                    txtOrdeCode.Focus();
                    return false;
                }
                else if (!Int32.TryParse(txtOrdeCode.Text, out intOrderCode))
                {
                    m_app.CustomMessageBox.MessageShow("排序码必须为数值！");
                    txtOrdeCode.Focus();
                    return false;
                }
                else if (!string.IsNullOrEmpty(txtMaxRow.Text))// add 允许最大行数 可为空 或者大于0行
                {
                    int rowMax = 0;
                    if (!Int32.TryParse(txtMaxRow.Text, out rowMax))
                    {
                        MessageBox.Show("最大行数必须大于0");
                        this.ActiveControl = txtMaxRow;
                        return false;
                    }
                }
                List<CommonNote_ItemEntity> commonNote_ItemEntityList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemEntityList == null || commonNote_ItemEntityList.Count <= 0)
                {
                    throw new Exception("数据元项不能为空");
                }
                int index = 0;//对单据项排序需要 by xlb 2013-01-22
                foreach (var item in commonNote_ItemEntityList)//add by xlb 2013-01-22
                {
                    if (string.IsNullOrEmpty(item.DataElementId))
                    {
                        throw new Exception("数据元ID不能为空");
                    }
                    item.OrderCode = (++index).ToString();
                }

                #region 已注销 by XLB 2013-01-31
                //if (commonNote_ItemEntityList == null || commonNote_ItemEntityList.Count == 0)
                //{
                //    m_app.CustomMessageBox.MessageShow("数据元项不能为空！");
                //    return false;
                //}

                //foreach (var item in commonNote_ItemEntityList)
                //{
                //    int itemorder = 0;
                //    if (string.IsNullOrEmpty(item.OrderCode))
                //    {
                //        m_app.CustomMessageBox.MessageShow("排序码不能为空！");
                //        return false;
                //    }
                //    if (!Int32.TryParse(item.OrderCode, out itemorder))
                //    {
                //        m_app.CustomMessageBox.MessageShow("排序码必须为数值！");
                //        return false;
                //    }
                //    if (string.IsNullOrEmpty(item.IsValidate))
                //    {
                //        m_app.CustomMessageBox.MessageShow("是否校验数据元字段不能为空！");
                //        return false;
                //    }
                //}
                #endregion
                #region 类表已隐藏该字段，自动排序无需此校验 xlb 2013-01-22
                //for (int i = 0; i < commonNote_ItemEntityList.Count; i++)
                //{
                //    for (int j = i + 1; j < commonNote_ItemEntityList.Count; j++)
                //    {
                //        if (commonNote_ItemEntityList[i].OrderCode == commonNote_ItemEntityList[j].OrderCode)
                //        {
                //            m_app.CustomMessageBox.MessageShow("排序码不能相同！");
                //            return false;
                //        }
                //    }
                //}
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断别名是否存在空
        /// </summary>
        /// <returns>true 存在 false不存在</returns>
        public bool HasBieMingNull()
        {
            try
            {
                List<CommonNote_ItemEntity> commonNote_ItemEntityList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemEntityList == null) return false;
                foreach (var item in commonNote_ItemEntityList)
                {
                    if (string.IsNullOrEmpty(item.OtherName))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //保存
        public void SaveCommonNoteTab()
        {
            try
            {
                m_commonNote_TabEntity.CommonNoteTabName = txtCommonTabName.Text;
                m_commonNote_TabEntity.OrderCode = txtOrdeCode.Text;
                m_commonNote_TabEntity.ShowType = cboShowType.Text;
                m_commonNote_TabEntity.UsingRole = cboUseRole.Text;
                m_commonNote_TabEntity.Valide = "1";
                m_commonNote_TabEntity.MaxRows = txtMaxRow.Text == null ? "" : txtMaxRow.Text.Trim();
                List<CommonNote_ItemEntity> commonNote_ItemEntityList = gridControlCommomItem.DataSource as List<CommonNote_ItemEntity>;
                if (commonNote_ItemEntityList == null)
                    commonNote_ItemEntityList = new List<CommonNote_ItemEntity>();
                //保存所有的对象包括已删除的对象，删除的对象为无效数据
                commonNote_ItemEntityList.AddRange(commonNote_ItemEntityDelList);
                m_commonNote_TabEntity.CommonNote_ItemList = commonNote_ItemEntityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCCommonNoteTab_Load(object sender, EventArgs e)
        {
            try
            {
                InitCommonNoteTabData(m_commonNote_TabEntity);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //数据元改变时触发事件 对CommonNote_ItemEntity进行赋值
        private void repositoryItemSearchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchLookUpEdit searchLookUpEdit = sender as SearchLookUpEdit;
                DataElementEntity dataElementEntity = searchLookUpEdit.Properties.View.GetFocusedRow() as DataElementEntity;
                CommonNote_ItemEntity commonNote_Item = gvCommonItem.GetFocusedRow() as CommonNote_ItemEntity;
                if (dataElementEntity != null && commonNote_Item != null)
                {
                    commonNote_Item.DataElementName = dataElementEntity.ElementName;
                    commonNote_Item.DataElementId = dataElementEntity.ElementId;
                    commonNote_Item.DataElementFlow = dataElementEntity.ElementFlow;
                    commonNote_Item.OtherName = dataElementEntity.ElementName;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 暂时未使用 xlb 2013-01-21
        // /// <summary>
        // /// 拖动完成后触发的事件
        // /// xlb 2013-01-21
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        //private void gridControlCommomItem_DragDrop(object sender,DragEventArgs e)
        //{
        //   try
        //   {
        //      GridHitInfo  hitinfo=gvCommonItem.CalcHitInfo(gvCommonItem.GridControl.PointToClient(new Point(e.X,e.Y)));
        //      if(hitinfo.InRow)
        //      {
        //         return;
        //      }
        //   }
        //   catch(Exception ex)
        //   {
        //    MyMessageBox.Show(1, ex);
        //   }
        //}

        // /// <summary>
        // /// 将对象拖过对象边界时触发
        // /// xlb 2013-01-21
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        //private void gridControlCommomItem_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Data.GetDataPresent(typeof(DataRow)))
        //        {
        //            e.Effect = DragDropEffects.Move;
        //        }
        //        else 
        //        {
        //            e.Effect = DragDropEffects.None;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}

        // /// <summary>
        // /// 鼠标按下事件
        // /// xlb 2013-01-21
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        //private void gvCommonItem_MouseDown(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        GridView view = sender as GridView;
        //        downHitInfo = null;
        //        GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
        //        if (Control.ModifierKeys != Keys.None)
        //        {
        //            return;
        //        }
        //        if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
        //        {
        //            downHitInfo = hitInfo;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}

        // /// <summary>
        // /// 鼠标移动事件
        // /// xlb 2013-01-21
        // /// </summary>
        // /// <param name="sender"></param>
        // /// <param name="e"></param>
        //private void gvCommonItem_MouseMove(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        GridView view = sender as GridView;
        //        if (e.Button == MouseButtons.Left && downHitInfo != null)
        //        {
        //            Size dragSize = SystemInformation.DragSize;
        //            Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
        //                downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

        //            if (!dragRect.Contains(new Point(e.X, e.Y)))
        //            {
        //                object row = view.GetRow(downHitInfo.RowHandle);
        //                view.GridControl.DoDragDrop(row, DragDropEffects.Move);
        //                downHitInfo = null;
        //                DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 配置是否可编辑
        /// </summary>
        /// <param name="canEdit"></param>
        public void SetFormEdit(bool canEdit)
        {
            try
            {
                btnUp.Enabled = btnDown.Enabled = btnAdd.Enabled = canEdit;
                btnDel.Enabled = canEdit;
                hyLinkRemove.Enabled = canEdit;
                //txtCommonTabName.Properties.ReadOnly = !canEdit;
                txtOrdeCode.Properties.ReadOnly = !canEdit;
                cboShowType.Properties.ReadOnly = !canEdit;
                cboUseRole.Properties.ReadOnly = !canEdit;
                //gvCommonItem.OptionsBehavior.Editable = canEdit;
                gridColumn2.OptionsColumn.AllowEdit = canEdit;
                gridColumn4.OptionsColumn.AllowEdit = canEdit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立事件
        /// xlb 2013-01-21
        /// </summary>
        private void RegisteEnvent()
        {
            try
            {
                repositoryItemSearchLookUpEdit1.EditValueChanged +=
                new EventHandler(repositoryItemSearchLookUpEdit1_EditValueChanged);//xlb 2013-01-21 
                btnUp.Click += new EventHandler(btnUp_Click);//xlb 2013-01-21
                btnDown.Click += new EventHandler(btnDown_Click);//xlb 2013-01-21
                //gridControlCommomItem.DragDrop+=new DragEventHandler(gridControlCommomItem_DragDrop);
                //gridControlCommomItem.DragOver+=new DragEventHandler(gridControlCommomItem_DragOver);
                //gvCommonItem.MouseDown+=new MouseEventHandler(gvCommonItem_MouseDown);
                //.MouseMove+=new MouseEventHandler(gvCommonItem_MouseMove);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
