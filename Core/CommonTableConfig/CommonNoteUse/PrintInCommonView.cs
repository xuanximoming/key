using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
   public class PrintInCommonView
    {
     private PrintInCommonTabView _PrintInCommonTabViewList1;
     private PrintInCommonTabView _PrintInCommonTabViewList2;
     private PrintInCommonTabView _PrintInCommonTabViewList3;
     private PrintInCommonTabView _PrintInCommonTabViewList4;
     private PrintInCommonTabView _PrintInCommonTabViewList5;
     private PrintInCommonTabView _PrintInCommonTabViewList6;
     private PrintInCommonTabView _PrintInCommonTabViewList7;
     private PrintInCommonTabView _PrintInCommonTabViewList8;
     private PrintInCommonTabView _PrintInCommonTabViewList9;
     private PrintInCommonTabView _PrintInCommonTabViewList10;
     private string _PrintFileName;
     private PrintInpatientView _PrintInpatientView;
     private string _RecordName;


     private string _IncommonNoteflow;

       /// <summary>
       /// 病人使用单据流水号
       /// </summary>
     public string IncommonNoteflow
     {
         get { return _IncommonNoteflow; }
         set { _IncommonNoteflow = value; }
     }

       /// <summary>
       /// 记录单名称
       /// </summary>
     public string RecordName
     {
         get { return _RecordName; }
         set { _RecordName = value; }
     }

       /// <summary>
       /// 病人的基本信息
       /// </summary>
     public PrintInpatientView PrintInpatientView
     {
         get { return _PrintInpatientView; }
         set { _PrintInpatientView = value; }
     }

       /// <summary>
       /// 打印模板名称
       /// </summary>
     public string PrintFileName
     {
         get { return _PrintFileName; }
         set { _PrintFileName = value; }
     }

     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList1
     {
         get
         {
             return _PrintInCommonTabViewList1;
         }
         set
         {
             _PrintInCommonTabViewList1 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList2
     {
         get
         {
             return _PrintInCommonTabViewList2;
         }
         set
         {
             _PrintInCommonTabViewList2 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList3
     {
         get
         {
             return _PrintInCommonTabViewList3;
         }
         set
         {
             _PrintInCommonTabViewList3 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList4
     {
         get
         {
             return _PrintInCommonTabViewList4;
         }
         set
         {
             _PrintInCommonTabViewList4 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList5
     {
         get
         {
             return _PrintInCommonTabViewList5;
         }
         set
         {
             _PrintInCommonTabViewList5 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList6
     {
         get
         {
             return _PrintInCommonTabViewList6;
         }
         set
         {
             _PrintInCommonTabViewList6 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList7
     {
         get
         {
             return _PrintInCommonTabViewList7;
         }
         set
         {
             _PrintInCommonTabViewList7 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList8
     {
         get
         {
             return _PrintInCommonTabViewList8;
         }
         set
         {
             _PrintInCommonTabViewList8 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList9
     {
         get
         {
             return _PrintInCommonTabViewList9;
         }
         set
         {
             _PrintInCommonTabViewList9 = value;
         }
     }
     /// <summary>
     /// 获取或设置一个值，该值指示 XXXX
     /// </summary>
     public virtual PrintInCommonTabView PrintInCommonTabViewList10
     {
         get
         {
             return _PrintInCommonTabViewList10;
         }
         set
         {
             _PrintInCommonTabViewList10 = value;
         }
     }

    }
}
