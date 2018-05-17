using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class PropertyCheckBox : PropertyBase
    {
        ZYCheckBox _e;
        public PropertyCheckBox(object o)
            : base(o)
        {
            _e = (ZYCheckBox)o;
        }
        [Category("数据元代码"), DisplayName("数据元代码")]
        public string Code
        {
            get { return _e.Code; }
            set { _e.Code = value; }
        }
        [Category("属性设置"), DisplayName("名称")]
        public string Name
        {
            get { return _e.Name; }
            set { _e.Name = value; }
        }

    }
}
