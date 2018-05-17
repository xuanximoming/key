using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace DrectSoft.Basic.Paint.NET
{
    /// <summary>
    /// 文本编辑器
    /// </summary>
    public class TextEditorForm : Form
    {
        private IShapeSource m_layer;
        private LabelShape m_shape;
        private TextBox m_box;

        public float FontSize { get; set; }

        public TextEditorForm(IShapeSource layer)
        {
            m_layer = layer;
            m_box = new TextBox();

            FontSize = 12;
            m_box.Multiline = true;
            m_box.ScrollBars = ScrollBars.Both;
            m_box.BorderStyle = BorderStyle.FixedSingle;
            m_box.Dock = DockStyle.Fill;

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Controls.Add(m_box);
        }

        public void SetShapeValue(LabelShape shape)
        {
            m_shape = shape;

            m_box.Font = new System.Drawing.Font("宋体", FontSize);
            m_box.Text = shape.Data.Text;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            m_shape.Data.Text = m_box.Text.Trim();
            m_shape.Font = m_box.Font;
            m_layer.Redraw();
            this.Visible = false;
            base.OnDeactivate(e);
        }

    }
}
