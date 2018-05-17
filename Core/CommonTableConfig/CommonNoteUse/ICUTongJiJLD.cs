using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Data;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public class ICUTongJiJLD : IPrintNurse
    {
        public int PageIndex = 0;
        public List<object> entitiesList = new List<object>();
        public ICUTongJiJLD()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 计算ICU护理记录单的总量 并消除相同时间
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        public List<PrintInCommonItemView> JiSuanZongLiang(PrintInCommonView printInCommonView)
        {
            List<PrintInCommonItemView> printInCommonItemViewList = printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            List<PrintInCommonItemView> printInCommonItemViewListNew = new List<PrintInCommonItemView>();
            if (printInCommonItemViewList == null || printInCommonItemViewList.Count == 0) return printInCommonItemViewListNew;
            //用于计算各列的总量
            decimal ZL20 = 0;
            decimal ZL22 = 0;
            decimal ZL23 = 0;
            decimal ZL28 = 0;
            decimal ZL29 = 0;
            decimal ZL31 = 0;
            decimal ZL32 = 0;

            decimal ZLW20 = 0;
            decimal ZLW22 = 0;
            decimal ZLW23 = 0;
            decimal ZLW28 = 0;
            decimal ZLW29 = 0;
            decimal ZLW31 = 0;
            decimal ZLW32 = 0;
            int time1 = 7 * 60;  //晚上7点前和早上7点前计算总量
            int time2 = 19 * 60;//晚上7点前和早上7点前计算总量
            //int LastTongJi = 0;  //标记上一条统计是何种统计 1早七点 2是晚七点 0为刚起步
            for (int i = 0; i < printInCommonItemViewList.Count; i++)
            {
                #region
                var item = printInCommonItemViewList[i];
                decimal L20 = 0;
                decimal L22 = 0;
                decimal L23 = 0;
                decimal L28 = 0;
                decimal L29 = 0;
                decimal L31 = 0;
                decimal L32 = 0;

                decimal.TryParse(item.Value20, out L20);
                decimal.TryParse(item.Value22, out L22);
                decimal.TryParse(item.Value23, out L23);
                decimal.TryParse(item.Value28, out L28);
                decimal.TryParse(item.Value29, out L29);
                decimal.TryParse(item.Value31, out L31);
                decimal.TryParse(item.Value32, out L32);

                ZL20 += L20;
                ZL22 += L22;
                ZL23 += L23;
                ZL28 += L28;
                ZL29 += L29;
                ZL31 += L31;
                ZL32 += L32;

                ZLW20 += L20;
                ZLW22 += L22;
                ZLW23 += L23;
                ZLW28 += L28;
                ZLW29 += L29;
                ZLW31 += L31;
                ZLW32 += L32;

                string datetimeStr = item.DateTimeShow;


                printInCommonItemViewListNew.Add(item);

                if (!string.IsNullOrEmpty(datetimeStr) && datetimeStr.Length == 16 && i < printInCommonItemViewList.Count-1)
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

                        if ((dTimeMin <= time1 && dTimeMinNext > time1 && dtNext.Date == dtime.Date)  //同一天
                            || (dTimeMin <= time1 && dtNext.Date > dtime.Date)
                             || (dTimeMin > time2 && dTimeMinNext > time1 && dtNext.Date > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 1;
                            printInCommonItemView.Date = "早七点";
                            printInCommonItemView.Value1 = "统计";
                            printInCommonItemView.Time = "";
                            printInCommonItemView.Value20 = ZL20.ToString();
                            printInCommonItemView.Value22 = ZL22.ToString();
                            printInCommonItemView.Value23 = ZL23.ToString();
                            printInCommonItemView.Value28 = ZL28.ToString();
                            printInCommonItemView.Value29 = ZL29.ToString();
                            printInCommonItemView.Value31 = ZL31.ToString();
                            printInCommonItemView.Value32 = ZL32.ToString();
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                            ZL20 = 0;
                            ZL22 = 0;
                            ZL23 = 0;
                            ZL28 = 0;
                            ZL29 = 0;
                            ZL31 = 0;
                            ZL32 = 0;

                            ZLW20 = 0;
                            ZLW22 = 0;
                            ZLW23 = 0;
                            ZLW28 = 0;
                            ZLW29 = 0;
                            ZLW31 = 0;
                            ZLW32 = 0;
 
                        }
                        else if ((dTimeMin <= time2 && dTimeMinNext > time2 && dtNext.Date == dtime.Date)
                          || (dTimeMin <= time2 && dtNext.Date > dtime.Date))
                        {
                            PrintInCommonItemView printInCommonItemView = new CommonNoteUse.PrintInCommonItemView();
                            printInCommonItemView.IsZongLiang = 2;
                           
                            printInCommonItemView.Date = "晚七点";
                            printInCommonItemView.Value1 = "统计";
                            printInCommonItemView.Time = "";
                            printInCommonItemView.Value20 = ZLW20.ToString();
                            printInCommonItemView.Value22 = ZLW22.ToString();
                            printInCommonItemView.Value23 = ZLW23.ToString();
                            printInCommonItemView.Value28 = ZLW28.ToString();
                            printInCommonItemView.Value29 = ZLW29.ToString();
                            printInCommonItemView.Value31 = ZLW31.ToString();
                            printInCommonItemView.Value32 = ZLW32.ToString();
                            printInCommonItemViewListNew.Add(printInCommonItemView);
                            ZLW20 = 0;
                            ZLW22 = 0;
                            ZLW23 = 0;
                            ZLW28 = 0;
                            ZLW29 = 0;
                            ZLW31 = 0;
                            ZLW32 = 0;

                            //ZL20 = 0;
                            //ZL22 = 0;
                            //ZL23 = 0;
                            //ZL28 = 0;
                            //ZL29 = 0;
                            //ZL31 = 0;
                            //ZL32 = 0;
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
                        printInCommonItemView.Value1 = "统计";
                        printInCommonItemView.Time = "";

                        printInCommonItemView.Value20 = ZL20.ToString();
                        printInCommonItemView.Value22 = ZL22.ToString();
                        printInCommonItemView.Value23 = ZL23.ToString();
                        printInCommonItemView.Value28 = ZL28.ToString();
                        printInCommonItemView.Value29 = ZL29.ToString();
                        printInCommonItemView.Value31 = ZL31.ToString();
                        printInCommonItemView.Value32 = ZL32.ToString();

                        printInCommonItemViewListNew.Add(printInCommonItemView);
                        ZL20 = 0;
                        ZL22 = 0;
                        ZL23 = 0;
                        ZL28 = 0;
                        ZL29 = 0;
                        ZL31 = 0;
                        ZL32 = 0;
                    }
                    else
                    {
                        printInCommonItemView.IsZongLiang = 2;
                        printInCommonItemView.Date = "晚七点";
                        printInCommonItemView.Value1 = "统计";
                        printInCommonItemView.Time = "";

                        printInCommonItemView.Value20 = ZLW20.ToString();
                        printInCommonItemView.Value22 = ZLW22.ToString();
                        printInCommonItemView.Value23 = ZLW23.ToString();
                        printInCommonItemView.Value28 = ZLW28.ToString();
                        printInCommonItemView.Value29 = ZLW29.ToString();
                        printInCommonItemView.Value31 = ZLW31.ToString();
                        printInCommonItemView.Value32 = ZLW32.ToString();

                        printInCommonItemViewListNew.Add(printInCommonItemView);
                        ZLW20 = 0;
                        ZLW22 = 0;
                        ZLW23 = 0;
                        ZLW28 = 0;
                        ZLW29 = 0;
                        ZLW31 = 0;
                        ZLW32 = 0;
                    }
                    
                }

            }
            //InCommonNoteBiz.ConvertForDateTime(printInCommonItemViewListNew);
            return printInCommonItemViewListNew;
        }


        /// <summary>
        /// ICU单据的横向计算
        /// </summary>
        /// <param name="inCommonNoteItemEntityList"></param>
        /// <param name="dr"></param>
        public void SetDataRowZongLiang(List<InCommonNoteItemEntity> inCommonNoteItemEntityList, DataRow datarow, DataTable dt)
        {
            try
            {
                if (inCommonNoteItemEntityList == null || inCommonNoteItemEntityList.Count != 38 || datarow == null || dt == null) return;
                Decimal zongLiangR = 0;
                Decimal decValue20 = 0;
                Decimal decValue22 = 0;
                string value20 = inCommonNoteItemEntityList[19].ValuesShow;
                string value22 = inCommonNoteItemEntityList[21].ValuesShow;
                Decimal.TryParse(value20, out decValue20);
                Decimal.TryParse(value22, out decValue22);
                zongLiangR = decValue20 + decValue22;
                string ZongRLFlow = inCommonNoteItemEntityList[22].CommonNote_Item_Flow;  //总入量的流水号
                Type type = dt.Columns[ZongRLFlow].DataType;
                if (type == typeof(String))
                {
                    if (zongLiangR == 0)
                    {
                        datarow[ZongRLFlow] = "";
                    }
                    else
                    {
                        datarow[ZongRLFlow] = zongLiangR.ToString();
                    }

                }
                else if (type == typeof(Decimal))
                {
                    datarow[ZongRLFlow] = zongLiangR;
                }
                else
                {

                }

                Decimal zongLiangC = 0;
                Decimal decValue28 = 0;
                Decimal decValue29 = 0;
                Decimal decValue31 = 0;
                string value28 = inCommonNoteItemEntityList[27].ValuesShow;
                string value29 = inCommonNoteItemEntityList[28].ValuesShow;
                string value31 = inCommonNoteItemEntityList[30].ValuesShow;
                Decimal.TryParse(value28, out decValue28);
                Decimal.TryParse(value29, out decValue29);
                Decimal.TryParse(value31, out decValue31);
                zongLiangC = decValue28 + decValue29 + decValue31;

                string ZongCLFlow = inCommonNoteItemEntityList[31].CommonNote_Item_Flow;  //总入量的流水号
                Type typeCL = dt.Columns[ZongCLFlow].DataType;
                if (type == typeof(String))
                {
                    if (zongLiangC == 0)
                    {
                        datarow[ZongCLFlow] = "";
                    }
                    else
                    {
                        datarow[ZongCLFlow] = zongLiangC.ToString();
                    }

                }
                else if (type == typeof(Decimal))
                {
                    datarow[ZongCLFlow] = zongLiangC;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void GetPreview(RecordPrintView recordPrintView, Graphics g)
        {
            try
            {
                Drawer drawer = new Drawer(g);
                drawer.Draw(recordPrintView);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void GetPrintimage(RecordPrintView recordPrintView, Graphics g)
        {
            throw new NotImplementedException();
        }

        public bool IsCustomPrint
        {
            get
            {
                return true;
            }
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
