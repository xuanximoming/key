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
using DrectSoft.Common.Ctrs.FORM;
using MedicalRecordManage.Object;

namespace MedicalRecordManage.UI
{
    /// <summary>
    /// 如果有的病人有小孩，此窗体弹出选择患者还是其孩子
    /// add by  ywk 2012年6月8日 11:46:33
    /// </summary>
    public partial class QueryPatientStatus : DevBaseForm
    {
        IEmrHost m_app;
        public string NOOfINPAT { get; set; }
        public string Incount { get; set; }
        DataTable dt = new DataTable();
        public QueryPatientStatus(DataTable dt_inpatient)
        {
            dt = dt_inpatient;
            InitializeComponent();
        }
        public QueryPatientStatus(IEmrHost myapp, DataTable dt_inpatient)
        {
            m_app = myapp;
            dt = dt_inpatient;
            InitializeComponent();
        }
        #region 事件
        /// <summary>
        ///确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            DataRow row = gridViewPaient.GetDataRow(gridViewPaient.FocusedRowHandle);//取得选择的行
            if (row == null)
            {
                return;
            }
            NOOfINPAT = row["patid"].ToString();//病人首页序号，用于返回上级窗体
            Incount = row["incount"].ToString();
            if (!string.IsNullOrEmpty(NOOfINPAT))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            this.Close();
        }

        public bool GoToEmrInupt { get; set; }//声明变量，双击进入文书录入 add ywk 
        private void gridViewPaient_DoubleClick(object sender, EventArgs e)
        {
            DataRow row = gridViewPaient.GetDataRow(gridViewPaient.FocusedRowHandle);//取得选择的行
            if (row == null)
            {
                return;
            }
            NOOfINPAT = row["patid"].ToString();//病人首页序号，用于返回上级窗体
            Incount = row["incount"].ToString();
            GoToEmrInupt = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        StringFormat sf = new StringFormat();
        /// <summary>
        /// 标注母亲和子女
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPaient_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            if (e.CellValue == null) return;
            DataRowView drv = gridViewPaient.GetRow(e.RowHandle) as DataRowView;
            //取得婴儿标志
            string isbaby = drv["isbaby"].ToString().Trim();
            string patname = drv["patname"].ToString().Trim();
            if (e.Column == gridColumn1)
            {
                if (isbaby=="1")//是婴儿
                {
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname+"【婴儿】", e.Appearance.Font, Brushes.Blue, e.Bounds, sf);
                    e.Handled = true;
                }
                if (isbaby=="0")
                {
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname + "【母亲】", e.Appearance.Font, Brushes.Blue, e.Bounds, sf);
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoosePatOrBaby_Load(object sender, EventArgs e)
        {
            LoadPatAndBaby();
        }
        #endregion
        
        #region 方法
        /// <summary>
        /// 此列表应显示患者和她的孩子
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void LoadPatAndBaby()
        {
            try
            {
                //加载性别图片
                //YD_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);

                //DataTable dt = SqlUtil.GetPatAndBaby(NOOfINPAT);
                this.gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPaient_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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