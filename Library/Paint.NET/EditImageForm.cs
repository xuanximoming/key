using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace DrectSoft.Basic.Paint.NET
{
    public partial class EditImageForm : Form
    {
        private IPaintPanel _pp;
        public IPaintPanel PP
        {
            get { return _pp; }
            set { _pp = value; }
        }

        public Image CurrentImage
        {
            get
            {
                return PP.CurrentImage;
            }

        }

        public string ImageContent
        {
            get
            {
                return _imageContent;
            }
        }

        private string _imageContent;

        public EditImageForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 创建值控件
        /// </summary>
        private void CreateValueControl()
        {
            Control c = CreateValueControlCore();
            c.Dock = DockStyle.Fill;
            if (c != null)
            {
                this.Controls.Add(c);
            }
        }

        private Control CreateValueControlCore()
        {
            Control result = new Control(); ;

            PP = new PaintDesignPanel();
            PP.ExitWithSave += new EventHandler(PP_ExitWithSave);
            PP.ExitWithoutSave += new EventHandler(PP_ExitWithoutSave);
            result = PP.GetControl();
            result.BackColor = Color.White;

            //m_tempPP = new PaintDesignPanel(false);
            return result;
        }

        void PP_ExitWithoutSave(object sender, EventArgs e)
        {
            this.Close(); this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        void PP_ExitWithSave(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PP.SaveImage(ms);
                _imageContent = Convert.ToBase64String(ms.ToArray());
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
        }

        private void Save(byte[] image, out string content)
        {
            //content = string.Empty;
            //m_tempPP.LoadOriginalImage(image);
            //using (MemoryStream ms = new MemoryStream())
            //{

            //    m_tempPP.SaveImage(ms);
            //    content = Convert.ToBase64String(ms.ToArray());
            //    //this.SetAtomValue(AtomValue.CreateGraphicAtom(ms.ToArray()));

            //}

            content = "";
        }

        public DialogResult LoadImages(string content)
        {
            if (PP == null)
                CreateValueControl();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(content));
            PP.LoadImage(ms);
            return this.ShowDialog();
        }

        public DialogResult LoadImages2(Image image)
        {

            if (PP == null)
                CreateValueControl();


            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imagedata = ms.GetBuffer();

            PP.LoadOriginalImage(imagedata);
            return this.ShowDialog();

        }

        private void EditImageForm_Load(object sender, EventArgs e)
        {


        }
    }
}
