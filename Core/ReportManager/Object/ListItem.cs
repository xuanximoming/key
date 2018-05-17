namespace DrectSoft.Core.DSReportManager
{
    using System;

    /// <summary>
    /// Combox 列表项
    /// </summary>
    public class ListItem
    {
        private string m_sText = string.Empty;
        private string m_sValue = string.Empty;

        /// <summary>
        /// 创建ListItem项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="text">文本</param>
        public ListItem(string value, string text)
        {
            this.m_sValue = value;
            this.m_sText = text;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj">比较的对象</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (base.GetType().Equals(obj.GetType()))
            {
                ListItem item = (ListItem) obj;
                return this.m_sText.Equals(item.Value);
            }
            return false;
        }

        /// <summary>
        /// 返回散列表
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.m_sValue.GetHashCode();
        }

        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.m_sText;
        }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get
            {
                return this.m_sText;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get
            {
                return this.m_sValue;
            }
        }
    }
}

