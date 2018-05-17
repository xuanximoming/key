using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
   public class RecordPrintView
    {

       private string _PrintFileName;
       private string _RecordName;

       private PrintInpatientView _PrintInpatientView;
       private List<PrintInCommonItemView> _PrintInCommonItemViewList;
       private PrintInCommonItemView _PrintInCommonItemViewOther;
       private PrintInCommonTabView _PrintInCommonTabView;

       public PrintInCommonTabView PrintInCommonTabView
       {
           get { return _PrintInCommonTabView; }
           set { _PrintInCommonTabView = value; }
       }


        /// <summary>
        /// 记录单据名称
        /// </summary>
       public string RecordName
       {
           get { return _RecordName; }
           set { _RecordName = value; }
       }



        /// <summary>
        /// 其他数据
        /// </summary>
       public PrintInCommonItemView PrintInCommonItemViewOther
       {
           get { return _PrintInCommonItemViewOther; }
           set { _PrintInCommonItemViewOther = value; }
       }


        /// <summary>
        /// 表格数据
        /// </summary>
       public List<PrintInCommonItemView> PrintInCommonItemViewList
       {
           get { return _PrintInCommonItemViewList; }
           set { _PrintInCommonItemViewList = value; }
       }


       /// <summary>
       /// 病人相关信息
       /// </summary>
       public PrintInpatientView PrintInpatientView
       {
           get { return _PrintInpatientView; }
           set { _PrintInpatientView = value; }
       }

        /// <summary>
        /// xml文件路径
        /// </summary>
        public string PrintFileName
        {
            get { return _PrintFileName; }
            set { _PrintFileName = value; }
        }

    }
}
