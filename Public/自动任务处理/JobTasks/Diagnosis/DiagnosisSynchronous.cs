using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.JobManager;
using System.Data;
using System.Management;
using DrectSoft.Core;
using System.Data.SqlClient;

namespace DrectSoft.JobTasks.Diagnosis
{
    /*
    /// <summary>
    /// 诊断信息从HIS中自动同步到EMR中，【作废，不需要使用】
    /// </summary>
    public class DiagnosisSynchronous : BaseJobAction, IDisposable
    {
        #region 作废

        //#region
        //private const string EMRDiagnosis = "DIAGNOSIS";//西医诊断表
        //private const string colMarkID = "MarkId";//诊断标识 诊断代码(ICD10)+肿瘤编码
        //private const string colICD = "ICD";//诊断代码(ICD10)
        //private const string colMapID = "MapID";//映射代码
        //private const string colStandardCode = "StandardCode";//(对应的)标准代码(用户自定义代码有标准代码时，填写此字段)
        //private const string colName = "Name";//疾病名称
        //private const string colPy = "Py";//拼音
        //private const string colWb = "Wb";//五笔
        //private const string colTumorID = "TumorID";//肿瘤编码
        //private const string colStatist = "Statist";//所属统计分类
        //private const string colInnerCategory = "InnerCategory";//内部分类
        //private const string colCategory = "Category";//病种类别
        //private const string colOtherCategroy = "OtherCategroy";//(疾病)其他类别
        //private const string colMemo = "Memo";//备注

        //#endregion

        //#region new properties
        ///// <summary>
        ///// 有初始化动作
        ///// </summary>
        //public override bool HasInitializeAction { get { return true; } }
        //#endregion

        //#region private variables & properties
        ///// <summary>
        ///// 标记 Dispose 方法是否被调用过
        ///// </summary>
        //private bool m_disposed;

        //private DataTable m_EmrDiagnosisTable;
        //private IDataAccess m_EmrHelper;
        //private IDataAccess m_HisHelper;

        //private string MacAddress
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_macAddress))
        //        {
        //            ManagementClass mcMAC = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //            ManagementObjectCollection mocMAC = mcMAC.GetInstances();
        //            string macAdd = string.Empty;
        //            foreach (ManagementObject m in mocMAC)
        //            {
        //                if ((bool)m["IPEnabled"])
        //                {
        //                    macAdd = m["MacAddress"].ToString().Replace(":", "");
        //                    break;
        //                }
        //            }

        //            _macAddress = macAdd;
        //        }
        //        return _macAddress;
        //    }
        //}
        //private string _macAddress;


        ///// <summary>
        ///// 同步病人时记录新入院的病人，以便发送质量监控的触发消息
        ///// </summary>
        //private Dictionary<string, DataRow> m_NewDiagnosis; // 记录需要更新床位表中的首页序号和首页表中床号的床位
        //private ISynchApplication m_App;
        //#endregion

        //#region ctor & dispose
        ///// <summary>
        ///// 
        ///// </summary>
        //public DiagnosisSynchronous()
        //{
        //    try
        //    {

        //        m_EmrHelper = DataAccessFactory.DefaultDataAccess;
        //        m_HisHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        //        m_NewDiagnosis = new Dictionary<string, DataRow>();

        //        m_EmrDiagnosisTable = m_EmrHelper.ExecuteDataTable("select * from " + EMRDiagnosis + " where 1=2");
        //        m_EmrHelper.ResetTableSchema(m_EmrDiagnosisTable, EMRDiagnosis);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    // Check to see if Dispose has already been called.
        //    if (!this.m_disposed)
        //    {
        //        // If disposing equals true, dispose all managed and unmanaged resources.
        //        if (disposing)
        //        {
        //            m_EmrDiagnosisTable.Dispose();
        //        }
        //    }
        //    m_disposed = true;
        //}

        //~DiagnosisSynchronous()
        //{
        //    Dispose(false);
        //}
        //#endregion

        //#region private method

        //private void ExecuteCore(bool isInit)
        //{
        //    base.SynchState = SynchState.Busy;
        //    try
        //    {
        //        //得到HIS中的诊断数据
        //        DataSet resultDS = GetDiagnosisTable(isInit);
        //        //初始化EMR中的诊断数据
        //        InitDiagnosisData(resultDS.Tables[0]);
        //    }
        //    catch
        //    { throw; }
        //    finally
        //    { base.SynchState = SynchState.Stop; }
        //}

        //private DataSet GetDiagnosisTable(bool isInit)
        //{
        //    try
        //    {
        //        string commandText = string.Empty;
        //        DataSet ds = new DataSet();
        //        string sqlInpatient = string.Empty;

        //        if (isInit)
        //        {
        //            //读取所有诊断信息
        //            DataTable diagnosis = m_HisHelper.ExecuteDataTable(" select * from yd_diagnosis ");
        //            diagnosis.TableName = "diagnosis";
        //            ds.Tables.Add(diagnosis.Copy());
        //        }
        //        else
        //        {
        //            DataTable diagnosis = m_HisHelper.ExecuteDataTable(" select * from yd_diagnosis ");
        //            diagnosis.TableName = "diagnosis";
        //            ds.Tables.Add(diagnosis.Copy());
        //        }

        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // /// <summary>
        ///// 初始化病人诊断
        ///// </summary>
        ///// <param name="patientTable">HIS中的诊断数据</param>
        ///// <param name="bedTable"></param>
        //private void InitDiagnosisData(DataTable diagnosisTable)
        //{
             
        //}

        //#endregion

        //#region public IJobAction 成员
        //public override void ExecuteDataInitialize()
        //{
        //    ExecuteCore(true);
        //}

        //public override void Execute()
        //{
        //    try
        //    {
        //        ExecuteCore(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        //todo
        //        JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, ex.Message));
        //    }
        //}
        //#endregion

        #endregion
    }
     */
}
