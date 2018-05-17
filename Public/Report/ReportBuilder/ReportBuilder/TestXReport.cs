using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YidanSoft.Common.Report
{
   partial class TestXReport : Form
   {
      [STAThread]
      public static void Main()
      {
         Application.Run(new TestXReport());
      }
      private DataSet ds = new DataSet("Test");
      public TestXReport()
      {
         InitializeComponent();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Cursor.Current = Cursors.WaitCursor;
         if (ds == null || ds.Tables.Count == 0)
            button3_Click(sender, e);
         ReportDesign rp = new ReportDesign( textBox1.Text,ds);
         rp.Design();
         Cursor.Current = Cursors.Default;
      }

     

      private void button2_Click(object sender, EventArgs e)
      {
         Cursor.Current = Cursors.WaitCursor;
         if (ds == null || ds.Tables.Count == 0)
            button3_Click(sender, e);
         XReport rp = new XReport(ds,textBox1.Text);
         rp.ShowPreview();
         Cursor.Current = Cursors.Default;
      }

       [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Int32.ToString"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
      private void button3_Click(object sender, EventArgs e)
      {
         Cursor.Current = Cursors.WaitCursor;
         int col = DateTime.Now.Second % 9;
         col = col + 1;
         DataTable dt = new DataTable("test1");
         for (int i = 0; i < col; i++)
         {
            dt.Columns.Add(new DataColumn(string.Format("col{0}", i)));
         }
         DataRow dr = dt.NewRow();
         dt.Rows.Add(dr);
         for (int i = 0; i < col; i++)
         {
            if (i > 3)
               dr[i] = string.Format("≤‚ ‘{0}", i);
            else
               dr[i] = i.ToString();
         }
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);
         dt.ImportRow(dr);         
         ds.Tables.Clear();
         ds.Tables.Add(dt);
         Cursor.Current = Cursors.Default;
      }

      private void TestXReport_Load(object sender, EventArgs e)
      {

      }

      private void button4_Click(object sender, EventArgs e)
      {
         this.openFileDialog1.FileName = this.textBox1.Text;
         DialogResult rt = this.openFileDialog1.ShowDialog();
         if (rt == DialogResult.OK
            || rt == DialogResult.Yes
            )
         {
            this.textBox1.Text = this.openFileDialog1.FileName;
            
         }
      }
      private void button5_Click(object sender, EventArgs e)
      {
         this.saveFileDialog1.FileName = this.textBox1.Text;
         DialogResult rt = this.saveFileDialog1.ShowDialog();
         if (rt == DialogResult.OK
            || rt == DialogResult.Yes
            )
         {
            this.textBox1.Text = this.saveFileDialog1.FileName;
         }

      }
   }
}