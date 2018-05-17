using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{

    public class NewPictureStrip
        : ToolStrip
    {

        #region Fields
        private ToolStripButton newButton;
        private ToolStripButton sizeButton;
        private ToolStripButton namedStyleButton;
        private IShapeSurface _surface;
        private ToolStripComboBox fontsizeComboBox;
        private ToolStripLabel lableInfo;
        #endregion

        #region Ctors

        public NewPictureStrip()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.newButton = new ToolStripButton();
            this.sizeButton = new ToolStripButton();
            this.namedStyleButton = new ToolStripButton();
            this.fontsizeComboBox = new ToolStripComboBox();
            this.lableInfo = new ToolStripLabel();
            base.SuspendLayout();

            this.newButton.Name = "newButton";
            this.newButton.Text = "更换图片"; //Modified by wwj 2013-04-19 将“加载背景图片” 改为“更换图片” 
            this.newButton.ToolTipText = "更换图片"; //Modified by wwj 2013-04-19 将“加载背景图片” 改为“更换图片” 
            this.newButton.Click += new EventHandler(newButton_Click);

            this.sizeButton.Name = "fillColorButton";
            this.sizeButton.Text = "设置尺寸";
            this.sizeButton.ToolTipText = "设置尺寸";
            this.sizeButton.Click += new EventHandler(sizeButton_Click);

            this.namedStyleButton.Name = "namedStyleButton";
            this.namedStyleButton.Text = "命名填充样式";
            this.namedStyleButton.ToolTipText = "命名填充样式";
            this.namedStyleButton.Click += new EventHandler(namedStyleButton_Click);

            this.lableInfo.Name = "lableinfo";
            this.lableInfo.Text = "字体大小";



            this.fontsizeComboBox.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11"});
            this.fontsizeComboBox.Name = "toolStripComboBox1";
            this.fontsizeComboBox.Size = new System.Drawing.Size(121, 25);
            this.fontsizeComboBox.TextChanged += new EventHandler(toolStripComboBox1_TextChanged);
            this.fontsizeComboBox.Text = "12";

            this.Dock = DockStyle.None;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.Items.Add(this.newButton);
            this.Items.Add(this.sizeButton);
            this.Items.Add(this.namedStyleButton);
            this.Items.Add(this.lableInfo);
            this.Items.Add(fontsizeComboBox);

            base.ResumeLayout(false);
        }

        public event EventHandler<SizeEventArgs> FontSizeChanged;

        protected void OnChanged(float size)
        {
            if (FontSizeChanged != null)
            {
                FontSizeChanged(this, new SizeEventArgs(size));
            }

        }

        void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fontsizeComboBox.Text))
            {
                OnChanged((float)Convert.ToDecimal(fontsizeComboBox.Text));
            }

        }

        #endregion

        #region Members

        #region Public

        public IShapeSurface Surface
        {
            get { return _surface; }
            set
            {
                _surface = value;
                if (!DesignMode)
                    this.Enabled = Surface != null && !Surface.ReadOnly;
            }
        }

        public float FontSize
        {
            get
            {
                if (string.IsNullOrEmpty(fontsizeComboBox.Text))
                    return 12;
                return (float)Convert.ToDecimal(fontsizeComboBox.Text);

            }
        }



        #endregion

        #region Event Handlers

        private void newButton_Click(object sender, EventArgs e)
        {
            string outImageName = "";

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

                //删除临时创建的压缩图片文件
                if (File.Exists(outImageName))
                    File.Delete(outImageName);
            }
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                Image image = Image.FromStream(ms);
                Picture pic = new Picture(image);
                Surface.Source.Load(pic, image.Size,
                    null, null, null);
            }
            catch
            {
                Surface.Source.Load(null, new Size(100, 100),
                    null, null, null);
            }
        }

        private void sizeButton_Click(object sender, EventArgs e)
        {
            using (NewSizeForm f = new NewSizeForm())
            {
                f.ImageSize = Surface.Source.Size;
                DialogResult dr = f.ShowDialog();
                if (dr == DialogResult.Cancel)
                    return;
                Surface.Source.Load(Surface.Source.Background, f.ImageSize,
                    Surface.Source.NamedHatchStyle,
                    Surface.Source.NamedTextureStyle,
                    Surface.Source.Shapes);
            }
        }

        private void namedStyleButton_Click(object sender, EventArgs e)
        {
            using (NamedStyleSettingForm f =
                new NamedStyleSettingForm())
            {
                CustomNamedHatchStyles cnhs;
                CustomNamedTextureStyles cnts;
                if (Surface.Source.NamedHatchStyle.IsEmpty)
                    cnhs = new CustomNamedHatchStyles();
                else
                    cnhs = Surface.Source.NamedHatchStyle as CustomNamedHatchStyles;
                if (Surface.Source.NamedTextureStyle.IsEmpty)
                    cnts = new CustomNamedTextureStyles();
                else
                    cnts = Surface.Source.NamedTextureStyle as CustomNamedTextureStyles;

                f.SetNamedStyle(cnhs, cnts);
                f.ShowDialog();

                if (f.NamedHatchStyle.GetNames().Length == 0)
                    cnhs = null;
                else
                    cnhs = f.NamedHatchStyle;
                if (f.NamedTextureStyle.GetNames().Length == 0)
                    cnts = null;
                else
                    cnts = f.NamedTextureStyle;

                Surface.Source.Load(Surface.Source.Background,
                    Surface.Source.Size, cnhs, cnts,
                    Surface.Source.Shapes);
            }
        }

        #endregion

        #endregion

    }

    public class SizeEventArgs : EventArgs
    {
        public float FontSize
        {
            get { return _fontsize; }
            set { _fontsize = value; }
        }

        private float _fontsize;

        public SizeEventArgs(float size)
        {
            _fontsize = size;

        }
    }
}

