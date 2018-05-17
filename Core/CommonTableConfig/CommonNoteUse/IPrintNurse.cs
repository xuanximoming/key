using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public interface IPrintNurse
    {
        /// <summary>
        /// 自定义打印界面
        /// </summary>
        bool IsCustomPrint { get; }

        int PageRecordCount { get; set; }
        /// <summary>
        /// 获取打印预览的图片
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        void GetPreview(RecordPrintView recordPrintView, Graphics g);

        /// <summary>
        /// 获取打印图片
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        void GetPrintimage(RecordPrintView recordPrintView, Graphics g);

        CommonNoteCountEntity commonNoteCountEntity{get;set;}
       
         /// <summary>
         /// 竖向总结总量
         /// </summary>
         /// <param name="printInCommonView"></param>
         /// <returns></returns>
       List<PrintInCommonItemView> JiSuanZongLiang(PrintInCommonView printInCommonView);

        /// <summary>
        /// 设置横向的总结
        /// </summary>
        /// <param name="printInCommonItemViewList"></param>
        /// <param name="dr"></param>
       void SetDataRowZongLiang(List<InCommonNoteItemEntity> inCommonNoteItemEntityList, DataRow dr, DataTable dt);
    }
}
