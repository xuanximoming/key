﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 多选的属性类
    /// </summary>
    public class PropertyMultiSelectableElement : PropertyBase
    {
        ZYSelectableElement _e;
        public PropertyMultiSelectableElement(object e)
            : base(e)
        {
            _e = (ZYSelectableElement)e;
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

        //[Category("属性设置"), DisplayName("选项")]
        //[TypeConverter(typeof(BlankConverter)), Description("选项中子模板名用:[子模板名] 表示；提示用:{提示名} 表示；多选分组用:<组号> 表示")]
        //public string[] Items
        //{
        //    get { return _e.Items; }
        //    set { _e.Items = value; }
        //}

        [Category("属性设置"), DisplayName("选项")]
        [TypeConverter(typeof(BlankConverter))]
        [EditorAttribute(typeof(SelectableMultiEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<ZYSelectableElementItem> Items
        {
            get { return _e.SelectList; }
            set { _e.SelectList = value; }
        }

        

    }

}
