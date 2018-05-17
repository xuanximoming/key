using System;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// <para>继承自System.Collections.CollectionBase</para>
    /// 文档元素属性集合
    /// </summary>
    [Serializable]
    public class ZYAttributeCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// 属性集合的内容是否改变标志
        /// </summary>
        private bool bolModified = false;
        /// <summary>
        /// 所属文档基本元素（ZYTextElement）
        /// </summary>
        public ZYTextElement OwnerElement;


        /// <summary>
        /// 属性集合的内容是否改变标志
        /// </summary>
        public bool Modified
        {
            get { return bolModified; }
            set { bolModified = value; }
        }


        /// <summary>
        /// 判断两个属性列表内容是否一样
        /// <para>传入属性为空放回false</para>
        /// <para>相同返回true</para>
        /// </summary>
        /// <param name="a">属性列表</param>
        /// <returns>两个属性列表内容是否一样</returns>
        public bool EqualsValue(ZYAttributeCollection a)
        {
            if (a == null) return false;
            if (a == this) return true;
            return (this.ToListString() == a.ToListString());
        }

        /// <summary>
        /// 复制所有属性到另一个属性变量
        /// </summary>
        /// <param name="descSet"></param>
        public void CopyTo(ZYAttributeCollection descSet)
        {
            descSet.List.Clear();
            foreach (ZYAttribute myAttr in this)
            {
                descSet.List.Add(myAttr.Clone());
            }
        }

        /// <summary>
        /// 通过名称获取指定属性对象
        /// </summary>
        public ZYAttribute this[string strName]
        {
            get
            {
                foreach (ZYAttribute myAttr in this)
                    if (myAttr.Name == strName)
                        return myAttr;
                return null;
            }
        }

        /// <summary>
        /// 删除指定名称的属性
        /// </summary>
        /// <param name="strName"></param>
        public void RemoveAttribute(string strName)
        {
            ZYAttribute myAttr = this[strName];
            if (myAttr != null)
                this.List.Remove(myAttr);
        }
        /// <summary>
        /// 覆写清除System.Collections.CollectionBase实例的内容时执行其他自定义进程
        /// </summary>
        protected override void OnClear()
        {
            base.OnClear();
            bolModified = true;
        }

        /// <summary>
        /// 是否包含指定名称的属性对象
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>是否包含该名称的属性</returns>
        public bool Contains(string strName)
        {
            foreach (ZYAttribute myAttr in this)
            {
                if (myAttr.Name == strName)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 设置指定名称的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <param name="vValue">属性值</param>
        public void SetValue(string strName, object vValue)
        {
            ZYAttribute myValue = null;
            foreach (ZYAttribute myAttr in this)
            {
                if (myAttr.Name == strName)
                {
                    myValue = myAttr;
                    break;
                }
            }
            if (myValue == null)
            {
                myValue = new ZYAttribute();
                myValue.Name = strName;
                this.List.Add(myValue);
            }

            if (myValue.Value == null || myValue.Value.Equals(vValue) == false)
            {
                if (myValue.Value != null || vValue != null)
                {
                    if (OwnerElement != null && OwnerElement.OwnerDocument != null && OwnerElement.OwnerDocument.ContentChangeLog != null)
                    //if (OwnerElement != null && OwnerElement.OwnerDocument != null)
                    {
                        OwnerElement.OwnerDocument.ContentChangeLog.LogAttribute(
                            OwnerElement,
                            myValue,
                            vValue);

                        OwnerElement.OwnerDocument.Modified = true;
                    }
                    myValue.Value = vValue;
                    //if( myValue.Value == myValue.DefaultValue )myList.Remove( myValue );
                    bolModified = true;
                }
            }
        }

        /// <summary>
        /// 设置所有属性为默认值
        /// </summary>
        /// <returns>是否修改了数据</returns>
        public bool SetDefaultValue()
        {
            bool bolChange = false;
            foreach (ZYAttribute myAttr in this)
            {
                object DefaultValue = ZYAttribute.GetDefaultValue(myAttr.Name);
                if (DefaultValue != null && DefaultValue.Equals(myAttr.Value))
                {
                    if (OwnerElement != null && OwnerElement.OwnerDocument != null && OwnerElement.OwnerDocument.ContentChangeLog != null)
                    {
                        OwnerElement.OwnerDocument.ContentChangeLog.LogAttribute(
                            OwnerElement,
                            myAttr,
                            DefaultValue);
                    }
                    myAttr.Value = DefaultValue;
                    bolModified = true;
                    bolChange = true;
                }
            }
            return bolChange;
        }

        /// <summary>
        /// 获得指定名称的布尔类型的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>属性值</returns>
        public bool GetBool(string strName)
        {
            foreach (ZYAttribute myAttr in this)
            {
                if (myAttr.Name == strName)
                    return (bool)myAttr.Value;
            }
            return (bool)ZYAttribute.GetDefaultValue(strName);
        }
        /// <summary>
        /// 获得指定名称的整数类型的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>属性值</returns>
        public int GetInt32(string strName)
        {
            try
            {
                foreach (ZYAttribute myAttr in this)
                {
                    if (myAttr.Name == strName)
                        return Convert.ToInt32(myAttr.Value);
                }
                return (int)ZYAttribute.GetDefaultValue(strName);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得指定名称的单精度浮点数类型的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>属性值</returns>
        public float GetFloat(string strName)
        {
            try
            {
                foreach (ZYAttribute myAttr in this)
                {
                    if (myAttr.Name == strName)
                        return Convert.ToSingle(myAttr.Value);
                }
                return Convert.ToSingle(ZYAttribute.GetDefaultValue(strName));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得指定名称的字符串类型的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>属性值</returns>
        public string GetString(string strName)
        {
            foreach (ZYAttribute myAttr in this)
            {
                if (myAttr.Name == strName)
                    return Convert.ToString(myAttr.Value);
            }
            return Convert.ToString(ZYAttribute.GetDefaultValue(strName));
        }

        /// <summary>
        /// 获得颜色值类型的指定名称的属性值
        /// </summary>
        /// <param name="strName">属性名称</param>
        /// <returns>属性值</returns>
        public System.Drawing.Color GetColor(string strName)
        {
            foreach (ZYAttribute myAttr in this)
                if (myAttr.Name == strName)
                    return (System.Drawing.Color)myAttr.Value;
            return System.Drawing.SystemColors.WindowText;
        }

        /// <summary>
        /// load attribute set from a xmlelement's attribute set
        /// </summary>
        /// <param name="myElement">xmlelement object</param>
        /// <returns>handle is ok</returns>
        public bool FromXML(System.Xml.XmlElement myElement)
        {
            // clear owner attribute set
            this.Clear();
            if (myElement == null) return false;
            // enumerate xmlelement's attribute set
            // for each xmlattribute create an attribute and add to owner attribute set
            foreach (System.Xml.XmlAttribute myXMLAttr in myElement.Attributes)
            {
                ZYAttribute myAttr = new ZYAttribute();
                myAttr.Name = myXMLAttr.Name;
                myAttr.ValueString = myXMLAttr.Value;
                if (myAttr.Value != myAttr.DefaultValue)
                    this.List.Add(myAttr);
            }
            bolModified = false;
            return true;
        }
        /// <summary>
        /// save owner attribute set's content to xmlelement's attribute set
        /// </summary>
        /// <param name="myElement">xmlelement object</param>
        /// <returns>Is handle ok</returns>
        public bool ToXML(System.Xml.XmlElement myElement)
        {
            // if xmlelement is null reference then return false
            if (myElement == null) return false;
            // enumerate owner attribute set 
            // for each attribute set xmlattribute 
            foreach (ZYAttribute myAttr in this)
            {
                //if (myAttr.isNeedSave())
                //{
                string strValue = myAttr.ValueString;
                if (strValue != null)
                    myElement.SetAttribute(myAttr.Name, myAttr.ValueString);
                //}
            }
            return true;
        }

        /// <summary>
        /// 将文档属性转变成可变字符字符串（StringBuilder）
        /// <para>StringBuilder.tostring返回</para>
        /// </summary>
        /// <returns></returns>
        public string ToListString()
        {
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            foreach (ZYAttribute myAttr in this)
            {
                if (myAttr.isNeedSave())
                {
                    string strValue = myAttr.ValueString;
                    if (strValue != null)
                        myStr.Append(myAttr.Name + "=" + strValue);
                }
            }
            return myStr.ToString();
        }
    }//public class ZYAttributeCollection : System.Collections.CollectionBase
}
