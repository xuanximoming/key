using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.ObjectModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 完成状态
    /// </summary>
    public enum CompleteType
    {
        /// <summary>
        /// 未完成
        /// </summary>
        NonComplete = 0, 

        /// <summary>
        /// 已完成
        /// </summary>
        Completed =1, 
    }

    /// <summary>
    /// 时限规则纪录状态
    /// </summary>
    public enum RuleRecordState
    { 
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 限时内未完成
        /// </summary>
        UndoIntime = 1,

        /// <summary>
        /// 限时内完成
        /// </summary>
        DoIntime = 2,

        /// <summary>
        /// 超时未完成
        /// </summary>
        UndoOuttime = 3,

        /// <summary>
        /// 超时完成
        /// </summary>
        DoOuttime = 4,
    }

    /// <summary>
    /// 数据记录状态
    /// </summary>
    public enum RecordState
    {
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// 有效
        /// </summary>
        Valid = 1,

        /// <summary>
        /// 有效但需要条件触发
        /// </summary>
        ValidWait = 2,

        /// <summary>
        /// 有效但不显示
        /// </summary>
        ValidNonVisible = 3,
    }

    /// <summary>
    /// 时限规则纪录状态中文显示
    /// </summary>
    public class Enum2Chinese
    {
        //Type _type;
        Collection<ChineseEnum> _enumNames = new Collection<ChineseEnum>();

        public Collection<ChineseEnum> EnumNames
        {
            get { return _enumNames; }
        }

        public struct ChineseEnum
        {
            public string Name;
            public Enum Value;
            public ChineseEnum(string name, Enum value)
            {
                Name = name;
                Value = value;
            }
            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="enumtype"></param>
        public Enum2Chinese(Type enumtype)
        {
            if (!enumtype.IsEnum) return;
            FieldInfo[]  fields = enumtype.GetFields();
            foreach (FieldInfo field in fields)
            {
                object[] attrs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    ChineseEnum ce = new ChineseEnum(((System.ComponentModel.DescriptionAttribute)attrs[0]).Description, field.GetValue(field) as Enum);
                    if (!_enumNames.Contains(ce)) _enumNames.Add(ce);
                }
            }
        }

        public ChineseEnum FindChineseEnum(Enum value)
        {
            for (int i = 0; i < _enumNames.Count; i++)
            {
                if (_enumNames[i].Value.Equals(value)) return _enumNames[i];
            }
            return new ChineseEnum();
        }
    }

}
