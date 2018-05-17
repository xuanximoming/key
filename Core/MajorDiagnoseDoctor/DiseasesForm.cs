using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using  DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
using DrectSoft.Service;
using DrectSoft.DSSqlHelper;
using System.Data.Common;
using System.Data.SqlClient;

namespace DrectSoft.Core.MajorDiagnoseDoctor
{
    public partial class DiseasesForm :DevBaseForm
    {
        IEmrHost m_app;
        //组合ID(新增：-1；其他：编辑)
        public int groupID = -1;
        //全部数据集(未勾选)
        private DataTable allSource = new DataTable();
        //初始化数据集
        private DataTable defaultSource = new DataTable();
        //已勾选和未勾选的数据集
        private List<DataRow> checkedList = new List<DataRow>();
        private List<DataRow> notCheckedList = new List<DataRow>();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <param name="App"></param>
        /// <param name="groupid"></param>
        public DiseasesForm(IEmrHost App,int groupid)
        {
            try
            {
                InitializeComponent();
                m_app = App;
                groupID = groupid;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiseasesForm_Load(object sender, EventArgs e)
        {
            try
            {
                //初始化病种列表数据
                InitData();
                //设置未勾选数据集
                SetCheckFlag(allSource,false);

                //初始化连续新增复选框
                if (groupID == -1)
                {
                    this.chb_continue.Visible = true;
                    this.btn_reset.Visible = false;
                    this.Text = "病种组合新增";
                }
                else
                {
                    this.chb_continue.Visible = false;
                    this.btn_reset.Visible = true;
                    this.Text = "病种组合编辑";
                }
                this.chb_continue.Checked = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region 事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-04</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                List<DbParameter> parameters = new List<DbParameter>();
                //名称、拼音、五笔
                string groupName = this.txt_groupName.Text;
                if (string.IsNullOrEmpty(groupName))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("组合名称不能为空");
                    return;
                }
                GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                string[] code = shortCode.GenerateStringShortCode(groupName);
                SqlParameter param1 = new SqlParameter("@name", SqlDbType.Char);
                SqlParameter param2 = new SqlParameter("@py", SqlDbType.Char);
                SqlParameter param3 = new SqlParameter("@wb", SqlDbType.Char);
                param1.Value = groupName;
                parameters.Add(param1);
                if (null != code && code.Length >= 2)
                {
                    param2.Value = null == code[0] ? string.Empty : code[0].ToString();
                    param3.Value = null == code[1] ? string.Empty : code[1].ToString();
                    parameters.Add(param2);
                    parameters.Add(param3);
                }
                //病种IDs
                string IDs = string.Join("$",checkedList.Select(p => p["ICD"].ToString()).ToArray());
                if (string.IsNullOrEmpty(IDs))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请在列表中勾选组合病种");
                    return;
                }
                SqlParameter param4 = new SqlParameter("@diseaseids", SqlDbType.Char);
                param4.Value = IDs;
                parameters.Add(param4);
                //是否有效
                SqlParameter param5 = new SqlParameter("@valid", SqlDbType.Int);
                param5.Value = 1;
                parameters.Add(param5);
                if (groupID == -1)
                {//新增
                    //创建人
                    SqlParameter param6 = new SqlParameter("@create_user", SqlDbType.Char);
                    param6.Value = DS_Common.currentUser.Id;
                    parameters.Add(param6);
                    //创建时间
                    SqlParameter param7 = new SqlParameter("@create_time", SqlDbType.Char);
                    param7.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param7);
                }
                else
                {//编辑
                    //更新人
                    SqlParameter param8 = new SqlParameter("@updateuser", SqlDbType.Char);
                    param8.Value = DS_Common.currentUser.Id;
                    parameters.Add(param8);
                    //更新时间
                    SqlParameter param9 = new SqlParameter("@updatetime", SqlDbType.Char);
                    param9.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param9);
                }
                //备注
                if (!string.IsNullOrEmpty(this.txt_memo.Text.Trim()))
                {
                    SqlParameter param10 = new SqlParameter("@memo", SqlDbType.Char);
                    param10.Value = this.txt_memo.Text;
                    parameters.Add(param10);
                }

                //保存（插入、更新）组合记录
                if (groupID == -1)
                {
                    int result = DS_SqlService.InsertDiseaseGroup(parameters);
                    if (result == 1)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("新增成功");
                    }
                }
                else
                {
                    SqlParameter param10 = new SqlParameter("@id", SqlDbType.Int);
                    param10.Value = groupID;
                    parameters.Add(param10);
                    int result = DS_SqlService.UpdateDiseaseGroup(parameters);
                    if (result == 1)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("编辑成功");
                    }
                }

                if (this.chb_continue.Checked)
                {//连续新增
                    groupID = -1;
                    ClearPage();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region 已注释 by cyq 2013-01-06 不够及时反应(需当前单元格失去焦点才触发此事件)
        /// <summary>
        /// 勾选事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void gridView_disease_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Column.FieldName == "FLAG")
        //        {
        //            DataRow row = gridView_disease.GetDataRow(e.RowHandle);
        //            if (null == row || null == row["FLAG"])
        //            {
        //                return;
        //            }

        //            if (Boolean.Parse(row["FLAG"].ToString()) == true)
        //            {
        //                if (notCheckedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
        //                {
        //                    notCheckedList.Remove(row);
        //                }
        //                if (!checkedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
        //                {
        //                    checkedList.Add(row);
        //                }
        //                checkedList = checkedList.OrderBy(p => p["NAME"]).ToList();
        //            }
        //            else
        //            {
        //                if (checkedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
        //                {
        //                    checkedList.Remove(row);
        //                }
        //                if (!notCheckedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
        //                {
        //                    notCheckedList.Add(row);
        //                }
        //                notCheckedList = notCheckedList.OrderBy(p => p["NAME"]).ToList();
        //            }
        //            RefreshDiseaseTextArea();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 筛选事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 勾选事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_disease_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName != "FLAG")
                {
                    return;
                }
                DataRow row = gridView_disease.GetDataRow(e.RowHandle);
                if (null == row || null == row["FLAG"])
                {
                    return;
                }

                if (Boolean.Parse(row["FLAG"].ToString()) == false)//勾选操作
                {
                    if (notCheckedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
                    {
                        DataRow theRow = notCheckedList.FirstOrDefault(p => p["MARKID"].ToString() == row["MARKID"].ToString());
                        if (null != theRow)
                        {
                            notCheckedList.Remove(theRow);
                        }
                    }
                    if (!checkedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
                    {
                        row["FLAG"] = true;
                        checkedList.Add(row);
                    }
                }
                else//反勾选操作
                {
                    if (checkedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
                    {
                        DataRow theRow = checkedList.FirstOrDefault(p => p["MARKID"].ToString() == row["MARKID"].ToString());
                        if (null != theRow)
                        {
                            checkedList.Remove(theRow);
                        }
                    }
                    if (!notCheckedList.Any(p => p["MARKID"].ToString() == row["MARKID"].ToString()))
                    {
                        row["FLAG"] = false;
                        notCheckedList.Add(row);
                    }
                }
                RefreshDiseaseTextArea();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPage();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框按键事件
        /// 注：回车即勾选/不勾选
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-29</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.cbx_KeyPress(sender);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号 
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_disease_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化数据 --- 病种列表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        private void InitData()
        {
            try
            {
                //获取所有病种列表
                DataTable dt = DS_SqlService.GetDiagnosis();
                dt.Columns.Add(new DataColumn("FLAG",typeof(bool)));
                DataRow[] listArry = dt.Select(" 1=1 ");
                if (allSource.Rows.Count == 0)
                {
                    allSource = listArry.Length == 0 ? new DataTable() : listArry.CopyToDataTable();
                }
                string searchStr = this.txt_search.Text.ToUpper();

                if (groupID == -1)
                {//新增
                    foreach (DataRow dr in listArry)
                    {
                        dr["FLAG"] = false;
                    }
                    notCheckedList = listArry.ToList();
                }
                else
                {//编辑
                    DataTable thisGroupDt = DS_SqlService.GetDiseaseGroupByID(groupID);
                    if (null == thisGroupDt || thisGroupDt.Rows.Count == 0)
                    {
                        return;
                    }
                    this.txt_groupName.Text = null == thisGroupDt.Rows[0]["NAME"] ? "" : thisGroupDt.Rows[0]["NAME"].ToString();
                    this.txt_memo.Text = null == thisGroupDt.Rows[0]["MEMO"] ? "" : thisGroupDt.Rows[0]["MEMO"].ToString();
                    List<string> idList = thisGroupDt.Rows[0]["diseaseids"].ToString().Split('$').ToList();
                    checkedList.Clear();
                    notCheckedList.Clear();

                    //已勾选项 不过滤
                    var checkedEnu = listArry.Where(p => null != p["ICD"] && idList.Contains(p["ICD"].ToString())).OrderBy(q => q["NAME"]);
                    foreach (DataRow dr in checkedEnu)
                    {
                        dr["FLAG"] = true;
                        checkedList.Add(dr);
                    }
                    //对未勾选项进行过滤
                    var notCheckedEnu = listArry.Where(p => null != p["ICD"] && !idList.Contains(p["ICD"].ToString()) && (p["ICD"].ToString().Contains(searchStr) || p["NAME"].ToString().Contains(searchStr) || p["PY"].ToString().Contains(searchStr) || p["WB"].ToString().Contains(searchStr))).OrderBy(q => q["NAME"]);
                    foreach (DataRow dr in notCheckedEnu)
                    {
                        dr["FLAG"] = false;
                        notCheckedList.Add(dr);
                    }
                    RefreshDiseaseTextArea();
                }
                DataTable endDt = checkedList.Union(notCheckedList).CopyToDataTable();
                defaultSource = endDt.Copy();
                this.gridControl_disease.DataSource = endDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 过滤数据 --- 病种列表
        /// </summary>
        /// 注：已勾选项 不过滤
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        private void SearchData()
        {
            try
            {
                string searchStr = string.IsNullOrEmpty(this.txt_search.Text) ? "" : this.txt_search.Text.ToUpper().Replace("'","''");
                //过滤后的所有病种数据集
                List<DataRow> fillterList = allSource.Select(" 1=1 ").Where(p => p["ICD"].ToString().ToUpper().Contains(searchStr) || p["NAME"].ToString().ToUpper().Contains(searchStr) || p["PY"].ToString().ToUpper().Contains(searchStr) || p["WB"].ToString().ToUpper().Contains(searchStr)).OrderBy(q => q["NAME"]).ToList();
                //除去已勾选的数据集
                if (null != fillterList && fillterList.Count() > 0)
                {
                    foreach (DataRow drow in checkedList)
                    {
                        if (fillterList.Any(p => p["MARKID"].ToString() == drow["MARKID"].ToString()))
                        {
                            fillterList.Remove(drow);
                        }
                    }
                }
                //剩余数据集为过滤后的未勾选数据集
                notCheckedList = fillterList;
                //设置gridview数据集
                List<DataRow> unionList = checkedList.Union(notCheckedList).ToList();
                this.gridControl_disease.DataSource = (null == unionList || unionList.Count() == 0) ? new DataTable() : unionList.CopyToDataTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-04</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txt_groupName.Text.Trim()))
                {
                    this.txt_groupName.Focus();
                    return "组合名称不能为空";
                }
                else if (checkedList.Count == 0)
                {
                    this.gridView_disease.Focus();
                    return "请在列表中勾选组合病种";
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刷新组合病种展示区域
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        private void RefreshDiseaseTextArea()
        {
            try
            {
                if (null == checkedList || checkedList.Count() == 0)
                {
                    this.txt_diseaseArea.Text = string.Empty;
                    return;
                }
                string content = string.Empty;
                foreach (DataRow drow in checkedList)
                {
                    if (string.IsNullOrEmpty(drow["FLAG"].ToString()) || bool.Parse(drow["FLAG"].ToString()) == false)
                    {
                        continue;
                    }
                    content += drow["NAME"] + "(" + drow["ICD"] + ")，";
                }
                if (!string.IsNullOrEmpty(content))
                {
                    content = content.Substring(0, content.Length - 1);
                }
                this.txt_diseaseArea.Text = content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        private void Reset()
        {
            try
            {
                if (groupID == -1)
                {
                    ClearPage();
                }
                else
                {
                    this.txt_search.Text = string.Empty;
                    DataTable thisGroupDt = DS_SqlService.GetDiseaseGroupByID(groupID);
                    if (null == thisGroupDt || thisGroupDt.Rows.Count == 0)
                    {
                        return;
                    }
                    this.txt_groupName.Text = null == thisGroupDt.Rows[0]["NAME"] ? "" : thisGroupDt.Rows[0]["NAME"].ToString();
                    this.txt_memo.Text = null == thisGroupDt.Rows[0]["MEMO"] ? "" : thisGroupDt.Rows[0]["MEMO"].ToString();
                    this.gridControl_disease.DataSource = defaultSource;
                    checkedList = defaultSource.Select(" FLAG = " + true).ToList();
                    notCheckedList = defaultSource.Select(" FLAG = " + false).ToList();
                    RefreshDiseaseTextArea();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        private void ClearPage()
        {
            try
            {
                this.txt_search.Text = string.Empty;
                this.txt_groupName.Text = string.Empty;
                this.txt_diseaseArea.Text = string.Empty;
                this.txt_memo.Text = string.Empty;
                if (groupID == -1)
                {
                    this.chb_continue.Checked = false;
                }
                if (allSource.Select(" 1=1 ").Any(p => string.IsNullOrEmpty(p["FLAG"].ToString()) || bool.Parse(p["FLAG"].ToString()) == true))
                {
                    SetCheckFlag(allSource,false);
                }
                this.gridControl_disease.DataSource = allSource;
                checkedList.Clear();
                notCheckedList = allSource.Select(" 1=1 ").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置勾选状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="dt"></param>
        private void SetCheckFlag(DataTable dt,bool flag)
        {
            try
            {
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                var notFlagRows = dt.Select(" 1=1 ").Where(p => string.IsNullOrEmpty(p["FLAG"].ToString()) || bool.Parse(p["FLAG"].ToString()) != flag);
                foreach (DataRow drow in notFlagRows)
                {
                    drow["FLAG"] = flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        

    }
}