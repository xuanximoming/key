using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class PropertyFormatDatetime : PropertyBase
    {
        ZYFormatDatetime _e;
        public PropertyFormatDatetime(object o)
            : base(o)
        {
            _e = (ZYFormatDatetime)o;
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

        [Category("属性设置"), DisplayName("必点项"), Description("必须用鼠标双击，以表示关注过此项。")]
        [TypeConverter(typeof(BlankConverter))]
        [EditorAttribute(typeof(BoolEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool MustClick
        {
            get { return _e.MustClick; }
            set { _e.MustClick = value; }
        }

        [Category("属性设置"), DisplayName("格式"), Description("示例：\r\nyyyy年MM月dd日 HH时mm分ss秒 \r\n可根据需要选择部分功能")]
        [EditorAttribute(typeof(DateTimeFormatEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String FormatString
        {
            get { return _e.FormatString; }
            set { _e.FormatString = value; }
        }
    }

}
