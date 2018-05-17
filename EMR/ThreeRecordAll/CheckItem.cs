using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.EMR.ThreeRecordAll
{
    /// <summary>
    /// 数据检查项 add by tj 2012-11-12   通过XML对其进行初始化
    /// </summary>
    public struct CheckItem
    {
        public string caption;//名称
        public decimal maxValue;//最小值
        public decimal minValue;//最小值
        public string fieldName;//绑定字段

        public CheckItem(string _caption, decimal _maxValue, decimal _minValue, string _fieldName)
        {
            this.caption = _caption;
            this.maxValue = _maxValue;
            this.minValue = _minValue;
            this.fieldName = _fieldName;
        }
    }
}
