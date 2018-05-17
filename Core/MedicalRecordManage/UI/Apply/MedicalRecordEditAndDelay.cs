using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MedicalRecordManage.Object;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;


namespace MedicalRecordManage.UI
{
    public partial class MedicalRecordEditAndDelay : DevBaseForm//,IEMREditor
    {
        //IEmrHost m_app;

        /// <summary>
        /// 最大延迟时间
        /// </summary>
        public int m_dealyMaxTime = 0;
        //最大延迟次数
        public int m_delaymaxTimes = 0;
        //最大时间
        public int m_readMaxTime = 0;

        public string m_sApplyId;
        public int m_iType;//
        public string m_sApplyDocId;
        public int m_iCommandFlag = 0;
        public string m_sNoOfInpat;
        /// <summary>
        /// add by jy.zhu 保存当前操作类型，1提交，0草稿
        /// </summary>
        public int m_iTabIndex = 0;

        #region    注销 by xlb 2013-03-28
        /*
        //是编辑页面还是延期页面
        public MedicalRecordEditAndDelay(IEmrHost app, object obj, int type)
        {
            m_app = app;
            InitializeComponent();
            CApplyObject tempobj = (CApplyObject)obj;
            m_sApplyId = tempobj.m_sApply;
            m_sApplyDocId = tempobj.m_sApplyDocId;
            this.txtDept.Text = tempobj.m_sDepartmentName;
            this.txtName.Text = tempobj.m_sName;
            this.txtNumber.Text = tempobj.m_sNoOfInpat;
            this.txtTimes.Text = tempobj.m_iApplyTimes.ToString();
            this.memoReason.Text = tempobj.m_sApplyContent;
            m_dealyMaxTime = ComponentCommand.GetDealyMaxTime();
            //最大延迟次数
            m_delaymaxTimes = ComponentCommand.GetDealyTimes();
            //最大时间
            m_readMaxTime = ComponentCommand.GetReadTime();
            m_iType = type;
            if (type == 1)
            {
                this.labelMessage.Text = "延期原因";
                this.labelSubMessage.Text = "延期时间";
                this.Text = "申请延期";
                labelDay.Text = "天(*)最大不超过：" + m_dealyMaxTime + "天";
            }
            else if (type == 0)
            {
                this.labelMessage.Text = "借阅目的";
                this.labelSubMessage.Text = "借阅期限";
                this.Text = "借阅编辑";
                labelDay.Text = "天(*)最大不超过：" + m_readMaxTime + "天";
            }
            else if (type == 2)
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
                this.txtTimes.Enabled = false;
                this.memoReason.Enabled = false;
                this.labelMessage.Text = "审核信息";
                this.labelSubMessage.Text = "审核日期";
                labelDay.Visible = false;
                this.txtTimes.Text = tempobj.m_sApproveDate;
                this.memoReason.Text = tempobj.m_sApproveContent;
                this.Text = "查看原因";
            }
        }
        */
        #endregion

        /// <summary>
        /// 是编辑页面还是延期页面
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public MedicalRecordEditAndDelay(object obj,int type)
        {
            try
            {
                InitializeComponent();
                CApplyObject tempobj = (CApplyObject)obj;
                m_sNoOfInpat = tempobj.m_sNoOfInpat;
                m_sApplyId = tempobj.m_sApply;
                m_sApplyDocId = tempobj.m_sApplyDocId;
                this.txtDept.Text = tempobj.m_sDepartmentName;
                this.txtName.Text = tempobj.m_sName;
                //this.txtNumber.Text = tempobj.m_sNoOfInpat;主键
                this.txtNumber.Text = tempobj.m_sPatid;//add by zjy 2013-6-16 修改成住院号
                m_dealyMaxTime = ComponentCommand.GetDealyMaxTime();
                //最大延迟次数
                m_delaymaxTimes = ComponentCommand.GetDealyTimes();
                //最大时间
                m_readMaxTime = ComponentCommand.GetReadTime();
                m_iType = type;
                if (type == 1)
                {
                    this.labelMessage.Text = "延期原因：";
                    this.labelSubMessage.Text = "延期天数：";
                    this.Text = "申请延期";
                    labelDay.Text = "天（*）最大不超过：" + m_dealyMaxTime + "天；" + "延期次数不超过：" + m_delaymaxTimes + "次";
                    this.txtTimes.Properties.MaxValue = m_dealyMaxTime;
                }
                else if (type == 0)
                {
                    this.labelMessage.Text = "借阅目的：";
                    this.labelSubMessage.Text = "借阅期限：";
                    this.Text = "借阅编辑";
                    labelDay.Text = "天（*）最大不超过：" + m_readMaxTime + "天";
                    this.txtTimes.Text = tempobj.m_iApplyTimes.ToString();
                    this.txtTimes.Properties.MaxValue = m_readMaxTime;
                    this.memoReason.Text = tempobj.m_sApplyContent;
                }
                else if (type == 2)
                {
                    this.btnSave.Enabled = false;
                    this.btnSubmit.Enabled = false;
                    this.txtTimes.Visible = false;
                    this.txtApproveDate.Visible = true;
                    this.txtApproveDate.Enabled = false;
                    this.memoReason.Enabled = false;
                    this.labelMessage.Text = "审核信息：";
                    this.labelSubMessage.Text = "审核日期：";
                    labelDay.Visible = false;
                    this.txtApproveDate.Text = tempobj.m_sApproveDate;
                    this.memoReason.Text = tempobj.m_sApproveContent;
                    this.Text = "查看原因";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Commit(1, "提交");
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Commit(0,"保存为草稿");
          
        }

        private void Commit(int type,string info)
        {
            try
            {
                string message = "";
                if (!CheckApplyTimes(ref message))
                {
                    MessageBox.Show(message);
                    return;
                }
                    CApplyObject applyObject = new CApplyObject();
                    applyObject.m_sApply = this.m_sApplyId;
                    applyObject.m_sNoOfInpat = this.m_sNoOfInpat;
                    applyObject.m_sApplyDocId = this.m_sApplyDocId;
                    applyObject.m_sApplyContent = this.memoReason.Text.Trim();
                    applyObject.m_iStatus = type;
                    applyObject.m_sYanqiflag = this.m_sApplyId;
                    applyObject.m_iApplyTimes = int.Parse(this.txtTimes.Text.Trim());
                    if (type == 1)
                    {
                        this.m_iTabIndex = 2;
                    }
                    if (!CheckIsExistDelayApply(applyObject))
                    {
                        if (CheckDelayTimes(m_iType, applyObject))
                        {
                            Save(applyObject);
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(info + "操作成功", "信息提示");
                            m_iCommandFlag = 1;
                            this.Close();
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("延期次数已超过限制，不能再延期", "信息提示");
                            this.btnCancel.Focus();
                        }
                    }
                    else
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("借阅申请已存在延期记录", "信息提示");
                        this.btnCancel.Focus();
                    }
                //}
                //else
                //{
                //    string msg = "";
                //    if (m_iType == 1)
                //    {
                //        msg = "请输入不超过" + this.m_dealyMaxTime + "的天数";
                //    }
                //    else
                //    {
                //        msg = "请输入不超过" + this.m_readMaxTime + "的天数";
                //    }
                //    Common.Ctrs.DLG.MessageBox.Show(msg, "信息提示");
                //    this.txtTimes.Focus();
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 校验时间方法
        /// </summary>
        /// <returns></returns>
        private bool CheckApplyTimes(ref string message)
        {
            try
            {
                //int i = int.Parse(this.txtTimes.Text.Trim());
                decimal delaytime = this.txtTimes.Value;
                int times = 0;
                //0：借阅
                if (m_iType == 0)
                {
                    //if (delaytime <= this.m_readMaxTime && delaytime > 0)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    if (delaytime <= 0)
                    {
                        message = "请输入借阅天数";
                        txtTimes.Focus();
                        return false;
                    }
                    else if (delaytime > 0 && delaytime <= this.m_readMaxTime)
                    {
                        if (!int.TryParse(delaytime.ToString(), out times))
                        {
                            message = "请输入整数";
                            txtTimes.Focus();
                            return false;
                        }
                        return true;
                    }
                    else if (delaytime > m_readMaxTime)
                    {
                        message = "借阅天数不能大于" + m_readMaxTime + "天";
                        txtTimes.Focus();
                        return false;
                    }
                }
                //1:申请延期时进行相应校验延期时间不能超过最大延迟时间
                else if (m_iType == 1)
                {
                    if (delaytime <= 0)
                    {
                        message = "请输入需要延期的天数";
                        txtTimes.Focus();
                        return false;
                    }
                    else if (delaytime > 0 && delaytime <= this.m_dealyMaxTime)
                    {
                        if (!int.TryParse(delaytime.ToString(), out times))
                        {
                            message = "请输入整数";
                            txtTimes.Focus();
                            return false;
                        }
                        return true;
                    }
                    else if (delaytime > m_dealyMaxTime)
                    {
                        message = "延期天数不能大于" + m_dealyMaxTime + "天";
                        txtTimes.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //验证延期次数是否合法
        private bool CheckDelayTimes(int type, CApplyObject applyObject)
        {
            try
            {
                if (type > 0)
                {
                    return (CApplyObject.IsCanDelay(applyObject, m_delaymaxTimes) > 0);
                }
                else //申请新的不需要验证延期次数判断
                {
                    return true;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        //
        private bool CheckIsExistDelayApply(CApplyObject applyObject)
        {
            try
            {
                if (this.m_iType == 1)
                {
                    return (CApplyObject.IsExistApply(applyObject) > 0);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void Save(CApplyObject applyObject)
        {
            //Status = 0;m_sApplyId
            try
            {
                if (this.m_iType == 0)//编辑，更新记录
                {
                    CApplyObject.Edit(applyObject);
                }
                else //延期新增记录
                {
                    //生成新的申请记录
                    CApplyObject.Delay(applyObject);
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MedicalRecordEditAndDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Enter)
            //    this.SelectNextControl(this.ActiveControl, true, true, true, false);
        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void txtDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void memoReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            //try
            //{
            //    if ((int)e.KeyChar == 13)
            //    {
            //        SendKeys.Send("{Tab}");
            //        SendKeys.Flush();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            //}
        }
        //
        private void txtTimes_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        //
        private void MedicalRecordEditAndDelay_Load(object sender, EventArgs e)
        {
            DrectSoft.Common.DS_Common.CancelMenu(this.panelHead, contextMenuStrip1);
            DrectSoft.Common.DS_Common.CancelMenu(this.panelBody, contextMenuStrip1);
        }

        private void txtApproveDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, false);
            }
        }

    }
}