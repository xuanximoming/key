using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DrectSoft.Common.Ctrs.DLG;
using DevExpress.XtraEditors;

namespace DevTextBoxAndButton
{
    [ToolboxBitmap(typeof(Button))]
    public partial class Bwj : DrectSoft.Common.Ctrs.OTHER.WaterTextBox
    {

        public Button obj = new Button();

        //private bool _IsButtonClick = false;

        //public bool IsButtonClick
        //{
        //    get
        //    {
        //        return _IsButtonClick;
        //    }
        //    set
        //    {
        //        if (value)
        //        {
        //            obj.Click += new EventHandler(obj_Click);
        //        }
        //        _IsButtonClick = value;
        //    }
        //}

        public Bwj()
        {
            InitializeComponent();
        }

        public Bwj(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            obj.Dock = DockStyle.Right;
            obj.FlatStyle = FlatStyle.Standard;

            obj.Text = "查询(&Q)";
            obj.Width = 40;
            obj.Height = 18;
            //obj.Click += new EventHandler(this.obj_Click);
            //obj.Click += new EventHandler(obj_Click);
            //TextBox txt = new TextBox();
            //txt.Click += new EventHandler(txt_Click);
            obj.Visible = false;
            this.Controls.Add(obj);
        }

        //public void obj_Click(object sender, EventArgs e )
        //{
        //    try
        //    {
        //        SendKeys.Send("{Click}");
        //        SendKeys.Flush();
        //    }
        //    catch (Exception ce)
        //    {
        //        MessageBox.Show(ce.Message.ToString());
        //        return;
        //    }


        //}

        //private void button_Click(object sender, EventArgs e)
        //{
        //    //if (sender.Equals=obj.Click)
        //    //{
        //        Form frm = new Form();
        //        frm.Show();
        //    //}
        //}
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            try
            {
                base.OnInvalidated(e);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void txt_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    try
        //    {
        //        if ((int)e.KeyCode == 13)
        //        {
        //            SendKeys.Send("{Tab}");
        //            SendKeys.Flush();
        //        }
        //        else if (e.KeyCode == Keys.Escape)
        //        {
        //            SendKeys.Send("+({Tab})");
        //            SendKeys.Flush();
        //        }
        //    }
        //    catch (Exception ce)
        //    {
        //        MessageBox.Show(ce.Message.ToString(), "志扬软件");
        //        return;
        //    }

        //}

        // public EventHandler obj_Click { get; set; }

        //public EventHandler txt_Click { get; set; }

        public string m_diacode = string.Empty;
        public string DiaCode
        {
            get { return m_diacode; }
            set { m_diacode = value; }
        }

        public string m_diavalue = string.Empty;
        public string DiaValue
        {
            get { return m_diavalue; }
            set { m_diavalue = value; }
        }


    }
}
