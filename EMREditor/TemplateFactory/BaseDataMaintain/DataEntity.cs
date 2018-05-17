using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    /// <summary>
    /// 宏
    /// </summary>
    class Macro
    {
        public static string s_TypeName = "D_TYPE";
        public static string s_MacroName = "D_NAME";
        public static string s_ExampleName = "Example";
        public static string s_ColumnName = "D_COLUMN";
        public static string s_TableName = "D_TABLE";
        public static string s_SqlName = "D_SQL";

        public string Type
        {
            get
            {
                return "宏";
            }
        }
        public string Name
        {
            get;
            set;
        }
        public string Example
        {
            get;
            set;
        }
        public string Column
        {
            get;
            set;
        }
        public string Table
        {
            get;
            set;
        }
        public string Sql
        {
            get;
            set;
        }

        public Macro()
        {
        }

        public Macro(DataRowView drv)
        {
            Name = drv["D_NAME"].ToString();
            Example = drv["EXAMPLE"].ToString();
            Column = drv["D_COLUMN"].ToString();
            Table = drv["D_TABLE"].ToString();
            Sql = drv["D_SQL"].ToString();
        }
    }

    class Dictionary
    {
        public static string s_ID = "ID";
        public static string s_Name = "NAME";
        public static string s_PY = "PY";
        public static string s_WB = "WB";

        public string ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string PY
        {
            get;
            set;
        }
        public string WB
        {
            get;
            set;
        }

        public Dictionary()
        { }

        public Dictionary(DataRowView drv)
        {
            ID = drv["ID"].ToString();
            Name = drv["NAME"].ToString();
            PY = drv["PY"].ToString();
            WB = drv["WB"].ToString();
        }
    }

    class DictionaryDetail
    {
        public static string s_DictionaryID = "DICTIONARY_ID";
        public static string s_Name = "NAME";
        public static string s_PY = "PY";
        public static string s_WB = "WB";
        public static string s_ID = "ID";
        public static string s_Code = "CODE";

        public string ID
        {
            get;
            set;
        }
        public string DictionaryID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string PY
        {
            get;
            set;
        }
        public string WB
        {
            get;
            set;
        }
        public string CODE
        {
            get;
            set;
        }

        public DictionaryDetail()
        { }

        public DictionaryDetail(DataRowView drv)
        {
            ID = drv["ID"].ToString();
            DictionaryID = drv["DICTIONARY_ID"].ToString();
            Name = drv["NAME"].ToString();
            CODE = drv["CODE"].ToString();
            PY = drv["PY"].ToString();
            WB = drv["WB"].ToString();
        }
    }

    class Symbol
    {
        public static string s_Symbol = "SYMBOL";

        public string NewSymbol
        {
            get;
            set;
        }
        public string OldSymbol
        {
            get;
            set;
        }
    }
}
