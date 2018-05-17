/***********************************************************************************************************************别文进插件*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Common.Ctrs.OTHER
{
    /// <summary>
    /// 功能描述：封装Dev的ComboBoxEdit
    ///          目标解决：1、鼠标右击弹出菜单；2、回车键跳下面控件
    /// 创 建 者：bwj
    /// 创建日期：20121012
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
    [Description("功能描述：封装Dev的ComboBoxEdit\r\n-----------------------\r\nbwj  20121016")]
    public partial class DevComboBoxEdit : ComboBoxEdit
    {
        public DevComboBoxEdit()
        {

            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool _IsEnterKeyToNextControl = false;
        private bool _IsEnterChangeBgColor = false;
        private Color _oldBgColor;

        /// <summary>
        /// 回车键是否跳到下个控件TabIndex
        /// </summary>
        [Description("值为true时,按Enter(或Esc)键将跳向上(或下)一个TabIndex的控件\r\nbwj 20121015")]
        [Browsable(true)]
        public bool IsEnterKeyToNextControl
        {
            get
            {
                return _IsEnterKeyToNextControl;
            }
            set
            {
                if (value)
                {
                    this.KeyUp += new KeyEventHandler(txt_KeyUp);
                }
                else
                {
                    this.KeyUp -= new KeyEventHandler(txt_KeyUp);
                }

                _IsEnterKeyToNextControl = value;
            }

        }
        /// <summary>
        /// 是否获取焦点时改变背景色
        /// </summary>
        [Description("值为true时,获取焦点时突出显示\r\nbwj 20121015")]
        [Browsable(true)]
        public bool IsEnterChangeBgColor
        {
            get
            {
                return _IsEnterChangeBgColor;
            }
            set
            {
                if (value)
                {
                    this.Enter += new EventHandler(txt_Enter);
                    this.Leave += new EventHandler(txt_Leave);
                }
                else
                {
                    this.Enter -= new EventHandler(txt_Enter);
                    this.Leave -= new EventHandler(txt_Leave);
                }

                _IsEnterChangeBgColor = value;
            }
        }
        public DevComboBoxEdit(IContainer container)
        {

            try
            {
                container.Add(this);

                InitializeComponent();

                //解决右击弹出快捷菜单
                this.Properties.ContextMenuStrip = new ContextMenuStrip();
                //为进入改变背景色做准备
                this._oldBgColor = this.BackColor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txt_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if ((int)e.KeyCode == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    SendKeys.Send("+({Tab})");
                    SendKeys.Flush();
                }
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString(), "志扬软件");
                return;
            }
        }

        private void DevTextEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_Enter(object sender, EventArgs e)
        {
            try
            {
                _oldBgColor = this.BackColor;
                this.BackColor = Color.FromArgb(242, 239, 156);


            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString(), "志扬软件");
                return;
            }
        }
        private void txt_Leave(object sender, EventArgs e)
        {
            try
            {

                this.BackColor = _oldBgColor;


            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString(), "志扬软件");
                return;
            }
        }
    }
}
