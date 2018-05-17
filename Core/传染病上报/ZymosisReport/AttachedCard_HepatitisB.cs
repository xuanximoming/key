using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class AttachedCard_HepatitisB : DevExpress.XtraEditors.XtraUserControl
    {
        public AttachedCard_HepatitisB()
        {
            try
            {
                InitializeComponent();
                SetDefaultValue();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-13</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttachedCard_HepatitisB_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 设置小标题
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <param name="title"></param>
        public void SetCardTitle(string title)
        {
            try
            {
                this.lbl_attachedCardTitle.Text = title;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置分割线长度
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <param name="width"></param>
        public void SetLineShapeWidth(int width)
        {
            try
            {
                this.lineShape1.Width = width;
                this.lbl_attachedCardTitle.Width = width;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        public void SetFocusControl()
        {
            try
            {
                this.txt_year.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        public void SetDefaultValue()
        {
            try
            {
                che_notTestHBcIgM.Checked = true;
                che_notTestHepar.Checked = true;
                che_notTestHBsAg.Checked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 填充UI值
        /// Modify by xlb 2013-05-30
        /// 解决保存选项和当前不一致问题,显示错误的选项均用flag1来判断
        /// </summary>
        /// <param name="drow">列名与数据库字段一致</param>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-13</date>
        public void FillUI(DataRow drow)
        {
            try
            {
                if (null == drow)
                {
                    return;
                }
                ///HBsAg阳性时间
                int flag1 = (drow.Table.Columns.Contains("hbsagtimetype") && !string.IsNullOrEmpty(drow["hbsagtimetype"].ToString())) ? int.Parse(drow["hbsagtimetype"].ToString()) : -1;
                if (flag1 == 0)
                {
                    che_big.Checked = true;
                }
                else if (flag1 == 1)
                {
                    che_small.Checked = true;
                }
                else if (flag1 == 2)
                {
                    che_notDone.Checked = true;
                }
                ///首次出现乙肝症状和体征的时间
                int flag2 = (drow.Table.Columns.Contains("issymptomatic") && !string.IsNullOrEmpty(drow["issymptomatic"].ToString())) ? int.Parse(drow["issymptomatic"].ToString()) : -1;
                if (flag2 == 1)
                {
                    che_asymptomatic.Checked = true;
                }
                else
                {
                    che_asymptomatic.Checked = false;
                    this.txt_year.Text = null == drow["firstattackyear"] ? string.Empty : drow["firstattackyear"].ToString();
                    this.txt_month.Text = null == drow["firstattackmonth"] ? string.Empty : drow["firstattackmonth"].ToString();
                }
                ///本次 ALT
                this.txt_currentALT.Text = null == drow["currentalt"] ? string.Empty : drow["currentalt"].ToString();
                ///抗-HBcIgM 1:1000检测结果
                int flag3 = (drow.Table.Columns.Contains("hbcigmresult") && !string.IsNullOrEmpty(drow["hbcigmresult"].ToString())) ? int.Parse(drow["hbcigmresult"].ToString()) : -1;
                if (flag3 == 0)//原先写的是flag1
                {
                    che_masculine.Checked = true;
                }
                else if (flag3 == 1)//原先写的是flag1
                {
                    che_negative.Checked = true;
                }
                else if (flag3 == 2)//原先写的是flag1
                {
                    che_notTestHBcIgM.Checked = true;
                }
                ///肝穿检测结果
                int flag4 = (drow.Table.Columns.Contains("heparresult") && !string.IsNullOrEmpty(drow["heparresult"].ToString())) ? int.Parse(drow["heparresult"].ToString()) : -1;
                if (flag4 == 0)
                {
                    che_acute.Checked = true;
                }
                else if (flag4 == 1)
                {
                    che_chronic.Checked = true;
                }
                else if (flag4 == 2)
                {
                    che_notTestHepar.Checked = true;
                }
                ///恢复期血清 HBsAg 阴转,抗 HBs 阳转
                int flag5 = (drow.Table.Columns.Contains("hbsagandhbschange") && !string.IsNullOrEmpty(drow["hbsagandhbschange"].ToString())) ? int.Parse(drow["hbsagandhbschange"].ToString()) : -1;
                if (flag5 == 1)//是应对应1否对应0 Modify by xlb 2013-05-30//原先写的是flag1
                {
                    che_yes.Checked = true;
                }
                else if (flag5 == 0)//原先写的是flag1
                {
                    che_no.Checked = true;
                }
                else if (flag5 == 2)//原先写的是flag1
                {
                    che_notTestHBsAg.Checked = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空页面值
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        public void ClearPage()
        {
            try
            {
                this.che_big.Checked = false;
                this.che_small.Checked = false;
                this.che_notDone.Checked = false;
                this.txt_year.Text = string.Empty;
                this.txt_month.Text = string.Empty;
                this.che_asymptomatic.Checked = false;
                this.txt_currentALT.Text = string.Empty;
                this.che_masculine.Checked = false;
                this.che_negative.Checked = false;
                this.che_notTestHBcIgM.Checked = false;
                this.che_acute.Checked = false;
                this.che_chronic.Checked = false;
                this.che_notTestHepar.Checked = false;
                this.che_yes.Checked = false;
                this.che_no.Checked = false;
                this.che_notTestHBsAg.Checked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 画面检查
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <returns></returns>
        public string CheckItem()
        {
            try
            {
                if (!che_big.Checked && !che_small.Checked && !che_notDone.Checked)
                {
                    che_big.Focus();
                    return "请勾选HBsAg阳性时间";
                }
                else if ((string.IsNullOrEmpty(txt_year.Text.Trim()) || string.IsNullOrEmpty(txt_month.Text.Trim())) && !che_asymptomatic.Checked)
                {
                    if (string.IsNullOrEmpty(txt_year.Text.Trim()) && !string.IsNullOrEmpty(txt_month.Text.Trim()))
                    {
                        txt_year.Focus();
                        return "请填写首次出现乙肝症状和体征的时间的年";
                    }
                    else if (!string.IsNullOrEmpty(txt_year.Text.Trim()) && string.IsNullOrEmpty(txt_month.Text.Trim()))
                    {
                        txt_month.Focus();
                        return "请填写首次出现乙肝症状和体征的时间的月";
                    }
                    else
                    {
                        txt_year.Focus();
                        return "请填写首次出现乙肝症状和体征的时间";
                    }
                }
                else if (string.IsNullOrEmpty(txt_currentALT.Text.Trim()))
                {
                    txt_currentALT.Focus();
                    return "本次ALT不能为空";
                }
                this.txt_year.Focus();

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// 获取病历参数
        /// </summary>
        /// 注：只插入或更新除病历内容外的项(病历内容单独更新)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="model"></param>
        /// <param name="state">操作状态</param>
        /// <returns></returns>
        public List<OracleParameter> GetHepatitisBParams()
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();

                //1.HBsAg阳性时间（0：大于6个月；1：6个月内由阴性转为阳性；2：未检测或结果不详）
                OracleParameter param1 = new OracleParameter("hbsagtimetype", OracleType.Int32);
                param1.Value = che_big.Checked ? 0 : (che_small.Checked ? 1 : (che_notDone.Checked ? 2 : -1));
                parameters.Add(param1);
                //2.首次出现乙肝症状和体征时间
                ///2.1 无症状
                OracleParameter param2 = new OracleParameter("issymptomatic", OracleType.Int32);
                param2.Value = che_asymptomatic.Checked ? 1 : 0;
                parameters.Add(param2);
                ///2.2 年
                OracleParameter param3 = new OracleParameter("firstattackyear", OracleType.VarChar);
                param3.Value = txt_year.Text;
                parameters.Add(param3);
                ///2.3 月
                OracleParameter param4 = new OracleParameter("firstattackmonth", OracleType.VarChar);
                param4.Value = txt_month.Text;
                parameters.Add(param4);
                //3.本次ALT
                OracleParameter param5 = new OracleParameter("currentalt", OracleType.VarChar);
                param5.Value = txt_currentALT.Text;
                parameters.Add(param5);
                //4.抗-HBc IgM 1:1000检测结果（0：阳性；1：阴性；2：未测）
                OracleParameter param6 = new OracleParameter("hbcigmresult", OracleType.Int32);
                param6.Value = che_masculine.Checked ? 0 : (che_negative.Checked ? 1 : (che_notTestHBcIgM.Checked ? 2 : -1));
                parameters.Add(param6);
                //5.肝穿检测结果（0：急性病变；1：慢性病变；2：未测）
                OracleParameter param7 = new OracleParameter("heparresult", OracleType.Int32);
                param7.Value = che_acute.Checked ? 0 : (che_chronic.Checked ? 1 : (che_notTestHepar.Checked ? 2 : -1));
                parameters.Add(param7);
                //6.恢复期血清HBsAg阴转，抗HBs阳转。（0：否；1：是；2：未测）
                OracleParameter param8 = new OracleParameter("hbsagandhbschange", OracleType.Int32);
                param8.Value = che_yes.Checked ? 1 : (che_no.Checked ? 0 : (che_notTestHBsAg.Checked ? 2 : -1));
                parameters.Add(param8);

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 无症状 勾选事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void che_asymptomatic_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (che_asymptomatic.Checked)
                {
                    txt_year.Text = string.Empty;
                    txt_month.Text = string.Empty;
                    txt_year.BackColor = Color.WhiteSmoke;
                    txt_month.BackColor = Color.WhiteSmoke;
                    txt_year.Enabled = false;
                    txt_month.Enabled = false;
                }
                else
                {
                    txt_year.Enabled = true;
                    txt_month.Enabled = true;
                    txt_year.BackColor = Color.White;
                    txt_month.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 文本框获取焦点颜色变化控制 add by cyq 2013-03-12
        private void Dev_Enter(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, true);
        }
        private void Dev_Leave(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, false);
        }
        #endregion




    }
}
