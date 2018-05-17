using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
   public class HLJLDQLS : IPrintNurse
    {
        public bool IsCustomPrint
        {
            get { return true; }
        }

       

        public void GetPreview(RecordPrintView recordPrintView, System.Drawing.Graphics g)
        {
            Drawer drawer = new Drawer(g);
            drawer.Draw(recordPrintView);
        }

        public void GetPrintimage(RecordPrintView recordPrintView, System.Drawing.Graphics g)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 计算危重护理记录单的总量 并消除相同时间
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        public List<PrintInCommonItemView> JiSuanZongLiang(PrintInCommonView printInCommonView)
        {
            List<PrintInCommonItemView> printInCommonItemViewList = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();
            if (printInCommonItemViewList == null || printInCommonItemViewList.Count == 0) return printInCommonItemViewListNew;
            //用于计算各列的总量
            decimal ZL11 = 0;
            decimal ZL13 = 0;
            int time1 = 7 * 60;  //晚上7点前和早上7点前计算总量
            int time2 = 19 * 60;//晚上7点前和早上7点前计算总量
            for (int i = 0; i < printInCommonItemViewList.Count; i++)
            {
                #region
                var item = printInCommonItemViewList[i];
                decimal L11 = 0;
                decimal L13 = 0;
                

                decimal.TryParse(item.Value11, out L11);
                decimal.TryParse(item.Value13, out L13);
                ZL11 += L11;
                ZL13 += L13;

                string datetimeStr = item.DateTimeShow;


                printInCommonItemViewListNew.Add(item);

                if (!string.IsNullOrEmpty(datetimeStr) && datetimeStr.Length == 16 && i < printInCommonItemViewList.Count - 1)
                {
                    datetimeStr = datetimeStr + ":00";
                    DateTime dtime = Convert.ToDateTime(datetimeStr);
                    string datetimeNextStr = printInCommonItemViewList[i + 1].DateTimeShow + ":00";
                    DateTime dtNext = Convert.ToDateTime(datetimeNextStr);

                    int dTimeMin = dtime.Hour * 60 + dtime.Minute;
                    int dTimeMinNext = dtNext.Hour * 60 + dtNext.Minute;

                    if (i < printInCommonItemViewList.Count - 1
                        && printInCommonItemViewList[i + 1].DateTimeShow.Length == 16)
                    {

                        if ((dTimeMin <= time1 && dTimeMinNext > time1 && dtNext.Date == dtime.Date)
                            || (dTimeMin <= time1 && dtNext.Date > dtime.Date)
                            || (dTimeMin > time2 && dTimeMinNext > time1 && dtNext.Date > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 1;
                            printInCommonItemView.Date = "早七点";
                            printInCommonItemView.Time = "统计";
                            printInCommonItemView.Value10 = "总入量";
                            printInCommonItemView.Value11 = ZL11.ToString();
                            printInCommonItemView.Value12 = "总出量";
                            printInCommonItemView.Value13 = ZL13.ToString();
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                            ZL11 = 0;
                            ZL13 = 0;
                        }
                        else if ((dTimeMin <= time2 && dTimeMinNext > time2 && dtNext.Date == dtime.Date)
                          || (dTimeMin <= time2 && dtNext.Date > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 2;
                            printInCommonItemView.Date = "晚七点";
                            printInCommonItemView.Time = "统计";
                            printInCommonItemView.Value10 = "总入量";
                            printInCommonItemView.Value11 = ZL11.ToString();
                            printInCommonItemView.Value12 = "总出量";
                            printInCommonItemView.Value13 = ZL13.ToString();
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                            ZL11 = 0;
                            ZL13 = 0;
                        }

                    }


                }
                #endregion
                //InCommonNoteBiz.ConvertForImgRec(printInCommonItemViewList[i]);

                //添加最后一条记录
                if (i == printInCommonItemViewList.Count - 1)
                {
                    datetimeStr = printInCommonItemViewList[printInCommonItemViewList.Count - 1].DateTimeShow + ":00";
                    DateTime dtime = Convert.ToDateTime(datetimeStr);
                    int dTimeMin = dtime.Hour * 60 + dtime.Minute;
                    PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                    if (dTimeMin <= time1||dTimeMin>time2)
                    {
                        printInCommonItemView.IsZongLiang = 1;
                        printInCommonItemView.Date = "早七点";
                        printInCommonItemView.Time = "统计";
                    }
                    else
                    {
                        printInCommonItemView.IsZongLiang = 2;
                        printInCommonItemView.Date = "晚七点";
                        printInCommonItemView.Time = "统计";
                    }
                    printInCommonItemView.Value10 = "总入量";
                    printInCommonItemView.Value11 = ZL11.ToString();
                    printInCommonItemView.Value12 = "总出量";
                    printInCommonItemView.Value13 = ZL13.ToString();
                    printInCommonItemViewListNew.Add(printInCommonItemView);
                    ZL11 = 0;
                    ZL13= 0;
                }

            }
            //InCommonNoteBiz.ConvertForDateTime(printInCommonItemViewListNew);
            return printInCommonItemViewListNew;
        }




        public void SetDataRowZongLiang(List<InCommonNoteItemEntity> inCommonNoteItemEntityList, System.Data.DataRow dr, System.Data.DataTable dt)
        {
        }


        public int PageRecordCount
        {
            get;
            set;
        }


        public CommonNoteCountEntity commonNoteCountEntity
        {
            get;

            set;

        }
    }
}
