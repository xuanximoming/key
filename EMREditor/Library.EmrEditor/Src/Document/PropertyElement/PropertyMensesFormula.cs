using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class PropertyMensesFormula : PropertyBase
    {
        ZYMensesFormula _e;
        public PropertyMensesFormula(object o)
            : base(o)
        {
            _e = (ZYMensesFormula)o;
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
        }


        [Category("属性设置"), DisplayName("必点项"), Description("必须用鼠标双击，以表示关注过此项。")]
        [TypeConverter(typeof(BlankConverter))]
        [EditorAttribute(typeof(BoolEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public bool MustClick
        {
            get { return _e.MustClick; }
            set { _e.MustClick = value; }
        }

        [Category("属性设置"), DisplayName("持续天数")]
        public string Last
        {
            get { return _e.Last; }
            set { _e.Last = value; }
        }

        [Category("属性设置"), DisplayName("周期")]
        public string Period
        {
            get { return _e.Period; }
            set { _e.Period = value; }
        }

    }
}
