using DevExpress.XtraGrid.Columns;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.QCReport.controls;
using DrectSoft.DrawDriver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
namespace DrectSoft.Core.QCReport
{
    public partial class Reportboard : UserControl
    {
        #region 事件

        public event EventHandler eh;


        public delegate void del_IniDataFieldValue(ref Dictionary<string, ParamObject> dic);
        public event del_IniDataFieldValue IniDataFieldValueHandle;
        private void OnIniDataFieldValueHandle(ref Dictionary<string, ParamObject> dic)
        {
            if (IniDataFieldValueHandle != null)
            {
                IniDataFieldValueHandle(ref dic);
            }
        }

        //private void OnHandle(object obj,EventArgs e)
        //{
        //    if (eh != null)
        //    {
        //        eh(obj,e);
        //    }
        //}
        #endregion

        private static XmlOperate xmlWork = null;
        private SqlQuery sqlQuery = null;
        private string reportName = string.Empty;//报表名称
        private List<DataColumn> columns;
        private DataTable dt = null;
        string fileName;
        int rowcount;
        TempEntity tempEntity = new TempEntity();

        /// <summary>
        /// 除dt以外的其他参数传值
        /// </summary>
        public Dictionary<string, ParamObject> paramList = new Dictionary<string, ParamObject>();
        public Reportboard(string path)
        {
            try
            {
                InitializeComponent();
                xmlWork = new XmlOperate(path);
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
            }
            catch
            {
            }

        }
        /// <summary>
        /// 根据配置文件描述动态创建报表窗体界面
        /// </summary>
        /// <param name="_reportName"></param>
        public void LoadReport(string _reportName)
        {
            try
            {

                reportName = _reportName;
                flowLayoutPanel1.Controls.Clear();
                sqlQuery = xmlWork.GetReportParam(reportName);
                Control ctl = null;
                foreach (ParamInfo param in sqlQuery.paramList)
                {
                    switch (param.controltype.ToLower().Trim())
                    {
                        case "mycomboboxdept":
                            ctl = new MyComboBoxDept();
                            break;
                        case "mydateedit":
                            ctl = new MyDateEdit();
                            break;
                        case "mydutydoctor":
                            ctl = new MyDutyDoctor();
                            break;
                        case "myqctype":
                            ctl = new MyQCType();
                            break;
                        case "mydia":
                            ctl = new Mydia();
                            break;
                        case "mydateeditbegin":
                            ctl = new MyDateEditBegin();
                            break;
                        //新增的输入年份控件 add by ywk 2013年8月1日 17:28:48
                        case "inputyear":
                            ctl = new InputYear();
                            break;
                        case "inputmonth":
                            ctl = new InputMonth();
                            break;
                    }
                    ctl.Name = param.name;
                    MyLabel lb = new MyLabel();
                    lb.AutoSize = false;
                    lb.Text = param.controlcaption;
                    flowLayoutPanel1.Controls.Add(lb);
                    flowLayoutPanel1.Controls.Add(ctl);
                }
                BindControlData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 给具有数据字典绑定功能的控件赋值，如下拉列表
        /// </summary>
        public void BindControlData()
        {
            try
            {
                foreach (Control ctl in flowLayoutPanel1.Controls)
                {
                    (ctl as IControlDataInit).InitControlBindData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建列表数据列
        /// </summary>
        private void CreateGridControlColumns()
        {
            try
            {
                gridView1.Columns.Clear();
                columns = xmlWork.GetColumns(reportName);
                GridColumn gridColumn = null;
                for (int i = 0; i < columns.Count; i++)
                {
                    gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
                    gridColumn.Caption = columns[i].caption;
                    gridColumn.FieldName = columns[i].datafield;
                    gridColumn.Width = Convert.ToInt32(columns[i].width);
                    gridColumn.Visible = true;
                    gridColumn.VisibleIndex = i;
                    this.gridView1.Columns.Add(gridColumn);
                    gridColumn.OptionsFilter.AllowAutoFilter = false;
                    gridColumn.OptionsFilter.AllowFilter = false;
                    gridColumn.OptionsFilter.ImmediateUpdateAutoFilter = false;
                }
                this.gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonQurey1_Click(object sender, EventArgs e)
        {
            try
            {
                CreateGridControlColumns();
                string procedure = sqlQuery.sqlStr;//存储过程名称
                SqlParameter[] sqlParam = new SqlParameter[sqlQuery.paramList.Count + 1];
                for (int i = 0; i < sqlQuery.paramList.Count; i++)
                {
                    Control ctl = FindControl(flowLayoutPanel1, sqlQuery.paramList[i].name);
                    if (ctl == null)
                    {
                        MyMessageBox.Show(1, "查询条件取值失败");
                        return;
                    }
                    sqlParam[i] = new SqlParameter("@" + sqlQuery.paramList[i].name, (ctl as IControlDataInit).Value.Trim().ToString());
                }
                sqlParam[sqlParam.Length - 1] = new SqlParameter("@result", SqlDbType.Structured);
                sqlParam[sqlParam.Length - 1].Direction = ParameterDirection.Output;
                dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(procedure, sqlParam, CommandType.StoredProcedure);

                gridControl1.DataSource = dt;

                paramList.Clear();
                //把查询条件作为Key-Value对
                foreach (Control ctl in flowLayoutPanel1.Controls)
                {
                    if (ctl is MyLabel) continue;
                    if (paramList.ContainsKey(ctl.Name))
                    {
                        continue;
                    }
                    paramList.Add(ctl.Name, new ParamObject(ctl.Text, "text", (ctl as IControlDataInit).Value.Trim().ToString()));
                }
                //OnHandle(null, null);
                OnIniDataFieldValueHandle(ref paramList);
                int allcount = 0;
                if (dt != null && dt.Rows != null)
                {
                    allcount = dt.Rows.Count;
                }
                lblCount.Text = "共" + allcount + "条记录";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonPrint1_Click(object sender, EventArgs e)
        {


            if (dt == null || dt.Rows.Count <= 0)
            {
                MessageBox.Show("数据为空");
                return;
            }
            tempEntity.AllCount = dt.Rows.Count.ToString();
            tempEntity.HosInfo = DrectSoft.Service.DS_BaseService.GetHosPatInfo();
            tempEntity.ReportName = reportName;
            XmlNode xnode = xmlWork.GetPrintSetting(reportName);
            fileName = xnode.Attributes["filename"].Value;
            string fileNameCopy = fileName.Substring(0, fileName.LastIndexOf(".")) + "1.xml";
            rowcount = Convert.ToInt32(xnode.Attributes["rowcount"].Value);

            List<Metafile> metaList = new List<Metafile>();
            int pageCount = (dt.Rows.Count + rowcount - 1) / rowcount;  //总页数
            for (int i = 0; i < pageCount; i++)
            {
                tempEntity.PageIndex = (i + 1).ToString();
                XmlCommomOp.Doc = null;
                XmlCommomOp.CopyTempalteXmlFile(AppDomain.CurrentDomain.BaseDirectory + fileName, AppDomain.CurrentDomain.BaseDirectory + fileNameCopy);
                XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + fileNameCopy;
                XmlCommomOp.CreaetDocument();
                //XmlCommomOp.BindingDate(CreateDataSet(i), CreateDate());
                //OnIniDataFieldValueHandle(ref paramList);
                CreateDate();
                XmlCommomOp.BindingDate(CreateDataSet(i), paramList);

                metaList.Add(DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc)[0]);
            }
            DrawOp.PrintView(metaList);
            if (File.Exists(XmlCommomOp.xmlPath))
            {
                File.Delete(XmlCommomOp.xmlPath);
            }

        }

        private DataSet CreateDataSet(int i)
        {
            DataSet dataSet = new DataSet();
            DataTable dtNew = dt.Clone();
            for (int j = 0; j < rowcount; j++)
            {
                //if (10 * i + j < dt.Rows.Count)
                //{
                //    dtNew.ImportRow(dt.Rows[10 * i + j]);
                //}
                //edit by ywk 2013年8月3日 15:41:04
                if (i * rowcount + j < dt.Rows.Count)
                {
                    dtNew.ImportRow(dt.Rows[i * rowcount + j]);
                }
                else
                {
                    dtNew.Rows.Add(dtNew.NewRow());
                }
            }
            dtNew.TableName = "DateTable";
            dataSet.Tables.Add(dtNew);
            return dataSet;
        }



        /// <summary>
        /// 构造参数集合
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ParamObject> CreateDate()
        {
            try
            {
                if (paramList == null)
                {
                    paramList = new Dictionary<string, ParamObject>();
                }
                PropertyInfo[] propertys = tempEntity.GetType().GetProperties();
                foreach (PropertyInfo item in propertys)
                {
                    ParamObject param = new ParamObject(item.Name, "", item.GetValue(tempEntity, null) == null ? "" : item.GetValue(tempEntity, null).ToString());
                    if (!paramList.ContainsKey(item.Name))
                    {

                        paramList.Add(item.Name, param);
                    }
                    else
                    {
                        paramList[item.Name] = param;
                    }

                }
                return paramList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonReset1_Click(object sender, EventArgs e)
        {
            try
            {
                string procedure = sqlQuery.sqlStr;//存储过程名称
                for (int i = 0; i < sqlQuery.paramList.Count; i++)
                {
                    Control ctl = FindControl(flowLayoutPanel1, sqlQuery.paramList[i].name);
                    if (ctl == null)
                    {
                        return;
                    }
                    (ctl as IControlDataInit).Reset();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <param name="parentControl">父控件容器</param>
        /// <param name="controlName">子控件名称</param>
        /// <returns></returns>
        private Control FindControl(Control parentControl, string controlName)
        {
            try
            {
                foreach (Control ctl in parentControl.Controls)
                {
                    if (ctl.Name.Equals(controlName))
                    {
                        return ctl;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DevButtonImportExcel1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    options.SheetName = "统计信息";
                    options.ShowGridLines = true;

                    string caption = gridControl1.MainView.ViewCaption;
                    gridControl1.MainView.ViewCaption = options.SheetName;
                    gridControl1.ExportToXls(saveFileDialog.FileName, options);
                    MessageBox.Show("导出成功");
                    gridControl1.MainView.ViewCaption = caption;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        public void ClearGridData()
        {
            try
            {
                this.gridControl1.DataSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
            this.gridView1.IndicatorWidth = 30;
        }

    }
}
