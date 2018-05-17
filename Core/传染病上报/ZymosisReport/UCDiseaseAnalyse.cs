using DevExpress.Utils;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class UCDiseaseAnalyse : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_App;
        DrawHeadRectangle1 m_DrawHeadRectangle = new DrawHeadRectangle1();
        SqlHelper m_SqlHelper;

        public UCDiseaseAnalyse()
        {
            InitializeComponent();
        }

        public UCDiseaseAnalyse(IEmrHost app)
            : this()
        {
            m_App = app;
            m_SqlHelper = new SqlHelper(m_App.SqlHelper);
        }

        private void UCDiseaseAnalyse_Load(object sender, EventArgs e)
        {
            gridViewAnalyse.ColumnPanelRowHeight = 30;
            Reset();
        }

        private void SetGridControlHead()
        {
        }

        private void gridViewAnalyse_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Graphics g = e.Graphics;

            string caption = string.Empty;
            if (e.Column.Tag != null)
            {
                caption = e.Column.Tag.ToString();
            }
            else
            {
                caption = e.Column.Caption;
            }
            m_DrawHeadRectangle.AddBound(caption, e.Bounds);

            Rectangle rect = new Rectangle(m_DrawHeadRectangle.Bound.Location.X, m_DrawHeadRectangle.Bound.Location.Y, m_DrawHeadRectangle.Bound.Width - 1, m_DrawHeadRectangle.Bound.Height - 1);
            g.FillRectangle(new SolidBrush(Color.FromArgb(235, 236, 239)), rect);
            g.DrawRectangle(SystemPens.ControlDark, rect);
            g.DrawString(m_DrawHeadRectangle.ColumnName, this.Font, Brushes.Black, rect, sf);

            e.Handled = true;
        }

        private void simpleButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.FileName = "资料构成分析报表";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                gridControlAnalyse.ExportToXlsx(saveFileDialog.FileName);
                m_App.CustomMessageBox.MessageShow("导出成功！");
            }
        }

        private void BindGridControlData(string dateTimeFrom, string dateTimeTo)
        {
            DataTable dt = m_SqlHelper.GetReportAnalyse(
                (Convert.ToDateTime(dateTimeFrom)).ToString("yyyy-MM-dd"),
                (Convert.ToDateTime(dateTimeTo)).ToString("yyyy-MM-dd"));
            ReplaceZero(dt);
            ChangeName(dt);
            gridControlAnalyse.DataSource = dt;
        }

        private void ChangeName(DataTable dt)
        {
            //LEVEL_NAME
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["LEVEL_NAME"].ToString() == "总计")
                {
                    if (i >= 0)
                    {
                        dt.Rows[i]["LEVEL_NAME"] = dt.Rows[i - 1]["LEVEL_NAME"];
                    }
                }
            }
        }

        private void ReplaceZero(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString().Trim() == "0")
                    {
                        dt.Rows[i][j] = "-";
                    }
                }
            }
        }

        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在查询，请稍等...");
            string dateTimeFrom = dateEditAnalyseFrom.EditValue == null ?
                DateTime.MinValue.ToString("yyyy-MM-dd") : dateEditAnalyseFrom.EditValue.ToString().Split(' ')[0];
            string dateTimeTo = dateEditAnalyseEnd.EditValue == null ?
                DateTime.MaxValue.ToString("yyyy-MM-dd") : dateEditAnalyseEnd.EditValue.ToString().Split(' ')[0];
            BindGridControlData(dateTimeFrom, dateTimeTo);
            HideWaitDialog();
        }

        private WaitDialogForm m_WaitDialog;
        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }
            else
            {
                m_WaitDialog = new WaitDialogForm(caption, "提示");
            }
        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        private void gridViewAnalyse_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                this.dateEditAnalyseFrom.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEditAnalyseEnd.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class DrawHeadRectangle1
    {
        public Rectangle Bound;
        public string ColumnName;

        public void AddBound(string columnName, Rectangle rect)
        {
            if (string.IsNullOrEmpty(ColumnName))
            {
                Bound = rect;
                ColumnName = columnName;
            }
            else
            {
                if (ColumnName == columnName)
                {
                    Bound = new Rectangle(Bound.Location, new Size(Bound.Width + rect.Width, Bound.Height));
                }
                else
                {
                    Bound = rect;
                    ColumnName = columnName;
                }
            }
        }
    }
}
