using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class UCJobDisease : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_App;
        SqlHelper m_SqlHelper;
        public UCJobDisease()
        {
            InitializeComponent();
        }
        public UCJobDisease(IEmrHost host)
            : this()
        {
            m_App = host;
            m_SqlHelper = new SqlHelper(m_App.SqlHelper);
        }

        public void InitCheckedComboBoxEditDisease()
        {
            DataTable dt = m_SqlHelper.Disease;
            checkedComboBoxEditDisease.Properties.DataSource = dt;
            checkedComboBoxEditDisease.Properties.ValueMember = "ID";
            checkedComboBoxEditDisease.Properties.DisplayMember = "NAME";
            checkedComboBoxEditDisease.Properties.SelectAllItemCaption = "全选";
        }

        DrawHeadRectangle2 m_DrawHeadRectangle = new DrawHeadRectangle2();
        int i = 0;
        private void gridViewAnalyse_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            i++;
            int gridRowHeaderHeight;
            if (e.Column == null) return;

            RowHeadFlag rhf = e.Column.Tag as RowHeadFlag;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Graphics g = e.Graphics;

            if (rhf != null)
            {
                gridRowHeaderHeight = e.Bounds.Size.Height / 2;
                Rectangle rect1 = new Rectangle(e.Bounds.Location, new System.Drawing.Size(e.Bounds.Size.Width, gridRowHeaderHeight));
                Rectangle rect2 = new Rectangle(new Point(e.Bounds.Location.X, e.Bounds.Location.Y + gridRowHeaderHeight),
                    new System.Drawing.Size(e.Bounds.Size.Width, gridRowHeaderHeight));

                m_DrawHeadRectangle.AddBound(rhf, rect1);

                Rectangle rect = new Rectangle(m_DrawHeadRectangle.Bound.Location.X, m_DrawHeadRectangle.Bound.Location.Y, m_DrawHeadRectangle.Bound.Width - 1, m_DrawHeadRectangle.Bound.Height - 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(235, 236, 239)), rect);
                g.DrawRectangle(SystemPens.ControlDark, rect);
                g.DrawString(rhf.HeaderName, this.Font, Brushes.Black, rect, sf);

                rect = new Rectangle(rect2.Location.X, rect2.Location.Y, rect2.Width - 1, rect2.Height - 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(235, 236, 239)), rect);
                g.DrawRectangle(SystemPens.ControlDark, rect);
                g.DrawString(e.Info.Caption, this.Font, Brushes.Black, rect, sf);
            }
            else
            {
                Rectangle rect = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(235, 236, 239)), rect);
                g.DrawRectangle(SystemPens.ControlDark, rect);
                g.DrawString(e.Info.Caption, this.Font, Brushes.Black, rect, sf);
            }

            if (e.Column.VisibleIndex == GetGridControlVisibleColumnsCount())
            {
                m_DrawHeadRectangle = new DrawHeadRectangle2();
            }

            e.Handled = true;
        }

        private int GetGridControlVisibleColumnsCount()
        {
            int i = -1;
            foreach (GridColumn gc in gridViewAnalyse.Columns)
            {
                if (gc.Visible)
                {
                    i++;
                }
            }
            return i;
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-13
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCJobDisease_Load(object sender, EventArgs e)
        {
            try
            {
                InitCheckedComboBoxEditDisease();
                checkedComboBoxEditDisease.CheckAll();
                Reset();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
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

        private void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在查询，请稍等...");
            string value = checkedComboBoxEditDisease.EditValue.ToString();
            string dateTimeFrom = dateEditAnalyseFrom.EditValue == null ?
                DateTime.MinValue.ToString("yyyy-MM-dd") : dateEditAnalyseFrom.EditValue.ToString().Split(' ')[0];
            string dateTimeTo = dateEditAnalyseEnd.EditValue == null ?
                DateTime.MaxValue.ToString("yyyy-MM-dd") : dateEditAnalyseEnd.EditValue.ToString().Split(' ')[0];
            DataTable dtDisease = FilterDisease(m_SqlHelper.Disease.Copy(), value);
            if (dtDisease.Rows.Count > 0)
            {
                DataTable dtJobDisease = m_SqlHelper.GetJobDisease(dtDisease, dateTimeFrom, dateTimeTo);
                BindGridControlData(dtJobDisease);
            }
            else
            {
                BindGridControlData(null);
            }
            HideWaitDialog();
        }

        private DataTable FilterDisease(DataTable dtDisease, string disease)
        {
            disease = "," + disease + ",";
            for (int i = dtDisease.Rows.Count - 1; i >= 0; i--)
            {
                if (disease.IndexOf(dtDisease.Rows[i]["id"].ToString()) < 0)
                {
                    dtDisease.Rows.RemoveAt(i);
                }
            }
            return dtDisease;
        }

        private void BindGridControlData(DataTable dt)
        {
            if (dt != null)
            {
                BindGridColumn(dt);
                gridControlAnalyse.DataSource = dt;
            }
            else
            {
                gridControlAnalyse.DataSource = null;
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

        public void BindGridColumn(DataTable dt)
        {
            gridViewAnalyse.Columns.Clear();

            gridViewAnalyse.ColumnPanelRowHeight = 100;

            int i = 0;

            GridColumn gc1 = new GridColumn();
            gc1.Caption = "职业";
            gc1.FieldName = "JOBNAME";
            gc1.Fixed = FixedStyle.Left;
            gc1.Visible = true;
            gc1.VisibleIndex = i;
            gc1.Width = 100;
            gridViewAnalyse.Columns.Add(gc1);

            i++;

            GridColumn gc2 = new GridColumn();
            gc2.Caption = "发病数";
            gc2.FieldName = "SUM_DISEASE";
            gc2.Visible = true;
            gc2.VisibleIndex = i;
            gc2.Width = 50;
            gc2.Tag = new RowHeadFlag { GroupID = "1", HeaderName = "合计" };
            gridViewAnalyse.Columns.Add(gc2);

            i++;

            GridColumn gc3 = new GridColumn();
            gc3.Caption = "死亡数";
            gc3.FieldName = "SUM_DIE";
            gc3.Visible = true;
            gc3.VisibleIndex = i;
            gc3.Width = 50;
            gc3.Tag = new RowHeadFlag { GroupID = "1", HeaderName = "合计" };
            gridViewAnalyse.Columns.Add(gc3);

            i++;

            int groupID = 1;
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.EndsWith("_发病数"))
                {
                    groupID++;
                    GridColumn gc = new GridColumn();
                    gc.Caption = "发病数";
                    gc.FieldName = dt.Columns[j].ColumnName;
                    gc.Visible = true;
                    gc.VisibleIndex = i;
                    gc.Width = 50;
                    gc.Tag = new RowHeadFlag { GroupID = groupID.ToString(), HeaderName = dt.Columns[j].ColumnName.Split('_')[0] };
                    gridViewAnalyse.Columns.Add(gc);
                    i++;
                }
                i++;
                if (dt.Columns[j].ColumnName.EndsWith("_死亡数"))
                {
                    GridColumn gc = new GridColumn();
                    gc.Caption = "死亡数";
                    gc.FieldName = dt.Columns[j].ColumnName;
                    gc.Visible = true;
                    gc.VisibleIndex = i;
                    gc.Width = 50;
                    gc.Tag = new RowHeadFlag { GroupID = groupID.ToString(), HeaderName = dt.Columns[j].ColumnName.Split('_')[0] };
                    gridViewAnalyse.Columns.Add(gc);
                    i++;
                }
            }

            ReplaceZero(dt);
        }

        private void simpleButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.FileName = "职业疾病统计报表";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                ChangeDataTableForExportting();
                gridControlAnalyse.ExportToXlsx(saveFileDialog.FileName);
                ChangeDataTableForExportted();
                m_App.CustomMessageBox.MessageShow("导出成功");
            }
        }

        private void ChangeDataTableForExportting()
        {
            DataTable dt = gridControlAnalyse.DataSource as DataTable;
            foreach (GridColumn gc in gridViewAnalyse.Columns)
            {
                RowHeadFlag rhf = gc.Tag as RowHeadFlag;
                if (rhf != null)
                {
                    gc.Caption = rhf.HeaderName + "|" + gc.Caption;
                }
            }
        }

        private void ChangeDataTableForExportted()
        {
            DataTable dt = gridControlAnalyse.DataSource as DataTable;
            foreach (GridColumn gc in gridViewAnalyse.Columns)
            {
                RowHeadFlag rhf = gc.Tag as RowHeadFlag;
                if (rhf != null)
                {
                    if (gc.Caption.IndexOf(rhf.HeaderName) >= 0)
                    {
                        gc.Caption = gc.Caption.Substring(rhf.HeaderName.Length + 1);
                    }
                }
            }
        }

        /// <summary>
        /// 将datatable某列转化为字符串
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-13</date>
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetAllValue(DataTable dt, string name)
        {
            try
            {
                string str = string.Empty;
                var enuList = dt.AsEnumerable().Select(p => p[name]);
                foreach (string s in enuList)
                {
                    str += s + ",";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                this.checkedComboBoxEditDisease.EditValue = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


    /// <summary>
    /// 合并表头的标题及类别
    /// </summary>
    public class RowHeadFlag
    {
        /// <summary>
        /// 表头群组
        /// </summary>
        public string GroupID { get; set; }

        /// <summary>
        /// 表头名称
        /// </summary>
        public string HeaderName { get; set; }

        public bool Equals(RowHeadFlag flag)
        {
            return GroupID.Equals(flag.GroupID) && HeaderName.Equals(flag.HeaderName);
        }
    }

    public class DrawHeadRectangle2
    {
        public Rectangle Bound;
        private RowHeadFlag CurrentRowHeadFlag;

        public void AddBound(RowHeadFlag flag, Rectangle rect)
        {
            if (CurrentRowHeadFlag == null)
            {
                Bound = rect;
                CurrentRowHeadFlag = flag;
            }
            else
            {
                if (CurrentRowHeadFlag.Equals(flag))
                {
                    Bound = new Rectangle(Bound.Location, new Size(Bound.Width + rect.Width, Bound.Height));
                }
                else
                {
                    Bound = rect;
                    CurrentRowHeadFlag = flag;
                }
            }
        }
    }
}
