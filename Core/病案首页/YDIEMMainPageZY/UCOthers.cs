using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Convertmy = DrectSoft.Core.UtilsForExtension;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 费用界面
    /// </summary>
    public partial class UCOthers : UserControl
    {
        string noofpat;
        private IemMainPageInfo m_IemInfo;
        private IEmrHost m_App;

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        #region 费用编辑界面相关方法
        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="noofpat"></param>
        public UCOthers(string noofpat)
        {
            try
            {
                this.noofpat = noofpat;
                InitializeComponent();
                this.ActiveControl = txtTotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FILL UI
        /// </summary>
        /// <param name="info"></param>
        /// <param name="app"></param>
        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;
                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate void FillUIDelegate();

        private void FillUIInner()
        {
            try
            {
                txtTotal.Text = m_IemInfo.IemFeeInfo.Total.Trim('-');
                txtOwnFee.Text = m_IemInfo.IemFeeInfo.OwnFee.Trim('-');
                txtYBYLFY.Text = m_IemInfo.IemFeeInfo.YBYLFY.Trim('-');

                txtZYBZLZF.Text = m_IemInfo.IemFeeInfo.ZYBZLZF.Trim('-');
                txtZYBZLZHZF.Text = m_IemInfo.IemFeeInfo.ZYBZLZHZF.Trim('-');

                txtYBZLFY.Text = m_IemInfo.IemFeeInfo.YBZLFY.Trim('-');
                txtCare.Text = m_IemInfo.IemFeeInfo.Care.Trim('-');

                txtZHQTFY.Text = m_IemInfo.IemFeeInfo.ZHQTFY.Trim('-');
                txtBLZDF.Text = m_IemInfo.IemFeeInfo.BLZDF.Trim('-');
                txtSYSZDF.Text = m_IemInfo.IemFeeInfo.SYSZDF.Trim('-');
                txtYXXZDF.Text = m_IemInfo.IemFeeInfo.YXXZDF.Trim('-');
                txtLCZDF.Text = m_IemInfo.IemFeeInfo.LCZDF.Trim('-');

                txtFSSZLF.Text = m_IemInfo.IemFeeInfo.FSSZLF.Trim('-');
                txtLCWLZLF.Text = m_IemInfo.IemFeeInfo.LCWLZLF.Trim('-');
                txtSSZLF.Text = m_IemInfo.IemFeeInfo.SSZLF.Trim('-');
                txtMZF.Text = m_IemInfo.IemFeeInfo.MZF.Trim('-');
                txtSSF.Text = m_IemInfo.IemFeeInfo.SSF.Trim('-');

                txtKFF.Text = m_IemInfo.IemFeeInfo.KFF.Trim('-');

                txtZYZDF.Text = m_IemInfo.IemFeeInfo.ZYZDF.Trim('-');
                txtZYZLF.Text = m_IemInfo.IemFeeInfo.ZYZLF.Trim('-');
                txtZYWZ.Text = m_IemInfo.IemFeeInfo.ZYWZ.Trim('-');
                txtZYGS.Text = m_IemInfo.IemFeeInfo.ZYGS.Trim('-');
                txtZCYJF.Text = m_IemInfo.IemFeeInfo.ZCYJF.Trim('-');
                txtZYTNZL.Text = m_IemInfo.IemFeeInfo.ZYTNZL.Trim('-');
                txtZYGCZL.Text = m_IemInfo.IemFeeInfo.ZYGCZL.Trim('-');
                txtZYTSZL.Text = m_IemInfo.IemFeeInfo.ZYTSZL.Trim('-');
                txtZYQT.Text = m_IemInfo.IemFeeInfo.ZYQT.Trim('-');
                txtZYTSTPJG.Text = m_IemInfo.IemFeeInfo.ZYTSTPJG.Trim('-');
                txtBZSS.Text = m_IemInfo.IemFeeInfo.BZSS.Trim('-');

                txtXYF.Text = m_IemInfo.IemFeeInfo.XYF.Trim('-');
                txtKJYWF.Text = m_IemInfo.IemFeeInfo.KJYWF.Trim('-');
                txtCPMedical.Text = m_IemInfo.IemFeeInfo.CPMedical.Trim('-');
                m_IemInfo.IemFeeInfo.YLJGZYZJF = txtYLJGZYZJF.Text.Trim('-');

                txtCMedical.Text = m_IemInfo.IemFeeInfo.CMedical.Trim('-');
                txtBloodFee.Text = m_IemInfo.IemFeeInfo.BloodFee.Trim('-');
                txtXDBLZPF.Text = m_IemInfo.IemFeeInfo.XDBLZPF.Trim('-');
                txtQDBLZPF.Text = m_IemInfo.IemFeeInfo.QDBLZPF.Trim('-');
                txtNXYZLZPF.Text = m_IemInfo.IemFeeInfo.NXYZLZPF.Trim('-');

                txtXBYZLZPF.Text = m_IemInfo.IemFeeInfo.XBYZLZPF.Trim('-');
                txtJCYYCXCLF.Text = m_IemInfo.IemFeeInfo.JCYYCXCLF.Trim('-');
                txtZLYYCXCLF.Text = m_IemInfo.IemFeeInfo.ZLYYCXCLF.Trim('-');
                txtSSYYCXCLF.Text = m_IemInfo.IemFeeInfo.SSYYCXCLF.Trim('-');
                txtOtherFee.Text = m_IemInfo.IemFeeInfo.OtherFee.Trim('-');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            m_IemInfo.IemFeeInfo.NOOFINPAT = this.noofpat;
            m_IemInfo.IemFeeInfo.Total = txtTotal.Text;
            m_IemInfo.IemFeeInfo.OwnFee = txtOwnFee.Text;
            m_IemInfo.IemFeeInfo.YBYLFY = txtYBYLFY.Text;
            
            m_IemInfo.IemFeeInfo.ZYBZLZF = txtZYBZLZF.Text;
            m_IemInfo.IemFeeInfo.ZYBZLZHZF = txtZYBZLZHZF.Text;

            m_IemInfo.IemFeeInfo.YBZLFY = txtYBZLFY.Text;
            m_IemInfo.IemFeeInfo.Care = txtCare.Text;

            m_IemInfo.IemFeeInfo.ZHQTFY = txtZHQTFY.Text;
            m_IemInfo.IemFeeInfo.BLZDF = txtBLZDF.Text;
            m_IemInfo.IemFeeInfo.SYSZDF = txtSYSZDF.Text;
            m_IemInfo.IemFeeInfo.YXXZDF = txtYXXZDF.Text;
            m_IemInfo.IemFeeInfo.LCZDF = txtLCZDF.Text;

            m_IemInfo.IemFeeInfo.FSSZLF = txtFSSZLF.Text;
            m_IemInfo.IemFeeInfo.LCWLZLF = txtLCWLZLF.Text;
            m_IemInfo.IemFeeInfo.SSZLF = txtSSZLF.Text;
            m_IemInfo.IemFeeInfo.MZF = txtMZF.Text;
            m_IemInfo.IemFeeInfo.SSF = txtSSF.Text;

            m_IemInfo.IemFeeInfo.KFF = txtKFF.Text;

            m_IemInfo.IemFeeInfo.ZYZDF = txtZYZDF.Text;
            m_IemInfo.IemFeeInfo.ZYZLF = txtZYZLF.Text;
            m_IemInfo.IemFeeInfo.ZYWZ = txtZYWZ.Text;
            m_IemInfo.IemFeeInfo.ZYGS = txtZYGS.Text;
            m_IemInfo.IemFeeInfo.ZCYJF = txtZCYJF.Text;
            m_IemInfo.IemFeeInfo.ZYTNZL = txtZYTNZL.Text;
            m_IemInfo.IemFeeInfo.ZYGCZL = txtZYGCZL.Text;
            m_IemInfo.IemFeeInfo.ZYTSZL = txtZYTSZL.Text;
            m_IemInfo.IemFeeInfo.ZYQT = txtZYQT.Text;
            m_IemInfo.IemFeeInfo.ZYTSTPJG = txtZYTSTPJG.Text;
            m_IemInfo.IemFeeInfo.BZSS = txtBZSS.Text;


            m_IemInfo.IemFeeInfo.XYF = txtXYF.Text;
            m_IemInfo.IemFeeInfo.KJYWF = txtKJYWF.Text;
            m_IemInfo.IemFeeInfo.CPMedical = txtCPMedical.Text;
            m_IemInfo.IemFeeInfo.YLJGZYZJF = txtYLJGZYZJF.Text;

            m_IemInfo.IemFeeInfo.CMedical = txtCMedical.Text;
            m_IemInfo.IemFeeInfo.BloodFee = txtBloodFee.Text;
            m_IemInfo.IemFeeInfo.XDBLZPF = txtXDBLZPF.Text;
            m_IemInfo.IemFeeInfo.QDBLZPF = txtQDBLZPF.Text;
            m_IemInfo.IemFeeInfo.NXYZLZPF = txtNXYZLZPF.Text;

            m_IemInfo.IemFeeInfo.XBYZLZPF = txtXBYZLZPF.Text;
            m_IemInfo.IemFeeInfo.JCYYCXCLF = txtJCYYCXCLF.Text;
            m_IemInfo.IemFeeInfo.ZLYYCXCLF = txtZLYYCXCLF.Text;
            m_IemInfo.IemFeeInfo.SSYYCXCLF = txtSSYYCXCLF.Text;
            m_IemInfo.IemFeeInfo.OtherFee = txtOtherFee.Text;

        }

        #endregion

        #region 费用相关信息界面基本事件

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_App == null || m_App.CurrentPatientInfo == null)
                    return;
                IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

                if (sqlHelper == null)
                {
                    m_App.CustomMessageBox.MessageShow("无法连接到HIS", CustomMessageBoxKind.ErrorOk);
                    return;
                }

                //xll 2013-04-22
                string sql = string.Format(@"select * from YD_IEMFEEINFOZY where NoofHis='{0}'", m_App.CurrentPatientInfo.NoOfHisFirstPage);

                DataTable dataTable = sqlHelper.ExecuteDataTable(sql, CommandType.Text);
                m_IemInfo.IemFeeInfo = DrectSoft.Common.DataTableToList<Iem_MainPage_Fee>.ConvertToModelOne(dataTable);
                FillUIInner();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接HIS失败");
            }
        }

       
        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCOthers_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                #region 注销 by xlb 2013-04-23重做了控件无需重绘导致的闪屏
                //foreach (Control control in this.Controls)
                //{
                //    if (control is LabelControl)
                //    {
                //        control.Visible = false;
                //        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                //    }
                //    if (control is TextEdit)
                //    {
                //        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                //            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                //    }
                //}

                //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
                //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
                //e.Graphics.DrawLine(Pens.Black, new Point(0, this.Height - 1), new Point(this.Width, this.Height - 1));
                #endregion
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// 保存后不关闭窗体
        /// <auth>Modify by xlb</auth>
        /// <date>2013-05-27</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                GetUI();
                IemMainPageManger.SaveIemFeeZy(m_IemInfo.IemFeeInfo);
                //((ShowUC)this.Parent).Close(true, m_IemInfo);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                ((ShowUC)this.Parent).Close(false, null); 
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        private void labelControl52_Click(object sender, EventArgs e)
        {

        }

        private void txtCare_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
