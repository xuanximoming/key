using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class Templet_FootManger : DevBaseForm
    {
        IEmrHost m_app;

        /// <summary>
        /// 页脚实体
        /// </summary>
        public EmrTemplet_Foot m_emrtemplet_foot;

        /// <summary>
        /// 是否修改
        /// </summary>
        public bool IsEdit = false;

        public Templet_FootManger(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
 

        private void BindData()
        {

            SQLManger m_sqlManager = new SQLManger(m_app);
            DataTable dt = m_sqlManager.GetTempletFoot_Table("");

            this.gridControl1.DataSource = dt;
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Foot_ID"))
                return;

            BindEntityByDataRow(foucesRow);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            
 
        }

        private void BindEntityByDataRow(DataRow dr)
        {
            m_emrtemplet_foot = new EmrTemplet_Foot();
            m_emrtemplet_foot.FootId = dr["Foot_Id"].ToString();
            m_emrtemplet_foot.Name = dr["Name"].ToString();
            m_emrtemplet_foot.CreateDatetime = dr["CREATE_DATETIME"].ToString();
            m_emrtemplet_foot.CreatorId = dr["CREATOR_ID"].ToString();
            m_emrtemplet_foot.LastTime = dr["Last_Time"].ToString();
            m_emrtemplet_foot.HospitalCode = dr["Hospital_Code"].ToString();
            m_emrtemplet_foot.Content = dr["Content"].ToString();

        }

        private void Templet_FootManger_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delFoot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0) return;
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null || foucesRow.IsNull("Foot_ID"))
                {
                    return;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除 " + foucesRow["NAME"] + "吗？", "删除页眉", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                SQLManger m_sqlmanager = new SQLManger(m_app);
                m_sqlmanager.DelTempletFoot(foucesRow["Foot_Id"].ToString());

                BindData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
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

        private void btn_loadFoot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1_DoubleClick(null, null);
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

        private void btn_editFoot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("Foot_ID"))
                return;

            BindEntityByDataRow(foucesRow);

            IsEdit = true;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

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