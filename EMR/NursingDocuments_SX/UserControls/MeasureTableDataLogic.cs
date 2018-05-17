using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YidanSoft.FrameWork.WinForm.Plugin;
using System.Drawing;
using System.Data;
using YidanSoft.Core.NursingDocuments.PublicSet;

namespace YidanSoft.Core.NursingDocuments.UserControls
{
    class MeasureTableDataLogic
    {
        private IYidanEmrHost m_App;
        public IYidanEmrHost App
        {
            get
            {
                return m_App;
            }
            set
            {
                m_App = value;
            }
        }

        #region 方法

        /// <summary>
        /// 初始化病人基本信息
        /// </summary>
        private void InitInpatInfo()
        {
            DataTable dt = MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.App.CurrentPatientInfo.NoOfFirstPage.ToString());
            if (dt.Rows.Count == 1)
            {
                MethodSet.AdmitDate = dt.Rows[0]["AdmitDate"].ToString().Trim();
                MethodSet.OutHosDate = dt.Rows[0]["OutHosDate"].ToString().Trim();
                MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// 体温表需要的数据
    /// </summary>
    class MeasureTableData
    {
        string m_InTime = string.Empty;//入院日期
        string m_OutTime = string.Empty;//出院日期
        string m_PatientName = string.Empty;//姓名
        string m_Age = string.Empty;//年龄
        string m_Gender = string.Empty;//性别
        string m_Section = string.Empty;//科别
        string m_BedCode = string.Empty;//床号
        string m_InpatientNo = string.Empty;//住院号

        DataTable m_DataTableTemperature = new DataTable();     //体温数据
        DataTable m_DataTableMaiBo = new DataTable();           //脉搏数据
        DataTable m_DataTableHuXi = new DataTable();            //呼吸数据
        DataTable m_DataTableWuLiJiangWen = new DataTable();    //物理降温数据
        DataTable m_DataTableXinLv = new DataTable();           //心率数据
        DataTable m_DateTableXueYa = new DataTable();           //血压数据
        DataTable m_DataTableZongRuLiang = new DataTable();     //总入量
        DataTable m_DataTableZongChuLiang = new DataTable();    //总出量
        DataTable m_DataTableYinLiuLiang = new DataTable();     //引流量
        DataTable m_DataTableDaBianCiShu = new DataTable();     //大便次数
        DataTable m_DataTableShenGao = new DataTable();         //身高
        DataTable m_DataTableTiZhong = new DataTable();         //体重
        DataTable m_DataTableGuoMinYaoWu = new DataTable();     //过敏药物
        DataTable m_DataTableTeShuZhiLiao = new DataTable();    //特殊治疗
        //DataTable m_DataTableOther1 = new DataTable();          //其他1
        //DataTable m_DataTableOther2 = new DataTable();          //其他2

        public MeasureTableData(IYidanEmrHost emrHost)
        {
            //得到病人基本信息
            DataTable dataTablePatientInfo = PublicSet.MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.App.CurrentPatientInfo.NoOfFirstPage.ToString());

            foreach (DataRow dr in dataTablePatientInfo.Rows)
            {
                m_InTime = dr["AdmitDate"].ToString();
                m_OutTime = dr["OutHosDate"].ToString();
                m_PatientName = dr["Name"].ToString();
                m_Age = dr["AgeStr"].ToString();
                m_Gender = dr["gender"].ToString();
                m_Section = dr["dept_name"].ToString();
                m_BedCode = dr["AdmitBed"].ToString();
                m_InpatientNo = dr["PatID"].ToString();
            }
        }

    }
}
