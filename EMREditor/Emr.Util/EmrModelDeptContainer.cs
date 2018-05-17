using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace DrectSoft.Emr.Util
{
    public class EmrModelDeptContainer
    {
        /// <summary>
        /// 容器类别(病历、住院志...)
        /// </summary>
        public string ContaineCatalog { get; set; }

        /// <summary>
        /// 设置当前容器的编辑器编辑器
        /// </summary>
        public string ModelEditor{ get; set; }

        /// <summary>
        /// 给当前容器分配角色
        /// 用户类型:1-医生  2-护士  3-医护公共
        /// </summary>
        public string ContainerRole { get; set; }

        /// <summary>
        ///当前容器是否可以添加病历文件
        /// </summary>
        public bool CanAddModel { get; set; }

        /// <summary>
        /// 容器类别(是否展开)
        /// </summary>
        public ContainerType EmrContainerType { get; set; }

        /// <summary>
        /// 参数，调用非编辑器的项目，如调用会诊记录单
        /// </summary>
        public string Args { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ChangeID
        /// </summary>
        public string ChangeID { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 病区ID
        /// </summary>
        public string WardID { get; set; }

        /// <summary>
        /// 病区名称
        /// </summary>
        public string WardName { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string BedNo { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        Collection<EmrModel> m_nodes = new Collection<EmrModel>();
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
        /// 添加模型
        /// </summary>
        /// <param name="model">加入的模板</param>
        /// <returns></returns>
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
            return m_nodes.Remove(model);
        }

        /// <summary>
        /// 清空模型
        /// </summary>
        public void ClearModel()
        {
            m_nodes.Clear();
        }

        public EmrModelDeptContainer() { }

        public EmrModelDeptContainer(DataRow row)
        {
            ContaineCatalog = ContainerCatalog.BingChengJiLu;
            //ModelEditor
            //ContainerRole
            //CanAddModel
            EmrContainerType = ContainerType.Docment;
            //Args
            ChangeID = row.IsNull("id") ? string.Empty : row["id"].ToString();
            DepartmentID = row.IsNull("newdeptid") ? string.Empty : row["newdeptid"].ToString();
            DepartmentName = row.IsNull("newdeptname") ? string.Empty : row["newdeptname"].ToString();
            WardID = row.IsNull("newwardid") ? string.Empty : row["newwardid"].ToString();
            WardName = row.IsNull("newwardname") ? string.Empty : row["newwardname"].ToString();
            BedNo = row.IsNull("newbedid") ? string.Empty : row["newbedid"].ToString();
            CreateUser = row.IsNull("createuser") ? string.Empty : row["createuser"].ToString();
            CreateTime = row.IsNull("createtime") ? DateTime.Now : DateTime.Parse(row["createtime"].ToString());
            Name = DepartmentName + (string.IsNullOrEmpty(WardName) ? string.Empty : "（" + WardName + "）");
        }

    }
}
