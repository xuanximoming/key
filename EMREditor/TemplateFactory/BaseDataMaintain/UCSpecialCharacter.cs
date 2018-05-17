using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    public partial class UCSpecialCharacter : DevExpress.XtraEditors.XtraUserControl
    {
        SqlHelp m_SqlHelp;
        EditState m_state = EditState.None;
        public IEmrHost Host
        {
            get;
            set;
        }

        public UCSpecialCharacter()
        {
            InitializeComponent();
        }

        //根据操作状态更新文本框|按钮状态
        private void FreshView()
        {
            switch (m_state)
            {
                case EditState.None:
                    this.textEditSymbol.Enabled = false;
                    this.textEditSymbol.Text = "";
                    this.simpleButtonAdd.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonSave.Enabled = false;
                    this.simpleButtonCancel.Enabled = false;
                    break;
                case EditState.Add:
                    this.textEditSymbol.Enabled = true;
                    this.textEditSymbol.Text = "";
                    this.simpleButtonAdd.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonSave.Enabled = true;
                    this.simpleButtonCancel.Enabled = true;
                    break;
                case EditState.Edit:
                    this.textEditSymbol.Enabled = true;
                    this.simpleButtonAdd.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonSave.Enabled = true;
                    this.simpleButtonCancel.Enabled = true;
                    break;
                case EditState.View:
                    this.textEditSymbol.Enabled = false;
                    this.simpleButtonAdd.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonSave.Enabled = false;
                    this.simpleButtonCancel.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSpecialCharacter_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    m_SqlHelp = new SqlHelp(Host);
                    RefreshSymbol();
                }
                this.FreshView();
                //初始化焦点
                this.simpleButtonAdd.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        List<string> m_ListSymbol = new List<string>();
        private void RefreshSymbol()
        {
            DataTable dt = m_SqlHelp.GetSymbol();
            Font font = new System.Drawing.Font("宋体", 13, FontStyle.Regular);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            imageListBoxControl1.Items.Clear();

            ImageList list = new ImageList();
            list.ImageSize = new Size(90, 90);
            foreach (DataRow dr in dt.Rows)
            {
                string symbol = dr[Symbol.s_Symbol].ToString();
                Bitmap bmp = new Bitmap(list.ImageSize.Width, list.ImageSize.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawString(symbol, font, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                g.Dispose();
                list.Images.Add(symbol, bmp);
            }
            imageListBoxControl1.ImageList = list;

            int i = 0;
            m_ListSymbol.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string symbol = dr[Symbol.s_Symbol].ToString();
                imageListBoxControl1.Items.Add(symbol, i);
                m_ListSymbol.Add(symbol);
                i++;
            }
            imageListBoxControl1.MultiColumn = true;
            imageListBoxControl1.HotTrackItems = true;
            imageListBoxControl1.HotTrackSelectMode = HotTrackSelectMode.SelectItemOnClick;
            imageListBoxControl1.HighlightedItemStyle = HighlightStyle.Skinned;
        }

        private void imageListBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItem lbi = imageListBoxControl1.SelectedItem as ListBoxItem;
            if (lbi != null)
            {
                textEditSymbol.Text = lbi.Value.ToString();
                textEditSymbol.Focus();
            }
            m_state = EditState.View;
            this.FreshView();
        }

        private Symbol GetFormSymbol()
        {
            Symbol symbol = new Symbol();
            symbol.NewSymbol = textEditSymbol.Text;
            ListBoxItem lbi = imageListBoxControl1.SelectedItem as ListBoxItem;
            symbol.OldSymbol = lbi.Value.ToString();
            return symbol;
        }

        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.Add;
                this.FreshView();
                this.simpleButtonAdd.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (imageListBoxControl1.SelectedItems.Count <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一个特殊字符");
                    return;
                }
                ListBoxItem lbi = imageListBoxControl1.SelectedItem as ListBoxItem;
                if (null == lbi)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一个特殊字符");
                    return;
                }
                this.textEditSymbol.Text = null == lbi.Value ? "" : lbi.Value.ToString();

                m_state = EditState.Edit;
                this.FreshView();
                this.simpleButtonAdd.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (imageListBoxControl1.SelectedItems.Count <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一个特殊字符");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除特殊字符 " + (imageListBoxControl1.SelectedItem as ListBoxItem).Value + " 吗？", "删除特殊字符", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                Symbol symbol = GetFormSymbol();
                m_SqlHelp.DeleteSymbol(symbol);
                RefreshSymbol();
                m_state = EditState.None;
                this.FreshView();
                Host.CustomMessageBox.MessageShow("删除成功", Core.CustomMessageBoxKind.InformationOk);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_state == EditState.Add)
                {
                    if (string.IsNullOrEmpty(textEditSymbol.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("特殊字符不能为空");
                        textEditSymbol.Focus();
                        return;
                    }
                    Symbol symbol = GetFormSymbol();
                    if (m_ListSymbol.Contains(symbol.NewSymbol))
                    {
                        //已经存在
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该特殊字符已存在，不能重复添加。");
                        textEditSymbol.Focus();
                        return;
                    }

                    m_SqlHelp.InsertSymbol(symbol);
                    RefreshSymbol();
                }
                else if (m_state == EditState.Edit)
                {
                    if (string.IsNullOrEmpty(textEditSymbol.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("特殊字符不能为空");
                        textEditSymbol.Focus();
                        return;
                    }
                    Symbol symbol = GetFormSymbol();
                    if (m_ListSymbol.Contains(symbol.NewSymbol))
                    {
                        //已经存在
                    }
                    else
                    {
                        m_SqlHelp.UpdateSymbol(symbol);
                    }
                    RefreshSymbol();
                }
                m_state = EditState.None;
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.None;
                this.FreshView();
                this.simpleButtonAdd.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
