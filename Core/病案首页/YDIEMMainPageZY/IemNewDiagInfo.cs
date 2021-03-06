﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Xml;
using DrectSoft.Common.Ctrs.DLG;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 诊断类型界面
    /// </summary>
    public partial class IemNewDiagInfo : DevBaseForm
    {
        #region field && Property
        /// <summary>
        ///  西医所有诊断
        /// </summary>
        //private const string SqlAllDiag = @"select markid, icd , mapid, shandardcode, name, py, wb" +
        //    " tumorid, statist, innercategory, categroy, othercategroy, valid, memo" +
        //    " from diagnosis where valid='1'";
        //private const string SqlAllDiag = @"select markid, icd , mapid, name, py, wb," +
        //   "  valid, memo" +
        //   " from diagnosis where valid='1'";
        //private const string SqlAllDiag = @"select py, wb, name, icd from diagnosis where valid='1'";
        //private const string SqlAllOper = @"select py, wb, name, id icd from operation where valid='1'";
        //private const string SqlAllDiagMZXIYI = @"select  py, wb, name, icd from diagnosis where valid='1'";

        //private const string SqlInsertDiag = @"insert into diagnosis(name,icd) values ({0},null)  where valid='1'";

        //private const string SqlPatDiagType = " SELECT code, diagname, typeid FROM patdiagtype ";

        //private const string SqlAllDiagChinese = @"select id icd, name, py, wb from diagnosisofchinese where valid='1'";
        //private const string SqlAllDaigMZChinese = @"select id icd , name, py, wb from diagnosisofchinese where valid='1'";

        //private const string SqlAllDoctor = @"SELECT py,wb, NAME,ID icd FROM users WHERE valid = 1 ORDER BY icd";

        private const string PYFilter = " py like '%{0}%'";
        private const string WBFilter = " wb like '%{0}%'";
        private const string NameFilter = "name like '%{0}%'";
        private const string ICDFilter = "ICD like '%{0}%'";

        private IEmrHost m_App;
        //private DataTable m_DiagDataSource;

        private string m_DiagName = string.Empty;
        private string m_DiagICD = string.Empty;

        private string m_MZDiagName = string.Empty;
        private string m_MZDiagICD = string.Empty;

        private string m_OperName = string.Empty;
        private string m_OperCode = string.Empty;

        delegate void DeleteLoadData();
        private bool m_isClosed = false;
        public bool IsClosed
        {
            get { return m_isClosed; }
            set { m_isClosed = value; }
        }

        public string inText
        {
            get { return m_inputText; }
            set { m_inputText = value; }
        }

        public string inCode
        {
            get { return m_DiagICD; }
            set { m_DiagICD = value; }
        }
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
        /// 选中的门诊诊断名称
        /// </summary>
        public string MZDiagName
        {
            get
            {
                return m_MZDiagName;
            }
        }

        /// <summary>
        /// 与选中的门诊名称相对应的门诊诊断编码
        /// </summary>
        public string MZDiagICD
        {
            get
            {
                return m_MZDiagICD;
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
        /// 选中的手术诊断的名称
        /// </summary>
        public string OperName
        {
            get
            {
                return m_OperName;
            }
        }

        /// <summary>
        /// 选中的手术的ICD编码
        /// </summary>
        public string OperICD
        {
            get
            {
                return m_OperCode;
            }
        }

        private string m_DiagTypeName = string.Empty;//诊断的类型
        private string m_GoType = string.Empty;//大类别的类型
        private string m_inputText = string.Empty;//文本框的内容

        private DataTable INDData;
        #endregion

        #region .ctor
        public IemNewDiagInfo()
        {
            InitializeComponent();
        }

        public IemNewDiagInfo(IEmrHost app,DataTable foreTable,string gotype, string diagTypeName, string inputText)
            : this()
        {
            m_App = app;
            m_DiagTypeName = diagTypeName;
            m_GoType = gotype;
            inText = inputText;//m_inputText
            INDData = foreTable;
        }

        //UCIemBasInfo ucibaseInfo= new UCIemBasInfo(

        //    public IemNewDiagInfo(IEmrHost app, string gotype, string diaType, string inputText)
        //        : this()
        //    {
        //}

        #endregion

        #region Method

        /// <summary>
        /// 获取数据源怎么绑定数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
                if (m_GoType == "OUTHOSDIAG")
                {

                    m_DiagName = "";
                    m_DiagICD = "";
                    if (!string.IsNullOrEmpty(m_DiagTypeName))
                    {
                        switch (m_DiagTypeName)
                        {
                            case "XIYI":
                                gridControlDiag.BeginUpdate();
                                gridControlDiag.DataSource = INDData;// 绑定数据
                                gridControlDiag.EndUpdate();
                                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                textEditInput.Text = inText;
                                //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);


                                //m_App.CustomMessageBox.MessageShow("XIYI");
                                //ProcLoadData();
                                // GetData(m_DiagTypeName, m_GoType);
                                //m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
                                break;
                            case "ZHONGYI":
                                gridControlDiag.BeginUpdate();
                                gridControlDiag.DataSource = INDData;// 绑定数据
                                gridControlDiag.EndUpdate();
                                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                textEditInput.Text = inText;
                                //m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
                                break;
                            default:
                                break;
                        }
                    }
                    //ProcLoadData();
                }
                else
                    if (m_GoType == "MZDIAG")
                    {
                        switch (m_DiagTypeName)
                        {
                            case "XIYI":
                                gridControlDiag.BeginUpdate();
                                gridControlDiag.DataSource = INDData;// 绑定数据
                                gridControlDiag.EndUpdate();
                                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                textEditInput.Text = inText;
                                break;
                            case "ZHONGYI":
                                gridControlDiag.BeginUpdate();
                                gridControlDiag.DataSource = INDData;// 绑定数据
                                gridControlDiag.EndUpdate();
                                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                textEditInput.Text = inText;
                                break;
                            default:
                                break;
                        }
                        // ProcLoadData();
                    }
                    else
                        if (m_GoType == "operate")
                        {
                            switch (m_DiagTypeName)
                            {
                                case "operate":
                                    gridControlDiag.BeginUpdate();
                                    gridControlDiag.DataSource = INDData;// 绑定数据
                                    gridControlDiag.EndUpdate();
                                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                    textEditInput.Text = inText;
                                    break;
                                case "anaesthetist":
                                    gridControlDiag.BeginUpdate();
                                    gridControlDiag.DataSource = INDData;// 绑定数据
                                    gridControlDiag.EndUpdate();
                                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
                                    textEditInput.Text = inText;
                                    break;
                                default:
                                    break;
                            }
                            // ProcLoadData();
                        }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获得搜索结果  王冀 2013 1 12
        /// </summary>
        /// <param name="m_DiagTypeName"></param>
        /// <param name="m_GoType"></param>
        //private void GetData(string m_DiagTypeName, string m_GoType)
        //{
        //    try
        //    {
        //        if (m_GoType == "OUTHOSDIAG")
        //        {
        //            if (m_DiagTypeName == "XIYI")
        //            {
        //                // gridControlDiag.BeginUpdate();
        //                DataTable dt = new DataTable();
        //                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
        //                //this.gridControlDiag.DataSource = 
        //                //gridControlDiag.EndUpdate();
        //                //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
        //                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;//解决第三方控件异步报错的问题
        //            }
        //            if (m_DiagTypeName == "ZHONGYI")
        //            {
        //                DataTable dt = new DataTable();
        //                m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
        //            }
        //        }
        //        else
        //            if (m_GoType == "MZDIAG")
        //            {
        //                if (m_DiagTypeName == "XIYI")
        //                {
        //                    DataTable dt = new DataTable();
        //                    m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
        //                }
        //                if (m_DiagTypeName == "ZHONGYI")
        //                {
        //                    DataTable dt = new DataTable();
        //                    m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);
        //                }
        //            }
        //            else
        //                if (m_GoType == "operate")
        //                {
        //                    if (m_DiagTypeName == "operate")
        //                    {
        //                        DataTable dt = new DataTable();
        //                        m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllOper, CommandType.Text);
        //                    }
        //                    if (m_DiagTypeName == "anaesthetist")
        //                    {
        //                        DataTable dt = new DataTable();
        //                        m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlAllDoctor, CommandType.Text);
        //                    }
        //                }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// 取数据
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="table"></param>
        //private DataTable GetData(Decimal queryType)
        //{
        //    SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
        //    paraType.Value = queryType;
        //    SqlParameter[] paramCollection = new SqlParameter[] { paraType };
        //    DataTable dataTable= AddTableColumn(m_App.SqlHelper.ExecuteDataTable(""))
        //}

        //private void ProcLoadData()
        //{

        //    try
        //    {
        //        m_DiagName = "";
        //        m_DiagICD = "";
        //        gridControlDiag.BeginUpdate();
        //        gridControlDiag.DataSource = m_DiagDataSource;// 绑定数据
        //        gridControlDiag.EndUpdate();
        //        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiag);
        //        textEditInput.Text = inText;
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show(ex.Message);
        //    }
        //}

        #endregion

        #region EnventHandler
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void IemNewDiagInfo_Load(object sender, EventArgs e)
        {
            try
            {
                if (INDData == null)
                {
                    LoadData();
                    //(new DeleteLoadData(LoadData)).BeginInvoke(null, null);
                }
                else
                {
                    LoadData();
                    if (m_GoType == "OUTHOSDIAG")
                    {
                        this.Text = "出院诊断";
                    }
                    else if (m_GoType == "MZDIAG")
                    {
                        this.Text = "门诊诊断";
                    }
                    else if (m_GoType == "operate")
                    {
                        this.Text = "手术诊断";
                    }
                   // ProcLoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textEditInput_TextChanged(object sender, EventArgs e)
        {

            try
            {
                string filter = string.Empty;
                filter += string.Format(PYFilter, textEditInput.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"));  //如果以上复选框°不选择就默认按照拼音检索，以保证不出错
                if (checkEditPY.Checked)
                {
                    filter += "or" + string.Format(PYFilter, textEditInput.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"));
                }
                if (checkEditWB.Checked)
                {
                    filter += " or " + string.Format(WBFilter, textEditInput.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"));
                }
                if (checkEditName.Checked)
                {
                    filter += " or " + string.Format(NameFilter, textEditInput.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"));
                }
                if (checkEditICD.Checked)
                {
                    filter += " or " + string.Format(ICDFilter, textEditInput.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"));
                }

                if (INDData != null)
                {
                    INDData.DefaultView.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void IemNewDiagInfo_FormClosed(object sender, FormClosedEventArgs e)
        {

            try
            {
                if (INDData == null)
                {
                    if (textEditInput.Text == null)
                    {
                        //MessageBox.Show("nihao");
                    }
                    //textEditInput.Text = inText;// m_inputText;
                    //m_DiagDataSource = m_App.SqlHelper.ExecuteDataTable(SqlInsertDiag, CommandType.Text);
                }
                else//(m_DiagDataSource != null)
                {
                    m_DiagName = inText;// m_inputText;
                    if (!string.IsNullOrEmpty(m_DiagName) == true)
                    {
                        inText = m_DiagName;//m_inputText
                        m_isClosed = true;
                    }
                    //if (m_DiagName != null)
                    //{
                    //    inText = m_DiagName;//m_inputText
                    //    m_isClosed = true;
                    //}
                    else
                    {
                        inText = string.Empty;//m_inputText
                        //m_isClosed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 是否有匹配值  有结果返回true，否则false
        /// </summary>
        /// <returns></returns>
        public bool GetFormResult()
        {
            try
            {
                LoadData();
                if (INDData == null)
                {
                    return false;//表示数据源里面没有相匹配的结果，用匹配的结果                    
                }
                string filter = string.Empty;
                int dataResult;
                if (checkEditPY.Checked)
                {
                    filter += string.Format(PYFilter, inText);//textEditInput.Text.Trim()
                }
                if (checkEditWB.Checked)
                {
                    filter += " or " + string.Format(WBFilter, inText);//textEditInput.Text.Trim()
                }
                if (checkEditName.Checked)
                {
                    filter += " or " + string.Format(NameFilter, inText);//textEditInput.Text.Trim()
                }
                if (checkEditICD.Checked)
                {
                    filter += " or " + string.Format(ICDFilter, inText);// textEditInput.Text.Trim()
                }
                INDData.DefaultView.RowFilter = filter;//m_DiagDataSource源数据有值如何获取数据源里面的值
                dataResult = INDData.DefaultView.ToTable().Rows.Count;
                if (dataResult > 0)
                {
                    return true;//表示数据源里面有相匹配的结果
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2013 1 10
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        private void FormClose()
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 键盘事件
        /// 支持回车键选中记录
        /// Add by xlb 2013-06-25
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlDiag_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //当前选中行
                DataRow dataRow = gridViewDiag.GetDataRow(gridViewDiag.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                if (e.KeyChar == 13)
                {
                    m_DiagICD = dataRow["icd"].ToString().Trim();
                    m_DiagName = dataRow["name"].ToString().Trim();
                    textEditInput.Text = m_DiagName;
                    inText = m_DiagName;
                    inCode = m_DiagICD;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDiag_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = gridViewDiag.GetDataRow(gridViewDiag.FocusedRowHandle) as DataRow;
                if (dr != null)
                {
                    m_DiagICD = dr["icd"].ToString().Trim();
                    m_DiagName = dr["name"].ToString().Trim();
                    textEditInput.Text = m_DiagName;
                    inText = m_DiagName;
                    inCode = m_DiagICD;
                }
                FormClose();
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        #endregion


    }
}


