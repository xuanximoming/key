using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.RedactPatientInfo;

namespace DrectSoft.Emr.QcManager
{
    public partial class UCPatientInfo : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_NoOfFirstPage;
        private IEmrHost m_Host;

        public UCPatientInfo()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { }
            finally
            {
                this.Paint += new PaintEventHandler(UCPatientInfo_Paint);
            }
        }

        void UCPatientInfo_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (DesignMode)
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString("病人信息" + this.GetType().Namespace + "." + this.GetType().Name, this.Font, Brushes.Black, new RectangleF(0f, 0f, this.Width, this.Height), sf);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void Init(string noOfFirstPage, IEmrHost host)
        {
            try
            {
                m_NoOfFirstPage = noOfFirstPage;
                m_Host = host;
                DataAccess.App = m_Host;
                InitInner();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitInner()
        {
            try
            {
                DataTable dt = DataAccess.GetRedactPatientInfoFrm("14", m_NoOfFirstPage);

                if (dt.Rows.Count > 0)
                {
                    textEditName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    textEditPatientSN.Text = dt.Rows[0]["PatID"].ToString().Trim().Substring(4);//屏蔽掉前4个0
                    textEditGender.Text = dt.Rows[0]["Gender"].ToString().Trim();
                    textEditAge.Text = dt.Rows[0]["AgeStr"].ToString().Trim();
                    textEditBedCode.Text = dt.Rows[0]["OutBed"].ToString().Trim();
                    textEditDepartment.Text = dt.Rows[0]["OutHosDeptName"].ToString().Trim();
                    textEditMarriage.Text = dt.Rows[0]["Marriage"].ToString().Trim();
                    textEditJob.Text = dt.Rows[0]["JobName"].ToString().Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonInpatientInfo_Click(object sender, EventArgs e)
        {
            try
            {
                //调用病人信息管理维护界面
                if (m_NoOfFirstPage == null || m_NoOfFirstPage == "")
                    return;
                //to do 调用病患基本信息窗体
                //BasePatientInfo info = new BasePatientInfo(m_Host);
                //info.ShowCurrentPatInfo(m_NoOfFirstPage);
                XtraFormPatientInfo patientInfo = new XtraFormPatientInfo(m_Host, m_NoOfFirstPage);
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
