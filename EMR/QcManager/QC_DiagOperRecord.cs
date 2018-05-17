using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;
using DrectSoft.Resources;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;
using DevExpress.Utils;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 诊断和手术病案
    /// </summary>
    public partial class QC_DiagOperRecord : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;

        #region ==========Methods============
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app"></param>
        public QC_DiagOperRecord(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                InitSqlWorkBook();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitSqlWorkBook()
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                LookUpWindow lookUpWindowInDiag = new LookUpWindow();
                LookUpWindow lookUpWindowOutDiag = new LookUpWindow();
                LookUpWindow lookUpWindowOper = new LookUpWindow();
                lookUpWindowInDiag.SqlHelper = m_app.SqlHelper;
                lookUpWindowOutDiag.SqlHelper = m_app.SqlHelper;
                lookUpWindowOper.SqlHelper = m_app.SqlHelper;
                this.lookUpEditorInDiag.ListWindow = lookUpWindowInDiag;
                this.lookUpEditorOutDiag.ListWindow = lookUpWindowInDiag;
                this.lookUpEditorOper.ListWindow = lookUpWindowOper;

                string sql_diag = "select  py, wb, name, id  from diagnosisofchinese where valid='1' union select py, wb, name, icdid from diagnosischiothername where valid='1'";
                DataTable diag = m_app.SqlHelper.ExecuteDataTable(sql_diag);
                diag.Columns["ID"].Caption = "编号";
                diag.Columns["NAME"].Caption = "诊断名称";

                string sql_diagxy = "select py, wb, name, icd from diagnosis  where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1'";
                DataTable diagxy = m_app.SqlHelper.ExecuteDataTable(sql_diagxy);
                diagxy.Columns["ICD"].Caption = "编号";
                diagxy.Columns["NAME"].Caption = "诊断名称";

                //add by jxh 实现中西医不同出院诊断查询条件的选择
                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                if (!valueStr.ToLower().Contains("SX"))
                {
                    Dictionary<string, int> colDiag = new Dictionary<string, int>();
                    colDiag.Add("ICD", 80);
                    colDiag.Add("NAME", 160);

                    SqlWordbook inWordBook = new SqlWordbook("inDiag", diagxy, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                    this.lookUpEditorInDiag.SqlWordbook = inWordBook;
                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diagxy, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                }
                else
                {
                    this.comboBoxEdit1.Visible = true;
                    this.label1.Visible = true;
                    this.comboBoxEdit1.Properties.Items.Add("中医出院诊断");
                    this.comboBoxEdit1.Properties.Items.Add("西医出院诊断");
                    this.comboBoxEdit1.Location = new System.Drawing.Point(882, 11);
                    this.label1.Left = 335;
                    this.comboBoxEdit1.Left = 460;
                    this.labelControl5.Left = 647;
                    this.lookUpEditorOutDiag.Left = 710;

                    Dictionary<string, int> colDiag = new Dictionary<string, int>();
                    colDiag.Add("ID", 80);
                    colDiag.Add("NAME", 160);

                    SqlWordbook inWordBook = new SqlWordbook("inDiag", diag, "ID", "NAME", colDiag, "ID//NAME//PY//WB");
                    this.lookUpEditorInDiag.SqlWordbook = inWordBook;
                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diag, "ID", "NAME", colDiag, "ID//NAME//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                }

                string sql_oper = "select id,name,py,wb from operation";
                DataTable oper = m_app.SqlHelper.ExecuteDataTable(sql_oper);
                oper.Columns["ID"].Caption = "编号";
                oper.Columns["NAME"].Caption = "手术名称";



                Dictionary<string, int> colOper = new Dictionary<string, int>();
                colOper.Add("ID", 80);
                colOper.Add("NAME", 160);


                SqlWordbook operWordBook = new SqlWordbook("oper", oper, "ID", "NAME", colOper, "ID//NAME//PY//WB");
                this.lookUpEditorOper.SqlWordbook = operWordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ==========Events=============
        private WaitDialogForm m_WaitDialog;//查询等待窗体add by ywk 2013年7月17日 11:29:48 

        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                //SetWaitDialogCaption("请稍候正在查询...");
                m_WaitDialog = new WaitDialogForm("正在查询相应记录...", "请稍候");
                //门急诊诊断代码
                string inDiag = this.lookUpEditorInDiag.CodeValue;
                //出院诊断代码
                string outDiag = this.lookUpEditorOutDiag.CodeValue;
                //手术代码
                string oper = this.lookUpEditorOper.CodeValue;
                string tablename = DataAccess.GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');
                if (this.dateEdit_begin.DateTime > this.dateEdit_end.DateTime)
                {
                    MessageBox.Show("起始时间不能大于结束时间");
                    HideWaitDialog();
                    return;
                }
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";//.AddDays(1)
                string sqlText = SQLUtil.sql_QueryDiagOperRecord;
                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sqlText = SQLUtil.sql_QueryDiagOperRecord_ZY;
                }
                string outdiagspliter = string.Empty;//根据传入出院诊断条件将查询条件拼接进去（后期建议改为存储过程实现）add by ywk 
                string indiagspliter = string.Empty;
                string operspliter = string.Empty;
                string menjidiag = "  diag1.name";
                string outdiagname = string.Empty;
                string admitdiagname = string.Empty;
                if (string.IsNullOrEmpty(outDiag))//出院诊断为空说明此不要
                {
                    outdiagspliter = "  ";//查有
                    outdiagname = "";
                }
                else//输入条件
                {
                    outdiagspliter = string.Format(" and  diag.diagnosis_code = '{0}'", outDiag);
                    outdiagname = string.Format("temp1.OUTDIAG ='{0}' and ", lookUpEditorOutDiag.Text.Trim());
                }
                if (string.IsNullOrEmpty(inDiag))//门急诊诊断为空
                {
                    indiagspliter = string.Format("inner join diagnosis diag1 on imd.diagnosis_code = diag1.icd and diag1.valid=1   and imd.DIAGNOSIS_TYPE_ID=13", inDiag);
                    admitdiagname = "";
                    // menjidiag = "''  ";
                }
                else
                {
                    //menjidiag = " diag1.name ";
                    admitdiagname = string.Format("diag1.name='{0}' and ", lookUpEditorInDiag.Text.Trim());
                    indiagspliter = string.Format("left join diagnosis diag1 on imd.diagnosis_code = diag1.icd and diag1.valid=1   and imd.DIAGNOSIS_TYPE_ID=13 and imd.diagnosis_code = '{0}'", inDiag);
                }
                if (string.IsNullOrEmpty(oper))
                {
                    oper = " ";
                }
                else
                {
                    oper = string.Format(" and oper.operation_code='{0}'", oper);

                }

                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlText, indiagspliter, outdiagspliter, oper, begin_time, end_time, mainpage[0], mainpage[3], mainpage[2], menjidiag, outdiagname, admitdiagname));

                ImpressDataSet(ref dt, "NOOFINPAT", "OutDiag");

                this.gridControlRecord.DataSource = dt;

                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(dr["mzzyzd_name"].ToString()))
                        {
                            dr["ADMITDIAG"] = dr["mzxyzd_name"].ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dr["mzxyzd_name"].ToString()))
                            {
                                dr["ADMITDIAG"] = dr["mzzyzd_name"].ToString();
                            }
                            else
                            {
                                dr["ADMITDIAG"] = dr["mzzyzd_name"].ToString() + "," + dr["mzxyzd_name"].ToString();
                            }

                        }
                    }
                }
                HideWaitDialog();
                this.labPatCount.Text = dt.Rows.Count.ToString();
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("没有符合条件的记录");
                    this.gridControlRecord.DataSource = null;
                    return;
                }


            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        //传入目标数据集合的地址，直接进行数据的整理压缩
        public static void ImpressDataSet(ref DataTable dataTable, string refercolumn, string targetcolumn)
        {
            try
            {
                string id = ""; string nextid = ""; string value = "";
                List<int> list = new List<int>();
                int j = 0; int itimes = 0;
                //合并记录数值,制定参考字段和合并目标字段
                for (int i = 0; i < dataTable.Rows.Count - 1; i++)
                {
                    j = i + 1;
                    itimes = 0;
                    value = dataTable.Rows[i][targetcolumn].ToString();
                    id = dataTable.Rows[i][refercolumn].ToString();
                    nextid = dataTable.Rows[j][refercolumn].ToString();
                    while (id == nextid && j < dataTable.Rows.Count)
                    {
                        if (!value.Contains(dataTable.Rows[j][targetcolumn].ToString()))
                        {
                            value = value + "," + dataTable.Rows[j][targetcolumn].ToString();
                        }
                        list.Add(j);
                        j++;
                        if (j < dataTable.Rows.Count)
                        {
                            nextid = dataTable.Rows[j][refercolumn].ToString();
                        }
                        //
                        itimes++;
                    }
                    dataTable.Rows[i][targetcolumn] = value;
                    i = i + itimes;
                }
                //删除重复记录,
                int index = 0;
                itimes = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    index = list[i] - itimes;
                    dataTable.Rows.RemoveAt(index);
                    itimes++;
                }
                //重新编写序号
                //for (int i = 0; i < dataTable.Rows.Count; i++)
                //{
                //    dataTable.Rows[i]["SEQ"] = i + 1;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.lookUpEditorInDiag.CodeValue = "";
                this.lookUpEditorOutDiag.CodeValue = "";
                this.lookUpEditorOper.CodeValue = "";
                ResetControl();

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void ResetControl()
        {

            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
            this.gridControlRecord.DataSource = null;
        }
        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    gridControlRecord.ExportToXls(saveFileDialog.FileName, true);

                    m_app.CustomMessageBox.MessageShow("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 加序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件回车同tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体大小变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QC_DiagOperRecord_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(20, this.Height - 21);
                this.labPatCount.Location = new Point(71, this.Height - 21);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRecord_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewRecord.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        //add by jxh 下拉选择事件触发出院诊断内容
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (this.comboBoxEdit1.Text == "西医出院诊断")
                {
                    string sql_diagxy = "select py, wb, name, icd from diagnosis  where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1'";
                    DataTable diagxy = m_app.SqlHelper.ExecuteDataTable(sql_diagxy);
                    diagxy.Columns["ICD"].Caption = "编号";
                    diagxy.Columns["NAME"].Caption = "诊断名称";

                    Dictionary<string, int> colDiag = new Dictionary<string, int>();
                    colDiag.Add("ICD", 80);
                    colDiag.Add("NAME", 160);

                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diagxy, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                }
                else
                {
                    string sql_diag = "select  py, wb, name, id  from diagnosisofchinese where valid='1' union select py, wb, name, icdid from diagnosischiothername where valid='1'";
                    DataTable diag = m_app.SqlHelper.ExecuteDataTable(sql_diag);
                    diag.Columns["ID"].Caption = "编号";
                    diag.Columns["NAME"].Caption = "诊断名称";

                    Dictionary<string, int> colDiag = new Dictionary<string, int>();
                    colDiag.Add("ID", 80);
                    colDiag.Add("NAME", 160);

                    SqlWordbook outWordBook = new SqlWordbook("outDiag", diag, "ID", "NAME", colDiag, "ID//NAME//PY//WB");
                    this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
