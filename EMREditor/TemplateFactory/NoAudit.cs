using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class NoAudit : DevBaseForm
    {
        IEmrHost m_app;
        SQLManger m_SqlManger;

        public bool IsClose = false;//声明审核完成后，刷新右侧的模板列表
        public ArrayList TempleteID = new ArrayList();//选择的模板
        /// <summary>
        /// 双击选中的模版
        /// </summary> 
        public string TempletID;

        public NoAudit(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void NoAudit_Load(object sender, EventArgs e)
        {

            m_SqlManger = new SQLManger(m_app);
            InitDepartment();
            btn_AuditNo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_AuditYes.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_LoadTemplet.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        /// <summary>
        /// 初始化科室
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                #region
                //string sql = string.Format(@"select DEPT_ID ID,DEPT_NAME NAME,py,wb from EMRDEPT a");
                //DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);
                DataTable Dept = m_SqlManger.GetDeptListByUser();

                Dept.Columns["ID"].Caption = "科室编码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
                btn_Query.Focus();

                //if (Dept.Rows.Count == 0)
                //    btn_Query.Enabled = false;
                #endregion
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, EventArgs e)
        {
            string deptid = lookUpEditorDepartment.CodeValue;
            DataTable dt = m_SqlManger.GetNoAuditList(deptid);
            gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 审核通过
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AuditYes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AuditYes();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 审核不通过
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AuditNo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AuditNo();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 审核通过
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_yes_Click(object sender, EventArgs e)
        {
            try
            {
                AuditYes();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 审核不通过
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_no_Click(object sender, EventArgs e)
        {
            try
            {
                AuditYes();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加载模板
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_load_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTemplet();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 审核通过
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        private void AuditYes()
        {
            try
            {
                string templetid = GetSelectTempletID();
                TempleteID.Add(templetid);//可审核多个，所以存储多个模板ID
                if (string.IsNullOrEmpty(templetid.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                string auditor = m_app.User.Id;
                string mess = m_SqlManger.AuditTemplet(templetid, auditor, "2");
                if (mess == "审核成功")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核成功");
                    btn_Query_Click(null, null);
                    IsClose = true;//用于传递到上级模板列表窗体 add by ywk 2012年6月18日 13:37:28
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(mess);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 审核不通过
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        private void AuditNo()
        {
            try
            {
                string templetid = GetSelectTempletID();
                if (string.IsNullOrEmpty(templetid.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                string auditor = m_app.User.Id;
                TempleteID.Add(templetid);//可审核多个，所以存储多个模板ID
                string mess = m_SqlManger.AuditTemplet(templetid, auditor, "3");
                if (mess == "审核成功")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核成功");
                    btn_Query_Click(null, null);
                    IsClose = true;
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow(mess);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取选中模版的templetID
        /// </summary>
        /// <returns></returns>
        private string GetSelectTempletID()
        {
            string templetid = "";
            if (gridView1.FocusedRowHandle < 0)
                return "";
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return "";
            if (foucesRow.IsNull("TEMPLET_ID"))
                return "";
            else
                templetid = foucesRow["TEMPLET_ID"].ToString();
            return templetid;
        }

        /// <summary>
        /// 右键事件
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、小标题右键无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridView1.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (e.Button == MouseButtons.Right)
                {
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    btn_AuditNo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btn_AuditYes.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btn_LoadTemplet.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击查看模版信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            LoadTemplet();
        }

        /// <summary>
        /// 加载模板
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadTemplet()
        {
            try
            {
                string templetid = GetSelectTempletID();
                if (string.IsNullOrEmpty(templetid.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;

                }
                TempletID = templetid;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键菜单加载模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadTemplet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadTemplet();
        }

        /// <summary>
        /// 根据当前登录人权限 已经选择的模版科室 判断是否能够修改当前病历
        /// </summary>
        /// <param name="emrtemplet"></param>
        private bool SetDocumentModel(string deptid)
        {
            string[] config = m_SqlManger.GetTempletAuditConfig();
            if (config.Length != 3)
                return false;
            else
            {
                DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                string jobid = dt.Rows[0][0].ToString();

                //判断如果登陆人有管理员权限 则全部可以修改
                if (CheckJobInConfig(jobid, config[1]))
                {
                    return true;
                }
                //如果有模板管理，主任医生权限 则可以修改当前科室模版
                else if (CheckJobInConfig(jobid, config[2]))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (deptid == dr["deptid"].ToString())
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断角色是否在配置的角色中
        /// </summary>
        /// <param name="jobid"></param>
        /// <param name="configjob"></param>
        /// <returns></returns>
        private bool CheckJobInConfig(string jobid, string configjob)
        {
            string[] configjobs = configjob.Split(',');

            foreach (string cng in configjobs)
            {
                if (jobid.IndexOf(cng) > -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

    }
}