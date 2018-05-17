using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class AddNewTemplet_Header : DevBaseForm
    {
        IEmrHost m_app;
        /// <summary>
        /// 页眉
        /// </summary>
        public EmrTempletHeader m_EmrTempletHeader;

        /// <summary>
        /// 页脚
        /// </summary>
        public EmrTemplet_Foot m_EmrTemplet_Foot;

        public EmrTempletType m_EmrTempletType;

        /// <summary>
        /// 记录是否保存状态
        /// </summary>
        public bool IsSave = false;

        public AddNewTemplet_Header(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
 
        }

        /// <summary>
        /// 保存事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
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

                //保存页眉
                if (m_EmrTempletType == EmrTempletType.Templet_Header)
                {
                    SaveTemplet_Heard();
                }
                //保存页脚
                else if (m_EmrTempletType == EmrTempletType.Templet_Foot)
                {
                    SaveTemplet_Foot();
                }

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                IsSave = true;

                this.Close();
            }
            catch
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败");
                IsSave = false;
                return;
            }
            

        }

        /// <summary>
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.lookUpEditorHospitel.CodeValue))
                {
                    this.lookUpEditorHospitel.Focus();
                    return "请选择使用医院";
                }
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    this.txtName.Focus();
                    return "页眉/页脚名称不能为空";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存页眉
        /// </summary>
        private void SaveTemplet_Heard()
        {
            string editType = "";
            //无templetID代表为新增模板
            if (m_EmrTempletHeader.HeaderId == "")
            {
                editType = "1";
                m_EmrTempletHeader.CreatorId = m_app.User.Id;
            }
            else
            {
                editType = "2";
            }
            SQLManger m_sqlmanger = new SQLManger(m_app);
            m_EmrTempletHeader.HospitalCode = this.lookUpEditorHospitel.CodeValue;
            m_EmrTempletHeader.Name = this.txtName.Text.Trim();
            //页眉中增加图片报错的问题  add by ywk 2012年10月25日 11:58:05 
            string strheaderid = m_sqlmanger.SaveTemplet_Header(m_EmrTempletHeader, editType);

            m_EmrTempletHeader = m_sqlmanger.GetTemplet_Header(strheaderid);
        }

        /// <summary>
        /// 保存页脚
        /// </summary>
        private void SaveTemplet_Foot()
        {
            string editType = "";
            //无FootID代表为新增页脚
            if (m_EmrTemplet_Foot.FootId == "")
            {
                editType = "1";
                m_EmrTemplet_Foot.CreatorId = m_app.User.Id;
            }
            else
            {
                editType = "2";
            }
            SQLManger m_sqlmanger = new SQLManger(m_app);
            m_EmrTemplet_Foot.HospitalCode = this.lookUpEditorHospitel.CodeValue;
            m_EmrTemplet_Foot.Name = this.txtName.Text.Trim();

            string str_Foot_id = m_sqlmanger.SaveTemplet_Foot(m_EmrTemplet_Foot, editType);

            m_EmrTemplet_Foot = m_sqlmanger.GetTemplet_Foot(str_Foot_id);
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
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

        #region 初始下拉框
        //初始化科室
        private void InitHospitel()
        {

            lookUpWindowHospitel.SqlHelper = m_app.SqlHelper;

            //yxy 暂时加载TP科室
            //DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
            //     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

            string sql = string.Format(@"select a.medicalid ID,a.name NAME from hospitalinfo a ");
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "医院代码";
            Dept.Columns["NAME"].Caption = "医院名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols);
            lookUpEditorHospitel.SqlWordbook = deptWordBook;


        }
        #endregion

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewTemplet_Header_Load(object sender, EventArgs e)
        {
            try
            {
                InitHospitel();

                //add by cyq 2013-03-08
                if (m_EmrTempletType == EmrTempletType.Templet_Header)
                {
                    labelControl2.Text = "页眉名称：";
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Foot)
                {
                    labelControl2.Text = "页脚名称：";
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}