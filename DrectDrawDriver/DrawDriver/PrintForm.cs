using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.DrawDriver
{
    public partial class PrintForm : Form
    {
        //private PaperSize paperSize;

        private List<Metafile> listMetafile;

        private int printPageNowPer = 0;

        private PageSetupDialog pageSetupDialog = null;

        private bool isPrintAll = true;

        private bool boolTemp = false;

        private Type _t;

        private FieldInfo _Position;

        private MethodInfo _SetPositionMethod;

        private IContainer components = null;

        private PrintPreviewDialog printPreviewDialog1;

        private Panel pnltop;

        private Panel pnlBottom;

        private SimpleButton btnLoadPdf;

        private SpinEdit spinEditPrintCount;

        private LabelControl labelControl4;

        private SimpleButton btnPrint;

        private LabelControl labelControl3;

        private LookUpEdit lookUpEditPrinter;

        private TrackBarControl trackBarControl1;

        private Panel panel1;

        private PrintPreviewControl printPreviewControl1;

        private PrintDocument printDocument1;

        private LookUpEdit lookUpEditPageSize;

        private LabelControl labelControl2;

        private LabelControl labelControl1;

        private SimpleButton simpleButtonPrint;

        private LabelControl labelControl5;

        private SpinEdit spinEdit1;

        private RadioButton rabLeftRight;

        private RadioButton rabUpDown;

        private CheckEdit checkEditPrintDuplex;

        public PrintForm()
        {
            try
            {
                this.InitializeComponent();
                this.printDocument1.OriginAtMargins = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PrintForm(List<Metafile> pages)
        {
            try
            {
                this.InitializeComponent();
                this._t = typeof(PrintPreviewControl);
                this._Position = this._t.GetField("position", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                this._SetPositionMethod = this._t.GetMethod("SetPositionNoInvalidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                this.printPreviewControl1.MouseWheel += new MouseEventHandler(this.printPreviewControl1_MouseWheel);
                this.listMetafile = pages;
                this.printPreviewControl1.Rows = pages.Count;
                this.spinEdit1.Properties.MinValue = 1m;
                this.spinEdit1.Properties.MaxValue = pages.Count;
                this.pageSetupDialog = new PageSetupDialog();
                this.pageSetupDialog.Document = this.printDocument1;
                this.pageSetupDialog.PageSettings.PaperSize = new PaperSize("A4", 827, 1146);
                this.pageSetupDialog.PageSettings.Margins = new Margins(0, 0, 15, 15);
                if (CommonMethods.Landspace())
                {
                    this.pageSetupDialog.PageSettings.Landscape = true;
                }
                else
                {
                    this.pageSetupDialog.PageSettings.Landscape = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPrintView()
        {
            try
            {
                this.printDocument1.DefaultPageSettings.PaperSize = this.pageSetupDialog.PageSettings.PaperSize;
                this.printDocument1.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
                this.printPreviewControl1.Document = this.printDocument1;
                this.printPreviewControl1.Zoom = 1.0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (this.printPageNowPer < this.listMetafile.Count)
                {
                    string text = this.lookUpEditPageSize.Text;
                    Size size = new Size(this.listMetafile[this.printPageNowPer].Width, this.listMetafile[this.printPageNowPer].Height);
                    if (!this.isPrintAll)
                    {
                        int index = int.Parse(this.spinEdit1.Value.ToString()) - 1;
                        if (text == "A4")
                        {
                            e.Graphics.DrawImage(this.listMetafile[index], new Rectangle(30, 50, Convert.ToInt32((double)size.Width * 1.02), Convert.ToInt32((double)size.Height * 1.02)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else if (text == "B5")
                        {
                            e.Graphics.DrawImage(this.listMetafile[index], new Rectangle(0, 50, Convert.ToInt32((double)size.Width * 0.92), Convert.ToInt32((double)size.Height * 0.92)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else if (text == "16K")
                        {
                            e.Graphics.DrawImage(this.listMetafile[index], new Rectangle(54, 50, Convert.ToInt32((double)size.Width * 0.95), Convert.ToInt32((double)size.Height * 0.95)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            e.Graphics.DrawImage(this.listMetafile[index], 0, 0);
                        }
                        this.printPageNowPer = 1;
                        e.HasMorePages = false;
                    }
                    else
                    {
                        if (text == "A4")
                        {
                            e.Graphics.DrawImage(this.listMetafile[this.printPageNowPer], new Rectangle(30, 50, Convert.ToInt32((double)size.Width * 1.02), Convert.ToInt32((double)size.Height * 1.02)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else if (text == "B5")
                        {
                            e.Graphics.DrawImage(this.listMetafile[this.printPageNowPer], new Rectangle(0, 50, Convert.ToInt32((double)size.Width * 0.92), Convert.ToInt32((double)size.Height * 0.92)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else if (text == "16K")
                        {
                            e.Graphics.DrawImage(this.listMetafile[this.printPageNowPer], new Rectangle(54, 50, Convert.ToInt32((double)size.Width * 0.95), Convert.ToInt32((double)size.Height * 0.95)), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            e.Graphics.DrawImage(this.listMetafile[this.printPageNowPer], 0, 0);
                        }
                        this.printPageNowPer++;
                        e.HasMorePages = true;
                    }
                }
                else
                {
                    this.printPageNowPer = 0;
                    e.HasMorePages = false;
                }
                if (this.printPageNowPer == this.listMetafile.Count)
                {
                    this.printPageNowPer = 0;
                    e.HasMorePages = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPrinterList()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("PrintName");
                PrintDocument printDocument = new PrintDocument();
                string printerName = printDocument.PrinterSettings.PrinterName;
                foreach (string value in PrinterSettings.InstalledPrinters)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["PrintName"] = value;
                    dataTable.Rows.Add(dataRow);
                }
                this.lookUpEditPrinter.Properties.DataSource = dataTable;
                this.lookUpEditPrinter.Properties.DisplayMember = "PrintName";
                this.lookUpEditPrinter.Properties.ValueMember = "PrintName";
                this.lookUpEditPrinter.EditValue = printerName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPageSize()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("PageName");
                dataTable.Columns.Add("PageValue");
                DataRow dataRow = dataTable.NewRow();
                dataRow["PageName"] = "A4";
                dataRow["PageValue"] = "A4";
                dataTable.Rows.Add(dataRow);
                dataRow = dataTable.NewRow();
                dataRow["PageName"] = "16K";
                dataRow["PageValue"] = "16K";
                dataTable.Rows.Add(dataRow);
                this.lookUpEditPageSize.Properties.DataSource = dataTable;
                this.lookUpEditPageSize.Properties.DisplayMember = "PageName";
                this.lookUpEditPageSize.Properties.ValueMember = "PageValue";
                this.lookUpEditPageSize.EditValue = "A4";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitPrinterList();
                this.InitPageSize();
                this.InitPrintView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.isPrintAll = true;
                string value = this.lookUpEditPageSize.EditValue.ToString();
                this.printDocument1.PrinterSettings.PrinterName = this.lookUpEditPrinter.Text.Trim().ToString();
                PaperSize paperSize = new PaperSize("A4", 850, 1160);
                foreach (PaperSize paperSize2 in this.pageSetupDialog.Document.PrinterSettings.PaperSizes)
                {
                    if (paperSize2.PaperName.Equals(value))
                    {
                        paperSize = paperSize2;
                    }
                }
                if (paperSize != null)
                {
                    this.pageSetupDialog.Document.DefaultPageSettings.PaperSize = paperSize;
                }
                if (this.checkEditPrintDuplex.Checked)
                {
                    if (!this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    {
                        MessageBox.Show("此打印机不支持双面打印!", "警告");
                        this.checkEditPrintDuplex.Checked = false;
                        return;
                    }
                    if (this.rabLeftRight.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                    }
                    else if (this.rabUpDown.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                    }
                    else
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                    }
                }
                else
                {
                    this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                }
                this.pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                for (int i = 0; i < Convert.ToInt32(this.spinEditPrintCount.EditValue); i++)
                {
                    if (this.rabLeftRight.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                        this.printDocument1.Print();
                    }
                    else if (this.rabUpDown.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                        this.printDocument1.Print();
                    }
                    else
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                        this.printDocument1.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.printPreviewControl1.Zoom = Convert.ToDouble(this.trackBarControl1.Value) / 100.0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PageSet_Click(object sender, EventArgs e)
        {
            try
            {
                this.pageSetupDialog = new PageSetupDialog();
                this.pageSetupDialog.Document = this.printDocument1;
                if (this.pageSetupDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!this.boolTemp)
                    {
                        this.boolTemp = true;
                    }
                    this.printPreviewControl1.Zoom = 1.0;
                    this.printPageNowPer = 0;
                    this.printPreviewControl1.InvalidatePreview();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.printDocument1.DefaultPageSettings.PaperSize = this.pageSetupDialog.PageSettings.PaperSize;
                this.printDocument1.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
                this.printPreviewControl1.Document = this.printDocument1;
                this.printPreviewControl1.Zoom = 1.0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnLoadPdf_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = "D:\\";
                saveFileDialog.Filter = "Pdf Files(*.pdf)|*.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DrawOp.ExportToPDf(this.listMetafile, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void printPreviewControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (SystemInformation.MouseWheelPresent)
                {
                    float num = (float)(Math.Abs(e.Delta) / SystemInformation.MouseWheelScrollDelta);
                    num *= (float)SystemInformation.MouseWheelScrollLines;
                    num *= 12f;
                    num *= (float)this.printPreviewControl1.Zoom;
                    int num2;
                    if (e.Delta < 0)
                    {
                        num2 = (int)num;
                    }
                    else
                    {
                        num2 = -(int)num;
                    }
                    Point point = (Point)this._Position.GetValue(this.printPreviewControl1);
                    this._SetPositionMethod.Invoke(this.printPreviewControl1, new object[]
					{
						new Point(point.X, point.Y + num2)
					});
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {
            try
            {
                this.printPreviewControl1.Select();
                this.printPreviewControl1.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.isPrintAll = false;
                string value = this.lookUpEditPageSize.EditValue.ToString();
                this.printDocument1.PrinterSettings.PrinterName = this.lookUpEditPrinter.Text.Trim().ToString();
                PaperSize paperSize = new PaperSize("A4", 850, 1130);
                if (CommonMethods.Landspace())
                {
                    this.pageSetupDialog.PageSettings.Landscape = true;
                }
                else
                {
                    this.pageSetupDialog.PageSettings.Landscape = false;
                }
                foreach (PaperSize paperSize2 in this.pageSetupDialog.Document.PrinterSettings.PaperSizes)
                {
                    if (paperSize2.PaperName.Equals(value))
                    {
                        paperSize = paperSize2;
                    }
                }
                if (paperSize != null)
                {
                    this.pageSetupDialog.Document.DefaultPageSettings.PaperSize = paperSize;
                }
                if (this.checkEditPrintDuplex.Checked)
                {
                    if (!this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    {
                        MessageBox.Show("此打印机不支持双面打印!", "警告");
                        this.checkEditPrintDuplex.Checked = false;
                        return;
                    }
                    if (this.rabLeftRight.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                    }
                    else if (this.rabUpDown.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                    }
                    else
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                    }
                }
                else
                {
                    this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                }
                this.pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                for (int i = 0; i < Convert.ToInt32(this.spinEditPrintCount.EditValue); i++)
                {
                    if (this.rabLeftRight.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                        this.printDocument1.Print();
                    }
                    else if (this.rabUpDown.Checked)
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                        this.printDocument1.Print();
                    }
                    else
                    {
                        this.pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                        this.printDocument1.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void checkEditPrintDuplex_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkEditPrintDuplex.Checked)
                {
                    this.rabLeftRight.Visible = true;
                    this.rabUpDown.Visible = true;
                }
                else
                {
                    this.rabLeftRight.Visible = false;
                    this.rabUpDown.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
