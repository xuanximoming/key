﻿using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class ShowTempletHeader : DevBaseForm
    {
        IEmrHost m_app;
        /// <summary>
        /// 是否修改
        /// </summary>
        public bool IsEdit = false;

        /// <summary>
        /// 页眉实体
        /// </summary>
        public EmrTempletHeader m_emrtempletheader;

        public ShowTempletHeader(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        /// <summary>
        /// GetTempletId
        /// </summary>
        private string TempletId = null;
        public void GetTempletId(string TempletId)
        {
            this.TempletId = TempletId;
        }


        private void ShowTempletHeader_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            SQLManger m_sqlManager = new SQLManger(m_app);
            DataTable dt = m_sqlManager.GetTempletHeader_Table("");

            this.gridControl1.DataSource = dt;
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Header_ID"))
                return;
            BindEntityByDataRow(foucesRow);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void BindEntityByDataRow(DataRow dr)
        {
            try
            {
                m_emrtempletheader = new EmrTempletHeader();
                m_emrtempletheader.HeaderId = dr["Header_Id"].ToString();
                string StMrName = null;
                string StContent = null;
                StContent = dr["Content"].ToString();
                if (dr["Name"].ToString() == "知情同意书")//add by Ukey 2016-08-25 处理 知情同意书页眉数量过多
                {
                    string SqlEmrTempFile = @"select mr_name from EMRTEMPLET where templet_id = '" + TempletId + "'";
                    DataTable DtEmrTempFile = m_app.SqlHelper.ExecuteDataTable(SqlEmrTempFile);
                    if (DtEmrTempFile.Rows.Count >= 1)
                    {
                        DataRow DrDtEmrTempFile = DtEmrTempFile.Rows[0];
                        StMrName = DrDtEmrTempFile["mr_name"].ToString();
                        StContent = StContent.Replace("知情同意书名称", StMrName);
                    }
                }
                else if (dr["Name"].ToString() == "其他记录")//add by Ukey 2016-08-25 处理 其他记录页眉数量过多
                {
                    string SqlEmrTempFile = @"select mr_name from EMRTEMPLET where templet_id = '" + TempletId + "'";
                    DataTable DtEmrTempFile = m_app.SqlHelper.ExecuteDataTable(SqlEmrTempFile);
                    if (DtEmrTempFile.Rows.Count >= 1)
                    {
                        DataRow DrDtEmrTempFile = DtEmrTempFile.Rows[0];
                        StMrName = DrDtEmrTempFile["mr_name"].ToString();
                        StContent = StContent.Replace("其他记录名称", StMrName);
                    }
                }
                else if (dr["Name"].ToString() == "护理文档")//add by Ukey 2016-08-25 处理 护理文档页眉数量过多
                {
                    string SqlEmrTempFile = @"select mr_name from EMRTEMPLET where templet_id = '" + TempletId + "'";
                    DataTable DtEmrTempFile = m_app.SqlHelper.ExecuteDataTable(SqlEmrTempFile);
                    if (DtEmrTempFile.Rows.Count >= 1)
                    {
                        DataRow DrDtEmrTempFile = DtEmrTempFile.Rows[0];
                        StMrName = DrDtEmrTempFile["mr_name"].ToString();
                        StContent = StContent.Replace("护理文档名称", StMrName);
                    }
                }
                else if (dr["Name"].ToString() == "手术护理记录")//add by Ukey 2016-08-25 处理 护理文档页眉数量过多
                {
                    string SqlEmrTempFile = @"select mr_name from EMRTEMPLET where templet_id = '" + TempletId + "'";
                    DataTable DtEmrTempFile = m_app.SqlHelper.ExecuteDataTable(SqlEmrTempFile);
                    if (DtEmrTempFile.Rows.Count >= 1)
                    {
                        DataRow DrDtEmrTempFile = DtEmrTempFile.Rows[0];
                        StMrName = DrDtEmrTempFile["mr_name"].ToString();
                        StContent = StContent.Replace("手术护理记录名称", StMrName);
                    }
                }
                m_emrtempletheader.Name = dr["Name"].ToString();
                m_emrtempletheader.CreateDatetime = dr["CREATE_DATETIME"].ToString();
                m_emrtempletheader.CreatorId = dr["CREATOR_ID"].ToString();
                m_emrtempletheader.LastTime = dr["Last_Time"].ToString();
                m_emrtempletheader.HospitalCode = dr["Hospital_Code"].ToString();
                m_emrtempletheader.Content = StContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null || foucesRow.IsNull("Header_Id"))
                {
                    return;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除 " + foucesRow["NAME"] + "吗？", "删除页眉", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                SQLManger m_sqlmanager = new SQLManger(m_app);
                m_sqlmanager.DelTempletHeader(foucesRow["Header_Id"].ToString());

                BindData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Header_Id"))
                return;

            BindEntityByDataRow(foucesRow);

            IsEdit = true;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btn_Load_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1_DoubleClick(null, null);
        }

        /// <summary>
        /// 右键事件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、小标题右键无操作
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (e.Button == MouseButtons.Right)
                {
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Control == gridControl1)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
    }
}