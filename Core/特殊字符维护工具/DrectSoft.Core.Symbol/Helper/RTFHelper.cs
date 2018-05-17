using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YidanSoft.Common.Object2Editor.Encoding;
using System.Drawing;
using System.Collections;

namespace YidanSoft.Core.Symbol
{
    class RTFHelper
    {
        private static Hashtable _defaultValueTable;

        public string GetRTFByStr(string str)
        {
            return FullRtfValue2(str);
        }

        /// <summary>
        /// 完整的rtf格式，默认宋体，12号
        /// </summary>
        public string FullRtfValue2(string Value)
        {
            return RtfEncoding.WrapRtfByCoreContent(Value, new Font("宋体", 12)); 
        }


        public string FullRtfValue(string Value)
        {
            return RtfEncoding.WrapRtfByCoreContent(Value, new Font("宋体", 12)); 
        }


        //#region privte method
        //private object GetValue(string )
        //{
        //    object value = _symbolRow[colname];
        //    if (Convert.IsDBNull(value))
        //        return DefaultValueTable["RTF"];
        //    return value;
        //}

        //private void SetValue(string colname, object value)
        //{
        //    _symbolRow[colname] = value;
        //}

        //#endregion

        //private static Hashtable DefaultValueTable
        //{
        //    get
        //    {
        //        if (_defaultValueTable == null)
        //        {
        //            _defaultValueTable.Add("RTF", string.Empty);
        //        }
        //        return _defaultValueTable;
        //    }
        //}
        public string GetStrBYRTF(string RTFstr)
        {
        
            return RtfEncoding.GetRtfCoreContent(RTFstr).Replace(RtfEncoding.RtfNewLine, "");// EmrSymbolEngine.EjectorRTFString(editor);
        }
    }
}
