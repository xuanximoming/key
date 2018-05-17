using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.MaintainBasicInfo
{
    /// <summary>
    ///类名:DeptEntity
    ///功能说明:科室实体
    ///创建人:wyt
    ///创建时间:2012-11-12
    /// </summary>
    class DeptEntity
    {
        private string m_id = "";
        private string m_name = "";
        private int m_sortid = 105;

        public string ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        public int SortID
        {
            get
            {
                return m_sortid;
            }
            set
            {
                m_sortid = value;
            }
        }
    }
}
