using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DrectSoft.Emr.QcManagerNew
{
    /// <summary>
    /// 与病历评分表相关的实体类
    /// add by ywk 2012年4月6日9:35:01
    /// </summary>
    class EmrPointInfo
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        public static string InpatientName = string.Empty;   
        /// <summary>
        /// 科室
        /// </summary>
        public static string DeptName = string.Empty;
        /// <summary>
        /// 住院号
        /// </summary>
        public static string InpatientNo = string.Empty;
        /// <summary>
        /// 住院医师 (对应InPatient表里的 Resident)
        /// </summary>
        public static string ResidentDoc = string.Empty;
        /// <summary>
        /// 上级医师（取得主任医师）
        /// </summary>
        public static string ChiefDoc = string.Empty;

        /// <summary>
        /// 得到病人基本信息
        /// </summary>
        /// <param name="emrPointInfo"></param>
        public void InitPatientInfo(DataTable dataemrPointInfo)
        {
            if (dataemrPointInfo.Rows.Count>0)
            {
                InpatientName = dataemrPointInfo.Rows[0]["patname"].ToString();//患者姓名
                DeptName = dataemrPointInfo.Rows[0]["deptname"].ToString();//科室名称
                InpatientNo = dataemrPointInfo.Rows[0]["patid"].ToString();//住院号
                ResidentDoc = dataemrPointInfo.Rows[0]["indocname"].ToString();//住院医师
                ChiefDoc = dataemrPointInfo.Rows[0]["updocname"].ToString();//上级医师
            }
        }
    }
    /// <summary>
    /// 设置与病人相关信息的位置
    /// </summary>
    class PatientInfoLocation
    {
        //病人信息的纵坐标
        public int InpatientInformationY = 65;

        //病人信息的横坐标
        public int DeptNameX = 60 - 40;
        public int PainetNameX = 170 - 40;
        public int InpatientNoX =290 - 40;
        public int ResidentDocX = 420 - 40;
        public int ChiefDocX = 550 - 40;
    }



}
