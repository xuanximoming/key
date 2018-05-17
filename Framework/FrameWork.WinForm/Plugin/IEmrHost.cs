using System.Collections.ObjectModel;
using System.Data;
using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.FrameWork.Plugin.Manager;

namespace DrectSoft.FrameWork.WinForm.Plugin
{
    /// <summary>
    /// 应用程序对象接口
    /// </summary>
    public interface IEmrHost
    {
        /// <summary>
        /// 获得用户对象
        /// </summary>
        /// <value></value>
        IUser User { get; }

        /// <summary>
        /// 加载制定插件类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="startupClassName"></param>
        /// <returns></returns>
        bool LoadPlugIn(string assemblyName, string startupClassName);

        /// <summary>
        /// 加载制定插件类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        bool LoadPlugIn(string typeName);

        /// <summary>
        /// 根据病人首页序号加载病人信息
        /// </summary>
        /// <param name="firstPageNo">病人首页序号</param>
        void ChoosePatient(decimal firstPageNo);

        /// <summary>
        /// 根据病人序号加载病人并获取信息
        /// </summary>
        /// <param name="firstPageNo">病人首页序号</param>
        /// <param name="MyInp">病人信息</param>
        void ChoosePatient(string firstPageNo, out Inpatient MyInp);

        /// <summary>
        /// 根据病人首页序号和状态加载病人信息
        /// </summary>
        /// <param name="firstPageNo">病人首页序号</param>
        /// <param name="FloderState">病人状态</param>
        void ChoosePatient(decimal firstPageNo, string FloderState);


        /// <summary>
        /// 当前病人信息
        /// </summary>
        Inpatient CurrentPatientInfo { get; set; }

        /// <summary>
        /// 新的当前病人信息
        /// </summary>
        Inpatient NEWCurrentPatientInfo { get; set; }

        /// <summary>
        /// 当前医院用户信息
        /// </summary>
        HospitalInfo CurrentHospitalInfo { get; }

        /// <summary>
        /// 获得自定义消息提示的对象接口，对MessageBox的一层封装，信息提示时不直接使用MessageBox，而使用该封装的对象
        /// </summary>
        ICustomMessageBox CustomMessageBox { get; }

        /// <summary>
        /// 获得访问数据库的辅助对象
        /// </summary>
        IDataAccess SqlHelper { get; }


        /// <summary>
        /// 网卡Mac地址
        /// </summary>
        string MacAddress { get; }


        /// <summary>
        /// 读取配置接口
        /// </summary>
        IAppConfigReader AppConfig { get; }

        /// <summary>
        /// 病人相关信息数据集，根据表名进行使用。
        /// 表名:Inpatients[当前科室病区的病人数据]Beds[当前科室病区的床位信息]
        /// </summary>
        DataSet PatientInfos { get; }

        /// <summary>
        /// 刷新病人信息数据集
        /// </summary>
        /// <returns>成功：空  失败：失败原因</returns>
        string RefreshPatientInfos();

        /// <summary>
        /// 当前帐号有权限的MENU
        /// </summary>
        Collection<PlugInConfiguration> PrivilegeMenu { get; }

        /// <summary>
        /// 当前插件管理器
        /// </summary>
        DrectSoft.FrameWork.Plugin.PluginManager Manager { get; }

        /// <summary>
        /// 是否以只读方式打开病历
        /// </summary>
        bool EmrAllowEdit { get; set; }

        /// <summary>
        /// 供插件调用的公共方法
        /// </summary>
        PluginUtil PublicMethod { get; }

        /// <summary>
        /// 病历编辑器默认设置
        /// </summary>
        EmrDefaultSetting EmrDefaultSettings
        { get; }
        /// <summary>
        /// 日志跟踪器
        /// </summary>
        DrectSoftLog Logger { get; }

        /// <summary>
        /// 当前选中的病历ID，供外部点击病历记录进入病历书写的界面
        /// </summary>
        string CurrentSelectedEmrID { get; set; }

        /// <summary>
        /// 病历权限状态
        /// </summary>
        string FloderState { get; set; }

        /// <summary>
        /// 在右下角动态的显示提示信息
        /// </summary>
        /// <param name="dt">添加的数据</param>
        /// <param name="isClear">是否清空原来的数据</param>
        void ShowMessageWindow(DataTable dt, bool isClear);

    }
}
