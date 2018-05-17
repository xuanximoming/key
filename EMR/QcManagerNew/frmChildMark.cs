using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DevExpress.Utils;
using DrectSoft.Common;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class frmChildMark : DevBaseForm
    {
        string m_noofinpat = "";
        public frmChildMark(QcManagerNew.frmContainer parent, string noofinpat, IEmrHost app, string patstatus)
        {
            InitializeComponent();
            this.MdiParent = parent;
            m_noofinpat = noofinpat;
            m_App = app;
            m_patientstatus = patstatus;
            m_SqlManger = new SqlManger(app);
            InitUCPoint();
            this.ControlBox = false;
            //LoadEmrDocPoint();
        }
        IEmrHost m_App;
        SqlManger m_SqlManger;
        private void frmChildMark_Load(object sender, EventArgs e)
        {
            try
            {
                GetgridControlMarkList(m_noofinpat);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化病历评分界面
        /// </summary>
        private void InitUCPoint()
        {
            try
            {
                ucPointmark.InitData(m_App, m_SqlManger, m_noofinpat, m_chiefID);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void GetgridControlMarkList(string noofinpat)
        {
            try
            {
                string sql = string.Format(@"select er.id,er.name,er.create_time,(case er.qctype when '0' then '环节质控' when '1' then '终末质控' end) QCTYPE from emr_automark_record er where er.noofinpat='{0}' and er.isvalid='1' and er.isauto='0' ", noofinpat);
                gridControlChiefMark.DataSource = DS_SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;
        /// <summary>
        /// 自动评分主表记录ID
        /// </summary>
        string m_chiefID = string.Empty;
        /// <summary>
        /// 记录状态
        /// </summary>
        private CheckState m_check = CheckState.NEW;
        /// <summary>
        /// 选中的病人状态
        /// </summary>
        string m_patientstatus = "";
        /// <summary>
        /// 质控类型
        /// </summary>
        private QCType m_qctype = QCType.FINAL;
        /// <summary>
        /// 质控权限
        /// </summary>
        private Authority m_auth = Authority.DEPTQC;

        public void LoadEmrDocPoint()
        {
            try
            {
                //DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                //if (drv == null) return;
                ucPointmark.RefreshLookUpEditorEmrDoc(m_auth, m_check, m_qctype, m_noofinpat, m_chiefID, null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonAddQC_Click(object sender, EventArgs e)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在创建综合评分", "请您稍后！");

                if (m_noofinpat == "")
                {
                    m_App.CustomMessageBox.MessageShow("请选择一个病人");
                    DS_Common.HideWaitDialog(m_WaitDialog);
                    return;
                }

                m_chiefID = InsertNewAutoRecord("0");
                InitUCPoint();
                if (m_chiefID == "")
                {
                    m_App.CustomMessageBox.MessageShow("出错");
                    DS_Common.HideWaitDialog(m_WaitDialog);
                    return;
                }
                m_check = CheckState.NEW;
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private string InsertNewAutoRecord(string isAuto)
        {
            try
            {
                if (m_noofinpat == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先选择一个病人");
                    return "";
                }
                if (m_patientstatus == "1502" || m_patientstatus == "1503")
                {
                    m_qctype = QCType.FINAL;
                }
                else
                {
                    m_qctype = QCType.PART;
                }
                string id = m_SqlManger.InsertAutoMarkRecord(m_noofinpat, isAuto, m_auth, m_qctype);//返回主表记录ID
                if (id == "")
                {
                    return id;
                }
                //LoadAutoMarkRecord(id, isAuto);
                GetgridControlMarkList(m_noofinpat);
                return id;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return "";
            }
        }

        /// <summary>
        /// 加载自动评分记录信息
        /// </summary>
        private void LoadAutoMarkRecord(string id, string isAuto)
        {
            try
            {
                if (isAuto == "0")
                {
                    gridControlChiefMark.DataSource = null;
                    DataTable dt = m_SqlManger.GetAutoMarkInfo(m_noofinpat, isAuto, m_auth);
                    if (dt == null && dt.Rows.Count <= 0)
                    {
                        return;
                    }
                    gridControlChiefMark.BeginUpdate();
                    gridControlChiefMark.DataSource = dt;
                    gridControlChiefMark.EndUpdate();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ID"].ToString() == id)
                        {
                            gridView1.FocusedRowHandle = i;
                            return;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonDel_Click(object sender, EventArgs e)
        {
            try
            {
                int index = this.gridView1.FocusedRowHandle;
                if (index < 0)
                {
                    m_App.CustomMessageBox.MessageShow("未选中记录");
                    return;
                }
                else
                {
                    if (m_App.CustomMessageBox.MessageShow("确定删除选中记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                    {
                        DataRowView drv = gridView1.GetRow(index) as DataRowView;
                        m_chiefID = drv["ID"].ToString();
                        m_SqlManger.DelAutoMarkRecord(m_chiefID);
                        this.simpleButtonDel.Enabled = true;
                        //LoadAutoMarkRecord(m_chiefID, "0"); //加载主评分记录
                        GetgridControlMarkList(m_noofinpat);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRowView drv = gridView1.GetRow(gridView1.FocusedRowHandle) as DataRowView;
                m_chiefID = drv["ID"].ToString();
                InitUCPoint();
                LoadEmrDocPoint();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
