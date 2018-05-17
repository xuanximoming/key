using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.TemplateFactory
{
    /// <summary>
    /// 模板工厂库
    /// </summary>
    public class EmrReplaceItem
    {
        #region 封装为属性
        private string m_Id = String.Empty;

        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        private string m_DestEmrName = String.Empty;

        public string DestEmrName
        {
            get { return m_DestEmrName; }
            set { m_DestEmrName = value; }
        }
        private string m_SourceEmrName = String.Empty;

        public string SourceEmrName
        {
            get { return m_SourceEmrName; }
            set { m_SourceEmrName = value; }
        }
        private string m_DestItemName = String.Empty;

        public string DestItemName
        {
            get { return m_DestItemName; }
            set { m_DestItemName = value; }
        }
        private string m_SourceItemName = String.Empty;

        public string SourceItemName
        {
            get { return m_SourceItemName; }
            set { m_SourceItemName = value; }
        }
        private int m_Valid = 0;

        public int Valid
        {
            get { return m_Valid; }
            set { m_Valid = value; }
        }
        #endregion declaration

    }
}
