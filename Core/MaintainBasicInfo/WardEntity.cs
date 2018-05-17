using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.MaintainBasicInfo
{
    /// <summary>
    ///类名:WardEntity
    ///功能说明:病区实体
    ///创建人:wyt
    ///创建时间:2012-11-12
    /// </summary>
    class WardEntity
    {
        private string m_id = "";
        private string m_name = "";

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
    }
}
