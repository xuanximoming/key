using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Service;
using Oracle.DataAccess.Client;
using DrectSoft.Core;
using DrectSoft.Common.Ctrs.DLG;
using System.Reflection;
namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class DictionaryDataSerach : DevBaseForm
    {
        #region field && Property

        /// <summary>
        /// 西医所有诊断
        /// </summary>
        private const string SqlAllDiag = " SELECT markid, icd, mapid, standardcode, name, py, wb, " +
            " tumorid, statist, innercategory, category, othercategroy, valid, memo " +
            " FROM diagnosis where valid = '1' union select icdid markid, icdid icd, '' mapid, '' standardcode, name,   py, wb,'' tumorid, '' statist, '' innercategory, '' category, '' othercategroy, 1 valid, '' memo from diagnosisothername where valid = '1' ";

        private const string SqlAllOper = @"SELECT id markid, id icd, mapid,'' standardcode,name,py,wb,'' tumorid,'' statist,''                 category, '' category, '' othercategroy, valid,  memo FROM operation where valid = '1'";


        /// <summary>
        /// 中医所有诊断
        /// </summary>
        private const string SqlAllDiagChinese = " select  py, wb, name, id icd  from diagnosisofchinese where valid='1' union select py, wb, name, icdid icd from diagnosischiothername where valid='1'";

        private const string PYFilter = " py like '%{0}%' ";
        private const string WBFilter = " wb like '%{0}%' ";
        private const string NameFilter = " name like '%{0}%' ";
        private const string ICDFilter = " icd like '%{0}%' ";

        private IEmrHost m_App;
        private DataTable m_DiagDataSource;

        private string m_DiagName = string.Empty;
        private string m_DiagICD = string.Empty;

        delegate void DeleteLoadData();

        /// <summary>
        /// 选中的诊断名
        /// </summary>
        public string DiagName
        {
            get
            {
                return m_DiagName;
            }
        }

        /// <summary>
        /// 选中的诊断的ICD编码
        /// </summary>
        public string DiagICD
        {
            get
            {
                return m_DiagICD;
            }
        }

        /// <summary>
        /// 诊断名称
        /// </summary>
        private string m_DiagTypeName = string.Empty;

        #endregion

        #region .ctor
        public DictionaryDataSerach()
        {
            InitializeComponent();
        }
        public DictionaryDataSerach(IEmrHost app, string diagTypeName)
            : this()
        {
            m_App = app;
            m_DiagTypeName = diagTypeName;
        }
        #endregion

        #region Method
        /// <summary>
        /// 二次修改诊断数据的捞取源
        /// add by ywk 2013年3月19日9:58:03 
        /// </summary>
        //private void LoadData()
        //{
        //    try
        //    {
        //        DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
        //        if (m_DiagTypeName == "手术信息")
        //        {
        //            m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllOper, CommandType.Text);
        //            gridControlDiag.DataSource = m_DiagDataSource;
        //            return;
        //        }



        //        //得到配置中的捞取诊断源0是从EMR，1是从HIS
        //        string getdiagtype = DS_SqlService.GetConfigValueByKey("GetDiagnosisType") == "" ? "0" : DS_SqlService.GetConfigValueByKey("GetDiagnosisType");


        //        m_DiagName = "";
        //        m_DiagICD = "";
        //        if (m_DiagTypeName.IndexOf("中医") >= 0)
        //        {
        //            m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
        //        }
        //        else
        //        {
        //            //西医诊断，中心医院要求添加诊断的时候捞取HIS中的诊断信息  ****0是从EMR，1是从HIS***
        //            //add by ywk 2013年3月19日9:31:24 
        //            if (getdiagtype == "0")//EMR
        //            {
        //                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
        //            }
        //            if (getdiagtype == "1")//HIS 
        //            {
        //                try
        //                {
        //                    using (OracleConnection conn = new OracleConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
        //                    {
        //                        if (conn.State != ConnectionState.Open)
        //                        {
        //                            conn.Open();
        //                        }
        //                        m_DiagDataSource = new DataTable();
        //                        OracleCommand cmd = conn.CreateCommand();
        //                        cmd.CommandType = CommandType.Text;
        //                        cmd.CommandText = "SELECT  MarkId as  ID,  NAME, py , WB, memo,  icd  FROM yd_diagnosis";
        //                        OracleDataAdapter myoadapt = new OracleDataAdapter(cmd.CommandText, conn);
        //                        myoadapt.Fill(m_DiagDataSource);
        //                    }
        //                }
        //                catch (Exception)//出异常就取HIS的
        //                {
        //                    m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
        //                }

        //            }

        //        }
        //        ProcLoadData();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        private void LoadData()
        {
            try
            {
                //  DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题

                m_DiagName = "";
                m_DiagICD = "";
                if (m_DiagTypeName.IndexOf("中医") >= 0)
                {
                    m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
                }
                else
                {
                    m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
                }
                ProcLoadData();

                //  DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;//解决第三方控件异步报错的问题
            }
            catch (Exception ex)
            { }
        }
        private void ProcLoadData()
        {
            try
            {
                m_DiagName = "";
                m_DiagICD = "";
                // gridControlDiag.BeginUpdate();
                SetControlPropertyValue(gridControlDiag, "DataSource", m_DiagDataSource);
                //gridControlDiag.DataSource = m_DiagDataSource;
                //gridControlDiag.EndUpdate();
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                // textEditInput.Text = "";
                SetControlPropertyValue(textEditInput, "Text", "");
                m_DiagDataSource.DefaultView.RowFilter = "";
            }
            catch (Exception ex)
            { }

        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        //private void ProcLoadData()
        //{
        //    try
        //    {
        //        m_DiagName = "";
        //        m_DiagICD = "";
        //        gridControlDiag.BeginUpdate();
        //        gridControlDiag.DataSource = m_DiagDataSource;
        //        gridControlDiag.EndUpdate();
        //        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
        //        textEditInput.Text = "";
        //        m_DiagDataSource.DefaultView.RowFilter = "";
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region EventHandler
        private void DictionaryDataSerach_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_DiagDataSource == null)
                {
                    (new DeleteLoadData(LoadData)).BeginInvoke(null, null);
                }
                else
                {
                    ProcLoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 输入文本框值改变s
        /// edit by ywk 2013年1月11日11:45:54
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filter = string.Empty;
                if (checkEditPY.Checked)
                {
                    filter += "OR " + string.Format(PYFilter, textEditInput.Text.Trim().Replace("[", " ").Replace("]", "   ").Replace("*", " ").Replace("%", " ").Replace("'", " "));
                }
                if (checkEditWB.Checked)
                {
                    filter += "OR " + string.Format(WBFilter, textEditInput.Text.Trim().Replace("[", " ").Replace("]", "   ").Replace("*", " ").Replace("%", " ").Replace("'", " "));
                }
                if (checkEditName.Checked)
                {
                    filter += "OR " + string.Format(NameFilter, textEditInput.Text.Trim().Replace("[", " ").Replace("]", "   ").Replace("*", " ").Replace("%", " ").Replace("'", " "));
                }
                if (checkEditICD.Checked)
                {
                    filter += "OR " + string.Format(ICDFilter, textEditInput.Text.Trim().Replace("[", " ").Replace("]", "   ").Replace("*", " ").Replace("%", " ").Replace("'", " "));
                }
                //filter = filter.Replace("[", "").Replace("]", " ").Replace("*", "").Replace("%", "")
                //        .Replace("[[ ", "").Replace(" ]]", "").Replace("'", "");
                //textEditInput.Text = textEditInput.Text.Replace("[", "").Replace("]", " ").Replace("*", "").Replace("%", "")
                //         .Replace("[[ ", "").Replace(" ]]", "").Replace("'", "");
                if (m_DiagDataSource != null)
                {
                    //如果以上复选框都不选择就默认按照的是拼音检索，以保证不出错edit by ywk 2012年9月20日 15:15:21
                    filter += "OR " + string.Format(PYFilter, textEditInput.Text.Trim().Replace("[", " ").Replace("]", "   ").Replace("*", " ").Replace("%", " ").Replace("'", " "));
                    m_DiagDataSource.DefaultView.RowFilter = filter.Substring(2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridControlDiag_MouseUp(object sender, MouseEventArgs e)
        {
        }
        /// <summary>
        /// 按钮按下事件
        /// edit by ywk 2013年1月11日11:46:19 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditInput_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    gridControlDiag.Focus();
                    gridViewDiag.FocusedRowHandle = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkEditPY_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textEditInput.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkEditWB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textEditInput.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkEditName_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textEditInput.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkEditICD_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textEditInput.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 双击选择
        /// edit by ywk 2013年1月11日11:46:48
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlDiag_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRowView dr = gridViewDiag.GetRow(gridViewDiag.FocusedRowHandle) as DataRowView;
                if (dr != null)
                {
                    m_DiagICD = dr["icd"].ToString();
                    m_DiagName = dr["name"].ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void gridViewDiag_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //YD_Common.AutoIndex(e);
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}