using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class HeaderFooterSetting : DevBaseForm
    {
        ZYTextDocument m_Doc;
        SQLUtil m_SqlUtil;
        public HeaderFooterSetting(ZYTextDocument doc, SQLUtil sqlUtil)
        {
            InitializeComponent();
            m_Doc = doc;
            m_SqlUtil = sqlUtil;
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// Modify by xlb 2013-07-08 设置完成关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要使用此设置么？该操作将影响全局", "", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                ChangeHeaderFooterHeight();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
            finally
            {
                this.Close();
            }
        }

        private void ChangeHeaderFooterHeight()
        {
            m_Doc.DocumentHeaderHeight = int.Parse(spinEditHeaderHeight.Text);
            m_Doc.DocumentFooterHeight = int.Parse(spinEditFooterHeight.Text);
            m_SqlUtil.SetHeaderFooterHeight(m_Doc.DocumentHeaderHeight, m_Doc.DocumentFooterHeight);
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderFooterSetting_Load(object sender, EventArgs e)
        {
            try
            {
                spinEditHeaderHeight.Text = m_Doc.DocumentHeaderHeight.ToString();
                spinEditFooterHeight.Text = m_Doc.DocumentFooterHeight.ToString();
                this.spinEditHeaderHeight.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}