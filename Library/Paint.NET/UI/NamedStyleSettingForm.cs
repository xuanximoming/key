using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public partial class NamedStyleSettingForm
        : Form
    {

        #region Fields
        private CustomNamedHatchStyles _cnhs;
        private CustomNamedTextureStyles _cnts;
        #endregion

        #region Ctors

        public NamedStyleSettingForm()
        {
            InitializeComponent();
            foreach (string s in DefaultNamedHatchStyles.Instance.GetNames())
                cmbHatch.Items.Add(s);
            InitControlStates();
        }

        #endregion

        #region Members

        #region Public Properties

        public CustomNamedHatchStyles NamedHatchStyle
        {
            get { return _cnhs; }
        }

        public CustomNamedTextureStyles NamedTextureStyle
        {
            get { return _cnts; }
        }

        public void SetNamedStyle(CustomNamedHatchStyles hatchStyle,
            CustomNamedTextureStyles textureStyle)
        {
            _cnhs = hatchStyle;
            _cnts = textureStyle;
            lstNamedHatch.Items.Clear();
            txtName.Clear();
            if (_cnhs != null)
                foreach (string s in _cnhs.GetNames())
                    lstNamedHatch.Items.Add(s);
            if (_cnts != null)
                foreach (string s in _cnts.GetNames())
                    lstNamedHatch.Items.Add(s);
        }

        #endregion

        #region Event Handlers

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (NamedHatchStyle.GetHatchStyle(name) != null ||
                NamedTextureStyle.GetTexture(name) != null)
            {
                MessageBox.Show("不能加入这个命名的样式，因为样式或名称存在重复项。", "无法添加");
                return;
            }
            if (rbHatch.Checked)
            {
                HatchStyle style = DefaultNamedHatchStyles.Instance.GetHatchStyle((string)cmbHatch.SelectedItem) ?? default(HatchStyle);
                NamedHatchStyle.Add(style, name);
                lstNamedHatch.Items.Add(name);
            }
            else
            {
                if (imgTexture.Image == null)
                {
                    MessageBox.Show("未指定自定义样式", "无法添加");
                    return;
                }
                Picture pic = new Picture(imgTexture.Image);
                NamedTextureStyle.Add(pic, name);
                lstNamedHatch.Items.Add(name);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string name = (string)lstNamedHatch.SelectedItem;
            lstNamedHatch.Items.RemoveAt(lstNamedHatch.SelectedIndex);
            if (!NamedHatchStyle.Remove(name))
                NamedTextureStyle.Remove(name);
        }

        private void lstNamedHatch_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            btnDel.Enabled = lstNamedHatch.SelectedIndex != -1;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = txtName.Text.Trim().Length > 0;
        }

        private void cmbHatch_MeasureItem(
            object sender, MeasureItemEventArgs e)
        {
            string s = this.cmbHatch.Items[e.Index].ToString();
            SizeF size = e.Graphics.MeasureString(s, this.Font);
            e.ItemHeight = (int)size.Height;
            e.ItemWidth = (int)size.Width;
        }

        private void cmbHatch_DrawItem(
            object sender, DrawItemEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            DrawHatchItem(e, cb.Items, cmbHatch.Font,
                DefaultNamedHatchStyles.Instance, null);
        }

        private void lstNamedHatch_MeasureItem(
            object sender, MeasureItemEventArgs e)
        {
            string text1 = this.lstNamedHatch.Items[e.Index].ToString();
            SizeF ef1 = e.Graphics.MeasureString(text1, this.Font);
            e.ItemHeight = (int)ef1.Height;
            e.ItemWidth = (int)ef1.Width;
        }

        private void lstNamedHatch_DrawItem(
            object sender, DrawItemEventArgs e)
        {
            ListBox lb = sender as ListBox;
            DrawHatchItem(e, lb.Items, lb.Font,
                this.NamedHatchStyle, this.NamedTextureStyle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string filename = UICommon.ShowOpenPictureDialog();
            byte[] bytes = null;
            FileStream fs = null;
            if (string.IsNullOrEmpty(filename))
                return;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                int length = (int)fs.Length;
                bytes = new byte[length];
                fs.Read(bytes, 0, length);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "IO Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            MemoryStream ms = new MemoryStream(bytes);
            try
            {
                Image image = Image.FromStream(ms);
                imgTexture.Image = image;
            }
            catch (ArgumentException)
            {
                ms.Dispose();
                MessageBox.Show("指定的文件不是图片文件或文件已经损坏", "无法打开图片");
            }
        }

        private void rbTexture_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbHatch.Enabled = rbHatch.Checked;
            this.btnBrowse.Enabled = rbTexture.Checked;
        }

        private void rbHatch_CheckedChanged(object sender, EventArgs e)
        {
            this.cmbHatch.Enabled = rbHatch.Checked;
            this.btnBrowse.Enabled = rbTexture.Checked;
        }

        #endregion

        #region Private

        private static void DrawHatchItem(
            DrawItemEventArgs e, IList items, Font font,
            NamedHatchStyles nhs, NamedTextureStyles nts)
        {
            e.DrawBackground();
            Rectangle rectangle1 = e.Bounds;
            if (e.Index == -1)
            {
                return;
            }
            string text1 = (string)items[e.Index];
            Rectangle rectangle2 = rectangle1;
            rectangle2.Width = rectangle2.Left + 0x19;
            rectangle1.X = rectangle2.Right;
            using (Brush brush1 = UICommon.GetBrush(text1, e.ForeColor, e.BackColor, nhs, nts))
            {
                e.Graphics.FillRectangle(brush1, rectangle2);
            }
            StringFormat format1 = new StringFormat();
            format1.Alignment = StringAlignment.Near;
            using (SolidBrush brush2 = new SolidBrush(Color.White))
            {
                if ((e.State & DrawItemState.Focus) == DrawItemState.None)
                {
                    brush2.Color = SystemColors.Window;
                    e.Graphics.FillRectangle(brush2, rectangle1);
                    brush2.Color = SystemColors.WindowText;
                    e.Graphics.DrawString(text1, font, brush2, (RectangleF)rectangle1, format1);
                }
                else
                {
                    brush2.Color = SystemColors.Highlight;
                    e.Graphics.FillRectangle(brush2, rectangle1);
                    brush2.Color = SystemColors.HighlightText;
                    e.Graphics.DrawString(text1, font, brush2, (RectangleF)rectangle1, format1);
                }
            }
            e.DrawFocusRectangle();
        }

        private void InitControlStates()
        {
            btnAdd.Enabled = false;
            btnDel.Enabled = false;
            this.cmbHatch.Enabled = rbHatch.Checked;
            this.btnBrowse.Enabled = rbTexture.Checked;
        }

        #endregion

        #endregion

    }
}