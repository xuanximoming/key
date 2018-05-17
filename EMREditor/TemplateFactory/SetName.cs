using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class SetName : DevBaseForm
    {
        public SetName()
        {
            InitializeComponent();
            this.textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    byte[] old = Encoding.Unicode.GetBytes(this.textBox1.Text);
                    byte[] des = Encoding.Unicode.GetBytes(FixLength(this.textBox2.Text));

                    UpdateBytes A = new UpdateBytes(diag.FileName);
                    A.Update(old, des);
                    A.Save(diag.FileName);
                }
            }
        }

        string FixLength(string str)
        {
            str = str.Trim();
            if (str.Length < 30)
            {
                for (int i = str.Length + 1; i <= 30; i++)
                {
                    str += " ";
                }
            }
            return str;
        }

        /// <summary>
        /// 回车光标后移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.win_KeyPress(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }

    public class UpdateBytes
    {
        public byte[] data;

        public UpdateBytes(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            data = new byte[(int)fs.Length];
            int len = fs.Read(data, 0, data.Length);
            if (len != data.Length)
            {
                byte[] bb = new byte[len];
                Buffer.BlockCopy(data, 0, bb, 0, len);
                data = bb;
            }
            fs.Close();
        }

        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }

        public void UpdateAndDelete(byte[] src, byte[] dest)
        {
            int index = IndexOf(src);
            if (index == -1)
            {
                MessageBox.Show("没有找到匹配项");
                return;
            }
            byte[] newdata = new byte[data.Length - src.Length + dest.Length];
            Buffer.BlockCopy(data, 0, newdata, 0, index);
            Buffer.BlockCopy(dest, 0, newdata, index, dest.Length);
            Buffer.BlockCopy(data, src.Length + index, newdata, index + dest.Length, data.Length - src.Length - index);
            data = newdata;
        }

        public void Update(byte[] src, byte[] dest)
        {
            int index = IndexOf(src);
            if (index == -1)
            {
                MessageBox.Show("没有找到匹配项");
                return;
            }
            Buffer.BlockCopy(dest, 0, data, index, dest.Length);
        }

        public int IndexOf(byte[] bt)
        {
            for (int i = 0, k = 0; i < data.Length; i++)
            {
                if (data[i] == bt[k])
                {
                    k++;
                }
                else if (k > 0)
                {
                    k = 0;
                }
                if (k == bt.Length)
                {
                    return i + 1 - bt.Length;
                }
            }
            return -1;
        }

        /// <summary>
        /// 回车光标后移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.win_KeyPress(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }

    //Update方法是覆盖，UpdateAndDelete是删除原来的插入新的
}
