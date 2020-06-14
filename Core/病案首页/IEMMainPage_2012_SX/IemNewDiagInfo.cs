using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class IemNewDiagInfo : DevBaseForm
    {
        #region field && Property

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

        public IemNewDiagInfo(IEmrHost app, DataTable foreTable, string gotype, string diagTypeName, string inputText)
            : this()
        {
            m_App = app;
            m_DiagTypeName = diagTypeName;
            m_GoType = gotype;
            inText = inputText;//m_inputText
            INDData = foreTable;
        }

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
                    }
                    //ProcLoadData();
                }
                else
                    if (m_GoType == "MZDIAG" || m_GoType == "RYZDIAG" || m_GoType == "YGDIAG")
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
                    else if (m_GoType == "RYZDIAG")
                    {
                        this.Text = "入院诊断";
                    }
                    else if (m_GoType == "operate")
                    {
                        this.Text = "手术诊断";
                    }
                }

                checkEditPY_CheckedChanged(sender, e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                }
                else//(m_DiagDataSource != null)
                {
                    m_DiagName = inText;// m_inputText;
                    if (!string.IsNullOrEmpty(m_DiagName) == true)
                    {
                        inText = m_DiagName;//m_inputText
                        m_isClosed = true;
                    }
                    else
                    {
                        inText = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
            catch (Exception)
            {
                throw;
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void FormClose()
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}


