using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace DrectSoft.Emr.Util
{
    public class EmrModelContainer
    {

        /// <summary>
        /// 容器类别
        /// </summary>
        public string ContainerCatalog { get; set; }
        /// <summary>
        /// 容器类型名称
        /// </summary>
        public string Name { get; set; }

        Collection<EmrModel> m_nodes = new Collection<EmrModel>();
        /// <summary>
        /// 设置当前容器的编辑器编辑器
        /// </summary>
        public string ModelEditor
        {
            get { return _modelEditor; }
            set { _modelEditor = value; }
        }
        private string _modelEditor;

        /// <summary>
        /// 给当前容器分配角色
        /// 用户类型:1-医生  2-护士  3-医护公共
        /// </summary>
        public string ContainerRole
        {
            get { return _containerRole; }
            set { _containerRole = value; }
        }
        private string _containerRole;

        /// <summary>
        ///当前容器是否可以添加病历文件
        /// </summary>
        public bool CanAddModel
        {
            get { return _canAddModel; }
            set { _canAddModel = value; }
        }
        private bool _canAddModel;

        /// <summary>
        /// 容器类别
        /// </summary>
        public ContainerType EmrContainerType
        {
            get { return _emrContainerType; }
            set { _emrContainerType = value; }
        }
        private ContainerType _emrContainerType;

        /// <summary>
        /// 参数，调用非编辑器的项目，如调用会诊记录单
        /// </summary>
        public string Args
        {
            get { return _args; }
            set { _args = value; }
        }
        private string _args = string.Empty;

        /// <summary>
        /// 病历文件列表
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"),]
        public List<EmrModel> Models
        {
            get
            {
                List<EmrModel> lem = new List<EmrModel>();
                for (int i = 0; i < m_nodes.Count; i++)
                {
                    lem.Add(m_nodes[i] as EmrModel);
                }
                return lem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EmrModelContainer()
        {


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public EmrModelContainer(DataRow row)
        {
            Name = row.IsNull("CNAME") ? string.Empty : row["CNAME"].ToString();
            ModelEditor = row.IsNull("MNAME") ? string.Empty : row["MNAME"].ToString();
            ContainerCatalog = row.IsNull("CCODE") ? string.Empty : row["CCODE"].ToString();
            EmrContainerType = row.IsNull("OPEN_FLAG") ? ContainerType.None : (ContainerType)Enum.Parse(typeof(ContainerType), row["OPEN_FLAG"].ToString());
            ContainerRole = row.IsNull("UTYPE") ? string.Empty : row["UTYPE"].ToString();
            //Args = row.IsNull("ARGS") ? String.Empty : row["ARGS"].ToString();

        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="model">加入的模板</param>
        /// <returns>加入成功则返回真</returns>
        public void AddModel(EmrModel model)
        {
            m_nodes.Add(model);

        }

        /// <summary>
        /// 删除模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RemoveModel(EmrModel model)
        {
            //bool ret = m_models.Remove(model);
            bool ret = m_nodes.Remove(model);
            return ret;
        }

        /// <summary>
        /// 清空模型
        /// </summary>
        public void ClearModel()
        {
            //m_models.Clear();
            m_nodes.Clear();
        }
    }

    /// <summary>
    /// 枚举类型
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// 入院记录
        /// </summary>
        Docment = 1,
        /// <summary>
        /// 病历
        /// </summary>
        CommonDocment = 2,
        /// <summary>
        /// 护理文书
        /// </summary>
        NurseDocment = 4,
        /// <summary>
        /// 其他
        /// </summary>
        None = 0

    }

    /// <summary>
    /// 病历类型
    /// </summary>
    public static class ContainerCatalog
    {
        /// <summary>
        /// 入院记录
        /// </summary>
        public static string RuYuanJiLu = "AB";

        /// <summary>
        /// 病程记录
        /// </summary>
        public static string BingChengJiLu = "AC";

        /// <summary>
        /// 会诊记录
        /// </summary>
        public static string huizhenjilu = "AL";

        /// <summary>
        /// 病案首页
        /// </summary>
        public static string BingAnShouYe = "AA";

        /// <summary>
        /// 医技报告
        /// </summary>
        public static string YiJiBaoGao = "AO";

        /// <summary>
        /// 三测表曲线
        /// </summary>
        public static string SanCeDan = "13";

        /// <summary>
        /// 医嘱浏览
        /// </summary>
        public static string YiZhuLiuLan = "29";

        /// <summary>
        /// 查房录音
        /// </summary>
        public static string ChaFangLuYin = "25";

        /// <summary>
        /// 护理记录表格
        /// </summary>
        public static string HuLiJiLuBiaoGe = "AP";
    }
}
